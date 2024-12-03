export interface CommentDTO {
    id: string;
    content: string;
    createdAt: Date;
    likes: number;
    parentId: string|null;
    userName: string;
    userId: string;
    userAvatar: string|null;
    childsId: string[]
}