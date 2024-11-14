import React from "react";
import MasterLayout from "./layouts/master-layout";
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';

const darkTheme = createTheme({
  palette: {
    mode: 'dark',
  },
});

export default function App() {
  return (
      <ThemeProvider theme={darkTheme}>
        <CssBaseline />
        <MasterLayout />
      </ThemeProvider>
  );
};
