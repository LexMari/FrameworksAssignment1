import IconButton from "@mui/material/IconButton";
import MenuIcon from "@mui/icons-material/Menu";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import * as React from "react";
import {useAuth} from "../../hooks/AuthProvider";
import LogoutButton from "../auth/LogoutButton";

const HeaderToolbar = ({handleDrawerOpen, open}) => {
    const auth = useAuth();

    return (
        <Toolbar>
            <IconButton
                color="inherit"
                aria-label="open drawer"
                onClick={handleDrawerOpen}
                edge="start"
                sx={[
                    {
                        mr: 2,
                    },
                    open && { display: 'none' },
                ]}
            >
                <MenuIcon />
            </IconButton>
            <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                TestVar Flashcards
            </Typography>
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
    );
}

export default HeaderToolbar;