import {useState} from "react";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid2";
import {Alert, Button, TextField} from "@mui/material";
import SaveIcon from "@mui/icons-material/Save";
import CancelIcon from "@mui/icons-material/Close";
import Typography from "@mui/material/Typography";
import * as React from "react";

const CollectionEdit = ({collection, saveHandler, cancelHandler, error}) => {
    const [comment, setComment] = useState( collection.comment);
    const [validComment, setValidComment] = useState(true);
    function validateComment() {
        setValidComment(comment.trim().length > 0);
    }
    function isValid() { return validComment; }

    function saveCommentClick() {
        saveHandler(comment);
    }

    return (
        <Grid container size={12} spacing={3} sx={{border: 1, borderRadius: 2, p:2, display: 'flex', justifyContent: "space-between"}}>
            <Grid sx={{flexGrow: 1}}>
                <TextField
                    id="collection-name"
                    label="Name"
                    required
                    fullWidth
                    multiline
                    maxRows={4}
                    value={comment}
                    error={!validComment}
                    helperText={validComment ? '' : 'A collection name is required'}
                    placeholder={"Enter the name for the collection"}
                    onChange={e => {
                        setComment(e.target.value);
                        validateComment();
                    }}
                    onBlur={validateComment}
                />
            </Grid>
            <Grid sx={{justifyContent: "center", alignContent: "center"}}>
                <Button
                    variant={"outlined"}
                    sx={{mr:2}}
                    color={"primary"}
                    disabled={!isValid()}
                    title={"Save changes"}
                    onClick={saveCommentClick}
                    startIcon={<SaveIcon />}
                >
                    Save
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
            {
                error &&
                <Alert variant="outlined" severity="error" sx={{mt: 2}}>
                    <Typography component={"span"} variant={"body1"} fontWeight={"bold"}>
                        {error.message}
                    </Typography>
                </Alert>
            }
        </Grid>
    );
};
export default CollectionEdit;