import {
    getCreateOrderEndpoint,
    getOrderEndpoint,
    getOrdersEndpoint,
    getOrderStatusChangeEndpoint,
} from '../client/ApiEndpoints';
import { HttpClient } from '../client/HttpClient';
import { ApiResponse } from '../models/ApiResponse';
import { Order } from '../models/Order';
import { makeApiRequest } from '../utils/makeApiRequest';

export class OrderService {
    public async getOrders() {
        const method = 'GET';
        const url = getOrdersEndpoint();
        const httpRequest = HttpClient(url, {
            method,
        });
        const result = await makeApiRequest<ApiResponse<Order[]>>(httpRequest);
        return result;
    }

    public async getOrder(orderId: string) {
        const method = 'GET';
        const url = getOrderEndpoint(orderId);
        const httpRequest = HttpClient(url, {
            method,
        });
        const result = await makeApiRequest<ApiResponse<Order>>(httpRequest);
        return result;
    }

    public async createOrder() {
        const method = 'POST';
        const url = getCreateOrderEndpoint();
        const httpRequest = HttpClient(url, {
            method,
            data: {},
            headers: {
                'Access-Control-Allow-Origin': true,
            },
        });

        const result = await makeApiRequest<ApiResponse<Order>>(httpRequest);
        return result;
    }

    public async orderStatusChange(orderId: string, status: string) {
        const method = 'PUT';
        const url = getOrderStatusChangeEndpoint(orderId);
        const httpRequest = HttpClient(url, {
            method,
            data: {
                status: status,
            },
            headers: {
                'Access-Control-Allow-Origin': true,
            },
        });
        const result = await makeApiRequest<ApiResponse<Order>>(httpRequest);
        return result;
    }
}
