import React from 'react';
import { AppBar, Box, Container, CssBaseline, Toolbar, Typography, Button } from '@mui/material';
import { Outlet, Link as RouterLink } from 'react-router-dom';
import { Business as BusinessIcon } from '@mui/icons-material';

const MainLayout: React.FC = () => {
    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
            <CssBaseline />
            <AppBar position="static">
                <Toolbar>
                    <BusinessIcon sx={{ mr: 2 }} />
                    <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                        Company Management System
                    </Typography>
                    <Button 
                        color="inherit" 
                        component={RouterLink} 
                        to="/companies"
                    >
                        Companies
                    </Button>
                </Toolbar>
            </AppBar>
            <Container component="main" sx={{ mt: 4, mb: 4, flex: 1 }}>
                <Outlet />
            </Container>
            <Box component="footer" sx={{ py: 3, px: 2, mt: 'auto', backgroundColor: (theme) => theme.palette.grey[200] }}>
                <Container maxWidth="sm">
                    <Typography variant="body2" color="text.secondary" align="center">
                        © {new Date().getFullYear()} Company Management System
                    </Typography>
                </Container>
            </Box>
        </Box>
    );
};

export default MainLayout; 