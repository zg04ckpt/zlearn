export interface UserManagement 
{
    firstName: string;
    lastName: string;
    address: string;
    gender: number;
    dateOfBirth: string;
    refreshToken: string;
    refreshTokenExpiryTime: string;
    description: string;
    userLinks: string;
    id: string;
    userName: string;
    email: string;
    emailConfirmed: boolean;
    phoneNumber: string;
    phoneNumberConfirmed: boolean;
    twoFactorEnabled: boolean;
    lockoutEnd: string;
    lockoutEnabled: boolean;
    accessFailedCount: number;
    roles: string[]
}