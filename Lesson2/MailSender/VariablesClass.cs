using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodePasswordDLL;

namespace MailSender
{
    public static class VariablesClass
    {
        private static Dictionary<string, string> dicSenders = new Dictionary<string, string>()
        {
            { "tst_for_gkbrns@rambler.ru",CodePassword.GetPassword("pass") },
            { "test@test.ru",CodePassword.GetPassword("testpass") }
        };

        public static Dictionary<string, string> Senders
        {
            get { return dicSenders; }
        }
    }
}
