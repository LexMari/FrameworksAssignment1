import {cleanup, fireEvent, render, screen} from "@testing-library/react";
import { BrowserRouter as Router } from 'react-router-dom';
import {act} from "react";
import userEvent from "@testing-library/user-event";
import FlashcardSetSummary from "../../../components/flashcardsets/FlashcardSetSummary";

afterEach(cleanup);

const flashcardSet = {
    id: 1,
    name: "TEST FLASHCARD SET",
    cards: [
        {question: "Question One", answer: "One", difficulty: "Easy"},
        {question: "Question Two", answer: "Two", difficulty: "Medium"},
        {question: "Question Three", answer: "Three", difficulty: "Hard"},
    ]
};

describe("Test FlashcardSetSummary component", () => {

    test("User presentation", async () => {

        render(
            <Router>
                <FlashcardSetSummary set={flashcardSet} allowEdit={false} allowRemove={false} />
            </Router>
        );

        const nameElement = screen.getByText(/TEST FLASHCARD SET/i);
        expect(nameElement).toBeInTheDocument();

        const countElement = screen.getByText(/3 Questions/i);
        expect(countElement).toBeInTheDocument();

        const editIcon = screen.queryAllByTitle("Edit flashcard set");
        expect(editIcon).toHaveLength(0);

        const removeIcon = screen.queryAllByTitle("Remove from collection");
        expect(removeIcon).toHaveLength(0);

        const container = screen.getByRole("link");
        expect(container).toBeInTheDocument();
        const baseClasses = container.classList;

        fireEvent.mouseEnter(container);
        expect(container.classList !== baseClasses);
        fireEvent.mouseLeave(container);
        expect(container.classList === baseClasses);

        const link = screen.getByRole("link");
        expect(link).toHaveAttribute('href', '/sets/1');

    });

    test("Owner presentation", async () => {

        const handleEditClick = jest.fn();

        render(
            <Router>
                <FlashcardSetSummary
                    set={flashcardSet}
                    allowEdit={true}
                    editCallback={handleEditClick}
                    allowRemove={false} />
            </Router>
        );

        const nameElement = screen.getByText(/TEST FLASHCARD SET/i);
        expect(nameElement).toBeInTheDocument();

        const countElement = screen.getByText(/3 Questions/i);
        expect(countElement).toBeInTheDocument();

        const editIcon = screen.getByTitle("Edit flashcard set")
        expect(editIcon).toBeInTheDocument();

        await act( async () => userEvent.click(editIcon));
        expect(handleEditClick).toHaveBeenCalled();

        const removeIcon = screen.queryAllByTitle("Remove from collection")
        expect(removeIcon).toHaveLength(0);

    });

    test("Curator presentation", async () => {

        const handleRemoveClick = jest.fn();

        render(
            <Router>
                <FlashcardSetSummary
                    set={flashcardSet}
                    allowEdit={false}
                    allowRemove={true}
                    removeCallback={handleRemoveClick}

                />
            </Router>
        );

        const nameElement = screen.getByText(/TEST FLASHCARD SET/i);
        expect(nameElement).toBeInTheDocument();

        const countElement = screen.getByText(/3 Questions/i);
        expect(countElement).toBeInTheDocument();

        const editIcon = screen.queryAllByTitle("Edit flashcard set")
        expect(editIcon).toHaveLength(0);

        const removeIcon = screen.getByTitle("Remove from collection")
        expect(removeIcon).toBeInTheDocument();

        await act( async () => userEvent.click(removeIcon));
        expect(handleRemoveClick).toHaveBeenCalled();

    });

})