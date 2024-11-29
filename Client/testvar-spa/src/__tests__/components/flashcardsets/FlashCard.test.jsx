import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import { useConfirm } from "material-ui-confirm";
import FlashCard from "../../../components/flashcardsets/FlashCard";

jest.mock("material-ui-confirm", () => ({
    useConfirm: jest.fn(),
}));

describe("FlashCard Component", () => {
    const mockEditCardHandler = jest.fn();
    const mockDeleteCardHandler = jest.fn();

    const sampleCard = {
        id: 1,
        question: "TEST_QUESTION",
        answer: "TEST_ANSWER",
        difficulty: "Medium",
    };

    beforeEach(() => {
        jest.clearAllMocks();
    });

    test("Renders card details", () => {
        render(<FlashCard card={sampleCard} allowEdit={true} />);
        expect(screen.getByText(sampleCard.question)).toBeInTheDocument();
        expect(screen.getByText(/click to reveal answer/i)).toBeInTheDocument();
        expect(screen.getByText(/medium/i)).toBeInTheDocument();
    });

    test("Toggles reveal state on click", () => {
        render(<FlashCard card={sampleCard} allowEdit={true} />);

        const revealText = screen.getByText(/click to reveal answer/i);
        fireEvent.click(revealText);

        expect(revealText).not.toBeVisible();
        expect(screen.getByText(sampleCard.answer)).toBeVisible();

        fireEvent.click(screen.getByText(sampleCard.answer));
        expect(revealText).toBeVisible();
    });

    test("Calls edit handler on edit icon click", () => {
        render(
            <FlashCard
                card={sampleCard}
                allowEdit={true}
                editCardHandler={mockEditCardHandler}
            />
        );

        const editButton = screen.getByTitle(/edit this card/i);
        fireEvent.click(editButton);

        expect(mockEditCardHandler).toHaveBeenCalledTimes(1);
        expect(mockEditCardHandler).toHaveBeenCalledWith(sampleCard);
    });

    test("Calls delete handler on delete icon click", async () => {
        const confirmMock = jest.fn().mockResolvedValueOnce();
        useConfirm.mockReturnValue(confirmMock);

        render(
            <FlashCard
                card={sampleCard}
                allowEdit={true}
                deleteCardHandler={mockDeleteCardHandler}
            />
        );

        const deleteButton = screen.getByTitle(/delete this card/i);
        fireEvent.click(deleteButton);

        expect(confirmMock).toHaveBeenCalledTimes(1);
        await confirmMock();

        expect(mockDeleteCardHandler).toHaveBeenCalledTimes(1);
        expect(mockDeleteCardHandler).toHaveBeenCalledWith(sampleCard);
    });

    test("Reveals state correctly", () => {
        render(<FlashCard card={sampleCard} defaultReveal={true} />);
        expect(screen.getByText(sampleCard.answer)).toBeVisible();
        expect(screen.queryByText(/click to reveal answer/i)).not.toBeVisible();
    });
});