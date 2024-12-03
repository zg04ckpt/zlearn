import { Question } from "./question.entity";

export interface Test {
    name: string;
    duration: number;
    questions: Question[];
}