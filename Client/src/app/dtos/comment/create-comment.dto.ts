export interface CreateCommentDTO {
    content: string;
    parentId: string|null;
    testId: string;
}