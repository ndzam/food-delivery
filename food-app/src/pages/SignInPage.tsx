import { Input } from '@material-ui/core';
import React from 'react';

export const SignInPage: React.FC = () => {
    const [email, setEmail] = React.useState('');
    const [password, setPassword] = React.useState('');
    const onEmailChange = (
        event: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>,
    ) => {
        setEmail(event.target.value);
    };
    const onPasswordChange = (
        event: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>,
    ) => {
        setPassword(event.target.value);
    };
    return (
        <div>
            <div>
                <Input
                    placeholder="Email"
                    value={email}
                    onChange={onEmailChange}
                />
                <Input
                    placeholder="Password"
                    value={password}
                    onChange={onPasswordChange}
                />
            </div>
        </div>
    );
};
