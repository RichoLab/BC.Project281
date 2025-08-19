using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project281_Ryno_File
{
    internal class AdminAuth
    {
        // SIMPLE: plain text password (change if you like)
        private const string Password = "Admin123";

        public static bool Verify(string input) => input == Password;
    }
}
