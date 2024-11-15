import React from 'react';
import ReactDOM from 'react-dom/client';
import { createBrowserRouter, RouterProvider,} from "react-router-dom";
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import FlashcardSetIndex from './components/FlashcardSets/flashcard-set-index';
import FlashcardSetDisplay from './components/FlashcardSets/flashcard-set-display';
import RouterErrorPage from "./pages/router-error-page";

const router = createBrowserRouter([
    {
        path: "/",
        element: <App />,
        errorElement: <RouterErrorPage />,
        children: [
            {
                index: true,
                element: <FlashcardSetIndex />,
            },
            {
                path: "sets/:setId",
                element: <FlashcardSetDisplay/>,
            }
        ]
    }
]);

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <React.StrictMode>
        <RouterProvider router={router} />
    </React.StrictMode>
);


// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
