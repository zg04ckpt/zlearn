export interface QuestionsResponse {
    data: {
        order: number
        content: string
        imageUrl: string | null
        answerA: string
        answerB: string
        answerC: string
        answerD: string
        correctAnswer: number
        mark: boolean
    } []
    code: number
    message: string | null
}

/*
{
  "data": [
    {
      "order": 0,
      "content": "Nguyên Nguyên Hoàng",
      "imageUrl": null,
      "answerA": " a",
      "answerB": "b",
      "answerC": "c",
      "answerD": "d",
      "correctAnswer": 1,
      "mark": false
    }
  ],
  "code": 200,
  "message": null
}
*/