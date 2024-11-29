import React from "react";
import { render } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import ProtectedRoute from "../../../components/auth/ProtectedRoute";
import { useAuth } from "../../../hooks/AuthProvider";

jest.mock("../../../hooks/AuthProvider");

describe("ProtectedRoute Component", () => {
    const mockChildren = <div data-testid="protected-content">Protected Content</div>;

    beforeEach(() => {
        jest.clearAllMocks();
    });

    test("Renders children when the user is authenticated", () => {
        useAuth.mockReturnValue({ isAuthenticated: true });

        const { getByTestId } = render(
            <MemoryRouter>
                <ProtectedRoute>{mockChildren}</ProtectedRoute>
            </MemoryRouter>
        );

        expect(getByTestId("protected-content")).toBeInTheDocument();
    });
    
    test("Does not render when redirecting", () => {
        useAuth.mockReturnValue({ isAuthenticated: false });

        const { queryByTestId } = render(
            <MemoryRouter>
                <ProtectedRoute>{mockChildren}</ProtectedRoute>
            </MemoryRouter>
        );

        expect(queryByTestId("protected-content")).not.toBeInTheDocument();
    });
});