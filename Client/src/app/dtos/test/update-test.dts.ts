export interface UpdateTestDTO 
{
    name : string;
    image : File|null;
    imageUrl: string;
    description : string;
    source : string;
    duration : number;
    categorySlug: string,
    isPrivate : boolean;
    questions : {
        id: string|null;
        content : string;
        image : File|null;
        imageUrl : string|null;
        answerA : string;
        answerB : string;
        answerC : string|null;
        answerD : string|null;
        correctAnswer : number;
    }[];
}