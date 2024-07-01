import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class QuestionSetService {
  constructor(private httpClient: HttpClient) { }
  baseUrl = environment.baseUrl;
  key = "12345678901234567890123456789012";

  getAll() : Observable<any> {
    return this.httpClient.get<any>(
      `${this.baseUrl}/api/tests`
    );
  }

  getById(id: string) : Observable<any> {
    
    return this.httpClient.get<any>(
      `${this.baseUrl}/api/tests/${id}`
    );
  }
  
  decrypt(cipherText: string) : any {
    // const rawData = CryptoJS.enc.Base64.parse(cipherText);
    // const iv = rawData.words.slice(0, 4); // IV is the first 16 bytes of rawData
    // const cipherParams = CryptoJS.lib.CipherParams.create({
    //   ciphertext: CryptoJS.lib.WordArray.create(rawData.words.slice(4)) // The rest is the actual ciphertext
    // });

    // const keyUtf8 = CryptoJS.enc.Utf8.parse(this.key.padEnd(32)); // Assuming key size is 256 bits

    // const decrypted = CryptoJS.AES.decrypt(cipherParams, keyUtf8, {
    //   mode: CryptoJS.mode.CBC,
    //   padding: CryptoJS.pad.Pkcs7,
    //   iv: CryptoJS.lib.WordArray.create(iv)
    // });

    // return JSON.parse(CryptoJS.enc.Utf8.stringify(decrypted).toString());
    return JSON.parse(cipherText);
  }
}
