import { StatusHistoryItem } from './StatusHistoryItem';

export type OrderStatusString =
    | 'placed'
    | 'canceled'
    | 'processing'
    | 'route'
    | 'delivered'
    | 'received';
export interface OrderStatus {
    currentStatus: OrderStatusString;
    statusHistory: StatusHistoryItem[];
}
