import { useParams} from "react-router-dom";
import {useEffect, useState} from "react";
import PageTitle from "../common/PageTitle";
import * as React from "react";
import {Stack, Typography} from "@mui/material";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid2";
import FlashcardSetSummary from "./FlashcardSetSummary";

const FlashcardSetDisplay = () => {
    let { setId } = useParams();
    const [flashcardSet, setFlashcardSets] = useState([]);

    useEffect(() => {
        let url =  `https://localhost:7233/api/sets/${setId}`;
        console.log(url);
        fetch(url)
            .then((response) => response.json())
            .then((result) => {
                setFlashcardSets(result);
                console.log(result);
            })
            .catch((err) => {
                console.log(err.message);
            });
    }, [setId]);

    function getBorderColour(difficulty) {
        if (difficulty === "hard")
            return "error.main";
        if (difficulty === "medium")
            return "warning.main";
        return "success.main";
    }

    return (
        <>
            <PageTitle title={flashcardSet.name} sx={{bgcolor: 'primary.dark', color: 'primary.contrastText'}}></PageTitle>
            <Grid container maxWidth={true} spacing={3} size={{ xs: 6, md: 4 }}
                  sx={{ display: 'flex', ml: 3, mr: 3, mt: 1, justifyContent: 'center' }}>
                {flashcardSet.cards?.map((card) => {
                    return (
                        <Box sx={{ alignItems: 'center',
                            mt: 1, p: 1,
                            borderRadius: 1,
                            border: 2,
                            borderColor: getBorderColour(card.difficulty.toLowerCase()),
                            boxShadow: 1,
                            bgColor: 'secondary.main'
                        }}>
                            <Typography variant="body1" gutterBottom color={"textPrimary"}>
                                {card.question}
                            </Typography>
                            <Typography variant="subtitle1" color={"textSecondary"}>
                                {card.answer}
                            </Typography>
                        </Box>
                    );
                })}
            </Grid>
        </>
    )
}
export default FlashcardSetDisplay;