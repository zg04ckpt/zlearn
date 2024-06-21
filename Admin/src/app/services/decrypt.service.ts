import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root'
})
export class DecryptService {
  constructor() { }
  key = "12345678901234567890123456789012";

  decrypt(cipherText: string) : any {
    const rawData = CryptoJS.enc.Base64.parse(cipherText);
    const iv = rawData.words.slice(0, 4); // IV is the first 16 bytes of rawData
    const cipherParams = CryptoJS.lib.CipherParams.create({
      ciphertext: CryptoJS.lib.WordArray.create(rawData.words.slice(4)) // The rest is the actual ciphertext
    });

    const keyUtf8 = CryptoJS.enc.Utf8.parse(this.key.padEnd(32)); // Assuming key size is 256 bits

    const decrypted = CryptoJS.AES.decrypt(cipherParams, keyUtf8, {
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7,
      iv: CryptoJS.lib.WordArray.create(iv)
    });

    return JSON.parse(CryptoJS.enc.Utf8.stringify(decrypted).toString());
  }
}
