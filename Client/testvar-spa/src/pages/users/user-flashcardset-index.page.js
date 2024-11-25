import * as React from 'react';
import Grid from '@mui/material/Grid2';
import {useEffect, useState} from "react";
import SortService from "../../services/SortService";
import PageTitle from "../../components/common/PageTitle";
import FlashcardSetSummary from "../../components/flashcardsets/FlashcardSetSummary";
import {getUserFlashcardSets} from "../../api/UserApi";
import {useAuth} from "../../hooks/AuthProvider";
import {Link, useNavigate, useParams} from "react-router-dom";
import {Button} from "@mui/material";
import Typography from "@mui/material/Typography";
import AddIcon from '@mui/icons-material/Add';

const UserFlashcardSetIndex = () => {
    let { userId } = useParams();
    const auth = useAuth();
    const navigate = useNavigate();
    const [flashcardSets, setFlashcardSets] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [sort] = useState({ field: 'created_at', direction: 'desc' });

    async function fetchData() {
        if (auth.token) {
            const data = await getUserFlashcardSets(userId, auth.token);
            const sortedData = SortService.sortFlashcardSets(data, sort);
            setFlashcardSets(sortedData);
        }
        setIsLoading(false);
    }

    useEffect(() => {
        fetchData();
    }, [isLoading, sort]);

    function editCallback(setId) {
        const url = `/sets/${setId}/edit`
        navigate(url)
    }

    return (
        <>
            <PageTitle title={`My Flashcard Sets`}>
                <Grid container spacing={1} justifyContent="space-between">
                    <Grid size="grow">
                        <Typography variant={"h6"} color={"textSecondary"} display={"inline-flex"}>Created by</Typography>
                        <Typography variant={"h6"} color={"textPrimary"} fontWeight={"bold"} display={"inline-flex" } sx={{ml: 1, mr:3}}>{auth.username}</Typography>
                    </Grid>
                    <Grid size="auto"  textAlign='right'>
                        <Link to="/sets/create">
                            <Button variant={"outlined"} secondary startIcon={<AddIcon />} title={"Create new flashcard set"}>
                                Create Set
                            </Button>
                        </Link>
                    </Grid>
                </Grid>
            </PageTitle>
            <Grid container maxWidth={true} spacing={3} sx={{ display: 'flex', ml: 3, mr: 3, mt: 1}}>
                {flashcardSets.map((_, index) => {
                    return (
                        <Grid size={{ xs: 6, md: 4 }} key={index}>
                            <FlashcardSetSummary set={_} allowEdit={true} editCallback={editCallback} />
                        </Grid>
                    );
                })}
            </Grid>
        </>
    );
}
export default UserFlashcardSetIndex