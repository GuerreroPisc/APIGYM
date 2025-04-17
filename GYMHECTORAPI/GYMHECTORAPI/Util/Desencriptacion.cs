using System.Security.Cryptography;
using System.Text;

namespace GYMHECTORAPI.Util
{
    public static class Desencriptacion
    {
        public static string Decrypt(this IServiceCollection services, string key, string textoEncriptado)
        {
            var textoCompleto = Convert.FromBase64String(textoEncriptado);

            using (Aes aes = Aes.Create())
            {
                var iv = new byte[aes.BlockSize / 8];
                var cipher = new byte[textoCompleto.Length - iv.Length];

                Array.Copy(textoCompleto, 0, iv, 0, iv.Length);
                Array.Copy(textoCompleto, iv.Length, cipher, 0, cipher.Length);

                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(cipher))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
