import React from "react";
import Container from "@mui/material/Container";
import Box from '@mui/material/Box'
import Typography from "@mui/material/Typography";

const PageTitle = ({title, sx, children}) => {
    return (
        <Container maxWidth={false}>
            <Box sx={{
                display: 'flex',
                alignContent: 'flex-end',
                mb: 1,
                ...sx
            }}>
                <Typography variant="h3" component="div" sx={{ flexGrow: 1, fontWeight: 'bold' }}>{title}</Typography>
                <Box sx={{flexShrink: 1, alignContent: 'flex-end'}}>
                    {children}
                </Box>
            </Box>
        </Container>
    );
};

export default PageTitle;