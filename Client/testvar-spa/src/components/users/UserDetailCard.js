import React, {useState} from "react";
import Grid from "@mui/material/Grid2";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import IconButton from "@mui/material/IconButton";
import PersonIcon from '@mui/icons-material/Person';
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import {useAuth} from "../../hooks/AuthProvider";
import {useConfirm} from "material-ui-confirm";

const UserDetailCard = ({user, allowEdit = false, onEdit = {},  onDelete = {}}) => {
    const auth = useAuth();
    const confirm = useConfirm();
    const [hoverState, setHoverState] = useState(false);

    function toggleHover() {
        setHoverState(!hoverState);
    }

    const onDeleteClick = (user) => {
        confirm({ description: `Do you want to permanently delete user "${user.username}".` })
            .then(() => onDelete(user))
            .catch(() => console.log("Deletion cancelled."));
    };

    return (
        <Grid container
              sx={{
                  alignItems: 'center',
                  mt: 1, p: 1,
                  borderRadius: 1,
                  border: 2,
                  borderColor: hoverState ? 'primary.main' : 'text.secondary',
                  boxShadow: 1,
                  bgColor: 'secondary.main'
              }}
              onMouseEnter={toggleHover}
              onMouseLeave={toggleHover}
        >
            <Grid flexShrink={1} sx={{p:2}}>
                <PersonIcon color={hoverState ? 'primary' : 'action'} fontSize={"large"} />
            </Grid>
            <Grid flexGrow={1}>
                <Box display={'flex'}>
                    <Typography variant="h6" gutterBottom color={"text.primary"} sx={{ flexGrow: 1 }}>
                        {user?.username}
                    </Typography>
                    {
                        allowEdit &&
                        <>
                            <IconButton
                                size="small"
                                title="Edit user"
                            >
                                <EditIcon fontSize="small"/>
                            </IconButton>
                        </>
                    }
                    {
                        allowEdit && auth.userId !== user.id &&
                        <IconButton
                            size="small"
                            title="Delete user"
                            onClick={() => onDeleteClick(user)}
                        >
                            <DeleteIcon fontSize="small"/>
                        </IconButton>
                    }
                </Box>
            </Grid>
        </Grid>
    );
};

export default UserDetailCard;