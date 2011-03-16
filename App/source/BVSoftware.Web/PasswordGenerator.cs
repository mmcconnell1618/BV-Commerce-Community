using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Web
{
    public class PasswordGenerator
    {
        public static string GeneratePassword(int length)
        {
            string result = string.Empty;

            System.Random r = new System.Random();
            r.Next();

            for (int i = 0; i < length; i++)
            {
                r.Next();
                if (i == 0)
                {                    
                    result += GetRandomPrintableLetter(r);
                }
                else
                {                    
                    result += GetRandomPrintableCharacter(r);
                }
            }

            return result;
        }

        private static string GetRandomPrintableCharacter(System.Random r)
        {
            const string passwordCharacters = "abcdefghijkmnopqrstuvwxyz23456789";
            int location = r.Next(passwordCharacters.Length - 1);
            return passwordCharacters.Substring(location, 1);
        }

        private static string GetRandomPrintableLetter(System.Random r)
        {
            const string passwordCharacters = "abcdefghijkmnopqrstuvwxyz";
            int location = r.Next(passwordCharacters.Length - 1);
            return passwordCharacters.Substring(location, 1);
        }

    }
}
