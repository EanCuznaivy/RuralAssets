using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace RuralAssets.WebApplication
{
    public interface ICryptoService
    {
        string Encrypt(string value);
        string Decrypt(string value);
    }

    public class AesCryptoService : ICryptoService
    {
        private readonly ModuleConfigOptions _moduleOptions;

        public AesCryptoService(IOptionsSnapshot<ModuleConfigOptions> moduleOptions)
        {
            _moduleOptions = moduleOptions.Value;
        }

        public string Encrypt(string value)
        {
            if (!_moduleOptions.EnableCrypto)
            {
                return value;
            }

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var toEncryptArray = Encoding.UTF8.GetBytes(value);

            var rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(_moduleOptions.CryptoKey),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var transform = rm.CreateEncryptor();
            var resultArray = transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public string Decrypt(string value)
        {
            if (!_moduleOptions.EnableCrypto)
            {
                return value;
            }

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var toEncryptArray = Convert.FromBase64String(value);

            var rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(_moduleOptions.CryptoKey),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var transform = rm.CreateDecryptor();
            var resultArray = transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
    }
}