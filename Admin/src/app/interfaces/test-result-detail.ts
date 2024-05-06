export interface TestResultDetail {
    id: string
    userInfo: string
    score: number
    correctsCount: number
    startTime: Date
    usedTime: {
        minutes: number
        seconds: number
    }
    questionSetId: string
}