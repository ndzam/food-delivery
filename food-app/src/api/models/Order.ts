import { OrderItem } from './OrderItem';
import { OrderStatus } from './OrderStatus';

export interface Order {
    orderId: string;
    userId: string;
    restaurantOwnerId: string;
    restaurantId: string;
    Items: OrderItem[];
    totalPrice: number;
    status: OrderStatus;
    date: number;
    isRestaurantDeleted: boolean;
    isUserBlocked: boolean;
}
