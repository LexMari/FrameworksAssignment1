import * as React from 'react';
import Grid from '@mui/material/Grid2';
import {useEffect, useState} from "react";
import SortService from "../../services/SortService";
import PageTitle from "../../components/common/PageTitle";
import FlashcardSetSummary from "../../components/flashcardsets/FlashcardSetSummary";
import {getFlashcardSets} from "../../api/FlashcardSetApi";
import {useAuth} from "../../hooks/AuthProvider";
import {Button} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import {Link} from "react-router-dom";

const FlashcardSetIndex = () => {
    const auth = useAuth();
    const [flashcardSets, setFlashcardSets] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [sort] = useState({ field: 'name', direction: 'asc' });

    async function fetchData() {
        if (auth.token) {
            const data = await getFlashcardSets(auth.token);
            const sortedData = SortService.sortFlashcardSets(data, sort);
            setFlashcardSets(sortedData);
        }
        setIsLoading(false);
    }

    useEffect(() => {
        fetchData();
    }, [isLoading, sort]);

    return (
        <>
            <PageTitle title={`Flashcard Sets`}>
                <Link to="/sets/create">
                    <Link to="/sets/create">
                        <Button variant={"outlined"} secondary startIcon={<AddIcon />} title={"Create new flashcard set"}>
                            Create Set
                        </Button>
                    </Link>
                </Link>
            </PageTitle>
            <Grid container maxWidth={true} spacing={3} sx={{ display: 'flex', ml: 3, mr: 3, mt: 1 }}>
                {flashcardSets.map((_, index) => {
                    return (
                        <Grid size={{ xs: 6, md: 4 }} key={index}>
                            <FlashcardSetSummary set={_} />
                        </Grid>
                    );
                })}
            </Grid>
        </>
    );
}
export default FlashcardSetIndex