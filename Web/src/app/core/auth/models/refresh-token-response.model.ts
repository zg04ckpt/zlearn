export interface RefreshTokenResponse {
    data: {
        accessToken: string,
        refreshToken: string,
        expiryTime: Date
    } | null,
    code: number,
    message: string | null
}