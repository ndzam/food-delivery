import { StatusHistoryItem } from './StatusHistoryItem';

export interface OrderStatus {
    currentStatus: string;
    statusHistory: StatusHistoryItem[];
}
