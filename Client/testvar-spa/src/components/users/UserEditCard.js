import React, {useState} from "react";
import Grid from "@mui/material/Grid2";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import IconButton from "@mui/material/IconButton";
import {Divider, MenuItem, TextField} from "@mui/material";
import SaveIcon from "@mui/icons-material/Save";
import CancelIcon from "@mui/icons-material/Close";

const UserEditCard = ({target, cancelCallback = {}, saveCallback = {}, error = false}) => {
    const [user, setUser] = useState(target);
    const roleList = ["User", "Administrator"];

    function isValid() { return user.username && user.username.length > 0; }

    function handleSaveClick() {
        saveCallback(user);
    }

    function handleCancelClick() {
        cancelCallback();
    }

    return (
        <Grid
            sx={{
                alignItems: 'center',
                mt: 1,
                borderRadius: 1,
                border: 2,
                borderColor: error ? 'error.main' : 'success.main' ,
                boxShadow: 1,
                bgColor: 'secondary.main'
            }}
        >
            <Box sx={{
                display: 'flex',
                ml: 2, mr: 2,
                justifyContent: 'space-between',
                alignItems: 'center'
            }}>
                <Typography
                    id="user-header"
                    color={"textSecondary"}
                    variant={"subtitle1"}
                >
                    Edit User
                </Typography>
                <Box>
                    <IconButton
                        sx={{ml:2}}
                        disabled={!isValid()}
                        title={"Save changes"}
                        onClick={handleSaveClick}
                    >
                        <SaveIcon />
                    </IconButton>
                    <IconButton title={"Cancel changes"} onClick={handleCancelClick}>
                        <CancelIcon />
                    </IconButton>
                </Box>
            </Box>
            <Divider></Divider>
            <Box sx={{ m: 2 }}>
                <TextField
                    id="username"
                    label="Username"
                    required
                    fullWidth
                    value={user.username}
                    error={!isValid()}
                    helperText={isValid() ? '' : 'A question is required'}
                    placeholder={"Enter a username"}
                    onChange={e => setUser({ ...user, username: e.target.value })}
                />
            </Box>
            <Divider></Divider>
            <Box sx={{ m: 2 }}>
                <TextField
                    id="user-role"
                    select
                    fullWidth
                    label="Role"
                    defaultValue="User"
                    value={user.admin ? "Administrator" : "User" }
                    onChange={e => setUser({ ...user, admin: e.target.value === "Administrator" })}
                >
                    {roleList.map((option) => (
                        <MenuItem key={option} value={option} >
                            {option}
                        </MenuItem>
                    ))}
                </TextField>
            </Box>
        </Grid>
    );

}
export default  UserEditCard;