import { Gender } from "../../enums/gender.enum";

export interface UserInfo {
    firstName: string;
    lastName: string;
    username: string;
    email: string;
    imageUrl: string|null;
    phoneNum: string;
    gender: Gender;
    dayOfBirth: string;
    address: string;
    intro: string;
    socialLinks: {name: string, url: string}[];
}