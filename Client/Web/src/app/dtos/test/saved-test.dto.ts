export interface SavedTestDTO {
    id: string;
    name: string;
    imageUrl: string|null;
    numberOfQuestions: number;
    numberOfAttempts: number;
    isPrivate: boolean,
    savedAt: Date
}