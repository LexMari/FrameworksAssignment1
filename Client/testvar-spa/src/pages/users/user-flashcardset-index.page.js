import * as React from 'react';
import Grid from '@mui/material/Grid2';
import {useEffect, useState} from "react";
import SortService from "../../services/SortService";
import PageTitle from "../../components/common/PageTitle";
import FlashcardSetSummary from "../../components/FlashcardSets/FlashcardSetSummary";
import {getUserFlashcardSets} from "../../api/UserApi";
import {useAuth} from "../../hooks/AuthProvider";
import {useParams} from "react-router-dom";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import {Button} from "@mui/material";

const UserFlashcardSetIndex = () => {
    
    let { userId } = useParams();
    const auth = useAuth();
    const [flashcardSets, setFlashcardSets] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [sort, setSort] = useState({ field: 'created_at', direction: 'desc' });
    
    async function fetchData() {
        if (auth.token) {
            const data = await getUserFlashcardSets(userId, auth.token);
            const sortedData = SortService.sortFlashcardSets(data, sort);
            setFlashcardSets(data);
        }
        setIsLoading(false);
    }
    
    useEffect(() => {
        fetchData();
    }, [isLoading, sort]);
    
    return (
        <>
            <PageTitle title={`Flashcard Sets created by ${auth.username}`}>
                <Button size={"large"} variant={"outlined"} color={'text.main'}>
                    Create Flashcard Set
                </Button>
            </PageTitle>
            <Grid container maxWidth={true} spacing={3} sx={{ display: 'flex', ml: 3, mr: 3, mt: 1 }}>
                {flashcardSets.map((_, index) => {
                    return (
                        <Grid item size={{ xs: 6, md: 4 }} key={index}>
                            <FlashcardSetSummary set={_} />
                        </Grid>
                    );
                })}
            </Grid>
        </>
    );
}

export default UserFlashcardSetIndex