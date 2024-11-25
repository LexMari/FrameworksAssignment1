import React, {useState} from "react";
import Box from '@mui/material/Box'
import Typography from "@mui/material/Typography";
import {Link} from "react-router-dom";
import IconButton from "@mui/material/IconButton";
import DeleteIcon from "@mui/icons-material/Delete";
import BookmarksIcon from "@mui/icons-material/Bookmarks";
import Grid from "@mui/material/Grid2";

const CollectionSummary = ({collection, showCurator = false, allowDelete = false, deleteCallback}) => {
    const [hoverState, setHoverState] = useState(false);

    function toggleHover() {
        setHoverState(!hoverState);
    }

    return (
        <Link to={{ pathname: `/users/${collection?.user?.id}/collections/${collection?.id}`}} style={{ textDecoration: 'none' }}>
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
                    <BookmarksIcon color={hoverState ? 'primary' : 'action'} fontSize={"large"} />
                </Grid>
                <Grid flexGrow={1}>
                    <Box display={'flex'}>
                        <Typography variant="h6" gutterBottom color={"text.primary"} sx={{ flexGrow: 1 }}>
                            {collection?.comment}
                        </Typography>
                        {
                            allowDelete &&
                            <IconButton
                                size="small"
                                title="Delete flashcard set collection"
                                onClick={e => {
                                e.preventDefault();
                                e.stopPropagation();
                                deleteCallback(collection?.id);
                            }}
                            >
                                <DeleteIcon fontSize="small"/>
                            </IconButton>
                        }
                    </Box>
                    <Box display={'flex'} sx={{ alignItems: 'end', mr: 2}}>
                        <Typography variant="subtitle1" color={"secondary.main"} sx={{ flexGrow: 1 }}>
                            {collection?.sets?.length} Flashcard Sets
                        </Typography>
                        {
                            showCurator &&
                            <Typography variant="subtitle2" color={"text.secondary"} sx={{ flexShrink: 1 }}>
                                Curated by {collection?.user?.username}
                            </Typography>

                        }
                    </Box>
                </Grid>
            </Grid>
        </Link>
    );
};

export default CollectionSummary;