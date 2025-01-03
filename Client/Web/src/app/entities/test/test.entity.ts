import { Question } from "./question.entity";

export interface Test {
    startTime: Date
    name: string;
    duration: number;
    questions: Question[];
}