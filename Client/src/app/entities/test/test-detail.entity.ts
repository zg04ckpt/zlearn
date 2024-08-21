export interface TestDetail {
    id: string;
    name: string;
    authorName: string;
    authorId: string;
    imageUrl: string|null;
    createdDate: string;
    updatedDate: string;
    numberOfQuestions: number;
    numberOfAttempts: number;
    description: string;
    source: string;
}