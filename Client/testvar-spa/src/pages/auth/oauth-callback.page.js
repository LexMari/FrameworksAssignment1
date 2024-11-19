import { useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";
import { getUser, handleOAuthCallback, isAuthenticated } from "../../services/AuthService"
import { useAuth } from "../../hooks/AuthProvider";

function OAuthCallback() {
    
    const isProcessed = useRef(false);
    const navigate = useNavigate();

    const auth = useAuth();
    
    useEffect(() => {
        async function processOAuthResponse() {
            
            // this is needed, because React.StrictMode makes component rerender
            // second time the auth code that is in req.url here is invalid,
            // so we want it to execute one time only.
            if (isProcessed.current) {
                return;
            }

            isProcessed.current = true;

            try {
                const currentUrl = window.location.href;
                await handleOAuthCallback(currentUrl);
                auth.loginAction(await getUser());
                
                navigate("/sets");
            } catch (error) {
                console.error("Error processing OAuth callback:", error);
            }
        }

        processOAuthResponse();
    }, [])
}

export default OAuthCallback;