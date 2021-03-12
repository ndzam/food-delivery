export interface User {
    emial: string;
    name: string;
    token: string;
    refreshToken: string;
    expiresIn: number;
    role: string;
    createAt: Date;
}
