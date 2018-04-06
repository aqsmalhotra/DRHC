using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject.LIB
{
    public class HashPass
    {
        //reference to https://stackoverflow.com/questions/11367727/how-can-i-sha512-a-string-in-c
        public static string SHA512(string input) 
        {
            string encryptedPass = "";
            if (String.IsNullOrEmpty(input))
            {
                encryptedPass = "No password found!";
            }
            else
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(input);
                using (var hash = System.Security.Cryptography.SHA512.Create())
                {
                    var hashedInputBytes = hash.ComputeHash(bytes);

                    // Convert to text
                    // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                    var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                    foreach (var b in hashedInputBytes)
                        hashedInputStringBuilder.Append(b.ToString("X2"));
                    encryptedPass = hashedInputStringBuilder.ToString();
                }
            }
            return encryptedPass;


        }
    }
}