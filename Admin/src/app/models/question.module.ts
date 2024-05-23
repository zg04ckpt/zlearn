export class QuestionModule 
{ 
  order: number;
  content: string;
  imageUrl: string;
  answerA: string;
  answerB: string;
  answerC: string;
  answerD: string;
  correctAnswer: number;
  mark: boolean;

  selected: boolean = false;

  constructor(order: number, content: string, imageUrl: string, answerA: string, answerB: string, answerC: string, answerD: string, correctAnswer: number, mark: boolean)
  {
    this.order = order;
    this.content = content;
    this.imageUrl = imageUrl;
    this.answerA = answerA;
    this.answerB = answerB;
    this.answerC = answerC;
    this.answerD = answerD;
    this.correctAnswer = correctAnswer;
    this.mark = mark;
  }

  changeCorrectAnswer(select: number) 
  {
    this.correctAnswer = select;
  }
}
