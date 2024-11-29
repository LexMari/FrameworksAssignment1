import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import { BrowserRouter as Router } from "react-router-dom";
import MasterLayout from "../../../components/layouts/master-layout";
import { useAuth } from "../../../hooks/AuthProvider";

jest.mock("../../../hooks/AuthProvider");
jest.mock("../../../components/auth/LogoutButton", () => jest.fn(() => <button>Logout</button>));

describe("MasterLayout Component", () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });

    const renderMasterLayout = (authMock) => {
        useAuth.mockReturnValue(authMock);
        return render(
            <Router>
                <MasterLayout />
            </Router>
        );
    };

    test("Renders AppBar and menu icon", () => {
        renderMasterLayout({ isAuthenticated: false });
        expect(screen.getByRole("button", { name: /menu/i })).toBeInTheDocument();
        expect(screen.getByText("TestVar Flashcards")).toBeInTheDocument();
    });

    test("Hides authenticated navigation for guests", () => {
        renderMasterLayout({ isAuthenticated: false });
        
        expect(screen.queryByRole("button", { name: /home/i })).not.toBeInTheDocument();
        expect(screen.queryByRole("button", { name: /my flashcard sets/i })).not.toBeInTheDocument();
        expect(screen.queryByText(/welcome/i)).not.toBeInTheDocument();
        expect(screen.queryByText("Logout")).not.toBeInTheDocument();
    });

    test("Renders links with correct paths for authenticated users", () => {
        renderMasterLayout({ isAuthenticated: true, userId: 42, username: "TestUser" });

        const homeLink = screen.getByRole("button", { name: /home/i }).closest("a");
        const mySetsLink = screen.getByRole("button", { name: /my flashcard sets/i }).closest("a");

        expect(homeLink).toHaveAttribute("href", "/sets");
        expect(mySetsLink).toHaveAttribute("href", "/users/42/sets");
    });
    
});