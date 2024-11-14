import {AppBar, Container, IconButton, Toolbar, Typography} from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';
import {Outlet} from "react-router-dom";

const MasterLayout = () => {
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
                            TestVar
                        </Typography>
                    </Toolbar>
                </AppBar>
            </Container>
            <Outlet />
        </>
    );
};
export default MasterLayout;