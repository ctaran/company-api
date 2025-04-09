import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import CompanyForm from '../components/CompanyForm';
import companyService from '../services/companyService';
import { CreateCompanyDto, UpdateCompanyDto } from '../types/company';
import { CircularProgress, Alert, Box } from '@mui/material';

const EditCompany: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [initialData, setInitialData] = useState<UpdateCompanyDto | null>(null);
    const [loadingData, setLoadingData] = useState(true);

    useEffect(() => {
        const fetchCompany = async () => {
            if (!id) return;
            
            try {
                setLoadingData(true);
                const data = await companyService.getCompanyById(parseInt(id));
                setInitialData(data);
            } catch (err) {
                setError('Failed to fetch company details. Please try again later.');
                console.error('Error fetching company:', err);
            } finally {
                setLoadingData(false);
            }
        };

        fetchCompany();
    }, [id]);

    const handleSubmit = async (data: CreateCompanyDto) => {
        if (!id) return;
        
        try {
            setIsLoading(true);
            setError(null);
            await companyService.updateCompany(parseInt(id), data as UpdateCompanyDto);
            navigate('/companies');
        } catch (err: any) {
            setError(err.response?.data?.message || 'Failed to update company. Please try again.');
            console.error('Error updating company:', err);
        } finally {
            setIsLoading(false);
        }
    };

    if (loadingData) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" minHeight="400px">
                <CircularProgress />
            </Box>
        );
    }

    if (error && !initialData) {
        return (
            <Box>
                <Alert severity="error">{error}</Alert>
            </Box>
        );
    }

    return (
        <CompanyForm
            initialData={initialData || undefined}
            onSubmit={handleSubmit}
            isLoading={isLoading}
            error={error}
            title="Edit Company"
            submitButtonText="Update Company"
        />
    );
};

export default EditCompany; 