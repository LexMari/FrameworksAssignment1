import { useState } from "react";
import { useContext, createContext } from "react";
import {getUser} from "../services/AuthService";
const AuthContext = createContext();

const AuthProvider = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [user, setUser] = useState(null);
    const [token, setToken] = useState(null);
    const [username, setUsername] = useState("Anonymous");
    const [userId, setUserId] = useState(-1);
    const [isAdmin, setIsAdmin] = useState(false);
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
        }
        else
        {
            setIsAuthenticated(false)
            setToken(null);
            setUserId(-1);
            setUsername("");
            setIsAdmin(false);
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

    return (
        <AuthContext.Provider
            value={{isAuthenticated, user, token, username, userId, isAdmin, loginAction, logoutAction}}>
            {children}
        </AuthContext.Provider>
    );
};
export default AuthProvider;
export const useAuth = () => {
    return useContext(AuthContext);
};