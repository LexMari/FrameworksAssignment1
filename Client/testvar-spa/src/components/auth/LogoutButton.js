import React from "react";
import Button from "@mui/material/Button";
import { logout } from '../../services/AuthService'

const LogoutButton = () => {
    return (
        <Button color="inherit" onClick={() => logout()} role={"button"}>
            Log Out
        </Button>
    );
};

export default LogoutButton;