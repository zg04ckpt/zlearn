import { Question } from "./question"

export interface QuestionSet {
    name: string
    creator: string
    description: string
    imageUrl: string
    questions: Question[]
}
