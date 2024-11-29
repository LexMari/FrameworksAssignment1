import React, {useState} from "react";
import Box from '@mui/material/Box'
import Typography from "@mui/material/Typography";
import {Link} from "react-router-dom";
import IconButton from "@mui/material/IconButton";
import EditIcon from "@mui/icons-material/Edit";
import Grid from "@mui/material/Grid2";
import QuizIcon from "@mui/icons-material/Quiz";
import BookmarkRemoveIcon from '@mui/icons-material/BookmarkRemove';

const FlashcardSetSummary = ({set, allowEdit = false, editCallback, allowRemove = false, removeCallback}) => {
    const [hoverState, setHoverState] = useState(false);

    function toggleHover() {
        setHoverState(!hoverState);
    }

    return (
        <Link 
            to={{ pathname: `/sets/${set.id}`}}
            role={"link"}
            style={{ textDecoration: 'none' }}>
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
                <Grid flexShrink={1} sx={{p:2}}>
                    <QuizIcon color={hoverState ? 'primary' : 'action'} fontSize={"large"} />
                </Grid>
                <Grid flexGrow={1}>
                    <Box display={'flex'}>
                        <Typography variant="h6" role={"heading"} gutterBottom color={"text.primary"} sx={{ flexGrow: 1 }}>
                            {set.name}
                        </Typography>
                        {
                            allowEdit &&
                            <IconButton
                                size="small"
                                role={"button"}
                                title="Edit flashcard set"
                                onClick={e => {
                                    e.preventDefault();
                                    e.stopPropagation();
                                    editCallback(set.id);
                                }}
                            >
                                <EditIcon fontSize="small"/>
                            </IconButton>
                        }
                        {
                            allowRemove && 
                            <IconButton 
                                size="small"
                                color={"info"}
                                role={"button"}
                                title="Remove from collection"
                                onClick={e => {
                                    e.preventDefault();
                                    e.stopPropagation();
                                    removeCallback(set.id);
                                }}
                            >
                              <BookmarkRemoveIcon fontSize="small"/>  
                            </IconButton>
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