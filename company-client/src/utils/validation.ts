import * as yup from 'yup';

export const companySchema = yup.object().shape({
  name: yup
    .string()
    .required('Company name is required')
    .min(2, 'Company name must be at least 2 characters')
    .max(100, 'Company name must not exceed 100 characters'),
  
  stockTicker: yup
    .string()
    .required('Stock ticker is required')
    .min(1, 'Stock ticker must be at least 1 character')
    .max(10, 'Stock ticker must not exceed 10 characters'),
  
  exchange: yup
    .string()
    .required('Exchange is required')
    .min(2, 'Exchange must be at least 2 characters')
    .max(50, 'Exchange must not exceed 50 characters'),
  
  isin: yup
    .string()
    .required('ISIN is required')
    .matches(/^[A-Z]{2}[A-Z0-9]{9}[0-9]$/, 'ISIN must be 12 characters with first two being letters')
    .test('first-two-letters', 'First two characters of ISIN must be letters', (value) => {
      if (!value) return false;
      return /^[A-Z]{2}/.test(value);
    }),
  
  website: yup
    .string()
    .url('Website must be a valid URL')
    .nullable()
    .optional(),
}); 