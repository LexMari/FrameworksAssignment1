import Grid from "@mui/material/Grid2";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import * as React from "react";
import {formatTimestamp} from "../../utils/dateTime";

const FlashcardSetComment = ({comment}) => {
    return (
        <Box sx={{mb: 2, p: 2, border: 1, borderRadius: 2, borderColor: 'primary.main'}}>
            <Grid container spacing={1}>
                <Grid size={8}>
                    <Typography variant={"subtitle2"} color={"primary"}>Comment</Typography>
                </Grid>
                <Grid size={2}>
                    <Typography variant={"subtitle2"} color={"primary"}>Author</Typography>
                </Grid>
                <Grid size={2}>
                    <Typography variant={"subtitle2"} color={"primary"}>Comment At</Typography>
                </Grid>
                <Grid size={8}>
                    <Typography variant={"body1"}>{comment.comment}</Typography>
                </Grid>
                <Grid size={2}>
                    <Typography variant={"body1"}>{comment.author.username}</Typography>
                </Grid>
                <Grid size={2}>
                    <Typography variant={"body1"}>{formatTimestamp(comment.created_at)}</Typography>
                </Grid>
            </Grid>
        </Box>
    )
}
export default FlashcardSetComment;