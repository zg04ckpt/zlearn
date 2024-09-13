import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class CommonService {
    getDateTimeString(date: Date): string {
        //change to GMT + 7
        date.setHours(date.getHours() + 7);
        return date.toISOString();
    }

    //2024-08-25T20:13:56.613Z
    getTime(date: string) {
        return date.split('T')[1].substring(0, 8);
    }

    getDate(date: string) {
        return date.split('T')[0];
    }
}