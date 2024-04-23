import { QuestionModule } from './question.module';

export class QuestionSetsModule {
  id: string
  name: string
  desc: string
  imageUrl: string
  creator: string
  createdDate: Date
  updatedDate: Date
  questions: Array<QuestionModule>

  constructor(id: string, name: string, desc: string, imageUrl: string, creator:string, createdDate:Date, updatedDate:Date, questions: Array<QuestionModule>) {
    this.id = id
    this.name = name
    this.desc = desc
    this.creator = creator
    this.createdDate = createdDate
    this.updatedDate = updatedDate
    this.imageUrl = imageUrl
    this.questions = questions
  }
}
