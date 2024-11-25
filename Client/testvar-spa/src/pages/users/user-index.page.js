import * as React from "react";
import {useEffect, useState} from "react";
import {useAuth} from "../../hooks/AuthProvider";
import {deleteUser, getUsers} from "../../api/UserApi";
import Grid from "@mui/material/Grid2";
import {Alert, Button} from "@mui/material";
import PageTitle from "../../components/common/PageTitle";
import UserDetailCard from "../../components/users/UserDetailCard";
import AddIcon from "@mui/icons-material/Add";
import Typography from "@mui/material/Typography";

const UserIndex = () => {
    const auth = useAuth();
    const [users, setUsers] = useState([]);
    const [error, setError] = useState();
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

    function handleDeleteUser (user) {
        console.log(`DELETE ${user.username}`);
        deleteUser(user.id, auth.token).then((result) => {
            setIsLoading(true);
        }).catch((e) => {
            setError(e);
        });
    }

    return ( !isLoading &&
        <>
            <PageTitle title="Application Users">
                <Button variant={"outlined"} secondary startIcon={<AddIcon />} title={"Add a new user to the application"}>
                    Add User
                </Button>
            </PageTitle>
            {
                error &&
                <Alert variant="outlined" severity="error" sx={{m: 2}}>
                    <Typography component={"span"} variant={"body1"} fontWeight={"bold"}>
                        {error.message}
                    </Typography>
                </Alert>
            }
            <Grid container spacing={3} sx={{ display: 'flex', ml: 3, mr: 3, mt: 1}}>
                {
                    (!users || users?.length < 1) &&
                    <Grid size={12}>
                        <Alert variant="outlined" severity="info">
                            There are no users to display
                        </Alert>
                    </Grid>
                }

                {users.map((_, index) => {
                    return (
                        <Grid size={{ xs: 6, md: 4 }} key={_.id}>
                            <UserDetailCard user={_} allowEdit={true} onDelete={handleDeleteUser} />
                        </Grid>
                    );
                })}
            </Grid>
        </>
    )

}
export default UserIndex