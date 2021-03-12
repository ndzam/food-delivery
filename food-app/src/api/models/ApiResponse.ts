export interface ApiResponse<T> {
    success: boolean;
    errorCode?: string | null;
    data?: T | null;
}
