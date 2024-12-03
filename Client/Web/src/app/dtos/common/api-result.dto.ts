export interface APIResult<T> {
    success: boolean;
    message: string|null;
    data?: T;
}

export interface APIError {
    success: boolean
    message: string|null;
    details: string[]
}