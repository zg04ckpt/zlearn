export interface CategoryNode {
    id: string;
    name: string;
    description: string;
    slug: string;
    children: CategoryNode[];
    isExpand: boolean
}