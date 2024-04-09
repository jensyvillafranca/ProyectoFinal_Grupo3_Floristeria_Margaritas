/*
 * Descripción:
 * Este código define una clase llamada PasswordHandler en el espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions.
 * Esta clase proporciona métodos estáticos para cifrar y verificar contraseñas utilizando la función de derivación de clave basada en contraseña (PBKDF2).
 * Se utiliza el algoritmo de hash HMACSHA1 para la derivación de clave.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions
{
    public class PasswordHandler
    {
        public static string EncryptPassword(string password)
        {
            // Genera un salt aleatorio
            byte[] salt = GenerateSalt();

            //  Combina el pasword, salt y hace hask
            byte[] hash = HashPassword(Encoding.UTF8.GetBytes(password), salt);

            // Combina el hash y salt para almacenamiento
            byte[] encryptedPassword = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, encryptedPassword, 0, salt.Length);
            Array.Copy(hash, 0, encryptedPassword, salt.Length, hash.Length);

            // Convierte el byte array a base64 string para almacenamiento
            return Convert.ToBase64String(encryptedPassword);
        }

        public static bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            // Convierte el base64 string a byte array
            byte[] encryptedPassword = Convert.FromBase64String(storedPassword);

            // Extrae Salt del pass guardado
            byte[] salt = new byte[32];
            Array.Copy(encryptedPassword, 0, salt, 0, 32);

            // Hash el pasword ingresado con el salt
            byte[] enteredHash = HashPassword(Encoding.UTF8.GetBytes(enteredPassword), salt);

            // Compara el hash guardado con el hash generado
            for (int i = 0; i < 32; i++)
            {
                if (enteredHash[i] != encryptedPassword[i + 32])
                {
                    return false; // Contraseñas no coinciden
                }
            }

            return true; // Contraseña Coinsiden
        }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[32];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        private static byte[] HashPassword(byte[] password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                return pbkdf2.GetBytes(32); // 32 bytes for a 256-bit key
            }
        }
    }
}
