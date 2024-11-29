import {cleanup, render, screen, fireEvent} from "@testing-library/react";
import FlashCardEdit from "../../../components/flashcardsets/FlashCardEdit";
import userEvent from "@testing-library/user-event";

afterEach(cleanup);

describe("FlashCardEdit Component", () => {
    const mockEditCardHandler = jest.fn();
    const mockCancelEditHandler = jest.fn();

    const sampleCard = {
        question: "TEST_QUESTION",
        answer: "TEST_ANSWER",
        difficulty: "Medium",
    };

    test("Renders component with initial values", () => {
        render(
            <FlashCardEdit
                target={sampleCard}
                editCardHandler={mockEditCardHandler}
                cancelEditHandler={mockCancelEditHandler}
            />
        );

        expect(screen.getByLabelText(/Difficulty/i)).toHaveValue("Medium");
        expect(screen.getByLabelText(/Question/i)).toHaveValue(sampleCard.question);
        expect(screen.getByLabelText(/Answer/i)).toHaveValue(sampleCard.answer);
        
        expect(screen.getByTitle("Save changes")).toBeInTheDocument();
        expect(screen.getByTitle("Cancel changes")).toBeInTheDocument();
    });

    test("Validates question and answer fields", async () => {
        render(
            <FlashCardEdit
                target={sampleCard}
                editCardHandler={mockEditCardHandler}
                cancelEditHandler={mockCancelEditHandler}
            />
        );
        
        const questionField = screen.getByLabelText(/Question/i);
        userEvent.clear(questionField);
        fireEvent.blur(questionField);

        expect(screen.getByText(/A question is required/i)).toBeInTheDocument();
        
        const answerField = screen.getByLabelText(/Answer/i);
        userEvent.clear(answerField);
        fireEvent.blur(answerField);

        expect(screen.getByText(/An answer is required/i)).toBeInTheDocument();
        
        userEvent.type(questionField, "New Question?");
        userEvent.type(answerField, "New Answer");
        fireEvent.blur(questionField);
        fireEvent.blur(answerField);

        expect(screen.queryByText(/A question is required/i)).not.toBeInTheDocument();
        expect(screen.queryByText(/An answer is required/i)).not.toBeInTheDocument();
    });

    test("Calls editCardHandler on save", () => {
        render(
            <FlashCardEdit
                target={sampleCard}
                editCardHandler={mockEditCardHandler}
                cancelEditHandler={mockCancelEditHandler}
            />
        );
        
        const questionField = screen.getByLabelText(/Question/i);
        const answerField = screen.getByLabelText(/Answer/i);

        userEvent.clear(questionField);
        userEvent.type(questionField, "Updated Question?");
        userEvent.clear(answerField);
        userEvent.type(answerField, "Updated Answer.");
        
        const saveButton = screen.getByTitle("Save changes");
        fireEvent.click(saveButton);

        expect(mockEditCardHandler).toHaveBeenCalledWith({
            ...sampleCard,
            question: "Updated Question?",
            answer: "Updated Answer.",
        });
    });

    test("Calls cancelEditHandler on cancel", () => {
        render(
            <FlashCardEdit
                target={sampleCard}
                editCardHandler={mockEditCardHandler}
                cancelEditHandler={mockCancelEditHandler}
            />
        );

        const cancelButton = screen.getByTitle("Cancel changes");
        fireEvent.click(cancelButton);

        expect(mockCancelEditHandler).toHaveBeenCalled();
    });

    test("Disables save button on invalid fields", () => {
        render(
            <FlashCardEdit
                target={sampleCard}
                editCardHandler={mockEditCardHandler}
                cancelEditHandler={mockCancelEditHandler}
            />
        );

        const saveButton = screen.getByTitle("Save changes");
        const questionField = screen.getByLabelText(/Question/i);
        const answerField = screen.getByLabelText(/Answer/i);
        
        userEvent.clear(questionField);
        userEvent.clear(answerField);
        
        expect(saveButton).toBeDisabled();
        
        userEvent.type(questionField, "Valid Question");
        userEvent.type(answerField, "Valid Answer");
        
        expect(saveButton).not.toBeDisabled();
    });
});