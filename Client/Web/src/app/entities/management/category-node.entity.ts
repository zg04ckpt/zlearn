export interface CategoryNode {
    id: string;
    name: string;
    description: string;
    slug: string;
    link: string;
    children: CategoryNode[];
    isExpand: boolean
}