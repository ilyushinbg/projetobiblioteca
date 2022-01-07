using System.Security.Cryptography;
using System.Text;

namespace Biblioteca.Models
{
    public class Criptografo
    {

        public static string TextoCriptografado (string textoSemFormatacao)
        {
            MD5 MD5Hasher = MD5.Create();

            byte [] bytesNaoCriptografado = Encoding.Default.GetBytes(textoSemFormatacao);
            byte [] bytesCriptografado = MD5Hasher.ComputeHash(bytesNaoCriptografado);

            StringBuilder SB = new StringBuilder();

            foreach (byte b in bytesCriptografado)
            {
                string DebugB = b.ToString("x2");
                SB.Append(DebugB);
            }
            return SB.ToString();
        }
    }
}