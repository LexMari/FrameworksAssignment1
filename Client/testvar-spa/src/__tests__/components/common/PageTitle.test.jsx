import { render, screen } from '@testing-library/react';
import PageTitle from "../../../components/common/PageTitle";

test("Page Title Test", () => {
    render(
        <PageTitle title="Page Title Text">
            <div>Child element</div>
        </PageTitle>
    );

    const elements = screen.getAllByText(/page title text/i);
    elements.map(el => {
        expect(el).toBeInTheDocument();
        expect(el.getAttribute('class')).toMatch(/MuiTypography-h/gi);
    })
    
    const child = screen.getByText(/Child element/i);
    expect(child).toBeInTheDocument()
});