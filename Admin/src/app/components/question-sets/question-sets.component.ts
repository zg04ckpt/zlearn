import { Component } from '@angular/core';
import { QuestionSetsModule } from 'src/app/models/question-sets.module';
import { QuestionModule } from 'src/app/models/question.module';

@Component({
  selector: 'question-sets',
  templateUrl: './question-sets.component.html',
  styleUrls: ['./question-sets.component.css']
})
export class QuestionSetsComponent {

  questionSetsList: Array<QuestionSetsModule> = 
  [
    new QuestionSetsModule(
      "1",
      "699 câu hỏi trắc nghiệm 1",
      "Đây là bộ câu hỏi trắc nghiệm 1",
      "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b2/Bootstrap_logo.svg/800px-Bootstrap_logo.svg.png",
      "admin",
      new Date("1-1-2003"),
      new Date("1-1-2003"),
      [
        new QuestionModule("1-1", "Câu hỏi thứ 1", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 1),
        new QuestionModule("1-2", "Câu hỏi thứ 2", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 2),
        new QuestionModule("1-3", "Câu hỏi thứ 3", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 3),
        new QuestionModule("1-4", "Câu hỏi thứ 4", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 4)
      ]
    ),
    new QuestionSetsModule(
      "2",
      "699 câu hỏi trắc nghiệm 1",
      "Đây là bộ câu hỏi trắc nghiệm 1",
      "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b2/Bootstrap_logo.svg/800px-Bootstrap_logo.svg.png",
      "admin",
      new Date("1-1-2003"),
      new Date("1-1-2003"),
      [
        new QuestionModule("1-1", "Câu hỏi thứ 1", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 1),
        new QuestionModule("1-2", "Câu hỏi thứ 2", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 2),
        new QuestionModule("1-3", "Câu hỏi thứ 3", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 3),
        new QuestionModule("1-4", "Câu hỏi thứ 4", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 4)
      ]
    ),
    new QuestionSetsModule(
      "3",
      "699 câu hỏi trắc nghiệm 1",
      "Đây là bộ câu hỏi trắc nghiệm 1",
      "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b2/Bootstrap_logo.svg/800px-Bootstrap_logo.svg.png",
      "admin",
      new Date("1-1-2003"),
      new Date("1-1-2003"),
      [
        new QuestionModule("1-1", "Câu hỏi thứ 1", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 1),
        new QuestionModule("1-2", "Câu hỏi thứ 2", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 2),
        new QuestionModule("1-3", "Câu hỏi thứ 3", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 3),
        new QuestionModule("1-4", "Câu hỏi thứ 4", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 4)
      ]
    ),
    new QuestionSetsModule(
      "4",
      "699 câu hỏi trắc nghiệm 1",
      "Đây là bộ câu hỏi trắc nghiệm 1",
      "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b2/Bootstrap_logo.svg/800px-Bootstrap_logo.svg.png",
      "admin",
      new Date("1-1-2003"),
      new Date("1-1-2003"),
      [
        new QuestionModule("1-1", "Câu hỏi thứ 1", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 1),
        new QuestionModule("1-2", "Câu hỏi thứ 2", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 2),
        new QuestionModule("1-3", "Câu hỏi thứ 3", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 3),
        new QuestionModule("1-4", "Câu hỏi thứ 4", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 4)
      ]
    ),
    new QuestionSetsModule(
      "5",
      "699 câu hỏi trắc nghiệm 1",
      "Đây là bộ câu hỏi trắc nghiệm 1",
      "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b2/Bootstrap_logo.svg/800px-Bootstrap_logo.svg.png",
      "admin",
      new Date("1-1-2003"),
      new Date("1-1-2003"),
      [
        new QuestionModule("1-1", "Câu hỏi thứ 1", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 1),
        new QuestionModule("1-2", "Câu hỏi thứ 2", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 2),
        new QuestionModule("1-3", "Câu hỏi thứ 3", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 3),
        new QuestionModule("1-4", "Câu hỏi thứ 4", false, "Đáp án a", "Đáp án b", "Đáp án c", "Đáp án d", 4)
      ]
    ),
  ]

  deleteQuestionSet(id: string) 
  {
    if(confirm("Are you sure you want to delete this question set?"))
    {
      this.questionSetsList = this.questionSetsList.filter(x => x.id != id);
      alert("Đã xóa thành công !");
      //handle with api
    }
  }
}
