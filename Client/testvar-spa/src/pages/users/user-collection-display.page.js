import {useParams} from "react-router-dom";
import {useAuth} from "../../hooks/AuthProvider";
import {useEffect, useState} from "react";
import PageTitle from "../../components/common/PageTitle";
import * as React from "react";
import {getUserCollection, updateUserCollection} from "../../api/UserApi";
import Grid from "@mui/material/Grid2";
import {Alert, Button, Divider, Typography} from "@mui/material";
import FlashcardSetSummary from "../../components/flashcardsets/FlashcardSetSummary";
import EditIcon from "@mui/icons-material/Edit";
import CollectionEdit from "../../components/collections/CollectionEdit";
import CollectionName from "../../components/collections/CollectionName";

const UserCollectionDisplay = () => {
    let { userId, collectionId } = useParams();
    const auth = useAuth();
    const [collection, setCollection] = useState([]);
    const [editOpen, setEditOpen] = useState(false);
    const [error, setError] = useState();
    const [collectionError, setCollectionError] = useState();
    const [isLoading, setIsLoading] = useState(true);

    async function fetchData() {
        if (auth.token) {
            const data = await getUserCollection(userId, collectionId, auth.token);
            setCollection(data);
        }
        setIsLoading(false);
    }

    useEffect(() => {
        fetchData();
    }, [isLoading, userId, collectionId]);

    function editCollection(card) {
        setEditOpen(true);
    }

    function editCollectionCancel() {
        setEditOpen(false);
    }

    function editCollectionSave(comment) {
        const currentSets = collection?.sets.map((_) => {
            return _.id;
        });
        const updateCollection = {
            id: collection.id,
            comment: comment,
            sets: currentSets
        }
        updateUserCollection(auth.userId, updateCollection, auth.token).then((result) => {
            auth.loadUserCollections(auth.userId, auth.token);
            setEditOpen(false);
            setCollection({...collection, comment: updateCollection.comment});
        }).catch((e) => {
            setCollectionError(e);
        });
    }

    function handleRemove(setId) {
        const updatedSets = collection?.sets
            .filter(x => x.id !== setId);
        const updateCollection = {
            id: collection.id,
            comment: collection.comment,
            sets: updatedSets.map((_) => { return _.id;})
        }
        updateUserCollection(auth.userId, updateCollection, auth.token).then((result) => {
            setCollection({...collection, sets: updatedSets});
            auth.loadUserCollections(auth.userId, auth.token);
        }).catch((e) => {
            setError(e);
        });
    }

    return (!isLoading &&
        <>
            <PageTitle title="Flashcard Set Collection">
                <Grid size="auto" textAlign='right'>
                    <Button variant={"outlined"}
                            startIcon={<EditIcon />}
                            onClick={editCollection}
                            disabled={editOpen}
                            title={"Edit this collection name"}>
                        Edit Collection
                    </Button>
                </Grid>
            </PageTitle>
            <Grid container spacing={3} sx={{ display: 'flex', ml: 3, mr: 3, mt: 3, justifyContent: "center" }}>
                { !editOpen &&
                    <CollectionName collection={collection} />
                }
                { editOpen &&
                    <CollectionEdit
                        collection={collection}
                        saveHandler={editCollectionSave}
                        cancelHandler={editCollectionCancel}
                        error={collectionError}
                    />
                }
                <Divider />
                {
                    error &&
                    <Alert variant="outlined" severity="error">
                        <Typography component={"span"} variant={"body1"} fontWeight={"bold"}>
                            {error.message}
                        </Typography>
                    </Alert>
                }
                {
                    (collection?.sets.length < 1) &&
                    <Grid size={12}>
                        <Alert variant="outlined" severity="info">
                            No flashcard sets have been added to this collection
                        </Alert>
                    </Grid>
                }
                {
                    collection?.sets.map((_, index) => {
                        return (
                            <Grid size={{ xs: 6, md: 4 }} key={index}>
                                { collection?.user.id === auth.userId &&
                                    <FlashcardSetSummary set={_} allowRemove={true} removeCallback={handleRemove} />
                                }
                                { collection?.user.id !== auth.userId &&
                                    <FlashcardSetSummary set={_} allowRemove={false} />
                                }

                            </Grid>
                        );
                    })}
            </Grid>
        </>

    );
}
export default UserCollectionDisplay;