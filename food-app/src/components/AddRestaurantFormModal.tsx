import React from 'react';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import { useTranslation } from 'react-i18next';
import { useForm } from 'react-hook-form';
import { makeStyles, Theme } from '@material-ui/core';

const useStyle = makeStyles((theme: Theme) => ({
    input: {
        marginBottom: 24,
    },
}));

interface AddRestaurantFormModalProps {
    open: boolean;
    handleClose: () => void;
    onSubmit: (data: AddRestaurantForm) => void;
    isLoading?: boolean;
}

export type AddRestaurantForm = {
    name: string;
    description: string;
};

export const AddRestaurantFormModal: React.FC<AddRestaurantFormModalProps> = (
    props,
) => {
    const { open, handleClose, onSubmit, isLoading } = props;
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            addRestaurant: t('labels.addRestaurant'),
            cancel: t('labels.cancel'),
            save: t('labels.save'),
            name: t('labels.name'),
            description: t('labels.description'),
        }),
        [t],
    );
    const { input } = useStyle();
    const { register, errors, handleSubmit } = useForm<AddRestaurantForm>();
    return (
        <Dialog
            open={open}
            onClose={handleClose}
            aria-labelledby="form-dialog-title"
        >
            <DialogTitle id="form-dialog-title">
                {copy.addRestaurant}
            </DialogTitle>
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
