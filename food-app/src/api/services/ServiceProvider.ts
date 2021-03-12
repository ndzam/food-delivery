import { AuthService } from './AuthService';

export function getAuthService() {
    return new AuthService();
}
