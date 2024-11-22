import * as React from 'react';
import Grid from '@mui/material/Grid2';
import {useEffect, useState} from "react";
import SortService from "../../services/SortService";
import PageTitle from "../../components/common/PageTitle";
import {getUserCollections} from "../../api/UserApi";
import {useAuth} from "../../hooks/AuthProvider";
import {Link, useParams} from "react-router-dom";
import {Alert, Button} from "@mui/material";
import Typography from "@mui/material/Typography";
import AddIcon from '@mui/icons-material/Add';
import BookmarksIcon from "@mui/icons-material/Bookmarks";
import CollectionSummary from "../../components/collections/CollectionSummary";

const UserCollectionIndex = () => {
    let { userId } = useParams();
    const auth = useAuth();
    const [collections, setCollections] = useState([]);
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

    return (
        <>
            <PageTitle title={`My Collections`}>
                <Grid container spacing={1} justifyContent="space-between">
                    <Grid item size="grow">
                        <Typography variant={"h6"} color={"textSecondary"} display={"inline-flex"}>Curated by</Typography>
                        <Typography variant={"h6"} color={"textPrimary"} fontWeight={"bold"} display={"inline-flex" } sx={{ml: 1, mr:3}}>{auth.username}</Typography>
                    </Grid>
                    <Grid item size="auto"  textAlign='right'>
                        <Button variant={"outlined"} secondary startIcon={<AddIcon />} title={"Create new flashcard set"}>
                            Create Collection
                        </Button>
                    </Grid>
                </Grid>
            </PageTitle>
            <Grid container spacing={3} sx={{ display: 'flex', ml: 3, mr: 3, mt: 1}}>
                {
                    (!collections || collections?.length < 1) &&
                    <Grid item size={12}>
                        <Alert variant="outlined" severity="info">
                            There are no flashcard set collections to display
                        </Alert>
                    </Grid>
                }

                {collections.map((_, index) => {
                    return (
                        <Grid item size={{ xs: 6, md: 4 }} key={index}>
                            <CollectionSummary collection={_} allowDelete={true} />
                        </Grid>
                    );
                })}
            </Grid>
        </>
    );
}
export default UserCollectionIndex