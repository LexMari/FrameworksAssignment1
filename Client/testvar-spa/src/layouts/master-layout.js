import {AppBar, Container, IconButton, Toolbar, Typography} from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';
import {Outlet} from "react-router-dom";
import LogoutButton from "../components/auth/LogoutButton";
import Box from "@mui/material/Box";

const MasterLayout = (isAuthenticated, user) => {
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
                            isAuthenticated &&
                            <>
                                <Box sx={{ color: 'info.dark'}}>Welcome</Box>
                                <Box sx={{ ml: 1, mr: 4, fontWeight: 'bold', color: 'info.main'}}></Box>
                            </>
                        }
                        {
                            isAuthenticated &&
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