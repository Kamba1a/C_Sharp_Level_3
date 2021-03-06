﻿using System.Text.RegularExpressions;

namespace MailSender.Services
{	
    /// <summary>
    /// Класс для проверки корректности емейла
    /// </summary>
    public static class CheckEmail
    {
    	/// <summary>
    	/// Проверка корректности емейл-адреса
    	/// </summary>
        public static bool IsEmail(string email)
        {
            Regex regex = new Regex(@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$");
            return regex.IsMatch(email);
        }
    }
}
