import {AppBar, Container, IconButton, Toolbar, Typography} from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';
import {Outlet} from "react-router-dom";
import CssBaseline from "@mui/material/CssBaseline";
import * as React from "react";

const EmptyLayout = () => {
    return (
        <>
            <CssBaseline />
            <Container disableGutters maxWidth={false} >
                <Outlet />
            </Container>
        </>
    );
};
export default EmptyLayout;