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

export function getNextStatus(
    current: OrderStatusString,
    role: 'user' | 'owner',
): OrderStatusString | null {
    if (role === 'user') {
        if (current === 'placed') {
            return 'canceled';
        }
        if (current === 'delivered') {
            return 'received';
        }
    } else if (role === 'owner') {
        if (current === 'placed') {
            return 'processing';
        }
        if (current === 'processing') {
            return 'route';
        }
        if (current === 'route') {
            return 'delivered';
        }
    }
    return null;
}
