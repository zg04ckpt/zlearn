export interface UserDTO {
    id: string;
    username: string;
    fullName: string;
    roles: string[];
    accessToken: string;
    refreshToken: string;
    expirationTime: string;
}