export class ShareFunction {
    public dateFormatter(date: Date): string {
        const now = Date.now();
        const past = date.getTime();
        const duration = (now - past) / 1000;
      
        if (duration < 60) {
          return Math.floor(duration) + " giây trước";
        } else if (duration < 3600) {
          return Math.floor(duration / 60) + " phút trước";
        } else if (duration < 86400) {
          return Math.floor(duration / 3600) + " giờ trước";
        } else {
          return Math.floor(duration / 86400) + " ngày trước";
        }
    }
}