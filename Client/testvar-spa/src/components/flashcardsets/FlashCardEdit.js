import React, {useState} from "react";
import Box from '@mui/material/Box'
import {Divider, MenuItem, TextField} from "@mui/material";
import IconButton from "@mui/material/IconButton";
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Close';

const FlashCardEdit = ({target, editCardHandler, cancelEditHandler}) => {

    const [card, setCard] = useState(target);

    const [validQuestion, setValidQuestion] = useState(true);
    const [validAnswer, setValidAnswer] = useState(true);
    const difficultyList = ["Easy", "Medium", "Hard"];

    function validateQuestion() { setValidQuestion(card.question && card.question.length > 0); }
    function validateAnswer() { setValidAnswer (card.answer && card.answer.length > 0); }
    function isValid() { return validQuestion && validAnswer; }

    function getBorderColour(difficulty) {
        if (difficulty === "hard")
            return "error.main";
        if (difficulty === "medium")
            return "warning.main";
        return "success.main";
    }

    function handleSaveClick() {
        editCardHandler(card);
    }

    function handleCancelClick() {
        cancelEditHandler();
    }


    return (
        <Box sx={{
            display: "flex",
            flexDirection: 'column',
            alignContent: 'stretch',
            alignItems: 'stretch',
            border: 1,
            borderRadius: 2,
            borderColor: getBorderColour(card?.difficulty?.toLowerCase()),
            backgroundColor: ''
        }}>
            <Box sx={{
                m: 2,
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center'
            }}>
                <TextField
                    id="outlined-select-currency"
                    select
                    fullWidth
                    label="Difficulty"
                    defaultValue="Medium"
                    value={card.difficulty}
                    onChange={e => setCard({ ...card, difficulty: e.target.value })}
                >
                    {difficultyList.map((option) => (
                        <MenuItem key={option} value={option}>
                            {option}
                        </MenuItem>
                    ))}
                </TextField>
                <IconButton
                    sx={{ml:2}}
                    disabled={!isValid()}
                    title={"Save changes"}
                    onClick={handleSaveClick}
                >
                    <SaveIcon />
                </IconButton>
                <IconButton title={"Cancel changes"} onClick={handleCancelClick}>
                    <CancelIcon />
                </IconButton>
            </Box>
            <Divider></Divider>
            <Box sx={{ m: 2 }}>
                <TextField
                    id="question"
                    label="Question"
                    required
                    fullWidth
                    multiline
                    rows={2}
                    value={card.question}
                    error={!validQuestion}
                    helperText={validQuestion ? '' : 'A question is required'}
                    placeholder={"Enter a question"}
                    onChange={e => setCard({ ...card, question: e.target.value })}
                    onBlur={validateQuestion}
                />
            </Box>
            <Divider></Divider>
            <Box sx={{m: 2}} >
                <TextField
                    id="answer"
                    label="Answer"
                    required
                    fullWidth
                    multiline
                    rows={2}
                    value={card.answer}
                    error={!validAnswer}
                    helperText={validAnswer ? '' : 'An answer is required'}
                    placeholder={"Enter the answer"}
                    onChange={e => setCard({ ...card, answer: e.target.value })}
                    onBlur={validateAnswer}
                />
            </Box>
        </Box>
    );
};

export default FlashCardEdit;