import * as React from "react";
import {useEffect, useState} from "react";
import {useAuth} from "../../hooks/AuthProvider";
import {getUsers} from "../../api/UserApi";
import Grid from "@mui/material/Grid2";
import {Alert, Button} from "@mui/material";
import PageTitle from "../../components/common/PageTitle";
import UserDetailCard from "../../components/users/UserDetailCard";
import AddIcon from "@mui/icons-material/Add";

const UserIndex = () => {
    const auth = useAuth();
    const [users, setUsers] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    async function fetchData() {
        if (auth.token) {
            const data = await getUsers(auth.token);
            setUsers(data);
        }
        setIsLoading(false);
    }
    useEffect(() => {
        fetchData();
    }, [isLoading]);
    return ( !isLoading &&
        <>
            <PageTitle title="Application Users">
                <Button variant={"outlined"} secondary startIcon={<AddIcon />} title={"Add a new user to the application"}>
                    Add User
                </Button>
            </PageTitle>
            <Grid container spacing={3} sx={{ display: 'flex', ml: 3, mr: 3, mt: 1}}>
                {
                    (!users || users?.length < 1) &&
                    <Grid item size={12}>
                        <Alert variant="outlined" severity="info">
                            There are no users to display
                        </Alert>
                    </Grid>
                }
                {users.map((_, index) => {
                    return (
                        <Grid item size={{ xs: 6, md: 4 }} key={_.id}>
                            <UserDetailCard user={_} allowEdit={true} />
                        </Grid>
                    );
                })}
            </Grid>
        </>
    )

}
export default UserIndex