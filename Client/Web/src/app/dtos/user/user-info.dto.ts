export interface UserInfoDTO {
    id: string;
    firstName: string;
    lastName: string;
    username: string;
    email: string;
    imageUrl: string|null;
    phoneNum: string;
    gender: number;
    dayOfBirth: string;
    address: string;
    intro: string;
    socialLinks: string;
    likes: number;
    isLiked: boolean;
}