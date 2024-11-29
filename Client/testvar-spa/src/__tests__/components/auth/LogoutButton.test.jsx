import { cleanup, render, screen } from '@testing-library/react'
import LogoutButton from '../../../components/auth/LogoutButton';
import userEvent from "@testing-library/user-event";
import {act} from "react";

afterEach(cleanup);

test("Test LogoutButton component", async () => {
    
    const authService = require('../../../services/AuthService');
    const spy = jest.spyOn(authService, 'logout');

    render(<LogoutButton />);

    const element = screen.getByText(/Log Out/i);
    expect(element).toBeInTheDocument();
    
    await act( async () => userEvent.click(element));
    expect(spy).toHaveBeenCalled();
})