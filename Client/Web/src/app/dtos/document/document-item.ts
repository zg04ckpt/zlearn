export interface DocumentItemDTO {
    id: string;
    name: string;
    imageUrl: string;
    authorName: string;
    authorId: string;
    updatedAt: Date;
    downloadedCount: number;
    originPrice: number;
    lastPrice: number;
    documentType: string;
}