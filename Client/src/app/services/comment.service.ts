import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { CommentDTO } from "../dtos/comment/comment.dto";
import { APIResult } from "../dtos/common/api-result.dto";
import { CreateCommentDTO } from "../dtos/comment/create-comment.dto";
import { environment } from "../../environments/environment";

@Injectable({providedIn: 'root'})
export class CommentService {
    constructor(
        private http : HttpClient
    ) {}

    getAllCommentsOfTest(testId: string): Observable<CommentDTO[]> {
        return this.http
            .get<APIResult<CommentDTO[]>>(`comments/${testId}`)
            .pipe(map(res => {
                res.data!.forEach(e => e.userAvatar = environment.baseUrl + e.userAvatar);
                return res.data!;
            }));
    }

    sendComment(dto: CreateCommentDTO): Observable<void> {
        return this.http
            .post<APIResult<void>>(`comments`, dto)
            .pipe(map(res => res.data!));
    }

    removeComment(testId: string): Observable<void> {
        return this.http
            .delete<APIResult<void>>(`comments/${testId}`)
            .pipe(map(res => res.data!));
    }

    like(commentId: string): Observable<void> {
        return this.http
            .patch<APIResult<void>>(`comments/${commentId}`, null)
            .pipe(map(res => res.data!));
    }
}