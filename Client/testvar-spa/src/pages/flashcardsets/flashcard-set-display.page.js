import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import PageTitle from "../../components/common/PageTitle";
import * as React from "react";
import Grid from "@mui/material/Grid2";
import FlashCard from "../../components/flashcardsets/FlashCard";
import {getFlashcardSet} from "../../api/FlashcardSetApi";
import {useAuth} from "../../hooks/AuthProvider";
import {Alert, Badge, Button, Chip, Divider, Stack, Tab, Tabs} from "@mui/material";
import Box from "@mui/material/Box";
import PropTypes from "prop-types";
import Typography from "@mui/material/Typography";
import {formatTimestamp} from "../../utils/dateTime";
import FlashcardSetComment from "../../components/flashcardsets/FlashcardSetComment";

function FlashcardSetTabPanel(props) {
    const { children, value, index, ...other } = props;

    return (
        <div
            role="tabpanel"
            hidden={value !== index}
            id={`simple-tabpanel-${index}`}
            aria-labelledby={`simple-tab-${index}`}
            {...other}
        >
            {value === index && <Box>{children}</Box>}
        </div>
    );
}

FlashcardSetTabPanel.propTypes = {
    children: PropTypes.node,
    index: PropTypes.number.isRequired,
    value: PropTypes.number.isRequired,
};

function a11yProps(index) {
    return {
        id: `simple-tab-${index}`,
        'aria-controls': `simple-tabpanel-${index}`,
    };
}

const FlashcardSeDisplay = () => {
    let { setId } = useParams();
    const auth = useAuth();
    const [flashcardSet, setFlashcardSets] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    async function fetchData() {
        if (auth.token) {
            const data = await getFlashcardSet(auth.token, setId);
            setFlashcardSets(data);
        }
        setIsLoading(false);
    }

    useEffect(() => {
        fetchData();
    }, [isLoading, setId]);

    const [selectedTab, setSelectedTab] = useState(0);

    const handleChange = (event, newValue) => {
        setSelectedTab(newValue);
    };

    return (!isLoading &&
        <>
            <PageTitle title={flashcardSet.name}>
                <Button size={"large"} variant={"outlined"} secondary>Comment</Button>
            </PageTitle>
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                <Tabs value={selectedTab} onChange={handleChange}>
                    <Tab label="Flashcards" {...a11yProps(0)} />
                    <Tab label="Comments" {...a11yProps(1)} />
                    <Tab label="Details" {...a11yProps(2)} />
                </Tabs>
            </Box>
            <FlashcardSetTabPanel value={selectedTab} index={0}>
                <Grid container spacing={2} sx={{ ml: 3, mr: 3, mt: 2, justifyContent: "center"}}>
                    {flashcardSet.cards?.map((_, index) => {
                        return (
                            <Grid size={{ xs: 12, sm: 5, md: 3, lg: 4 }}>
                                <FlashCard card={_}></FlashCard>
                            </Grid>
                        )
                    })}
                </Grid>
            </FlashcardSetTabPanel>
            <FlashcardSetTabPanel value={selectedTab} index={1}>
                <Grid container spacing={3} sx={{ display: 'flex', ml: 3, mr: 3, mt: 3, justifyContent: "center" }}>
                    {
                        (!flashcardSet.comments || flashcardSet.comments?.length < 1) &&
                        <Grid item size={12}>
                            <Alert variant="outlined" severity="info">
                                This flashcard set has no comments at this time
                            </Alert>
                        </Grid>
                    }
                    {
                        (flashcardSet.comments && flashcardSet.comments?.length > 0) &&
                        <Grid item size={{xs: 12, sm: 6}}>
                            <Stack divider={<Divider orientation="horizontal" flexItem/>}>
                                {flashcardSet.comments?.map((_, index) => {
                                    return (
                                        <FlashcardSetComment comment={_} />
                                    )
                                })}
                            </Stack>
                        </Grid>
                    }
                </Grid>
            </FlashcardSetTabPanel>
            <FlashcardSetTabPanel value={selectedTab} index={2}>
                <Stack divider={<Divider orientation="horizontal" flexItem/>}>
                    <Box sx={{display: "flex", justifyContent: "center", alignItems: "center"}}>
                        <Typography variant={"body2"} color={"secondary"} textAlign={"right"} sx={{p: 2, width: 1/2}}>Owner</Typography>
                        <Typography variant={"body1"} fontWeight={"bold"} sx={{p: 2, width: 1/2}}>{flashcardSet.user.username}</Typography>
                    </Box>
                    <Box sx={{display: "flex", justifyContent: "center", alignItems: "center"}}>
                        <Typography variant={"body2"} color={"secondary"} textAlign={"right"} sx={{p: 2, width: 1/2}}>Created At</Typography>
                        <Typography variant={"body1"} fontWeight={"bold"} sx={{p: 2, width: 1/2}}>{formatTimestamp(flashcardSet.created_at)}</Typography>
                    </Box>
                    <Box sx={{display: "flex", justifyContent: "center", alignItems: "center"}}>
                        <Typography variant={"body2"} color={"secondary"} textAlign={"right"} sx={{p: 2, width: 1/2}}>Updated At At</Typography>
                        <Typography variant={"body1"} fontWeight={"bold"} sx={{p: 2, width: 1/2}}>{formatTimestamp(flashcardSet.updated_at)}</Typography>
                    </Box>
                </Stack>
                <Divider orientation="horizontal" flexItem/>
            </FlashcardSetTabPanel>

        </>
    )
}
export default FlashcardSeDisplay;