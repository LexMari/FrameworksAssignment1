import React, {useState} from "react";
import Grid from "@mui/material/Grid2";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import {Chip} from "@mui/material";
import IconButton from "@mui/material/IconButton";
import EditIcon from "@mui/icons-material/Edit";

const SettingDetailCard = ({setting, onEdit = {},  editOpen = false}) =>  {
    const [hoverState, setHoverState] = useState(false);
    function toggleHover() {
        setHoverState(!hoverState);
    }

    return (
        <Box
            sx={{
                alignItems: 'center',
                mt: 1, p: 1,
                borderRadius: 1,
                border: 2,
                borderColor: hoverState ? 'primary.main' : 'text.secondary',
                boxShadow: 1,
                bgColor: 'secondary.main',
                display: 'flex',
                justifyContent: 'space-between'
            }}
            onMouseEnter={toggleHover}
            onMouseLeave={toggleHover}
            key={setting.id}
        >
            <Box flexGrow={1}>
                <Grid
                    container
                    spacing={2}
                    sx={{
                        flexGrow: 1,
                        ml:2,
                        mr: 2,
                        mb: 1,
                        display: 'flex',
                        justifyContent: 'left'
                    }}>
                    <Grid>
                        <Typography variant={"h6"}>
                            {setting.id}
                        </Typography>
                    </Grid>
                    <Grid>
                        <Chip label={setting.type} variant="filled" />
                    </Grid>
                </Grid>
                <Typography variant={"subtitle1"} color={"secondary"} sx={{ml: 2, mr: 2}}>
                    {setting.description}
                </Typography>
            </Box>
            <Box flexShrink={1}>
                <Grid
                    container
                    spacing={2}
                    sx={{
                        ml: 4, mr: 2,
                        alignItems: "center",
                        justifyItems: "right"
                    }}
                >
                    <Grid>
                        <Typography variant={"h3"} color={"primary"}>
                            {setting.value}
                        </Typography>
                    </Grid>
                    <Grid>
                        <IconButton
                            size="small"
                            title="Edit setting"
                            disabled={editOpen}
                            onClick={() => onEdit(setting) }
                        >
                            <EditIcon fontSize="small"/>
                        </IconButton>
                    </Grid>
                </Grid>
            </Box>


        </Box>
    );
}
export default SettingDetailCard;