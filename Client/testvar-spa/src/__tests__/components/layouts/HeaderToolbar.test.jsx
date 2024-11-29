import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import HeaderToolbar from "../../../components/layouts/HeaderToolbar";
import { useAuth } from "../../../hooks/AuthProvider";

jest.mock("../../../hooks/AuthProvider");
jest.mock("../../../components/auth/LogoutButton", () => jest.fn(() => <button>Logout</button>));

describe("HeaderToolbar Component", () => {
    const mockHandleDrawerOpen = jest.fn();

    beforeEach(() => {
        jest.clearAllMocks();
    });

    const renderHeaderToolbar = (authMock, open = false) => {
        useAuth.mockReturnValue(authMock);
        return render(
            <HeaderToolbar handleDrawerOpen={mockHandleDrawerOpen} open={open} />
        );
    };

    test("Renders title and menu button", () => {
        renderHeaderToolbar({ isAuthenticated: false });

        expect(screen.getByText("TestVar Flashcards")).toBeInTheDocument();
        expect(screen.getByRole("button", { name: /open drawer/i })).toBeInTheDocument();
    });

    test("Hides menu button when drawer is open", () => {
        renderHeaderToolbar({ isAuthenticated: false }, true);

        expect(screen.queryByRole("button", { name: /open drawer/i })).not.toBeInTheDocument();
    });

    test("Calls handleDrawerOpen when button is clicked", () => {
        renderHeaderToolbar({ isAuthenticated: false });

        const menuButton = screen.getByRole("button", { name: /open drawer/i });
        fireEvent.click(menuButton);

        expect(mockHandleDrawerOpen).toHaveBeenCalledTimes(1);
    });
});