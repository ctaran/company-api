import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import CompanyForm from '../components/CompanyForm';
import companyService from '../services/companyService';
import { CreateCompanyDto } from '../types/company';

const AddCompany: React.FC = () => {
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (data: CreateCompanyDto) => {
        try {
            setIsLoading(true);
            setError(null);
            await companyService.createCompany(data);
            navigate('/companies');
        } catch (err: any) {
            setError(err.response?.data?.message || 'Failed to create company. Please try again.');
            console.error('Error creating company:', err);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <CompanyForm
            onSubmit={handleSubmit}
            isLoading={isLoading}
            error={error}
            title="Add New Company"
            submitButtonText="Create Company"
        />
    );
};

export default AddCompany; 