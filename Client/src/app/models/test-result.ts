import { TestTime } from "./test-time";

export interface TestResult
{
    score: number;
    corrects: number;
    used_time: TestTime;
}