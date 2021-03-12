import { getSignInEnpoint, getSignUpEnpoint } from '../client/ApiEndpoints';
import { HttpClient } from '../client/HttpClient';
import { ApiResponse } from '../models/ApiResponse';
import { User } from '../models/User';
import { makeApiRequest } from '../utils/makeApiRequest';

export class AuthService {
    public async signIn(email: string, password: string) {
        const method = 'POST';
        const url = getSignInEnpoint();
        const httpRequest = HttpClient(url, {
            method,
            data: {
                email: email,
                password: password,
            },
            headers: {
                'Access-Control-Allow-Origin': true,
            },
        });

        const result = await makeApiRequest<ApiResponse<User>>(httpRequest);
        return result;
    }

    public async SignUp(
        email: string,
        name: string,
        password: string,
        confirmPassword: string,
        role: string,
    ) {
        const method = 'POST';
        const url = getSignUpEnpoint();
        const httpRequest = HttpClient(url, {
            method,
            data: {
                email: email,
                name: name,
                password: password,
                confirmPassword: confirmPassword,
                role: role,
            },
            headers: {
                'Access-Control-Allow-Origin': true,
            },
        });

        const result = await makeApiRequest<ApiResponse<User>>(httpRequest);
        return result;
    }
}
