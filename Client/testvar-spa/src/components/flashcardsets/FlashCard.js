import React, {useState} from "react";
import Box from '@mui/material/Box'
import Typography from "@mui/material/Typography";
import Grid from "@mui/material/Grid2";
import {Divider} from "@mui/material";
import IconButton from "@mui/material/IconButton";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import {useConfirm} from "material-ui-confirm";

const FlashCard = ({card, allowEdit = false, defaultReveal = false, editCardHandler, deleteCardHandler}) => {
    const confirm = useConfirm();
    const [isRevealed, setIsRevealed] = useState(defaultReveal);
    const [deleting, setDeleting] = useState(false);

    function toggleReveal() {
        setIsRevealed(!isRevealed);
    }

    function getBorderColour(difficulty) {
        if (difficulty === "hard")
            return "error.main";
        if (difficulty === "medium")
            return "warning.main";
        return "success.main";
    }

    function onEditClick() {
        editCardHandler(card);
    }

    const onDeleteClick = () => {
        setDeleting(true)
        confirm({ description: `Do you wish to delete this card?`})
            .then(() => {
                setDeleting(false);
                deleteCardHandler(card);
            })
            .catch(() => setDeleting(false));
    };

    return (
        <Box sx={{
            display: "flex",
            flexDirection: 'column',
            alignContent: 'stretch',
            alignItems: 'stretch',
            backgroundColor: deleting ? "text.disabled" : "",
            border: 1,
            borderRadius: 2,
            borderColor: getBorderColour(card.difficulty.toLowerCase())
        }}>
            <Grid container direction={"row"} justifyContent={'space-between'}>
                <Grid size={2}  sx={{textAlign: "left"}}></Grid>
                <Grid size={8}>
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
                <Grid size={2} sx={{textAlign: "right"}}>
                    {
                        allowEdit &&
                        <>
                            <IconButton
                                size="small"
                                title="Edit this card"
                                onClick={onEditClick}
                            >
                                <EditIcon fontSize="small"/>
                            </IconButton>
                            <IconButton
                                size="small"
                                title="Delete this card"
                                onClick={onDeleteClick}
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