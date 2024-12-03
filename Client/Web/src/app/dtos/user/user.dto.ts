export interface UserDTO {
    id: string;
    username: string;
    fullName: string;
    profileImage: string|null;
    roles: string[];
    accessToken: string;
    refreshToken: string;
    expirationTime: string;
}