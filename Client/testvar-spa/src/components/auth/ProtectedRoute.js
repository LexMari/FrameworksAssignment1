import { Navigate } from "react-router-dom";
import {useAuth } from "../../hooks/AuthProvider";

function ProtectedRoute({children, redirectPath = '/'}) 
{
    const auth = useAuth();
    
     if (!auth.isAuthenticated) {
        return <Navigate to={redirectPath} replace />;
    }
    return children;
}

export default ProtectedRoute;