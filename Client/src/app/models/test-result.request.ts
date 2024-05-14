export interface TestResultRequest {
    score: number
    correctsCount: number
    usedTime: {
        minutes: number
        seconds: number
    }
    startTime: string
    userInfo: string
    questionSetId: string
}