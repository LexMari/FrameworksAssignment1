import { useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";
import { getUser, handleOAuthCallback } from "../../services/AuthService"
import { useAuth } from "../../hooks/AuthProvider";

function OAuthCallback() {
    
    const isProcessed = useRef(false);
    const navigate = useNavigate();

    const auth = useAuth();
    
    useEffect(() => {
        async function processOAuthResponse() {
            
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