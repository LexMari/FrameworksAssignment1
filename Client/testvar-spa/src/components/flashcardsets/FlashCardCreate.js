import React, {useState} from "react";
import Box from '@mui/material/Box'
import Typography from "@mui/material/Typography";
import {MenuItem, TextField} from "@mui/material";
import IconButton from "@mui/material/IconButton";
import SaveIcon from '@mui/icons-material/Save';
import CancelIcon from '@mui/icons-material/Close';
import Grid from "@mui/material/Grid2";

const FlashCardCreate = ({addCardHandler, cancelAddHandler}) => {
    
    const [card, setCard] = useState( {question: "", answer: "", difficulty: "Medium"});
    const [validQuestion, setValidQuestion] = useState(true);
    const [validAnswer, setValidAnswer] = useState(true);
    const difficultyList = ["Easy", "Medium", "Hard"];

    function validateQuestion() { setValidQuestion(card.question && card.question.length > 0); }
    function validateAnswer() { setValidAnswer (card.answer && card.answer.length > 0); }
    function isValid() { return validQuestion && validAnswer; }


    function handleSaveClick() {
        addCardHandler(card);
    }

    function handleCancelClick() {
        cancelAddHandler(card);
    }

    return (
        <Box sx={{
            width: "100%",
            border: 1,
            borderRadius: 2,
            p: 2,
            mb: 3
        }}>
            <Typography variant={"h6"} color={"primary"} sx={{mb: 2}}>Add flashcard</Typography>
            <Grid container
                  spacing={2}
                  column={{xs: 1, sm: 4}}
                  sx={{
                      width: "100%",
                      display: "flex",
                      flexDirection: 'row',
                      alignItems: "center"
                  }}>
                <Grid sx={{flexGrow: 1}}>
                    <TextField
                        id="question"
                        label="Question"
                        required
                        fullWidth
                        multiline
                        maxRows={4}
                        value={card.question}
                        error={!validQuestion}
                        helperText={validQuestion ? '' : 'A question is required'}
                        placeholder={"Enter a question"}
                        onChange={e => setCard({ ...card, question: e.target.value })}
                        onBlur={validateQuestion}
                    />
                </Grid>
                <Grid sx={{flexGrow: 1}}>
                    <TextField
                        id="answer"
                        label="Answer"
                        required
                        fullWidth
                        multiline
                        maxRows={4}
                        value={card.answer}
                        error={!validAnswer}
                        helperText={validAnswer ? '' : 'An answer is required'}
                        placeholder={"Enter the answer"}
                        onChange={e => setCard({ ...card, answer: e.target.value })}
                        onBlur={validateAnswer}
                    />
                </Grid>
                <Grid sx={{flexGrow: 1}}>
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
                </Grid>
                <Grid>
                    <IconButton
                        sx={{ml:2}}
                        disabled={!isValid()}
                        title={"Save changes"}
                        onClick={handleSaveClick}
                    >
                        <SaveIcon />
                    </IconButton>
                    <IconButton
                        title={"Cancel changes"}
                        onClick={handleCancelClick}>
                        <CancelIcon />
                    </IconButton>
                </Grid>
            </Grid>
        </Box>
    );
};

export default FlashCardCreate;