export interface Question {
    id: string;
    content: string;
    imageUrl: string|null;
    answerA: string;
    answerB: string;
    answerC: string|null;
    answerD: string|null;
    selectedAnswer: number;
}