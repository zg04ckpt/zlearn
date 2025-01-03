export interface UserDetailDTO {
    firstName: string;
    lastName: string;
    email: string;
    image: File|null;
    imageUrl: string|null;
    phoneNum: string;
    gender: number;
    dayOfBirth: Date;
    address: string;
    intro: string;
    socialLinks: string;
}