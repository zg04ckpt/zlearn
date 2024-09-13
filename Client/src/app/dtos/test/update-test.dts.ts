export interface UpdateTestDTO 
{
    name : string;
    image : File|null;
    description : string;
    source : string;
    duration : number;
    isPrivate : boolean;
    questions : {
        id: string|null;
        content: string;
        image : File|null;
        answerA: string;
        answerB: string;
        answerC: string|null;
        answerD: string|null;
        correctAnswer: number;
    }[];
}