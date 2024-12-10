import {useAuth} from "../../hooks/AuthProvider";
import {useEffect, useState} from "react";
import PageTitle from "../../components/common/PageTitle";
import {Alert} from "@mui/material";
import Typography from "@mui/material/Typography";
import Grid from "@mui/material/Grid2";
import * as React from "react";
import {getSettings, updateSetting} from "../../api/SettingsApi";
import SettingDetailCard from "../../components/settings/SettingDetailCard";
import SettingEditCard from "../../components/settings/SettingEditCard";

const SettingsIndex = () => {
    const auth = useAuth();
    const [settings, setSettings] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [editKey, setEditKey] = useState(null);
    const [error, setError] = useState();

    async function fetchData() {
        if (auth.token) {
            const data = await getSettings(auth.token);
            setSettings(data);
        }
        setIsLoading(false);
    }

    useEffect(() => {
        fetchData();
    }, [isLoading]);


    function editSetting(setting) {
        setEditKey(setting.id);
    }

    function editSettingCancel() {
        setEditKey(null);
        setError(undefined);
    }

    function editSettingSave(setting) {
        updateSetting(auth.token, editKey, setting).then((result) => {
            setEditKey(null);
            setIsLoading(true);
            setError(undefined);
        }).catch((e) => {
            setError(e);
        });
    }

    return ( !isLoading &&
        <>
            <PageTitle title="Application Settings">
            </PageTitle>
            {
                error &&
                <Alert variant="outlined" severity="error" sx={{m: 2}}>
                    <Typography component={"span"} variant={"body1"} fontWeight={"bold"}>
                        {error.message}
                    </Typography>
                </Alert>
            }
            <Grid
                container
                sx={{ ml: 3, mr: 3, mt: 1, size: {sm: 12, md: 6}, justifyContent: "center"}}>
                {
                    settings.map((_, index) => {
                        return (
                            <Grid size={6} key={_.id}>
                                {
                                    (editKey === _.id) &&
                                    <SettingEditCard
                                        target={_}
                                        error={error}
                                        saveCallback={editSettingSave}
                                        cancelCallback={editSettingCancel}
                                    />
                                }
                                {
                                    (editKey !== _.id) &&
                                    <SettingDetailCard
                                        setting={_}
                                        editOpen={editKey !== null}
                                        onEdit={() => editSetting(_) }/>
                                }
                            </Grid>
                        );
                    })
                }
            </Grid>
        </>
    )

}
export default SettingsIndex;