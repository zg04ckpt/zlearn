export interface CreateTestDTO 
{
    name: string,
    image: File|null,
    description: string,
    source: string,
    authorName: string,
    authorId: string,
    duration: number,
    questions: {
        content: string,
        image: File|null,
        answerA: string,
        answerB: string,
        answerC: string|null,
        answerD: string|null,
        correctAnswer: number
    }[];
}
