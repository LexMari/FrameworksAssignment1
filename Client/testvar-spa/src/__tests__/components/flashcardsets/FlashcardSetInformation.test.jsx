import {cleanup, render, screen} from "@testing-library/react";
import FlashcardSetInformation from "../../../components/flashcardsets/FlashcardSetInformation";
import {formatTimestamp} from "../../../utils/dateTime";

jest.mock("../../../utils/dateTime", () => ({
    formatTimestamp: jest.fn(),
}));

afterEach(cleanup);

const flashcardSet = {
    user: {
        username: "TestUser",
    },
    created_at: "2024-11-24T10:00:00Z",
    updated_at: "2024-11-24T12:00:00Z",
};

describe("Test FlashcardSetInformation", () => {
    test("Displays flashcard set information correctly", () => {
        formatTimestamp.mockImplementation((timestamp) => `Formatted: ${timestamp}`);

        render(<FlashcardSetInformation set={flashcardSet} />);
        
        const ownerLabel = screen.getByText(/Owner/i);
        expect(ownerLabel).toBeInTheDocument();

        const ownerValue = screen.getByText(/TestUser/i);
        expect(ownerValue).toBeInTheDocument();
        
        const createdLabel = screen.getByText(/Created At/i);
        expect(createdLabel).toBeInTheDocument();

        const createdValue = screen.getByText(/Formatted: 2024-11-24T10:00:00Z/i);
        expect(createdValue).toBeInTheDocument();
        
        const updatedLabel = screen.getByText(/Updated At At/i);
        expect(updatedLabel).toBeInTheDocument();

        const updatedValue = screen.getByText(/Formatted: 2024-11-24T12:00:00Z/i);
        expect(updatedValue).toBeInTheDocument();
    });
});