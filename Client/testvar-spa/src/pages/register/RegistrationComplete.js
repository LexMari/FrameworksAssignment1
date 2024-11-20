import RegistrationFrame from "./RegistrationFrame";
import {Alert, Button} from "@mui/material";
import {sendOAuthRequest} from "../../services/AuthService";
import React from "react";

function RegistrationComplete() {
    return (
        <RegistrationFrame>
            <Alert variant="outlined" severity="success" sx={{ml: 2, mr:2, mt: 2}}>
                Registration for successful.
            </Alert>
            <Button
                variant="outlined"
                size="large"
                onClick={sendOAuthRequest}
                sx={{m: 2, display: 'inline-flex'}}
            >
                Click to Login
            </Button>
        </RegistrationFrame>
    );
}

export default RegistrationComplete