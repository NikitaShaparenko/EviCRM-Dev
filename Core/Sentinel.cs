using System.Security.Cryptography;
using System.Text;
using Force.Crc32;

namespace EviCRM.Server.Core
{
    public class Sentinel
    {

        public static readonly string TripleDES_key = "6A576E5A7234753778214125442A462D";
        public static readonly Encoding Encoder = Encoding.UTF8;


        public string TripleDesEncrypt(string plainText)
        {
            var des = CreateDes(TripleDES_key);
            var ct = des.CreateEncryptor();
            var input = Encoding.UTF8.GetBytes(plainText);
            var output = ct.TransformFinalBlock(input, 0, input.Length);
            return Convert.ToBase64String(output);
        }

        public static bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }

        public string TripleDesDecrypt(string cypherText)
        {
            var des = CreateDes(TripleDES_key);
            var ct = des.CreateDecryptor();

            byte[] input;

           if (IsBase64String(cypherText))
            {
                input = Convert.FromBase64String(cypherText);
            }
           else
            {
                //systemcore.Log("Sentinel", "Декомплиирование 3DES пропущено - попытка декодировать невалидную строку: < " + cypherText + " > ",SystemCore.LogLevels.Debug);
                return cypherText;
            }
           
            var output = ct.TransformFinalBlock(input, 0, input.Length);
            return Encoding.UTF8.GetString(output);
        }

        public TripleDES CreateDes(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            var desKey = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            des.Key = desKey;
            des.IV = new byte[des.BlockSize / 8];
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;
            return des;
        }

        public string SHA512(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        public string CRC(string input)
        {
            return Crc32Algorithm.ComputeAndWriteToEnd(Encoding.ASCII.GetBytes(input),0,input.Length/2).ToString();
        }

    }
}
