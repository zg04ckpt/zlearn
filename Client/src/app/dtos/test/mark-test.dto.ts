export interface MarkTestDTO {
    answers: {
      id: string;
      selected: number;
    }[];
    startTime: string;
    endTime: string;
    testId: string;
    testName: string;
}