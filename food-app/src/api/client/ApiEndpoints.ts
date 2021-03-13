export function getSignInEnpoint() {
    return '/users/token';
}

export function getSignUpEnpoint() {
    return '/users';
}

export function getRestaurantsEndpoint(
    name: string | null,
    lastId: string | null,
    limit: number,
) {
    let query = '?';
    if (name) {
        query += `&name=${name}`;
    }
    if (lastId) {
        query += `&lastId=${lastId}`;
    }
    if (limit) {
        query += `&limit=${limit}`;
    }

    return `/restaurants${query}`;
}

export function getRestaurantEndpoint(id: string) {
    return `/restaurants/${id}`;
}

export function getCreateRestaurantEndpoint() {
    return '/restaurants';
}

export function getEditRestaurantEndpoint(id: string) {
    return `/restaurants/${id}`;
}

export function getDeleteRestaurantEndpoint(id: string) {
    return `/restaurants/${id}`;
}

export function getFoodsEndpoint(restaurantId: string) {
    return `/restaurants${restaurantId}/foods`;
}

export function getCreateFoodEndpoint(restaurantId: string) {
    return `/restaurants${restaurantId}/foods`;
}

export function getEditFoodEndpoint(restaurantId: string, foodId: string) {
    return `/restaurants${restaurantId}/foods/${foodId}`;
}

export function getDeleteFoodEndpoint(restaurantId: string, foodId: string) {
    return `/restaurants${restaurantId}/foods/${foodId}`;
}
