import {useAuth} from "../../hooks/AuthProvider";
import React, {useState} from "react";
import {Navigate} from "react-router-dom";
import {createUser} from "../../api/UserApi";
import RegistrationComplete from "./RegistrationComplete";
import RegistrationForm from "./RegistrationForm";

export default function Register() {
    const auth = useAuth();
    const [user, setUser] = useState({username: "", password: ""})
    const [created, setCreated] = useState(false);
    const [error, setError] = useState();
    
    if (auth.isAuthenticated) {
        return <Navigate to ='/sets' replace />;
    }
    
    function handleSubmit(event) {
        event.preventDefault();
        createUser(user).then((result) => {
            setCreated(true);
        }).catch((e) => {
            setCreated(false);
            setError(e)
        });
    }
    
    return (
        created ? 
        <RegistrationComplete /> :
            <RegistrationForm
                user = {user}
                onSubmit = {handleSubmit}
                onChangeUsername={e => setUser({ ...user, username: e.target.value })}
                onChangePassword={e => setUser({ ...user, password: e.target.value })}
                error={error}
            />
    );
}