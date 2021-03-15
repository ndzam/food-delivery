import React from 'react';
import { useApiRequestHook } from '../../api/hooks/useApiRequestHook';
import { OrderDetailsPageStyles } from './styles';
import { Loading } from '../../components/Loading';
import {
    Button,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography,
} from '@material-ui/core';
import { useTranslation } from 'react-i18next';
import { getDate } from '../../utils/dateConverterUtils';
import { Block, CheckCircleOutline } from '@material-ui/icons';
import { ConfirmDialog } from '../../components/ConfirmDialog';

export const UsersPage: React.FC = () => {
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            users: t('labels.users'),
        }),
        [t],
    );
    const {
        page,
        content,
        details,
        title,
        tableHolder,
        table,
        options,
        optionItem,
    } = OrderDetailsPageStyles();

    return (
        <div className={page}>
            <Typography variant="h3" color="primary" className={title}>
                {copy.users}
            </Typography>
        </div>
    );
};
