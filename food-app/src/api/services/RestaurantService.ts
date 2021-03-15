import {
    getBlockUserEndpoint,
    getCreateRestaurantEndpoint,
    getDeleteRestaurantEndpoint,
    getEditRestaurantEndpoint,
    getRestaurantEndpoint,
    getRestaurantsEndpoint,
} from '../client/ApiEndpoints';
import { HttpClient } from '../client/HttpClient';
import { ApiResponse } from '../models/ApiResponse';
import { Restaurant } from '../models/Restaurant';
import { makeApiRequest } from '../utils/makeApiRequest';

export class RestaurantService {
    public async getRestaurants(
        name: string | null,
        lastId: string | null,
        limit: number,
    ) {
        const method = 'GET';
        const url = getRestaurantsEndpoint(name, lastId, limit);
        const httpRequest = HttpClient(url, {
            method,
        });

        const result = await makeApiRequest<ApiResponse<Restaurant[]>>(
            httpRequest,
        );
        return result;
    }

    public async getRestaurant(id: string) {
        const method = 'GET';
        const url = getRestaurantEndpoint(id);
        const httpRequest = HttpClient(url, {
            method,
        });

        const result = await makeApiRequest<ApiResponse<Restaurant>>(
            httpRequest,
        );
        return result;
    }

    public async createRestaurant(name: string, description: string) {
        const method = 'POST';
        const url = getCreateRestaurantEndpoint();
        const httpRequest = HttpClient(url, {
            method,
            data: {
                name: name,
                description: description,
            },
        });

        const result = await makeApiRequest<ApiResponse<Restaurant>>(
            httpRequest,
        );
        return result;
    }

    public async editRestaurant(id: string, name: string, description: string) {
        const method = 'PUT';
        const url = getEditRestaurantEndpoint(id);
        const httpRequest = HttpClient(url, {
            method,
            data: {
                name: name,
                description: description,
            },
        });

        const result = await makeApiRequest<ApiResponse<Restaurant>>(
            httpRequest,
        );
        return result;
    }

    public async deleteRestaurant(id: string) {
        const method = 'DELETE';
        const url = getDeleteRestaurantEndpoint(id);
        const httpRequest = HttpClient(url, {
            method,
        });

        const result = await makeApiRequest<ApiResponse<unknown>>(httpRequest);
        return result;
    }

    public async blockUser(id: string, userId: string) {
        const method = 'PUT';
        const url = getBlockUserEndpoint(id);
        const httpRequest = HttpClient(url, {
            method,
            data: {
                userId,
            },
        });

        const result = await makeApiRequest<ApiResponse<unknown>>(httpRequest);
        return result;
    }
}
