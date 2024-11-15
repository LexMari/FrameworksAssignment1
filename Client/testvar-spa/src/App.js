import React from "react";
import { useEffect, useState } from 'react';
import { getUser, logout } from './services/AuthService'
import { BrowserRouter, Routes, Route } from "react-router-dom";
import UnAuthenticated from './pages/auth/unauthenticated.page';
import ProtectedRoute from './components/ProtectedRoute';
import OAuthCallback from './pages/auth/oauth-callback.page';
import MasterLayout from "./layouts/master-layout";
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import FlashcardSetIndex from "./components/FlashcardSets/flashcard-set-index";
import FlashcardSeDisplay from "./components/FlashcardSets/flashcard-set-display";
import EmptyLayout from "./layouts/emtpy-layout";

const darkTheme = createTheme({
    palette: {
        mode: 'dark',
    },
});

//https://stackoverflow.com/questions/68382679/toggling-between-dark-light-mode-material-ui

export default function App() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [user, setUser] = useState(null);
    const [username, setUsername] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    async function fetchData() {
        const user = await getUser();
        const accessToken = user?.access_token;

        setUser(user);


        if (accessToken) {
            setIsAuthenticated(true);
            setUsername(user.profile?.sub);
            //const data = await getAndreykaResources(accessToken);
            //setResource(data);
        }

        setIsLoading(false);
    }

    useEffect(() => {
        fetchData();
    }, [isAuthenticated]);

    if (isLoading) {
        return (<>Loading...</>)
    }

    return (
        <ThemeProvider theme={darkTheme}>
            <CssBaseline />
            <BrowserRouter>
                <Routes>
                    <Route element={<EmptyLayout />}>
                        <Route path={'/'} element={<UnAuthenticated authenticated={isAuthenticated} />} />
                    </Route>

                    <Route element={<MasterLayout isAuthenticated={isAuthenticated} user="" />}>
                        <Route path={'/sets'} element={
                            <ProtectedRoute authenticated={isAuthenticated} redirectPath='/'>
                                <FlashcardSetIndex />
                            </ProtectedRoute>
                        } />
                        <Route path={'/sets/:setId'} element={
                            <ProtectedRoute authenticated={isAuthenticated} redirectPath='/'>
                                <FlashcardSeDisplay />
                            </ProtectedRoute>
                        } />
                    </Route>

                    <Route path='/oauth/callback' element={<OAuthCallback setIsAuthenticated={setIsAuthenticated} />} />
                </Routes>
            </BrowserRouter>
        </ThemeProvider>
    )
};
