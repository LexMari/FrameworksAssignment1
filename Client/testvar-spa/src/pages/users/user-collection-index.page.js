import * as React from 'react';
import Grid from '@mui/material/Grid2';
import {useEffect, useState} from "react";
import SortService from "../../services/SortService";
import PageTitle from "../../components/common/PageTitle";
import {deleteUserCollection, getUserCollections} from "../../api/UserApi";
import {useAuth} from "../../hooks/AuthProvider";
import {useParams} from "react-router-dom";
import Typography from "@mui/material/Typography";
import AddIcon from '@mui/icons-material/Add';
import CollectionSummary from "../../components/collections/CollectionSummary";
import {createCollection} from "../../api/CollectionsApi";
import CollectionCreate from "../../components/collections/CollectionCreate";
import {Alert, Button} from "@mui/material";
import {useConfirm} from "material-ui-confirm";

const UserCollectionIndex = () => {
    let { userId } = useParams();
    const auth = useAuth();
    const confirm = useConfirm();
    const [collections, setCollections] = useState([]);
    const [error, setError] = useState();
    const [createOpen, setCreateOpen] = useState(false);
    const [createError, setCreateError] = useState();
    const [isLoading, setIsLoading] = useState(true);
    const [sort] = useState({ field: 'created_at', direction: 'desc' });

    async function fetchData() {
        if (auth.token) {
            const data = await getUserCollections(userId, auth.token);
            const sortedData = SortService.sortCollections(data, sort);
            setCollections(sortedData);
        }
        setIsLoading(false);
    }

    useEffect(() => {
        fetchData();
    }, [isLoading, sort]);

    function addCollection() {
        setCreateError();
        setCreateOpen(true);
    }
    function addCollectionCancel() {
        setCreateError();
        setCreateOpen(false);
    }

    function addCollectionSave(comment) {
        createCollection(comment, auth.token).then((result) => {
            setCollections([result, ...collections]);
            setCreateOpen(false);
        }).catch((e) => {
            setCreateError(e);
        });
    }

    function handleCollectionDelete(collectionId) {
        setError(undefined);
        confirm({ description: `Do you wish to delete this collection?` })
            .then(() => {
                deleteUserCollection(auth.userId, collectionId, auth.token).then(() => {
                    let updatedCollections = collections.filter(x => x.id !== collectionId);
                    setCollections(updatedCollections);
                    auth.loadUserCollections(auth.userId, auth.token);
                }).catch((e) => {
                    setError(e);
                });
            })
            .catch(() => {});
    }

    return (!isLoading &&
        <>
            <PageTitle title={`My Collections`}>
                <Grid container spacing={1} justifyContent="space-between">
                    <Grid size="grow">
                        <Typography variant={"h6"} color={"textSecondary"} display={"inline-flex"}>Curated by</Typography>
                        <Typography variant={"h6"} color={"textPrimary"} fontWeight={"bold"} display={"inline-flex" } sx={{ml: 1, mr:3}}>{auth.username}</Typography>
                    </Grid>
                    <Grid size="auto"  textAlign='right'>
                        <Button variant={"outlined"}
                                startIcon={<AddIcon />}
                                onClick={addCollection}
                                disabled={createOpen}
                                title={"Create a flashcard set collection"}>
                            Create Collection
                        </Button>
                    </Grid>
                </Grid>
            </PageTitle>
            {
                error &&
                <Alert variant="outlined" severity="error" sx={{m: 2}}>
                    <Typography component={"span"} variant={"body1"} fontWeight={"bold"}>
                        {error.message}
                    </Typography>
                </Alert>
            }
            {
                createOpen &&
                <Grid container spacing={3} sx={{ display: 'flex', ml: 3, mr: 3, mt: 3, justifyContent: "center" }}>
                    <Grid size={{lg:12,  xl: 8}} justifyContent="center" alignItems="center">
                        <CollectionCreate
                            saveHandler={addCollectionSave}
                            cancelHandler={addCollectionCancel}
                            error={createError}
                        />
                    </Grid>
                </Grid>
            }
            <Grid container spacing={3} sx={{ display: 'flex', ml: 3, mr: 3, mt: 1}}>
                {
                    (!collections || collections?.length < 1) &&
                    <Grid size={12}>
                        <Alert variant="outlined" severity="info">
                            There are no flashcard set collections to display
                        </Alert>
                    </Grid>
                }

                {
                    collections?.map((_, index) => {
                        return (
                            <Grid size={{ xs: 6, md: 4 }} key={index}>
                                <CollectionSummary 
                                    collection={_} 
                                    allowDelete={true} 
                                    deleteCallback={handleCollectionDelete} />
                            </Grid>
                        );
                    })
                }
            </Grid>
        </>
    );
}
export default UserCollectionIndex