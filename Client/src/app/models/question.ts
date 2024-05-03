export interface Question
{
    order: number
    content: string
    answerA: string
    answerB: string
    answerC: string
    answerD: string
    correctAnswer: number
    selectedAnswer: number
    mark: boolean
}