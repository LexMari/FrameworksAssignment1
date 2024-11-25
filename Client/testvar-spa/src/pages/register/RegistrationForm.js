import RegistrationFrame from "./RegistrationFrame";
import Grid from "@mui/material/Grid2";
import {Alert, Button, TextField} from "@mui/material";
import Box from "@mui/material/Box";
import {Link} from "react-router-dom";
import React from "react";
import Typography from "@mui/material/Typography";

function RegistrationForm({user, onSubmit, onChangeUsername, onChangePassword, error}) {
    return  (
        <RegistrationFrame>
            {
                error &&
                <Alert variant="outlined" severity="error" sx={{m: 2}}>
                    <Typography component={"span"} variant={"body1"} fontWeight={"bold"}>
                        {error.message}
                    </Typography>
                </Alert>
            }
            <Grid container component="form" columns={1} spacing={2} sx={{ml: 2, mr: 2, mb: 2}} onSubmit={onSubmit}>
                <Grid size={{xs: 12, sm: 6}}>
                    <TextField
                        required
                        fullWidth
                        id="usernameInput"
                        label="Username"
                        name="user[username]"
                        value={user.username}
                        onChange={onChangeUsername}
                        placeholder="Enter a username"
                    />
                </Grid>
                <Grid size={{xs: 12, sm: 6}}>
                    <TextField
                        required
                        fullWidth
                        name="user[password]"
                        value={user.password}
                        onChange={onChangePassword}
                        label="Password"
                        type="password"
                        placeholder="Enter a password"
                    />
                </Grid>
                <Grid size={{xs: 12, sm: 6}}>
                    <Box sx={{ display: "flex", justifyContent:"space-between"}}>
                        <Button variant={"outlined"} type={"submit"}>Register</Button>
                        <Link to="/">
                            <Button variant={"text"} color={"secondary"} type={"submit"}>Cancel</Button>
                        </Link>
                    </Box>
                </Grid>
            </Grid>
        </RegistrationFrame>
    );
}

export default RegistrationForm;