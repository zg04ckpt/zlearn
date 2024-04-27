export interface Question {
    order: number
    content: string
    image: File | null
    answerA: string
    answerB: string
    answerC: string
    answerD: string
    correctAnswer: number
    mark: boolean
}
