import * as React from 'react';
import Grid from '@mui/material/Grid2';

import {useEffect, useState} from "react";
import SortService from "../../services/SortService";
import PageTitle from "../common/PageTitle";
import FlashcardSetSummary from "./FlashcardSetSummary";
import {getUser} from "../../services/AuthService";
import {getFlashcardSets} from "../../api/FlashcardSetApi";

const FlashcardSetIndex = () => {
    const [flashcardSets, setFlashcardSets] = useState([]);
    const [user, setUser] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [sort, setSort] = useState({ field: 'name', direction: 'asc' });


    async function fetchData() {
        const user = await getUser();
        const accessToken = user?.access_token;

        setUser(user);

        if (accessToken) {
            const data = await getFlashcardSets(accessToken);
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
                {flashcardSets.map((set) => {
                    return (
                        <Grid item size={{ xs: 6, md: 4 }} key={set.id}>
                            <FlashcardSetSummary setId={set.id} name={set.name} count={set.cards.length} />
                        </Grid>
                    );
                })}
            </Grid>
        </>
    );
}
export default FlashcardSetIndex