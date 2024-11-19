import React, {useState} from "react";
import Box from '@mui/material/Box'
import Typography from "@mui/material/Typography";
import {Divider} from "@mui/material";

const FlashCard = ({card}) => {

    const [isRevealed, setIsRevealed] = useState(false);
    function toggleReveal() {
        console.log("click");
        setIsRevealed(!isRevealed);
    }

    function getBorderColour(diffiiculty) {
        if (diffiiculty === "hard")
            return "error.main";
        if (diffiiculty === "medium")
            return "warning.main";
        return "success.main";
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