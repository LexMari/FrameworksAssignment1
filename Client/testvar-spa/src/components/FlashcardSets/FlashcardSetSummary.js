import React, {useState} from "react";
import Box from '@mui/material/Box'
import Typography from "@mui/material/Typography";
import {Link} from "react-router-dom";

const FlashcardSetSummary = ({setId, name, count}) => {

    const [hoverState, setHoverState] = useState(false);
    function toggleHover() {
        setHoverState(!hoverState);
    }

    return (
        <Link to={{ pathname: `/sets/${setId}`}} style={{ textDecoration: 'none' }}>
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
                <Typography variant="body1" gutterBottom color={"textPrimary"}>
                    {name}
                </Typography>
                <Typography variant="subtitle1" color={"textSecondary"}>
                    {count} Questions
                </Typography>
            </Box>
        </Link>
    );
};

export default FlashcardSetSummary;