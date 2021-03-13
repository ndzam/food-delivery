import { User } from '../api/models/User';

const USER_STORAGE_KEY = 'user';

export function saveUser(user: User) {
    localStorage.setItem(USER_STORAGE_KEY, JSON.stringify(user));
}

export function removeUser() {
    localStorage.removeItem(USER_STORAGE_KEY);
}

export function getUser(): User | null {
    try {
        const userJson = localStorage.getItem(USER_STORAGE_KEY);
        if (userJson) {
            const user = JSON.parse(userJson);
            return user as User;
        }
    } catch {
        console.log('warning', 'storage is empty');
    }
    return null;
}
