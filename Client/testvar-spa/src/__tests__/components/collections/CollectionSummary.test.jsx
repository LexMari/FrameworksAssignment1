import React from "react";
import { render, fireEvent } from "@testing-library/react";
import { BrowserRouter as Router } from "react-router-dom";
import CollectionSummary from "../../../components/collections/CollectionSummary";

describe("CollectionSummary Component", () => {
    const mockCollection = {
        id: 1,
        comment: "TEST_COLLECTION",
        sets: [{ id: 1 }, { id: 2 }],
        user: { id: 2, username: "testuser" },
    };
    const mockDeleteCallback = jest.fn();

    beforeEach(() => {
        jest.clearAllMocks();
    });

    test("Renders collection details correctly", () => {
        const { getByText } = render(
            <Router>
                <CollectionSummary collection={mockCollection} />
            </Router>
        );

        expect(getByText("TEST_COLLECTION")).toBeInTheDocument();
    });

    test("Shows curator details when showCurator is true", () => {
        const { getByText } = render(
            <Router>
                <CollectionSummary collection={mockCollection} showCurator={true} />
            </Router>
        );

        expect(getByText("Curated by testuser")).toBeInTheDocument();
    });

    test("Does not show curator details when showCurator is false", () => {
        const { queryByText } = render(
            <Router>
                <CollectionSummary collection={mockCollection} showCurator={false} />
            </Router>
        );

        expect(queryByText("Curated by testuser")).not.toBeInTheDocument();
    });

    test("Renders delete button when allowDelete is true", () => {
        const { getByTitle } = render(
            <Router>
                <CollectionSummary
                    collection={mockCollection}
                    allowDelete={true}
                    deleteCallback={mockDeleteCallback}
                />
            </Router>
        );

        expect(getByTitle("Delete flashcard set collection")).toBeInTheDocument();
    });

    test("Does not render delete button when allowDelete is false", () => {
        const { queryByTitle } = render(
            <Router>
                <CollectionSummary collection={mockCollection} allowDelete={false} />
            </Router>
        );

        expect(queryByTitle("Delete flashcard set collection")).not.toBeInTheDocument();
    });

    test("Calls deleteCallback when delete button is clicked", () => {
        const { getByTitle } = render(
            <Router>
                <CollectionSummary
                    collection={mockCollection}
                    allowDelete={true}
                    deleteCallback={mockDeleteCallback}
                />
            </Router>
        );

        fireEvent.click(getByTitle("Delete flashcard set collection"));
        expect(mockDeleteCallback).toHaveBeenCalledWith(mockCollection.id);
    });

    test("Link navigates to the correct URL", () => {
        const { container } = render(
            <Router>
                <CollectionSummary collection={mockCollection} />
            </Router>
        );

        const link = container.querySelector("a");
        expect(link).toHaveAttribute("href", "/users/2/collections/1");
    });
});