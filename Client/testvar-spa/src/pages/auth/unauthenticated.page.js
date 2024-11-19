import { sendOAuthRequest } from '../../services/AuthService';
import {Link, Navigate} from "react-router-dom";
import Typography from "@mui/material/Typography";
import Grid from "@mui/material/Grid2";
import Box from "@mui/material/Box";
import {Button, Divider} from "@mui/material";
import { useAuth } from "../../hooks/AuthProvider";

function UnAuthenticated() {
    const auth = useAuth();
    
    if (auth.isAuthenticated) {
        return <Navigate to='/sets' replace />;
    }

    return (
        <Grid
            container
            spacing={0}
            direction="column"
            alignItems="center"
            sx={{ minHeight: '100vh', mt: 3 }}
        >
            <Grid item offset={{sm: 0, md: 1}} size={{sm: 12, md: 6, xl: 4 }} p={1}>
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
                        Welcome to TestVar Flashcards
                    </Typography>
                    <Divider></Divider>
                    <Typography
                        variant="body2"
                        sx={{
                            textAlign: "center",
                            mb: 2, mt: 2
                        }}>
                        You must be a member to use this site so please login or sign-up for an account
                    </Typography>
                    <Button
                        variant="outlined"
                        size="large"
                        onClick={sendOAuthRequest}
                        sx={{m: 2, display: 'inline-flex'}}
                    >
                        Login
                    </Button>
                    <Link to="#" style={{ textDecoration: 'none' }}>
                        <Typography
                            variant="subtitle1"
                            sx={{
                                textAlign: "center",
                                color: 'success.main',
                                pb: 2
                            }}
                        >
                            or click here to register
                        </Typography>
                    </Link>
                </Box>

            </Grid>
        </Grid>
    )
}

export default UnAuthenticated;