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
import { Restaurant } from '../api/models/Restaurant';
import { Error } from './Error';

const useStyle = makeStyles((theme: Theme) => ({
    input: {
        marginBottom: 24,
    },
}));

interface EditRestaurantFormModalProps {
    open: boolean;
    handleClose: () => void;
    onSubmit: (data: EditRestaurantForm) => void;
    data: Restaurant;
    isLoading?: boolean;
    errorCode?: string | null;
}

export type EditRestaurantForm = {
    id: string;
    name: string;
    description: string;
};

export const EditRestaurantFormModal: React.FC<EditRestaurantFormModalProps> = (
    props,
) => {
    const { open, handleClose, onSubmit, isLoading, data, errorCode } = props;
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            editRestaurant: t('labels.editRestaurant'),
            cancel: t('labels.cancel'),
            save: t('labels.save'),
            name: t('labels.name'),
            description: t('labels.description'),
        }),
        [t],
    );
    const { input } = useStyle();
    const { register, errors, handleSubmit } = useForm<EditRestaurantForm>({
        defaultValues: {
            name: data.name,
            id: data.id,
            description: data.description,
        },
    });
    return (
        <Dialog
            open={open}
            onClose={handleClose}
            aria-labelledby="form-dialog-title"
            disableEnforceFocus
        >
            <DialogTitle id="form-dialog-title">
                {copy.editRestaurant}
            </DialogTitle>
            <form onSubmit={handleSubmit(onSubmit)}>
                <input id="id" name="id" hidden ref={register()} />
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
