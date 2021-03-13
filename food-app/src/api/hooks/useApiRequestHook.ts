import { useState, useCallback } from 'react';
import { ApiResponse } from '../models/ApiResponse';

interface RequestStateSuccess<T> {
    state: 'success';
    response: T;
}

interface RequestStateFail<T> {
    state: 'fail';
    response: T;
}

interface RequestStateIdl {
    state: 'idle';
    response: null;
}

interface RequestStateLoading {
    state: 'loading';
    response: null;
}

export const requestStateIdl = (): RequestStateIdl => ({
    state: 'idle',
    response: null,
});
export const requestStateLoading = (): RequestStateLoading => ({
    state: 'loading',
    response: null,
});
export const requestStateSuccess = <T>(
    response: T,
): RequestStateSuccess<T> => ({ state: 'success', response: response });
export const requestStateFail = <T>(response: T): RequestStateFail<T> => ({
    state: 'fail',
    response: response,
});

export type RequestState<T> =
    | RequestStateLoading
    | RequestStateIdl
    | RequestStateFail<T>
    | RequestStateSuccess<T>;

export const useApiReqeustState = <T>() => {
    const [request, setRequest] = useState<RequestState<T>>(requestStateIdl());
    return [request, setRequest] as const;
};

export const useApiRequestHook = <T>() => {
    const [requestState, setRequestState] = useApiReqeustState<
        ApiResponse<T>
    >();

    const makeRequest = useCallback(
        async (request: Promise<ApiResponse<T>>) => {
            setRequestState(requestStateLoading());
            const response = await request;
            console.log('IN CALL', response);
            if (response.success) {
                setRequestState(
                    requestStateSuccess<ApiResponse<T>>(
                        response as ApiResponse<T>,
                    ),
                );
            } else {
                setRequestState(
                    requestStateFail<ApiResponse<T>>(
                        response as ApiResponse<T>,
                    ),
                );
            }
        },
        [setRequestState],
    );

    return [requestState, makeRequest] as const;
};
