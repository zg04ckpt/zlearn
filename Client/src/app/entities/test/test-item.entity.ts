export interface TestItem {
    id: string;
    name: string;
    imageUrl: string|null;
    numberOfQuestions: number;
    numberOfAttempts: number;
    isPrivate: boolean
}