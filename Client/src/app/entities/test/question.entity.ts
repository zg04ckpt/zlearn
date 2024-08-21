export interface Question {
    content: string;
    imageUrl: string|null;
    answerA: string;
    answerB: string;
    answerC: string|null;
    answerD: string|null;
    selectedAnswer: number;
}