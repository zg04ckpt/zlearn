import { TestResult } from "./test-result";
import { TestTime } from "./test-time";

export interface Test 
{
    duration: TestTime
    status: TestStatus
    result: TestResult | null
}

export enum TestStatus
{
    InProgress = 1,
    Completed = 2,
    ShowDetail = 3
}