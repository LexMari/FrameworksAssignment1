import React from "react";
import Container from "@mui/material/Container";
import Box from '@mui/material/Box'
import Typography from "@mui/material/Typography";

const PageTitle = ({title, sx, children}) => {
    return (
        <Container maxWidth={false}>
            <Box sx={{
                display: 'flex',
                alignItems: 'center',
                mt: 2, p: 2,
                borderRadius: 1,
                border: 1,
                borderColor: 'text.primary',
                bgcolor: 'success.main',
                ...sx
            }}>
                <Typography variant="h5" component="div" sx={{ flexGrow: 1 }} >{title}</Typography>
                <Box sx={{flexShrink: 1}}>
                    {children}
                </Box>
            </Box>
        </Container>
    );
};

export default PageTitle;