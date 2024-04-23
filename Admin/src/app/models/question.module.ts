export class QuestionModule 
{ 
  id: string;
  content: string;
  selected: boolean;
  a: string;
  b: string;
  c: string;
  d: string;
  correctAnswer: number;

  constructor(
    id: string, 
    content: string, 
    selected: boolean, 
    a: string, b: string, c: string, d: string, 
    correctAnswer: number
  ) {
    this.id = id;
    this.content = content;
    this.selected = selected;
    this.a = a;
    this.b = b;
    this.c = c;
    this.d = d;
    this.correctAnswer = correctAnswer;
  }

  changeCorrectAnswer(select: number) 
  {
    this.correctAnswer = select;
  }
}
