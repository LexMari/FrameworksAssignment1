import React, {useState} from "react";
import Box from '@mui/material/Box'
import Typography from "@mui/material/Typography";
import {Link, useNavigate} from "react-router-dom";
import IconButton from "@mui/material/IconButton";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import Grid from "@mui/material/Grid";
import QuizIcon from "@mui/icons-material/Quiz";

const FlashcardSetSummary = ({set, allowEdit = false}) => {
    const [hoverState, setHoverState] = useState(false);

    function toggleHover() {
        setHoverState(!hoverState);
    }

    return (
        <Link to={{ pathname: `/sets/${set.id}`}} style={{ textDecoration: 'none' }}>
            <Grid container
                  sx={{
                      alignItems: 'center',
                      mt: 1, p: 1,
                      borderRadius: 1,
                      border: 2,
                      borderColor: hoverState ? 'primary.main' : 'text.secondary',
                      boxShadow: 1,
                      bgColor: 'secondary.main'
                  }}
                  onMouseEnter={toggleHover}
                  onMouseLeave={toggleHover}
            >
                <Grid item flexShrink={1} sx={{p:2}}>
                    <QuizIcon color={hoverState ? 'primary' : 'action'} fontSize={"large"} />
                </Grid>
                <Grid item flexGrow={1}>
                    <Box display={'flex'}>
                        <Typography variant="h6" gutterBottom color={"text.primary"} sx={{ flexGrow: 1 }}>
                            {set.name}
                        </Typography>
                        {
                            allowEdit &&
                            <Link to={{pathname: `/sets/${set.id}/edit`}}>
                                <IconButton
                                    size="small"
                                    title="Edit flashcard set"
                                >
                                    <EditIcon fontSize="small"/>
                                </IconButton>
                            </Link>
                        }
                    </Box>

                    <Typography variant="subtitle1" color={"secondary.main"}>
                        {set.cards?.length} Questions
                    </Typography>
                </Grid>
            </Grid>
        </Link>
    );
};

export default FlashcardSetSummary;