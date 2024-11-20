import React, {useState} from "react";
import Box from '@mui/material/Box'
import Typography from "@mui/material/Typography";
import {Link, useNavigate} from "react-router-dom";
import IconButton from "@mui/material/IconButton";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";

const FlashcardSetSummary = ({set, allowEdit = false}) => {
    const [hoverState, setHoverState] = useState(false);

    function toggleHover() {
        setHoverState(!hoverState);
    }

    return (
        <Link to={{ pathname: `/sets/${set.id}`}} style={{ textDecoration: 'none' }}>
            <Box sx={{
                alignItems: 'center',
                mt: 1, p: 1,
                borderRadius: 1,
                border: 2,
                borderColor: hoverState ? 'info.main' : 'text.secondary',
                boxShadow: 1,
                bgColor: 'secondary.main'
            }}
                 onMouseEnter={toggleHover}
                 onMouseLeave={toggleHover}>
                <Box display={'flex'}>
                    <Typography variant="body1" gutterBottom color={"textPrimary"} sx={{ flexGrow: 1 }}>
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

                <Typography variant="subtitle1" color={"textSecondary"}>
                    {set.cards?.length} Questions
                </Typography>
            </Box>
        </Link>
    );
};

export default FlashcardSetSummary;