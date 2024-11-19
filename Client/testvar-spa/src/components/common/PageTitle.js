import React from "react";
import Typography from "@mui/material/Typography";
import Grid from "@mui/material/Grid2";

const PageTitle = ({title, children}) => {
    return (
        <Grid container maxWidth={true} sx={{ml: 3, mr: 3, mb: 1, alignItems: 'end'}} columns={{xs:1, sm: 2}} >
            <Grid item sx={{flexGrow: 1}}>
                <Typography variant="h3" component="div" sx={
                    {fontWeight: 'bold', color: 'success.main', display: 
                            { xs: 'none', sm: 'block' }, 
                    }}>{title}
                </Typography>
                <Typography variant="h4" component="div" sx={
                    {fontWeight: 'bold', color: 'success.main', display:
                            { xs: 'block', sm: 'none' }, mb: 1
                    }}>{title}
                </Typography>
            </Grid>
            <Grid item>
                {children}
            </Grid>
        </Grid>
    );
};

export default PageTitle;
