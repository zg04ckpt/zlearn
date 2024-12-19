export interface UpdateDocumentDTO {
    name: string;
    categoryId: string;
    description: string;
    fileName: string;
    imageUrl: string|null;
    newImage: File|null;
    newSourceFile: File|null;
    previewImages: {
        id: string|null,
        imageUrl: string,
        image: File|null
    }[];
    paymentInfo: string|null;
}