export interface User {
    emial: string;
    name: string;
    token: string;
    refreshToken: string;
    expiresIn: number;
    role: 'user' | 'owner';
    createAt: Date;
}
