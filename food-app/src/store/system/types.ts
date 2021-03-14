import { User } from '../../api/models/User';

export const SET_ALERT = 'SET_ALERT';
export const CLEAR_ALERT = 'CLEAR_ALERT';

export interface SetAlert {
    type: typeof SET_ALERT;
    alert: string;
}

export interface ClearAlert {
    type: typeof CLEAR_ALERT;
}

export type SystemActions = SetAlert | ClearAlert;
