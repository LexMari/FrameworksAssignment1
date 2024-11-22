import { useState } from "react";
import { useContext, createContext } from "react";
import {getUser} from "../services/AuthService";
import {getUserCollections, getUserCollection} from "../api/UserApi";

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [user, setUser] = useState(null);
    const [token, setToken] = useState(null);
    const [username, setUsername] = useState("Anonymous");
    const [userId, setUserId] = useState(-1);
    const [isAdmin, setIsAdmin] = useState(false);
    const [userCollections, setUserCollections] = useState([]);
    const loginAction = (user) => {
        setUser(user);

        const accessToken = user?.access_token;
        const role = user.profile?.role;
        if (accessToken) {
            setIsAuthenticated(true);
            setToken(accessToken);
            setUsername(user.profile?.username);
            setUserId(user.profile?.nickname);
            setIsAdmin(role === "Administrator");
            loadUserCollections();
        }
        else
        {
            setIsAuthenticated(false)
            setToken(null);
            setUserId(-1);
            setUsername("");
            setIsAdmin(false);
            setUserId([]);
        }
    };

    const logoutAction = () => {
        setIsAuthenticated(false)
        setUser(null);
        setToken(null);
        setUserId(-1);
        setUsername("");
        setIsAdmin(false);
    };

    const loadUserCollections = () => {
        getUserCollections(userId, token).then((result) => {
            var data = result.map((_) => {
                return {id: _?.id, comment: _?.comment};
            });
            setUserCollections(data);
        }).catch((e) => {
            console.log(e.message);
            setUserId([]);
        });
    }

    return (
        <AuthContext.Provider
            value={{isAuthenticated, user, token, username, userId, isAdmin, loginAction, logoutAction, loadUserCollections}}>
            {children}
        </AuthContext.Provider>
    );
};
export default AuthProvider;
export const useAuth = () => {
    return useContext(AuthContext);
};