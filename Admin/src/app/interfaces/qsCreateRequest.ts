import { Question } from "./questionRequest"

export interface QSCreateRequest {
    name: string
    description: string
    creator: string
    image: File
    questions: Question[]
    testTime: {
        minutes: number
        seconds: number
    }
}
