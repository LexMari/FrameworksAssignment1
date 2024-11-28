import { render, screen } from '@testing-library/react'
import LogoutButton from '../../../components/auth/LogoutButton';

test("Test logout button", () => {
    render(<LogoutButton/>);
    
    const element = screen.getByText(/Log Out/i);
    
    expect(element).toBeInTheDocument();
})