export interface QuestionSetsResponse {
    data: {
        id: string
        name: string
        description: string
        creator: string
        createdDate: Date
        updatedDate: Date
        imageUrl: string
        questionCount: number
        testTime: {
            minutes: number
            seconds: number
        }
    } []
    code: number
    message: string | null
}