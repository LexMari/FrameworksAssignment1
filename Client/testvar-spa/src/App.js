import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import UnAuthenticated from './pages/auth/unauthenticated.page';
import ProtectedRoute from './components/auth/ProtectedRoute';
import OAuthCallback from './pages/auth/oauth-callback.page';
import MasterLayout from "./layouts/master-layout";
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import FlashcardSetIndex from "../src/components/FlashcardSets/flashcard-set-index"
import FlashcardSetDisplay from "../src/components/FlashcardSets/flashcard-set-display"
import EmptyLayout from "../src/layouts/emtpy-layout";
import AuthProvider from "./hooks/AuthProvider";
import UserFlashcardSetIndex from "./pages/users/user-flashcardset-index.page";

const darkTheme = createTheme({
    palette: {
        mode: 'dark',
    },
});

//https://stackoverflow.com/questions/68382679/toggling-between-dark-light-mode-material-ui

export default function App() {
    return (
        <ThemeProvider theme={darkTheme}>
            <CssBaseline />
            <BrowserRouter>
                <AuthProvider>
                    <Routes>
                        <Route element={<EmptyLayout />}>
                            <Route path={'/'} element={<UnAuthenticated />} />
                        </Route>
                        <Route element={<MasterLayout />}>
                            <Route path={'/sets'} element={
                                <ProtectedRoute redirectPath='/'>
                                    <FlashcardSetIndex />
                                </ProtectedRoute>
                            } />
                            <Route path={'/sets/:setId'} element={
                                <ProtectedRoute redirectPath='/'>
                                    <FlashcardSetDisplay />
                                </ProtectedRoute>
                            } />
                            <Route path={'/users/:userId/sets'} element={
                                <ProtectedRoute redirectPath='/'>
                                    <UserFlashcardSetIndex />
                                </ProtectedRoute>
                            } />
                        </Route>
                        <Route path='/oauth/callback' element={<OAuthCallback />} />
                    </Routes>
                </AuthProvider>
            </BrowserRouter>
        </ThemeProvider>
    )
};
