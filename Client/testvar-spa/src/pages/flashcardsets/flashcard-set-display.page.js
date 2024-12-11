import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import PageTitle from "../../components/common/PageTitle";
import * as React from "react";
import Grid from "@mui/material/Grid2";
import FlashCard from "../../components/flashcardsets/FlashCard";
import {addFlashcardSetComment, getFlashcardSet} from "../../api/FlashcardSetApi";
import {useAuth} from "../../hooks/AuthProvider";
import {Alert, Button, Divider, Rating, Stack, Tab, Tabs} from "@mui/material";
import Box from "@mui/material/Box";
import PropTypes from "prop-types";
import FlashcardSetComment from "../../components/flashcardsets/FlashcardSetComment";
import FlashcardSetInformation from "../../components/flashcardsets/FlashcardSetInformation";
import AddCommentIcon from "@mui/icons-material/AddComment";
import AddComment from "../../components/flashcardsets/AddComment";
import SaveIcon from "@mui/icons-material/Save";
import AddToCollection from "../../components/collections/AddToCollection";

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
    const [flashcardSet, setFlashcardSet] = useState([]);
    const [commentOpen, setCommentOpen] = useState(false);
    const [commentError, setCommentError] = useState();
    const [collectionOpen, setCollectionOpen] = useState(false);
    const [isLoading, setIsLoading] = useState(true);

    async function fetchData() {
        if (auth.token) {
            const data = await getFlashcardSet(auth.token, setId);
            setFlashcardSet(data);
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

    function addComment() {
        setCommentError();
        setCommentOpen(true);
        setSelectedTab(1);
    }
    function addCommentCancel() {
        setCommentError();
        setCommentOpen(false);
    }

    function addCommentSave(comment, rating) {
        addFlashcardSetComment(auth.token, flashcardSet.id, comment, rating).then((result) => {
            let newComments = flashcardSet.comments ?? [];
            newComments = [...newComments,  {
                comment: result.comment,
                rating: result.rating,
                created_at: result.created_at,
                author: result.author
            }];
            setFlashcardSet({...flashcardSet, comments: newComments});
            setCommentOpen(false);
        }).catch((e) => {
            setCommentError(e);
        });
    }

    function addToCollection() {
        setCollectionOpen(true);
    }

    function addToCollectionCancel() {
        setCollectionOpen(false);
    }

    return (!isLoading &&
        <>
            <PageTitle title={flashcardSet.name}>
                <Box display="flex" justifyContent="space-between" alignItems="center">
                    <Rating value={flashcardSet.rating} precision={0.1} size="large" readOnly sx={{mr: 3}} />
                { auth.userCollections?.length > 0 &&
                    <Button
                        variant={"outlined"}
                        startIcon={<SaveIcon />}
                        title={"Create the flashcard set"}
                        sx={{mr: 2}}
                        disabled={commentOpen || collectionOpen }
                        onClick={addToCollection}
                    >
                        Add to collection
                    </Button>
                }
                <Button
                    size={"large"}
                    variant={"outlined"}
                    disabled={commentOpen || collectionOpen }
                    startIcon={<AddCommentIcon />}
                    onClick={addComment}
                >
                    Add comment
                </Button>
                </Box>
            </PageTitle>
            {
                collectionOpen &&
                <AddToCollection
                    setId={flashcardSet.id}
                    cancelHandler={addToCollectionCancel}
                />
            }
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                <Tabs value={selectedTab} onChange={handleChange}>
                    <Tab label="Flashcards" {...a11yProps(0)} />
                    <Tab label="Comments" {...a11yProps(1)} />
                    <Tab label="Information" {...a11yProps(2)} />
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
                        commentOpen &&
                        <Grid size={{lg:12,  xl: 8}} justifyContent="center" alignItems="center">
                            <AddComment
                                saveHandler={addCommentSave}
                                cancelHandler={addCommentCancel}
                                error={commentError}
                            />
                        </Grid>
                    }
                    {
                        (!flashcardSet.comments || flashcardSet.comments?.length < 1) &&
                        <Grid size={12}>
                            <Alert variant="outlined" severity="info">
                                This flashcard set has no comments at this time
                            </Alert>
                        </Grid>
                    }
                    {
                        (flashcardSet.comments && flashcardSet.comments?.length > 0) &&
                        <Grid size={{lg:12,  xl: 8}}>
                            <Stack divider={<Divider orientation="horizontal" flexItem/>}>
                                {flashcardSet.comments?.sort((a,b) => a.created_at > b.created_at ? -1 : 1)
                                    .map((_, index) => {
                                        return (
                                            <FlashcardSetComment comment={_} key={`comment-${index}`} />
                                        )
                                    })}
                            </Stack>
                        </Grid>
                    }
                </Grid>
            </FlashcardSetTabPanel>
            <FlashcardSetTabPanel value={selectedTab} index={2}>
                <FlashcardSetInformation set={flashcardSet} />
            </FlashcardSetTabPanel>

        </>
    )
}
export default FlashcardSeDisplay;