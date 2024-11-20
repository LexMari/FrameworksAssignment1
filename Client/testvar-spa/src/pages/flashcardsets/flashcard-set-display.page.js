import {useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import PageTitle from "../../components/common/PageTitle";
import * as React from "react";
import Grid from "@mui/material/Grid2";
import FlashCard from "../../components/flashcardsets/FlashCard";
import {getFlashcardSet} from "../../api/FlashcardSetApi";
import {useAuth} from "../../hooks/AuthProvider";
import {Button} from "@mui/material";

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

    return (
        <>
            <PageTitle title={flashcardSet.name}>
                <Button size={"large"} variant={"outlined"} secondary>Comment</Button>
            </PageTitle>
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