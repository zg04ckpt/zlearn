export interface TestDetail {
    id: string;
    name: string;
    authorName: string;
    authorId: string;
    imageUrl: string|null;
    categoryName: string;
    categorySlug: string;
    createdDate: Date;
    updatedDate: Date;
    numberOfQuestions: number;
    numberOfAttempts: number;
    description: string;
    source: string;
    isPrivate: boolean;
}