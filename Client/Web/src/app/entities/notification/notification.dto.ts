export interface Notification {
    id: number,
    title: string,
    message: string,
    createdAt: Date,
    isRead: boolean,
    type: NotificationType
}

export enum NotificationType {
    System = 0,
    User = 1
}