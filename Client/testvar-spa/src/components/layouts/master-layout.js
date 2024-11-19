import {AppBar, Button, Container, Drawer, IconButton, List, ListItem, Toolbar, Typography} from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';
import {Link, Outlet} from "react-router-dom";
import LogoutButton from "../components/auth/LogoutButton";
import Box from "@mui/material/Box";
import {useAuth} from "../hooks/AuthProvider";
import Grid from "@mui/material/Grid2";
import { useState } from "react";

const MasterLayout = () => {
    const auth = useAuth();
    
    const [open, setOpen] = useState(true);
    
    const toggleDrawer = (newOpen) => () => {
        setOpen(newOpen);
    };
    
    return (
        <>
            <Container disableGutters maxWidth={false} >
                <AppBar position="static">
                    <Toolbar>
                        <IconButton
                            size="large"
                            edge="start"
                            color="inherit"
                            aria-label="menu"
                            sx={{ mr: 2 }}
                        >
                            <MenuIcon />
                        </IconButton>
                        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                            TestVar Flashcards
                        </Typography>
                        {
                            auth.isAuthenticated &&
                            <Grid container sx={{pl: 2, pr: 5}}>
                                <Link to={{pathname: `/sets`}}>
                                    <Button secondary>
                                        Home
                                    </Button>
                                </Link>
                                <Link to={{pathname: `/users/${auth.userId}/sets`}}>
                                    <Button secondary>
                                        My Flashcard Sets
                                    </Button>
                                </Link>
                            </Grid>
                        }

                        {
                            auth.isAuthenticated &&
                            <>
                                <Box sx={{ color: 'info.dark'}}>Welcome</Box>
                                <Box sx={{ ml: 1, mr: 4, fontWeight: 'bold', color: 'info.main'}}>{auth.username}</Box>
                            </>
                        }
                        {
                            auth.isAuthenticated &&
                            <>
                                <LogoutButton />
                            </>
                        }
                    </Toolbar>
                </AppBar>
            </Container>
            <Outlet />
        </>
    );
};

export default MasterLayout;