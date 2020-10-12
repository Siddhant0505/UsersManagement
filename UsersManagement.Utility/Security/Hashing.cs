using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Text;

namespace UsersManagement.Utility.Security
{
    public class Hashing
    {
		public static string Create(string value, string salt)
		{
			var valueBytes = KeyDerivation.Pbkdf2(
								password: value,
								salt: Encoding.UTF8.GetBytes(salt),
								prf: KeyDerivationPrf.HMACSHA512,
								iterationCount: 10000,
								numBytesRequested: 256 / 8);

			return Convert.ToBase64String(valueBytes);
		}

		public static bool Validate(string value, string salt, string hash)
				=> Create(value, salt) == hash;

	}
}
