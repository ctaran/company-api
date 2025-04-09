import React, { useEffect } from 'react';
import { useForm, Controller } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import {
  Box,
  Button,
  TextField,
  Grid,
  Paper,
  Typography,
  CircularProgress,
  Snackbar,
  Alert,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
} from '@mui/material';
import { ArrowBack as ArrowBackIcon } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { companySchema } from '../utils/validation';
import { CreateCompanyDto, UpdateCompanyDto } from '../types/company';

interface CompanyFormProps {
  initialData?: UpdateCompanyDto;
  onSubmit: (data: CreateCompanyDto) => Promise<void>;
  isLoading: boolean;
  error: string | null;
  title: string;
  submitButtonText: string;
  successMessage?: string;
}

type FormValues = CreateCompanyDto;

const CompanyForm: React.FC<CompanyFormProps> = ({
  initialData,
  onSubmit,
  isLoading,
  error,
  title,
  submitButtonText,
  successMessage = 'Company saved successfully!',
}) => {
  const navigate = useNavigate();
  const [showSuccess, setShowSuccess] = React.useState(false);
  const [showUnsavedDialog, setShowUnsavedDialog] = React.useState(false);
  const [pendingNavigation, setPendingNavigation] = React.useState<string | null>(null);
  
  const {
    control,
    handleSubmit,
    formState: { errors, isDirty },
  } = useForm<FormValues>({
    resolver: yupResolver(companySchema) as any,
    defaultValues: initialData || {
      name: '',
      stockTicker: '',
      exchange: '',
      isin: '',
      website: '',
    },
  });

  useEffect(() => {
    if (error) {
      setShowSuccess(false);
    }
  }, [error]);

  const handleBack = () => {
    if (isDirty) {
      setShowUnsavedDialog(true);
    } else {
      navigate('/companies');
    }
  };

  const handleUnsavedDialogClose = (shouldNavigate: boolean) => {
    setShowUnsavedDialog(false);
    if (shouldNavigate && pendingNavigation) {
      navigate(pendingNavigation);
      setPendingNavigation(null);
    }
  };

  const onSubmitForm = async (data: FormValues) => {
    try {
      await onSubmit(data as CreateCompanyDto);
      setShowSuccess(true);
    } catch (err) {
      // Error is handled by the parent component
    }
  };

  return (
    <Box>
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
        <Button
          startIcon={<ArrowBackIcon />}
          onClick={handleBack}
        >
          Back to Companies
        </Button>
      </Box>

      <Paper sx={{ p: 3 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          {title}
        </Typography>

        {error && (
          <Alert severity="error" sx={{ mb: 3 }}>
            {error}
          </Alert>
        )}

        <form onSubmit={handleSubmit(onSubmitForm)}>
          <Grid container spacing={3}>
            <Grid item xs={12} md={6}>
              <Controller
                name="name"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    label="Company Name"
                    fullWidth
                    error={!!errors.name}
                    helperText={errors.name?.message}
                    disabled={isLoading}
                  />
                )}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <Controller
                name="stockTicker"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    label="Stock Ticker"
                    fullWidth
                    error={!!errors.stockTicker}
                    helperText={errors.stockTicker?.message}
                    disabled={isLoading}
                  />
                )}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <Controller
                name="exchange"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    label="Exchange"
                    fullWidth
                    error={!!errors.exchange}
                    helperText={errors.exchange?.message}
                    disabled={isLoading}
                  />
                )}
              />
            </Grid>

            <Grid item xs={12} md={6}>
              <Controller
                name="isin"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    label="ISIN"
                    fullWidth
                    error={!!errors.isin}
                    helperText={errors.isin?.message}
                    disabled={isLoading}
                    placeholder="e.g., US0378331005"
                  />
                )}
              />
            </Grid>

            <Grid item xs={12}>
              <Controller
                name="website"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    label="Website (Optional)"
                    fullWidth
                    error={!!errors.website}
                    helperText={errors.website?.message}
                    disabled={isLoading}
                    placeholder="e.g., https://www.example.com"
                  />
                )}
              />
            </Grid>

            <Grid item xs={12}>
              <Box display="flex" justifyContent="flex-end" mt={2}>
                <Button
                  type="submit"
                  variant="contained"
                  color="primary"
                  disabled={isLoading}
                  sx={{ minWidth: 120 }}
                >
                  {isLoading ? <CircularProgress size={24} /> : submitButtonText}
                </Button>
              </Box>
            </Grid>
          </Grid>
        </form>
      </Paper>

      <Snackbar
        open={showSuccess}
        autoHideDuration={6000}
        onClose={() => setShowSuccess(false)}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
      >
        <Alert onClose={() => setShowSuccess(false)} severity="success" sx={{ width: '100%' }}>
          {successMessage}
        </Alert>
      </Snackbar>

      <Dialog
        open={showUnsavedDialog}
        onClose={() => handleUnsavedDialogClose(false)}
      >
        <DialogTitle>Unsaved Changes</DialogTitle>
        <DialogContent>
          <DialogContentText>
            You have unsaved changes. Are you sure you want to leave? Your changes will be lost.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => handleUnsavedDialogClose(false)}>Cancel</Button>
          <Button onClick={() => handleUnsavedDialogClose(true)} color="primary">
            Leave
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default CompanyForm; 