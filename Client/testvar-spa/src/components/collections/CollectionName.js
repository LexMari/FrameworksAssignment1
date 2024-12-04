import {Typography} from "@mui/material";
import * as React from "react";
import Grid from "@mui/material/Grid2";

const CollectionName = ({collection}) => {
    return (
        <Grid size={12} sx={{border: 1, borderRadius: 2, borderColor: 'primary.main', p:2, display: 'flex', alignItems: 'end'}}>
            <Typography variant={"h4"} sx={{ flexGrow: 1 }}>
                {collection?.comment}
            </Typography>
            <Typography variant="body2" color={"text.secondary"} sx={{ flexShrink: 1 }}>
                Curated by {collection?.user?.username}
            </Typography>
        </Grid>
    )
}
export default CollectionName;