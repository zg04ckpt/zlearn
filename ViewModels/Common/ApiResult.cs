using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace  ViewModels.Common
{
    public class ApiResult
    {
        private const string Key = "12345678901234567890123456789012";
        private const int KeySize = 256;
        private const int BlockSize = 128;

        public object Data { get; set; }
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }

        public ApiResult()
        {
            Code = HttpStatusCode.OK;
        }

        public ApiResult(object data)
        {
            Code = HttpStatusCode.OK;
            Data = Encrypt(JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public ApiResult(string message, HttpStatusCode code)
        {
            Code = code;
            Message = message;
        }

        private string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.GenerateIV();
                aes.Key = Encoding.UTF8.GetBytes(Key.PadRight(KeySize / 8));

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var ms = new MemoryStream())
                    {
                        ms.Write(aes.IV, 0, aes.IV.Length);
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (var sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

    }
}
