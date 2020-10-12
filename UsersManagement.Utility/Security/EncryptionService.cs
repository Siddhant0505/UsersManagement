using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace UsersManagement.Utility.Security
{
    /// <summary>
    /// Use this class for query string encryption and decryption 
    /// If deployment machine change then we can't decrypt saved database encrypted value by this Class & IDataProtectionProvider
    /// </summary>
    public class EncryptionService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private string Key { get; set; }
        private IConfiguration _Configuration { get; }
        public EncryptionService(IDataProtectionProvider dataProtectionProvider, IConfiguration configuration)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _Configuration = configuration;
            Key = _Configuration["QueryStringEncryptionKey"];
        }

        public string Encrypt(string input)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Protect(input);
        }

        public string Decrypt(string cipherText)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Unprotect(cipherText);
        }
    }
}
