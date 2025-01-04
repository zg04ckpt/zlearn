export interface CreateDocumentDTO {
    name: string
    categoryId: string
    description: string
    image: File|null
    sourceFile: File|null
    previewImages: File[]
    paymentInfo: string|null
}