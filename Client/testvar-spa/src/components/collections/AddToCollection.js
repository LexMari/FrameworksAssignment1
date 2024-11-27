import React, {useState} from "react";
import {useAuth} from "../../hooks/AuthProvider";
import Box from '@mui/material/Box'
import Typography from "@mui/material/Typography";
import Select from "@mui/material/Select";
import InputLabel from "@mui/material/InputLabel";
import CancelIcon from '@mui/icons-material/Close';
import Grid from "@mui/material/Grid2";
import FormControl from '@mui/material/FormControl';
import {updateUserCollection} from "../../api/UserApi";
import {Alert, Button, MenuItem} from "@mui/material";
import BookmarkIcon from "@mui/icons-material/Bookmark";
import {useNavigate} from "react-router-dom";

const AddToCollection = ({setId, saveHandler, cancelHandler}) => {
    const auth = useAuth();
    const navigate = useNavigate();
    const [selected, setSelected] = useState(null);
    const [error, setError] = useState();

    function handleCancelClick() {
        cancelHandler();
    }
    
    function handleSaveClick() {
        const currentSets = selected?.sets.map((_) => {
            return _.id;
        });
        
        const updateCollection = {
            id: selected.id,
            comment: selected.comment,
            sets: [...currentSets, setId]
        }
        
        updateUserCollection(auth.userId, updateCollection, auth.token).then((result) => {
            auth.loadUserCollections(auth.userId, auth.token);
            const collectionUrl = `/users/${auth.userId}/collections/${updateCollection.id}`;
            navigate(collectionUrl);
        }).catch((e) => {
            setError(e);
        });
    }
    
    return (
        <>
            {
                error &&
                <Alert variant="outlined" severity="error" sx={{m: 2}}>
                    <Typography component={"span"} variant={"body1"} fontWeight={"bold"}>
                        {error.message}
                    </Typography>
                </Alert>
            }
            {
                <Box sx={{
                    width: "100%",
                    border: 1,
                    borderRadius: 2,
                    borderColor: "primary.main",
                    p: 2,
                    m: 3
                }}>
                    <Typography variant={"h6"}>Add flashcard set to collection</Typography>
                    <Grid container
                          spacing={2}
                          column={{xs: 1, sm: 4}}
                          sx={{
                              width: "100%",
                              display: "flex",
                              flexDirection: 'row',
                              alignItems: "center",
                              mt: 2
                          }}>
                        <Grid sx={{flexGrow: 1}}>
                            <FormControl variant="outlined" sx={{ width: "100%" }}>
                                <InputLabel id="demo-simple-select-helper-label">Collection</InputLabel>
                                <Select
                                    label="Collection"
                                    onChange={(e) => setSelected(e.target.value)}
                                >
                                    {auth.userCollections?.map((item) => (
                                        <MenuItem  key={item} value={item}>
                                            {item.comment}
                                        </MenuItem >
                                    ))}
                                </Select>
                            </FormControl>
                        </Grid>
                        <Grid>
                            <Button
                                sx={{mr: 2}}
                                variant={"outlined"}
                                color={"primary"}
                                startIcon={<BookmarkIcon/>}
                                onClick={handleSaveClick}
                            >
                                Add to collection
                            </Button>
                            <Button
                                variant={"outlined"}
                                color={"secondary"}
                                startIcon={<CancelIcon/>}
                                onClick={handleCancelClick}
                            >
                                Cancel
                            </Button>
                        </Grid>
                    </Grid>
                </Box>
            }
        </>
    )
};
export default AddToCollection;