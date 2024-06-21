import { Time } from "@angular/common"

export interface Week {
    name: string,
    start: Date,
    end: Date,
    t2: Session[],
    t3: Session[],
    t4: Session[],
    t5: Session[],
    t6: Session[],
    t7: Session[],
    cn: Session[]
}

interface Session {
    start: Time,
    end: Time,
    nameSub: string,
    room: string,
    teacher: string,
    group: string
}