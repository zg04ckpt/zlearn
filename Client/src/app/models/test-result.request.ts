export interface TestResultRequest {
    score: number
    correctsCount: number
    usedTime: {
        minutes: number
        seconds: number
    }
    startTime: Date
    userInfo: string
    questionSetId: string
}