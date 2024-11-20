import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import UnAuthenticated from './pages/auth/unauthenticated.page';
import ProtectedRoute from './components/auth/ProtectedRoute';
import OAuthCallback from './pages/auth/oauth-callback.page';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import FlashcardSetIndex from "./pages/flashcardsets/flashcard-set-index.page";
import FlashcardSeDisplay from "./pages/flashcardsets/flashcard-set-display.page";
import EmptyLayout from "./components/layouts/empty-layout";
import AuthProvider from "./hooks/AuthProvider";
import UserFlashcardSetIndex from "./pages/users/user-flashcardset-index.page";
import DrawerLayout from "./components/layouts/drawer-layout";
import Register from "./pages/register/register.page";
import FlashcardSetCreate from "./pages/flashcardsets/flashcard-set-create.page";

const darkTheme = createTheme({
    palette: {
        mode: 'dark',
    },
});

//https://stackoverflow.com/questions/68382679/toggling-between-dark-light-mode-material-ui

export default function App() {
    return (
        <ThemeProvider theme={darkTheme}>
            <BrowserRouter>
                <AuthProvider>
                    <Routes>
                        <Route element={<EmptyLayout />}>
                            <Route path={'/'} element={<UnAuthenticated />} />
                            <Route path={'/register'} element={<Register />} />
                        </Route>
                        <Route element={<DrawerLayout />}>
                            <Route path={'/sets'} element={
                                <ProtectedRoute redirectPath='/'>
                                    <FlashcardSetIndex />
                                </ProtectedRoute>
                            } />
                            <Route path={'/sets/create'} element={
                                <ProtectedRoute redirectPath='/'>
                                    <FlashcardSetCreate />
                                </ProtectedRoute>
                            } />
                            <Route path={'/sets/:setId'} element={
                                <ProtectedRoute redirectPath='/'>
                                    <FlashcardSeDisplay />
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