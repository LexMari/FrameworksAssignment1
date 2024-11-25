import * as React from 'react';
import Grid from '@mui/material/Grid2';
import {useEffect, useState} from "react";
import SortService from "../../services/SortService";
import PageTitle from "../../components/common/PageTitle";
import {useAuth} from "../../hooks/AuthProvider";
import {Alert} from "@mui/material";
import CollectionSummary from "../../components/collections/CollectionSummary";
import {getCollections} from "../../api/CollectionsApi";

const CollectionIndex = () => {
    const auth = useAuth();
    const [collections, setCollections] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [sort] = useState({ field: 'created_at', direction: 'desc' });

    async function fetchData() {
        if (auth.token) {
            const data = await getCollections(auth.token);
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
            <PageTitle title={`All Collections`}></PageTitle>
            <Grid container spacing={3} sx={{ display: 'flex', ml: 3, mr: 3, mt: 1}}>
                {
                    (!collections || collections?.length < 1) &&
                    <Grid size={12}>
                        <Alert variant="outlined" severity="info">
                            There are no flashcard set collections to display
                        </Alert>
                    </Grid>
                }

                {collections.map((_, index) => {
                    return (
                        <Grid size={{ xs: 6, md: 4 }} key={index}>
                            <CollectionSummary collection={_} showCurator={true} />
                        </Grid>
                    );
                })}
            </Grid>
        </>
    );
}
export default CollectionIndex