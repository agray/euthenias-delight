using System;
using System.Text;

namespace euthenias_delight.Decryption {
    public class Decryptor {
        public static string decrypt(string encrypted) {
            string rightwayup = Reverse(encrypted);
            string decrypted = decryptMe(rightwayup);
            decrypted = decryptMe(decrypted);
            decrypted = decryptMe(decrypted);
            return decrypted;
        }

        private static string decryptMe(string encrypted) {
            byte[] data = Convert.FromBase64String(encrypted);
            return Encoding.UTF8.GetString(data);
        }

        private static string Reverse(string text) {
            if(text == null) {
                return null;
            }
            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }
    }
}