export interface QuestionSet 
{
    id: string
    name: string
    description: string
    imageUrl: string
    creator: string
    createdDate: Date
    updatedDate: Date
    attemptCount: number
    numberOfQuestions: number
    testTime: {
        minutes: number
        seconds: number
    }
}