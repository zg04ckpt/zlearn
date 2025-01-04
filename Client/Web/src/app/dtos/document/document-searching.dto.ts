export interface DocumentSearchingDTO {
    pageIndex: number,
    pageSize: number;
    totalPage: number;
    name: string|null;
    categorySlug: string|null;
}