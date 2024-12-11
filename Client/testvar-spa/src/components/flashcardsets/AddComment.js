import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid2";
import * as React from "react";
import {Alert, Button, Rating, TextField} from "@mui/material";
import SaveIcon from "@mui/icons-material/Save";
import CancelIcon from "@mui/icons-material/Close";
import {useState} from "react";
import Typography from "@mui/material/Typography";

const AddComment = ({saveHandler, cancelHandler, error}) => {
    const [comment, setComment] = useState( "");
    const [rating, setRating] = useState(null);
    const [wasTouched, setWasTouched] = useState( false);
    const [validComment, setValidComment] = useState(true);
    
    function validateComment() {
        setValidComment(comment.trim().length > 0);
        setWasTouched(true);
    }
    function isValid() { return validComment && wasTouched; }

    function saveCommentClick() {
        saveHandler(comment, rating);
    }

    return (
        <Box sx={{mb: 2, p: 2, border: 1, borderRadius: 2}}>
            <Grid container spacing={1} justify="space-between">
                <Grid flexGrow={1}>
                    <TextField
                        id="comment"
                        label="Comment"
                        required
                        fullWidth
                        multiline
                        maxRows={4}
                        value={comment}
                        error={!validComment}
                        helperText={validComment ? '' : 'A comment is required'}
                        placeholder={"Enter a comment"}
                        onChange={e => {
                            setComment(e.target.value);
                            validateComment();
                        }}
                        onBlur={validateComment}
                    />
                </Grid>
                <Grid flexShrink={1} sx={{alignContent: "top", ml: 1, mr: 1}}>
                    <Rating
                        name="comment-rating"
                        sx={{mt: 2}}
                        value={rating}
                        onChange={ (e, newValue) => { setRating(newValue); }}
                    />
                </Grid>
                <Grid flexShrink={1} sx={{textAlign: "center", alignContent: "center"}}>
                    <Button
                        variant={"outlined"}
                        sx={{mr:2}}
                        color={"primary"}
                        disabled={!isValid()}
                        title={"Add comment"}
                        onClick={saveCommentClick}
                        startIcon={<SaveIcon />}
                    >
                        Add comment
                    </Button>
                    <Button
                        variant={"outlined"}
                        color={"secondary"}
                        title={"Cancel changes"}
                        onClick={cancelHandler}
                        startIcon={<CancelIcon />}
                    >
                        Cancel
                    </Button>
                </Grid>
            </Grid>
            {
                error &&
                <Alert variant="outlined" severity="error" sx={{mt: 2}}>
                    <Typography component={"span"} variant={"body1"} fontWeight={"bold"}>
                        {error.message}
                    </Typography>
                </Alert>
            }
        </Box>
    );
};
export default AddComment;