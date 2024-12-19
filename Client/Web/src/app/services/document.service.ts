import { Injectable } from '@angular/core';
import { CreateDocumentDTO } from '../dtos/document/create-document.dto';
import { lastValueFrom, map, Observable } from 'rxjs';
import { APIResult } from '../dtos/common/api-result.dto';
import { HttpClient } from '@angular/common/http';
import { DocumentSearchingDTO } from '../dtos/document/document-searching.dto';
import { DocumentItemDTO } from '../dtos/document/document-item';
import { environment } from '../../environments/environment';
import { BankInfo } from '../dtos/document/bank-info.dto';
import { ComponentService } from './component.service';
import { CategoryItem } from '../entities/management/category-item.entity';
import { PagingResultDTO } from '../dtos/common/paging-result.dto';
import { DocumentDetailDTO } from '../dtos/document/document-detail.dto';
import { UpdateDocumentDTO } from '../dtos/document/update-document.dto';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {
  baseUrl = environment.baseUrl
  constructor(
    private http: HttpClient,
    private componentService: ComponentService
  ) { }

  public create(data: CreateDocumentDTO): Observable<void> {
    const formData = new FormData();
    formData.append(`name`, data.name);
    formData.append(`categoryId`, data.categoryId);
    formData.append(`description`, data.description);
    if(data.image) {
      formData.append(`image`, data.image);
    }
    formData.append(`sourceFile`, data.sourceFile!);
    data.previewImages.forEach((img, idx) => {
      formData.append(`previewImages`, img);
    });
    if(data.paymentInfo) {
      formData.append(`paymentInfo`, data.paymentInfo);
    }

    return this.http
      .post<APIResult<void>>(`documents`, formData)
      .pipe(map(res => res.data!));
  }

  public getAsList(data: DocumentSearchingDTO): Observable<PagingResultDTO<DocumentItemDTO>> {
    return this.http
      .get<APIResult<PagingResultDTO<DocumentItemDTO>>>(
        `documents?pageIndex=${data.pageIndex}&pageSize=${data.pageSize}&name=${data.name}&categorySlug=${data.categorySlug}`
      ).pipe(map(res => res.data!))
      .pipe(map(res => {
        res.data.forEach(e => {
          if(e.imageUrl) {
            e.imageUrl = e.imageUrl? this.baseUrl + e.imageUrl : '';
          }
          e.updatedAt = new Date(e.updatedAt);
        });
        return res;
      })
    );
  }

  public getMyDocuments(): Observable<DocumentItemDTO[]> {
    return this.http
      .get<APIResult<DocumentItemDTO[]>>(`documents/my-documents`)
      .pipe(map(res => res.data!))
      .pipe(map(res => {
        res.forEach(e => {
          if(e.imageUrl) {
            e.imageUrl = e.imageUrl? this.baseUrl + e.imageUrl : '';
          }
          e.updatedAt = new Date(e.updatedAt);
        });
        return res;
      }));
  }

  async getBankList(): Promise<BankInfo[]> {
    return lastValueFrom(this.http
      .get<any>(`https://api.vietqr.io/v2/banks`)
      .pipe(map(res => res.data as BankInfo[])));
  }

  public getCategories(): Observable<CategoryItem[]> {
    return this.http
        .get<APIResult<CategoryItem[]>>(`documents/categories`)
        .pipe(map(res => res.data!));
  }

  public getDetail(documentId: string): Observable<DocumentDetailDTO> {
    return this.http
      .get<APIResult<DocumentDetailDTO>>(`documents/${documentId}`)
      .pipe(
        map(res => res.data!),
        map(res => {
          res.previewImagePaths = res.previewImagePaths.map(e => this.baseUrl + e);
          return res;
        })
      );
  }

  public getUpdateContent(documentId: string): Observable<UpdateDocumentDTO> {
    return this.http
      .get<APIResult<UpdateDocumentDTO>>(`documents/${documentId}/update`)
      .pipe(
        map(res => res.data!),
        map(res => {
          if(res.imageUrl) {
            res.imageUrl = this.baseUrl + res.imageUrl;
          }
          res.previewImages.forEach(e => {
            e.imageUrl = this.baseUrl + e.imageUrl;
          });
          return res;
        })
      );
  }

  public updateDocument(documentId: string, data: UpdateDocumentDTO): Observable<void> {

    const formData = new FormData();
    formData.append(`name`, data.name);
    formData.append(`categoryId`, data.categoryId);
    formData.append(`description`, data.description);

    if(data.newImage) {
      formData.append(`newImage`, data.newImage);
    }

    if(data.newSourceFile) {
      formData.append(`newSourceFile`, data.newSourceFile);
    }

    data.previewImages.forEach((img, idx) => {
      if (img.id) {
          formData.append(`previewImages[${idx}].id`, img.id);
      }
      if (img.image) {
          formData.append(`previewImages[${idx}].newImage`, img.image);
      }
    });

    return this.http
      .put<APIResult<void>>(`documents/${documentId}`, formData)
      .pipe(map(res => res.data!));
  }

  public deleteDocument(documentId: string): Observable<void> {
    return this.http
      .delete<APIResult<void>>(`documents/${documentId}`)
      .pipe(map(res => res.data!));
  }

  public download(documentId: string): Observable<Blob> {
    return this.http.get(`documents/${documentId}/download`, {
      responseType: 'blob'
    });
  }
}
