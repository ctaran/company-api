import axios from 'axios';
import { Company, CreateCompanyDto, UpdateCompanyDto } from '../types/company';

const API_BASE_URL = 'http://localhost:5020/api';

const companyService = {
    getAllCompanies: async (): Promise<Company[]> => {
        const response = await axios.get(`${API_BASE_URL}/companies`);
        return response.data;
    },

    getCompanyById: async (id: number): Promise<Company> => {
        const response = await axios.get(`${API_BASE_URL}/companies/${id}`);
        return response.data;
    },

    getCompanyByIsin: async (isin: string): Promise<Company> => {
        const response = await axios.get(`${API_BASE_URL}/companies/isin/${isin}`);
        return response.data;
    },

    createCompany: async (company: CreateCompanyDto): Promise<Company> => {
        const response = await axios.post(`${API_BASE_URL}/companies`, company);
        return response.data;
    },

    updateCompany: async (id: number, company: UpdateCompanyDto): Promise<Company> => {
        const response = await axios.put(`${API_BASE_URL}/companies/${id}`, company);
        return response.data;
    },

    deleteCompany: async (id: number): Promise<void> => {
        await axios.delete(`${API_BASE_URL}/companies/${id}`);
    }
};

export default companyService; 