export interface Company {
    id: number;
    name: string;
    stockTicker: string;
    exchange: string;
    isin: string;
    website?: string;
    createdAt: string;
    updatedAt: string;
}

export interface CreateCompanyDto {
    name: string;
    stockTicker: string;
    exchange: string;
    isin: string;
    website?: string;
}

export interface UpdateCompanyDto extends CreateCompanyDto {
    id: number;
} 