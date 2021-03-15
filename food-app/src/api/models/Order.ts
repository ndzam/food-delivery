import { OrderItem } from './OrderItem';
import { OrderStatus } from './OrderStatus';

export interface Order {
    orderId: string;
    userId: string;
    restaurantOwnerId: string;
    restaurantName: string;
    restaurantId: string;
    items: OrderItem[];
    totalPrice: number;
    status: OrderStatus;
    date: number;
    isRestaurantDeleted: boolean;
    isUserBlocked: boolean;
    userName: string;
}
