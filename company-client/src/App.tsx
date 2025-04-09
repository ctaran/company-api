import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { ThemeProvider, createTheme } from '@mui/material';
import MainLayout from './layouts/MainLayout';
import CompaniesList from './pages/CompaniesList';
import CompanyDetails from './pages/CompanyDetails';
import AddCompany from './pages/AddCompany';
import EditCompany from './pages/EditCompany';

const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#dc004e',
    },
  },
});

const App: React.FC = () => {
  return (
    <ThemeProvider theme={theme}>
      <Router>
        <Routes>
          <Route path="/" element={<MainLayout />}>
            <Route index element={<Navigate to="/companies" replace />} />
            <Route path="companies" element={<CompaniesList />} />
            <Route path="companies/new" element={<AddCompany />} />
            <Route path="companies/:id" element={<CompanyDetails />} />
            <Route path="companies/:id/edit" element={<EditCompany />} />
          </Route>
        </Routes>
      </Router>
    </ThemeProvider>
  );
};

export default App;
