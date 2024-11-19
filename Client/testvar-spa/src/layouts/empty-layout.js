import {AppBar, Container, IconButton, Toolbar, Typography} from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';
import {Outlet} from "react-router-dom";

const EmptyLayout = () => {
    return (
        <>
            <Container disableGutters maxWidth={false} >
                <Outlet />
            </Container>
        </>
    );
};
export default EmptyLayout;