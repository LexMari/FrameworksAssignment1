import React from "react";
import { render, fireEvent } from "@testing-library/react";
import UserDetailCard from "../../../components/users/UserDetailCard";
import { useAuth } from "../../../hooks/AuthProvider";
import { useConfirm } from "material-ui-confirm";

jest.mock("../../../hooks/AuthProvider");
jest.mock("material-ui-confirm");

describe("UserDetailCard Component", () => {
    const mockUser = { id: 1, username: "TestUser", admin: false };
    const adminUser = { id: 2, username: "TestAdmin", admin: true };
    const mockOnEdit = jest.fn();
    const mockOnDelete = jest.fn();

    beforeEach(() => {
        jest.clearAllMocks();
    });

    test("Renders the user details correctly", () => {
        useAuth.mockReturnValue({ userId: 3 });
        const { getByText, getByTitle } = render(
            <UserDetailCard user={mockUser} allowEdit={true} onEdit={mockOnEdit} onDelete={mockOnDelete} />
        );

        expect(getByText("TestUser")).toBeInTheDocument();
        expect(getByTitle("Edit user")).toBeInTheDocument();
        expect(getByTitle("Delete user")).toBeInTheDocument();
    });

    test("Renders admin icon for admin users", () => {
        useAuth.mockReturnValue({ userId: 3 });
        const { getByTestId } = render(
            <UserDetailCard user={adminUser} allowEdit={false} />
        );

        expect(getByTestId("SupervisorAccountIcon")).toBeInTheDocument();
    });

    test("Does not render edit and delete icons when allowEdit is false", () => {
        useAuth.mockReturnValue({ userId: 3 });
        const { queryByTitle } = render(
            <UserDetailCard user={mockUser} allowEdit={false} />
        );

        expect(queryByTitle("Edit user")).not.toBeInTheDocument();
        expect(queryByTitle("Delete user")).not.toBeInTheDocument();
    });

    test("Hides delete icon for currently logged-in user", () => {
        useAuth.mockReturnValue({ userId: mockUser.id });
        const { queryByTitle } = render(
            <UserDetailCard user={mockUser} allowEdit={true} />
        );

        expect(queryByTitle("Delete user")).not.toBeInTheDocument();
    });

    test("Calls onDelete when delete button is clicked", async () => {
        useAuth.mockReturnValue({ userId: 3 });
        const confirmMock = jest.fn().mockResolvedValue();
        useConfirm.mockReturnValue(confirmMock);

        const { getByTitle } = render(
            <UserDetailCard user={mockUser} allowEdit={true} onDelete={mockOnDelete} />
        );

        fireEvent.click(getByTitle("Delete user"));

        expect(confirmMock).toHaveBeenCalledWith({
            description: `Do you want to permanently delete user "TestUser".`,
        });

        await confirmMock();
        expect(mockOnDelete).toHaveBeenCalledWith(mockUser);
    });

    test("Does not call onDelete when deletion is cancelled", async () => {
        useAuth.mockReturnValue({ userId: 3 });
        const confirmMock = jest.fn().mockRejectedValue();
        useConfirm.mockReturnValue(confirmMock);

        const { getByTitle } = render(
            <UserDetailCard user={mockUser} allowEdit={true} onDelete={mockOnDelete} />
        );

        fireEvent.click(getByTitle("Delete user"));

        expect(confirmMock).toHaveBeenCalled();
        await confirmMock().catch(() => {});
        expect(mockOnDelete).not.toHaveBeenCalled();
    });
});