export interface User {
    id: string;
    userName: string;
    fullName: string;
    profileImage: string|null;
    roles: string[];
}