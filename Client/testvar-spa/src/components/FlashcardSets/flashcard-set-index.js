import * as React from 'react';
import Grid from '@mui/material/Grid2';

import {useEffect, useState} from "react";
import SortService from "../../services/SortService";
import PageTitle from "../common/PageTitle";
import FlashcardSetSummary from "./FlashcardSetSummary";

const FlashcardSetIndex = () => {

    const [flashcardSets, setFlashcardSets] = useState([]);
    const [sort, setSort] = useState({ field: 'name', direction: 'asc' });

    useEffect(() => {
        const apiCollection =  "https://localhost:7233/api/sets";
        fetch(apiCollection)
            .then((response) => response.json())
            .then((result) => {
                const sortedData = SortService.sortFlashcardSets(result, sort);
                console.log(sortedData);
                setFlashcardSets(result);
            })
            .catch((err) => {
                console.log(err.message);
            });
    }, [sort]);

    return (
        <>
            <PageTitle title={"Flashcard Sets"}>
            </PageTitle>
            <Grid container maxWidth={true} spacing={3} sx={{ display: 'flex', ml: 3, mr: 3, mt: 1 }}>
                {flashcardSets.map((set) => {
                    return (
                        <Grid item size={{ xs: 6, md: 4 }}>
                            <FlashcardSetSummary name={set.name} count={set.cards.length} />
                        </Grid>
                    );
                })}
            </Grid>
        </>
    );
}
export default FlashcardSetIndex