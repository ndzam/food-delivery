import {
    getCreateFoodEndpoint,
    getDeleteFoodEndpoint,
    getFoodsEndpoint,
    getEditFoodEndpoint,
} from '../client/ApiEndpoints';
import { HttpClient } from '../client/HttpClient';
import { ApiResponse } from '../models/ApiResponse';
import { Food } from '../models/Food';
import { makeApiRequest } from '../utils/makeApiRequest';

export class FoodService {
    public async getFoods(restaurantId: string) {
        const method = 'GET';
        const url = getFoodsEndpoint(restaurantId);
        const httpRequest = HttpClient(url, {
            method,
        });

        const result = await makeApiRequest<ApiResponse<Array<Food>>>(
            httpRequest,
        );
        return result;
    }

    public async createFood(
        restaurantId: string,
        name: string,
        description: string,
        price: number,
    ) {
        const method = 'POST';
        const url = getCreateFoodEndpoint(restaurantId);
        const httpRequest = HttpClient(url, {
            method,
            data: {
                name: name,
                description: description,
                price: price,
            },
            headers: {
                'Access-Control-Allow-Origin': true,
            },
        });

        const result = await makeApiRequest<ApiResponse<Food>>(httpRequest);
        return result;
    }

    public async editFood(
        restaurantId: string,
        foodId: string,
        name: string,
        description: string,
        price: number,
    ) {
        const method = 'PUT';
        const url = getEditFoodEndpoint(restaurantId, foodId);
        //TODO id gvinda
        const httpRequest = HttpClient(url, {
            method,
            data: {
                name: name,
                description: description,
            },
            headers: {
                'Access-Control-Allow-Origin': true,
            },
        });

        const result = await makeApiRequest<ApiResponse<Food>>(httpRequest);
        return result;
    }

    public async deleteRestaurant(restaurantId: string, foodId: string) {
        const method = 'DELETE';
        const url = getDeleteFoodEndpoint(restaurantId, foodId);
        //TODO id gvinda
        const httpRequest = HttpClient(url, {
            method,
            data: {},
            headers: {
                'Access-Control-Allow-Origin': true,
            },
        });

        const result = await makeApiRequest<ApiResponse<Food>>(httpRequest);
        return result;
    }
}
