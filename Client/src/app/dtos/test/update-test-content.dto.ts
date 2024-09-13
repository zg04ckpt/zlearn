export interface TestUpdateContent 
{
    name : string;
    imageUrl : string;
    description : string;
    source : string;
    duration : number;
    isPrivate : boolean;
    questions : {
        id: string|null;
        content: string;
        imageUrl: string;
        answerA: string;
        answerB: string;
        answerC: string|null;
        answerD: string|null;
        correctAnswer: number;
    }[];
}