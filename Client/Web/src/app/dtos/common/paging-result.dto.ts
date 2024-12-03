export interface PagingResultDTO<T> 
{
    total: number;
    data: T[];
}