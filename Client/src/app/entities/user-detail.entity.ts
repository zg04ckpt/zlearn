import { Gender } from "../enums/gender.enum";

export interface UserDetail {
    firstName: string;
    lastName: string;
    email: string;
    phoneNum: string;
    gender: Gender;
    dayOfBirth: string;
    address: string;
    intro: string;
    socialLinks: {name: string, url: string}[];
}