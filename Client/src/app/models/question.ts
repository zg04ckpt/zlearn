export interface Question
{
    order: number
    content: string
    imageUrl: string | null
    answerA: string
    answerB: string
    answerC: string
    answerD: string
    correctAnswer: number
    selectedAnswer: number
    mark: boolean
}