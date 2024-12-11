import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import AddComment from "../../../components/flashcardsets/AddComment";

describe("AddComment Component", () => {
    const mockSaveHandler = jest.fn();
    const mockCancelHandler = jest.fn();

    beforeEach(() => {
        jest.clearAllMocks();
    });

    test("Load page", () => {
        render(<AddComment saveHandler={mockSaveHandler} cancelHandler={mockCancelHandler} />);
        expect(screen.getByLabelText(/comment/i)).toBeInTheDocument();
        expect(screen.getByRole("button", { name: /add comment/i })).toBeInTheDocument();
        expect(screen.getByRole("button", { name: /cancel/i })).toBeInTheDocument();
    });

    test("Disables Add Comment button initially", () => {
        render(<AddComment saveHandler={mockSaveHandler} cancelHandler={mockCancelHandler} />);
        const addButton = screen.getByRole("button", { name: /add comment/i });
        expect(addButton).toBeDisabled();
    });

    test("Enables Add Comment button when comment is entered", () => {
        render(<AddComment saveHandler={mockSaveHandler} cancelHandler={mockCancelHandler} />);
        const commentField = screen.getByLabelText(/comment/i);
        const addButton = screen.getByRole("button", { name: /add comment/i });

        fireEvent.change(commentField, { target: { value: "This is a test comment" } });
        fireEvent.blur(commentField);

        expect(addButton).toBeEnabled();
    });

    test("Calls saveHandler with the comment when Add Comment button is clicked", () => {
        render(<AddComment saveHandler={mockSaveHandler} cancelHandler={mockCancelHandler} />);
        const commentField = screen.getByLabelText(/comment/i);
        const addButton = screen.getByRole("button", { name: /add comment/i });

        fireEvent.change(commentField, { target: { value: "This is a test comment" } });
        fireEvent.blur(commentField);
        fireEvent.click(addButton);

        expect(mockSaveHandler).toHaveBeenCalledTimes(1);
        expect(mockSaveHandler).toHaveBeenCalledWith("This is a test comment", null);
    });

    test("Calls cancelHandler when Cancel button is clicked", () => {
        render(<AddComment saveHandler={mockSaveHandler} cancelHandler={mockCancelHandler} />);
        const cancelButton = screen.getByRole("button", { name: /cancel/i });

        fireEvent.click(cancelButton);

        expect(mockCancelHandler).toHaveBeenCalledTimes(1);
    });

    test("Displays error message when the error prop is passed", () => {
        const errorMessage = "An error occurred while saving the comment.";
        render(<AddComment saveHandler={mockSaveHandler} cancelHandler={mockCancelHandler} error={{ message: errorMessage }} />);

        expect(screen.getByText(errorMessage)).toBeInTheDocument();
    });
});