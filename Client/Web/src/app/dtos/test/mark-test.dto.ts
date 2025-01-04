export interface MarkTestDTO {
    answers: {
      id: string;
      selected: number;
    }[];
    startTime: string;
    testId: string;
    testName: string;
}