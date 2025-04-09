import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
    Box,
    Paper,
    Typography,
    Grid,
    Button,
    CircularProgress,
    Alert,
    Divider,
    Link,
} from '@mui/material';
import { ArrowBack as ArrowBackIcon, Edit as EditIcon } from '@mui/icons-material';
import companyService from '../services/companyService';
import { Company } from '../types/company';

const CompanyDetails: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [company, setCompany] = useState<Company | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchCompany = async () => {
            if (!id) return;
            
            try {
                setLoading(true);
                setError(null);
                const data = await companyService.getCompanyById(parseInt(id));
                setCompany(data);
            } catch (err) {
                setError('Failed to fetch company details. Please try again later.');
                console.error('Error fetching company:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchCompany();
    }, [id]);

    const handleEdit = () => {
        navigate(`/companies/${id}/edit`);
    };

    const handleBack = () => {
        navigate('/companies');
    };

    if (loading) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" minHeight="400px">
                <CircularProgress />
            </Box>
        );
    }

    if (error) {
        return (
            <Box>
                <Button
                    startIcon={<ArrowBackIcon />}
                    onClick={handleBack}
                    sx={{ mb: 2 }}
                >
                    Back to Companies
                </Button>
                <Alert severity="error">{error}</Alert>
            </Box>
        );
    }

    if (!company) {
        return (
            <Box>
                <Button
                    startIcon={<ArrowBackIcon />}
                    onClick={handleBack}
                    sx={{ mb: 2 }}
                >
                    Back to Companies
                </Button>
                <Alert severity="info">Company not found.</Alert>
            </Box>
        );
    }

    return (
        <Box>
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
                <Button
                    startIcon={<ArrowBackIcon />}
                    onClick={handleBack}
                >
                    Back to Companies
                </Button>
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<EditIcon />}
                    onClick={handleEdit}
                >
                    Edit Company
                </Button>
            </Box>

            <Paper sx={{ p: 3 }}>
                <Typography variant="h4" component="h1" gutterBottom>
                    {company.name}
                </Typography>
                <Divider sx={{ my: 2 }} />
                
                <Grid container spacing={3}>
                    <Grid item xs={12} md={6}>
                        <Typography variant="subtitle1" color="text.secondary">
                            Stock Ticker
                        </Typography>
                        <Typography variant="body1" paragraph>
                            {company.stockTicker}
                        </Typography>
                    </Grid>
                    
                    <Grid item xs={12} md={6}>
                        <Typography variant="subtitle1" color="text.secondary">
                            Exchange
                        </Typography>
                        <Typography variant="body1" paragraph>
                            {company.exchange}
                        </Typography>
                    </Grid>
                    
                    <Grid item xs={12} md={6}>
                        <Typography variant="subtitle1" color="text.secondary">
                            ISIN
                        </Typography>
                        <Typography variant="body1" paragraph>
                            {company.isin}
                        </Typography>
                    </Grid>
                    
                    <Grid item xs={12} md={6}>
                        <Typography variant="subtitle1" color="text.secondary">
                            Website
                        </Typography>
                        <Typography variant="body1" paragraph>
                            {company.website ? (
                                <Link href={company.website} target="_blank" rel="noopener noreferrer">
                                    {company.website}
                                </Link>
                            ) : (
                                'Not provided'
                            )}
                        </Typography>
                    </Grid>
                    
                    <Grid item xs={12} md={6}>
                        <Typography variant="subtitle1" color="text.secondary">
                            Created At
                        </Typography>
                        <Typography variant="body1" paragraph>
                            {new Date(company.createdAt).toLocaleString()}
                        </Typography>
                    </Grid>
                    
                    <Grid item xs={12} md={6}>
                        <Typography variant="subtitle1" color="text.secondary">
                            Last Updated
                        </Typography>
                        <Typography variant="body1" paragraph>
                            {new Date(company.updatedAt).toLocaleString()}
                        </Typography>
                    </Grid>
                </Grid>
            </Paper>
        </Box>
    );
};

export default CompanyDetails; 