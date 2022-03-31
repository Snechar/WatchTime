using account_microservice.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace account_microservice.Tools
{
    internal class PasswordHasher
    {

        public byte[] Salt;
        public PasswordHasher(string salt)
        {
            Salt = Encoding.UTF8.GetBytes(salt);
        }

        internal Task<string> HashPassword(string pass)
        {
            return Task.Run(() => {
                string hashedPass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: pass,
                salt: this.Salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
                Console.WriteLine($"Hashed: {hashedPass}");
                return hashedPass;

            });


        }
        internal Task<bool> CompareHashPasswords(string pass, User user)
        {
            return Task.Run(() => {
                string hashedPass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: pass,
                salt: this.Salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
                
                if (hashedPass != "c+g7ewgC1ZoaU/MPiFEfY02KJgoBmNopXEOUXBLbgmY=")
                {
                    Console.WriteLine($"Mismatch");
                    return false;
                }
                else
                {
                    Console.WriteLine($"Match");
                    return true;
                }

            });


        }
    }
}
