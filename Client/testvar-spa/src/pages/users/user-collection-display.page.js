import {useParams} from "react-router-dom";
import {useAuth} from "../../hooks/AuthProvider";
import {useEffect, useState} from "react";
import PageTitle from "../../components/common/PageTitle";
import * as React from "react";
import {getUserCollection} from "../../api/UserApi";
import Grid from "@mui/material/Grid2";
import {Alert, Divider, Typography} from "@mui/material";
import FlashcardSetSummary from "../../components/flashcardsets/FlashcardSetSummary";

const UserCollectionDisplay = () => {
    let { userId, collectionId } = useParams();
    const auth = useAuth();
    const [collection, setCollection] = useState([]);
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

    return (!isLoading &&
        <>
            <PageTitle title="Flashcard Set Collection">
            </PageTitle>
            <Grid container spacing={3} sx={{ display: 'flex', ml: 3, mr: 3, mt: 3, justifyContent: "center" }}>
                <Grid item size={12} sx={{border: 1, borderRadius: 2, borderColor: 'primary.main', p:2, display: 'flex', alignItems: 'end'}}>
                    <Typography variant={"h4"} sx={{ flexGrow: 1 }}>
                        {collection?.comment}
                    </Typography>
                    <Typography variant="body2" color={"text.secondary"} sx={{ flexShrink: 1 }}>
                        Curated by {collection?.user?.username}
                    </Typography>
                </Grid>
                <Divider />
                {
                    (collection?.sets.length < 1) &&
                    <Grid item size={12}>
                        <Alert variant="outlined" severity="info">
                            No flashcard sets have been added to this collection
                        </Alert>
                    </Grid>
                }
                {
                    collection?.sets.map((_, index) => {
                        return (
                            <Grid item size={{ xs: 6, md: 4 }} key={index}>
                                <FlashcardSetSummary set={_} />
                            </Grid>
                        );
                    })}
            </Grid>
        </>

    );
}
export default UserCollectionDisplay;