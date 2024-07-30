export interface UserDetail {
    firstName: string,
    lastName: string
    dob: string,
    gender: Gender,
    email: string,
    phone: string,
    address: string,
    description: string,
    links: string
}

export enum Gender {
    Male,
    Female,
    Other
}