import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Box,
    Button,
    Typography,
    CircularProgress,
    Alert,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogContentText,
    DialogActions,
} from '@mui/material';
import { Add as AddIcon } from '@mui/icons-material';
import CompanyTable from '../components/CompanyTable';
import companyService from '../services/companyService';
import { Company } from '../types/company';

const CompaniesList: React.FC = () => {
    const navigate = useNavigate();
    const [companies, setCompanies] = useState<Company[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [companyToDelete, setCompanyToDelete] = useState<Company | null>(null);

    const fetchCompanies = async () => {
        try {
            setLoading(true);
            setError(null);
            const data = await companyService.getAllCompanies();
            setCompanies(data);
        } catch (err) {
            setError('Failed to fetch companies. Please try again later.');
            console.error('Error fetching companies:', err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchCompanies();
    }, []);

    const handleEdit = (company: Company) => {
        navigate(`/companies/${company.id}/edit`);
    };

    const handleView = (company: Company) => {
        navigate(`/companies/${company.id}`);
    };

    const handleDelete = (company: Company) => {
        setCompanyToDelete(company);
        setDeleteDialogOpen(true);
    };

    const handleDeleteConfirm = async () => {
        if (!companyToDelete) return;

        try {
            await companyService.deleteCompany(companyToDelete.id);
            setCompanies(companies.filter(c => c.id !== companyToDelete.id));
            setDeleteDialogOpen(false);
            setCompanyToDelete(null);
        } catch (err) {
            setError('Failed to delete company. Please try again later.');
            console.error('Error deleting company:', err);
        }
    };

    const handleDeleteCancel = () => {
        setDeleteDialogOpen(false);
        setCompanyToDelete(null);
    };

    if (loading) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" minHeight="400px">
                <CircularProgress />
            </Box>
        );
    }

    return (
        <Box>
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
                <Typography variant="h4" component="h1">
                    Companies
                </Typography>
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<AddIcon />}
                    onClick={() => navigate('/companies/new')}
                >
                    Add Company
                </Button>
            </Box>

            {error && (
                <Alert severity="error" sx={{ mb: 2 }}>
                    {error}
                </Alert>
            )}

            {companies.length === 0 ? (
                <Alert severity="info">No companies found. Add your first company!</Alert>
            ) : (
                <CompanyTable
                    companies={companies}
                    onEdit={handleEdit}
                    onDelete={handleDelete}
                    onView={handleView}
                />
            )}

            <Dialog
                open={deleteDialogOpen}
                onClose={handleDeleteCancel}
            >
                <DialogTitle>Delete Company</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Are you sure you want to delete {companyToDelete?.name}? This action cannot be undone.
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleDeleteCancel}>Cancel</Button>
                    <Button onClick={handleDeleteConfirm} color="error" autoFocus>
                        Delete
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
};

export default CompaniesList; 