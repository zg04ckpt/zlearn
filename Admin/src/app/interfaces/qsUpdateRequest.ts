import { Question } from "./questionRequest"

export interface QSUpdateRequest {
    name: string
    description: string
    image: File | null
    questions: Question[]
    mark: boolean
}
