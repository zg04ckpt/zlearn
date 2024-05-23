export interface TestResultResponse {
    data: {
        id: string
        score: number
        correctsCount: number
        usedTime: {
            minutes: number
            seconds: number
        }
        startTime: Date
        userInfo: string
        questionSetId: string
    } []
    code: number
    message: string | null
} 