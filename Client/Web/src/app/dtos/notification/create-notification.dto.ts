import { NotificationType } from "../../entities/notification/notification.dto";

export interface CreateNotificationDTO {
    title: string,
    message: string,
    type: NotificationType,
    userId: string|null
}