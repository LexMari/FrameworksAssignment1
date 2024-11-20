import Grid from "@mui/material/Grid2";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import {Divider} from "@mui/material";
import React from "react";

function RegistrationFrame({children}) {
    return (
        <Grid
            container
            spacing={0}
            direction="column"
            alignItems="center"
            sx={{ minHeight: '100vh', mt: 3}}
        >
            <Box offset={{sm: 0, md: 1}} size={{sm: 12, md: 6, xl: 4 }} p={1}>
                <Box offset={{sm: 0, md: 1}} size={{sm: 12, md: 6, xl: 4 }} p={1}>
                    <Box sx={{
                        display: "flex",
                        flexDirection: 'column',
                        alignContent: 'center',
                        border: 1,
                        borderRadius: 2,
                    }}>
                        <Typography
                            variant="h3"
                                sx={{
                                    textAlign: "center",
                                    color: "info.main",
                                    mt: 2, mb: 2
                                }}>
                            Create Account
                        </Typography>
                        <Divider></Divider>
                        <Typography
                            variants="subtitle1"
                                sx={{
                                    textAlign: "center",
                                    m: 2
                                }}>
                            Create an account on TestVar Flashcards to allow you to create and collect flashcard sets
                        </Typography>
                        {children}
                    </Box>
                </Box>
            </Box>
        </Grid>
    );
}

export default RegistrationFrame;