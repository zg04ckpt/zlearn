import { Time } from "@angular/common"

export interface Session {
    start: Time,
    end: Time,
    nameSub: String,
    room: String,
    teacher: String,
    group: String
}