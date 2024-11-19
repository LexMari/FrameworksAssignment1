import * as React from 'react';
import Grid from '@mui/material/Grid2';
import {useEffect, useState} from "react";
import SortService from "../../services/SortService";
import PageTitle from "../../components/common/PageTitle";
import FlashcardSetSummary from "../flashcardsets/FlashcardSetSummary";
import {getFlashcardSets} from "../../api/FlashcardSetApi";
import {useAuth} from "../../hooks/AuthProvider";

const FlashcardSetIndex = () => {
    const auth = useAuth();
    const [flashcardSets, setFlashcardSets] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [sort, setSort] = useState({ field: 'name', direction: 'asc' });

    async function fetchData() {
        if (auth.token) {
            const data = await getFlashcardSets(auth.token);
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
            <PageTitle title={"Flashcard Sets"}>
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
export default FlashcardSetIndex