import {
    getCreateOrderEndpoint,
    getOrderEndpoint,
    getOrdersEndpoint,
    getOrderStatusChangeEndpoint,
} from '../client/ApiEndpoints';
import { HttpClient } from '../client/HttpClient';
import { ApiResponse } from '../models/ApiResponse';
import { Order } from '../models/Order';
import { OrderItem } from '../models/OrderItem';
import { OrderStatusString } from '../models/OrderStatus';
import { makeApiRequest } from '../utils/makeApiRequest';

export class OrderService {
    public async getOrders(lastId: string | null, limit: number) {
        const method = 'GET';
        const url = getOrdersEndpoint(lastId, limit);
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

    public async createOrder(restaurantId: string, items: OrderItem[]) {
        const method = 'POST';
        const url = getCreateOrderEndpoint();
        const httpRequest = HttpClient(url, {
            method,
            data: {
                restaurantId: restaurantId,
                items: items,
            },
            headers: {
                'Access-Control-Allow-Origin': true,
            },
        });

        const result = await makeApiRequest<ApiResponse<Order>>(httpRequest);
        return result;
    }

    public async orderStatusChange(orderId: string, status: OrderStatusString) {
        const method = 'PUT';
        const url = getOrderStatusChangeEndpoint(orderId);
        const httpRequest = HttpClient(url, {
            method,
            data: {
                status: status,
            },
        });
        const result = await makeApiRequest<ApiResponse<Order>>(httpRequest);
        return result;
    }
}
