import React, {useState} from "react";
import Grid from "@mui/material/Grid2";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import IconButton from "@mui/material/IconButton";
import SaveIcon from "@mui/icons-material/Save";
import CancelIcon from "@mui/icons-material/Close";
import {Chip, Divider, TextField} from "@mui/material";

const SettingEditCard = ({target, cancelCallback = {}, saveCallback = {}, error = false}) => {
    const [setting, setSetting] = useState(target);

    function isValid() {
        return setting.value && setting.value.length > 0;
    }

    function handleSaveClick() {
        saveCallback(setting);
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
                <Box sx={{
                    display: 'flex',
                    justifyContent: 'left',
                    alignItems: 'center'
                }}>
                    <Typography
                        id="user-header"
                        color={"textSecondary"}
                        variant={"subtitle1"}
                    >
                        {setting.id}
                    </Typography>
                    <Chip label={setting.type} variant="filled" size="small" sx={{ml: 2}}/>
                </Box>
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
                    id="setting-value"
                    label="Value"
                    required
                    fullWidth
                    value={setting.value}
                    error={!isValid()}
                    helperText={isValid() ? '' : 'A value is required'}
                    placeholder={"Enter the setting value"}
                    onChange={e => setSetting({ ...setting, value: e.target.value })}
                />
            </Box>
            <Divider></Divider>

        </Grid>
    )

}
export default SettingEditCard