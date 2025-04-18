import { api } from './api';
import { Company, CreateCompanyDto, UpdateCompanyDto } from '../types/company';

const companyService = {
    getAllCompanies: async (): Promise<Company[]> => {
        const response = await api.get<Company[]>('/companies');
        return response.data;
    },

    getCompanyById: async (id: number): Promise<Company> => {
        const response = await api.get<Company>(`/companies/${id}`);
        return response.data;
    },

    getCompanyByIsin: async (isin: string): Promise<Company> => {
        const response = await api.get<Company>(`/companies/isin/${isin}`);
        return response.data;
    },

    createCompany: async (company: CreateCompanyDto): Promise<Company> => {
        const response = await api.post<Company>('/companies', company);
        return response.data;
    },

    updateCompany: async (id: number, company: UpdateCompanyDto): Promise<Company> => {
        const response = await api.put<Company>(`/companies/${id}`, company);
        return response.data;
    },

    deleteCompany: async (id: number): Promise<void> => {
        await api.delete(`/companies/${id}`);
    }
};

export default companyService; 