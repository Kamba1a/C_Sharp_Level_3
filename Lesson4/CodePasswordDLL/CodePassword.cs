using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePasswordDLL
{
    public class CodePassword
    {
        /// <summary>
        /// Дешифрование пароля
        /// </summary>
        /// <param name="p_sPassword"></param>
        /// <returns></returns>
        public static string GetPassword(string codPassword)
        {
            string password = "";
            foreach (char a in codPassword)
            {
                char ch = a;
                ch--;
                password += ch;
            }
            return password;
        }

        /// <summary>
        /// Шифрование пароля
        /// </summary>
        /// <param name="p_sPassword"></param>
        /// <returns></returns>
        public static string GetCodPassword(string password)
        {
            string codPassword = "";
            foreach (char a in password)
            {
                char ch = a;
                ch++;
                codPassword += ch;
            }
            return codPassword;
        }
    }
}
