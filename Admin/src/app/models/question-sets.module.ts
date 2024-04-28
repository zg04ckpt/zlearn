import { QuestionModule } from './question.module';

export class QuestionSetsModule {
  id: string
  name: string
  description: string
  imageUrl: string
  creator: string
  createdDate: Date
  updatedDate: Date
  questionCount: number

  constructor(id: string, name: string, description: string, imageUrl: string, creator: string, createdDate: Date, updatedDate: Date, questionCount: number) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.imageUrl = imageUrl;
    this.creator = creator;
    this.createdDate = createdDate;
    this.updatedDate = updatedDate;
    this.questionCount = questionCount;
  }
}
