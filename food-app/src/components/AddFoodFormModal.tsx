import React from 'react';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import { useTranslation } from 'react-i18next';
import { useForm } from 'react-hook-form';
import { makeStyles, Theme } from '@material-ui/core';
import { Error } from './Error';

const useStyle = makeStyles((_theme: Theme) => ({
    input: {
        marginBottom: 24,
    },
}));

interface AddFoodFormModalProps {
    open: boolean;
    handleClose: () => void;
    onSubmit: (data: AddFoodForm) => void;
    isLoading?: boolean;
    errorCode?: string | null;
}

export type AddFoodForm = {
    name: string;
    description: string;
    price: number;
};

export const AddFoodFormModal: React.FC<AddFoodFormModalProps> = (props) => {
    const { open, handleClose, onSubmit, isLoading, errorCode } = props;
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            addFood: t('labels.addFood'),
            cancel: t('labels.cancel'),
            save: t('labels.save'),
            name: t('labels.name'),
            price: t('labels.price'),
            description: t('labels.description'),
        }),
        [t],
    );
    const { input } = useStyle();
    const { register, errors, handleSubmit } = useForm<AddFoodForm>();
    return (
        <Dialog
            open={open}
            onClose={handleClose}
            aria-labelledby="form-dialog-title"
            disableEnforceFocus
        >
            <DialogTitle id="form-dialog-title">{copy.addFood}</DialogTitle>
            <form onSubmit={handleSubmit(onSubmit)}>
                <DialogContent>
                    <TextField
                        id="name"
                        name="name"
                        label={copy.name}
                        fullWidth
                        variant="outlined"
                        className={input}
                        size="small"
                        inputRef={register({ required: true })}
                        error={!!errors.name}
                    />
                    <TextField
                        id="description"
                        name="description"
                        label={copy.description}
                        fullWidth
                        variant="outlined"
                        className={input}
                        size="small"
                        inputRef={register({ required: true })}
                        error={!!errors.description}
                    />
                    <TextField
                        id="price"
                        name="price"
                        label={copy.price}
                        fullWidth
                        variant="outlined"
                        className={input}
                        size="small"
                        inputRef={register({ required: true })}
                        error={!!errors.price}
                        type="number"
                    />
                    {errorCode ? <Error errorCode={errorCode} /> : null}
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose} color="secondary">
                        {copy.cancel}
                    </Button>
                    <Button type="submit" color="primary" disabled={isLoading}>
                        {copy.save}
                    </Button>
                </DialogActions>
            </form>
        </Dialog>
    );
};
