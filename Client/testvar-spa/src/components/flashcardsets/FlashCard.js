import React, {useState} from "react";
import Box from '@mui/material/Box'
import Typography from "@mui/material/Typography";
import Grid from "@mui/material/Grid2";
import {Divider} from "@mui/material";
import IconButton from "@mui/material/IconButton";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";

const FlashCard = ({card, allowEdit = false, defaultReveal = false, editCardHandler, deleteCardHandler}) => {
    const [isRevealed, setIsRevealed] = useState(defaultReveal);

    function toggleReveal() {
        setIsRevealed(!isRevealed);
    }

    function getBorderColour(diffiiculty) {
        if (diffiiculty === "hard")
            return "error.main";
        if (diffiiculty === "medium")
            return "warning.main";
        return "success.main";
    }

    function handleEditClick() {
        editCardHandler(card);
    }

    function handleDeleteClick() {
        deleteCardHandler(card);
    }

    return (
        <Box sx={{
            display: "flex",
            flexDirection: 'column',
            alignContent: 'stretch',
            alignItems: 'stretch',
            border: 1,
            borderRadius: 2,
            borderColor: getBorderColour(card.difficulty.toLowerCase())
        }}>
            <Grid container direction={"row"} justifyContent={'space-between'}>
                <Grid item  size={1}  sx={{textAlign: "left"}}></Grid>
                <Grid item  size={10}>
                    <Typography
                        variant={"subtitle1"}
                        sx={{
                            p: 0.5,
                            textAlign: "center",
                            textTransform: 'capitalize',
                            color: getBorderColour(card.difficulty.toLowerCase())
                        }}
                    >
                        {card.difficulty}
                    </Typography>
                </Grid>
                <Grid item size={1} sx={{textAlign: "right"}}>
                    {
                        allowEdit &&
                        <>
                            <IconButton
                                size="small"
                                title="Edit this card"
                                onClick={handleEditClick}
                            >
                                <EditIcon fontSize="small"/>
                            </IconButton>
                            <IconButton
                                size="small"
                                title="Delete this card"
                                onClick={handleDeleteClick}
                            >
                                <DeleteIcon fontSize="small"/>
                            </IconButton>
                        </>
                    }
                </Grid>
            </Grid>

            <Divider></Divider>
            <Typography
                variant={"h6"}
                sx={{
                    p: 2,
                    textAlign: "center"
                }}
            >
                {card.question}
            </Typography>
            <Divider></Divider>
            <Box onClick={toggleReveal}>
                <Typography
                    variant={"body1"}
                    sx={{
                        p: 2,
                        textAlign: "center",
                        color: 'text.secondary',
                        visibility: isRevealed ? 'hidden' : 'visible',
                        display: isRevealed ? 'none' : 'block',
                    }}
                >
                    Click to reveal answer
                </Typography>
                <Typography
                    variant={"body1"}
                    fontWeight={"bold"}
                    sx={{
                        p: 2,
                        textAlign: 'center',
                        visibility: isRevealed ? 'visible' : 'hidden',
                        display: isRevealed ? 'block' : 'none',
                        border: 0,
                        borderBottomLeftRadius: 8,
                        borderBottomRightRadius: 8,
                        bgcolor: getBorderColour(card.difficulty.toLowerCase())
                    }}
                >
                    {card.answer}
                </Typography>
            </Box>
        </Box>
    );
};

export default FlashCard;