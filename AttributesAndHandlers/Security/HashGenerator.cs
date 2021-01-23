using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AttributesAndHandlers.Security
{
    public class HashGenerator
    {
        public static string GenerateHash(string plainText)
        {
            byte[] bytesArray = Encoding.Unicode.GetBytes(plainText);
            byte[] hashed = HashAlgorithm.Create("MD5").ComputeHash(bytesArray);
            return Convert.ToBase64String(hashed);
        }
    }
}
