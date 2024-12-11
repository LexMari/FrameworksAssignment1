import React from "react";
import { render, screen } from "@testing-library/react";
import { BrowserRouter as Router } from "react-router-dom";
import SideNavigation from "../../../components/layouts/SideNavigation";
import { useAuth } from "../../../hooks/AuthProvider";

jest.mock("../../../hooks/AuthProvider");

describe("SideNavigation Component", () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });

    const renderSideNavigation = (authMock) => {
        useAuth.mockReturnValue(authMock);
        return render(
            <Router>
                <SideNavigation />
            </Router>
        );
    };

    test("Renders navigation links for all users", () => {
        renderSideNavigation({ userId: 1, isAdmin: false });

        expect(screen.getByText("Home")).toBeInTheDocument();
        expect(screen.getByText("Collections")).toBeInTheDocument();
        expect(screen.getByText("My Flashcard Sets")).toBeInTheDocument();
        expect(screen.getByText("My Collections")).toBeInTheDocument();
        
        expect(screen.getByText("Home").closest("a")).toHaveAttribute("href", "/sets");
        expect(screen.getByText("Collections").closest("a")).toHaveAttribute("href", "/collections");
        expect(screen.getByText("My Flashcard Sets").closest("a")).toHaveAttribute("href", "/users/1/sets");
        expect(screen.getByText("My Collections").closest("a")).toHaveAttribute("href", "/users/1/collections");
    });

    test("Does not render admin specific links for non-admin users", () => {
        renderSideNavigation({ userId: 1, isAdmin: false });

        expect(screen.queryByText("Users")).not.toBeInTheDocument();
        expect(screen.queryByText("Settings")).not.toBeInTheDocument();
    });

    test("Renders admin-specific links for admin users", () => {
        renderSideNavigation({ userId: 1, isAdmin: true });

        expect(screen.getByText("Users")).toBeInTheDocument();
        expect(screen.getByText("Settings")).toBeInTheDocument();
        
        expect(screen.getByText("Users").closest("a")).toHaveAttribute("href", "/users");
        expect(screen.getByText("Settings")).not.toHaveAttribute("href");
    });

    test("Renders correctly with different user IDs", () => {
        renderSideNavigation({ userId: 42, isAdmin: false });

        expect(screen.getByText("My Flashcard Sets").closest("a")).toHaveAttribute("href", "/users/42/sets");
        expect(screen.getByText("My Collections").closest("a")).toHaveAttribute("href", "/users/42/collections");
    });
});