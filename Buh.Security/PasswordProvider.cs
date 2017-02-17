using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buh.Security
{
    public class PasswordProvider
    {
        public string GetPassword()
        {
            using (var streamReader = new StreamReader(".gitoptions"))
            {
                var textBase64 = streamReader.ReadLine();
                if (textBase64 == null)
                {
                    throw new Exception("Password not found");
                }

                return Encoding.UTF8.GetString(Convert.FromBase64String(textBase64));
            }
        }
    }
}
