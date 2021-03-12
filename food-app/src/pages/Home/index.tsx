import React from 'react';
import { useSelector } from 'react-redux';
import { AppState } from '../../store';

export const Home: React.FC = () => {
    const { user } = useSelector((state: AppState) => {
        return {
            user: state.UserStore.user,
        };
    });
    return (
        <div>
            <pre>{JSON.stringify(user, null, 2)}</pre>
        </div>
    );
};
