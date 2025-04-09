import React from 'react';
import {
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Paper,
    IconButton,
    Tooltip,
    Link,
} from '@mui/material';
import { Edit as EditIcon, Delete as DeleteIcon, Visibility as ViewIcon } from '@mui/icons-material';
import { Company } from '../types/company';

interface CompanyTableProps {
    companies: Company[];
    onEdit: (company: Company) => void;
    onDelete: (company: Company) => void;
    onView: (company: Company) => void;
}

const CompanyTable: React.FC<CompanyTableProps> = ({ companies, onEdit, onDelete, onView }) => {
    return (
        <TableContainer component={Paper}>
            <Table sx={{ minWidth: 650 }} aria-label="company table">
                <TableHead>
                    <TableRow>
                        <TableCell>Name</TableCell>
                        <TableCell>Stock Ticker</TableCell>
                        <TableCell>Exchange</TableCell>
                        <TableCell>ISIN</TableCell>
                        <TableCell>Website</TableCell>
                        <TableCell align="right">Actions</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {companies.map((company) => (
                        <TableRow key={company.id}>
                            <TableCell component="th" scope="row">
                                {company.name}
                            </TableCell>
                            <TableCell>{company.stockTicker}</TableCell>
                            <TableCell>{company.exchange}</TableCell>
                            <TableCell>{company.isin}</TableCell>
                            <TableCell>
                                {company.website ? (
                                    <Link href={company.website} target="_blank" rel="noopener noreferrer">
                                        {company.website}
                                    </Link>
                                ) : (
                                    '-'
                                )}
                            </TableCell>
                            <TableCell align="right">
                                <Tooltip title="View Details">
                                    <IconButton
                                        size="small"
                                        onClick={() => onView(company)}
                                        color="primary"
                                    >
                                        <ViewIcon />
                                    </IconButton>
                                </Tooltip>
                                <Tooltip title="Edit">
                                    <IconButton
                                        size="small"
                                        onClick={() => onEdit(company)}
                                        color="primary"
                                    >
                                        <EditIcon />
                                    </IconButton>
                                </Tooltip>
                                <Tooltip title="Delete">
                                    <IconButton
                                        size="small"
                                        onClick={() => onDelete(company)}
                                        color="error"
                                    >
                                        <DeleteIcon />
                                    </IconButton>
                                </Tooltip>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};

export default CompanyTable; 