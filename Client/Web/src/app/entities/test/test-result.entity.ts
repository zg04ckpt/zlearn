export interface TestResult 
{
    id: string;
    score: number;
    correct: number;
    startTime: Date;
    endTime: Date;
    usedTime: number;
    testId: string;
    testName: string;
    userId: string;
    userName: string;
}