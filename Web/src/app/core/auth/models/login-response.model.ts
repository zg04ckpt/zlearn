export interface LoginResponse {
    data: {
        id: string,
        userName: string,
        email: string,
        accessToken: string,
        refreshToken: string
    } | null,
    code: number,
    message: string | null
}