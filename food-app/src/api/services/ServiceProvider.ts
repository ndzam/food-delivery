import { AuthService } from './AuthService';
import { FoodService } from './FoodService';
import { RestaurantService } from './RestaurantService';

export function getAuthService() {
    return new AuthService();
}

export function getRestaurantService() {
    return new RestaurantService();
}

export function getFoodService() {
    return new FoodService();
}
