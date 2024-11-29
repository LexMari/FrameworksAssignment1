import {cleanup, render, screen} from "@testing-library/react";
import FlashcardSetComment from "../../../components/flashcardsets/FlashcardSetComment";
import * as dateTimeUtils from "../../../utils/dateTime";

afterEach(cleanup);

describe("FlashcardSetComment Component", () => {
    test("Renders comment details correctly", () => {
        
        const originalFormatTimestamp = dateTimeUtils.formatTimestamp;
        dateTimeUtils.formatTimestamp = (timestamp) => `Formatted Date: ${timestamp}`;
        
        const comment = {
            comment: "This is a test comment.",
            author: { username: "test_user" },
            created_at: "2024-01-01T12:00:00Z",
        };
        
        render(<FlashcardSetComment comment={comment} />);
        
        expect(screen.getByText("Comment")).toBeInTheDocument();
        expect(screen.getByText("Author")).toBeInTheDocument();
        expect(screen.getByText("Comment At")).toBeInTheDocument();
        
        expect(screen.getByText("This is a test comment.")).toBeInTheDocument();
        expect(screen.getByText("test_user")).toBeInTheDocument();
        expect(screen.getByText("Formatted Date: 2024-01-01T12:00:00Z")).toBeInTheDocument();
        
        dateTimeUtils.formatTimestamp = originalFormatTimestamp;
    });
});