import { useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import PageTitle from "../common/PageTitle";
import * as React from "react";
import Grid from "@mui/material/Grid2";
import FlashCard from "./FlashCard";
import {getUser} from "../../services/AuthService";
import {getFlashcardSet} from "../../api/FlashcardSetApi";

const FlashcardSeDisplay = () => {
    let { setId } = useParams();
    const [flashcardSet, setFlashcardSets] = useState([]);
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [user, setUser] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    async function fetchData() {
        const user = await getUser();
        const accessToken = user?.access_token;

        setUser(user);

        if (accessToken) {
            setIsAuthenticated(true);
            const data = await getFlashcardSet(accessToken, setId);
            setFlashcardSets(data);
        }

        setIsLoading(false);
    }

    useEffect(() => {
        fetchData();
    }, [isLoading, setId]);

    return (
        <>
            <PageTitle title={flashcardSet.name} sx={{bgcolor: 'primary.dark'}}></PageTitle>
            <Grid container spacing={2} sx={{ ml: 3, mr: 3, mt: 2}}>
                {flashcardSet.cards?.map((_, index) => {
                    return (
                        <Grid size={{ xs: 12, sm: 5, md: 3, lg: 4 }}>
                            <FlashCard card={_}></FlashCard>
                        </Grid>
                    )
                })}

            </Grid>
        </>
    )
}
export default FlashcardSeDisplay;