export interface UserDTO {
    id: string;
    userName: string;
    roles: string[];
    accessToken: string;
    refreshToken: string;
    expirationTime: string;
}