export interface UpdateTestDTO 
{
    name : string;
    image : File|null;
    imageUrl: string;
    description : string;
    source : string;
    duration : number;
    isPrivate : boolean;
    questions : {
        id: string|null;
        content: string;
        image : File|null;
        imageUrl: string;
        answerA: string;
        answerB: string;
        answerC: string|null;
        answerD: string|null;
        correctAnswer: number;
    }[];
}