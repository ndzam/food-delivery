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
import { Food } from '../api/models/Food';
import { Error } from './Error';

const useStyle = makeStyles((theme: Theme) => ({
    input: {
        marginBottom: 24,
    },
}));

interface EditFoodFormModalProps {
    open: boolean;
    handleClose: () => void;
    onSubmit: (data: EditFoodForm) => void;
    data: Food;
    isLoading?: boolean;
    errorCode?: string | null;
}

export type EditFoodForm = {
    id: string;
    name: string;
    description: string;
    price: number;
};

export const EditFoodFormModal: React.FC<EditFoodFormModalProps> = (props) => {
    const { open, handleClose, onSubmit, isLoading, data, errorCode } = props;
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            editFood: t('labels.editFood'),
            cancel: t('labels.cancel'),
            save: t('labels.save'),
            name: t('labels.name'),
            description: t('labels.description'),
            price: t('labels.price'),
        }),
        [t],
    );
    const { input } = useStyle();
    const { register, errors, handleSubmit } = useForm<EditFoodForm>({
        defaultValues: {
            name: data.name,
            id: data.id,
            description: data.description,
            price: data.price,
        },
    });
    return (
        <Dialog
            open={open}
            onClose={handleClose}
            aria-labelledby="form-dialog-title"
            disableEnforceFocus
        >
            <DialogTitle id="form-dialog-title">{copy.editFood}</DialogTitle>
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
                    <TextField
                        id="price"
                        name="price"
                        label={copy.price}
                        fullWidth
                        variant="outlined"
                        className={input}
                        size="small"
                        inputRef={register({ required: true })}
                        error={!!errors.description}
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
