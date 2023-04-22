using Azure.Core.Cryptography;
using Microsoft.Extensions.Options;

using System.Text;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace FMDC.Security
{

    public interface IEncryptionHandler
    {
        string Encrypt(string plainString);
        string Decrypt(string encryptedString);
        string DecryptRsa(string encryptedString);
        string EncryptRsa(string encryptedString);

    }
    public class EncryptionHandler : IEncryptionHandler
    {
        private readonly AppSettings _appSettings;
        private readonly byte[] _encryptionKey;
        private readonly byte[] _encryptionIv;
        private readonly string _keyFileName = "private.pem";
        private readonly string _keyFilePath = "Security";
        private readonly RSACryptoServiceProvider _cryptoServiceProvider;

        public EncryptionHandler(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _encryptionKey = Encoding.UTF8.GetBytes(_appSettings.EncryptionKey);
            _encryptionIv = Encoding.UTF8.GetBytes(_appSettings.EncryptionIv);
            _cryptoServiceProvider = GetPrivateKeyFromFile();
        }


        public string Decrypt(string encryptedString)
        {
            var plainString = "";
            try
            {
                var sb = new StringBuilder();
                byte[] encryptedBytes = Convert.FromBase64String(encryptedString);
                using Aes aes = Aes.Create();

                aes.Key = _encryptionKey;
                aes.IV = _encryptionIv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using MemoryStream memoryStream = new(encryptedBytes);
                using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);

                using StreamReader streamReader = new(cryptoStream, Encoding.UTF8);
                plainString = streamReader.ReadToEnd();
            }
            catch (Exception e)
            {

                throw;
            }

            return plainString;

        }

        public string DecryptRsa(string encryptedString)
        {
            var plainString = _cryptoServiceProvider.Decrypt(Convert.FromBase64String(encryptedString), false);
            return Encoding.UTF8.GetString(plainString, 0, plainString.Length);
        }

        private RSACryptoServiceProvider GetPrivateKeyFromFile()
        {

            using TextReader privateKeyStringReader = new StringReader(File.ReadAllText(GetPath()));
            AsymmetricCipherKeyPair pemReader = (AsymmetricCipherKeyPair)new PemReader(privateKeyStringReader).ReadObject();
            RSAParameters rsaKeyParameters = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)pemReader.Private);
            RSACryptoServiceProvider cryptoProvider = new();
            cryptoProvider.ImportParameters(rsaKeyParameters);
            return cryptoProvider;
        }


        public string Encrypt(string plainString)
        {
            var encryptedString = "";
            byte[] plainBytes = Encoding.ASCII.GetBytes(plainString);
            using (Aes aes = Aes.Create())
            {
                aes.Key = _encryptionKey;
                aes.IV = _encryptionIv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 256 / 8;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);

                using (StreamWriter streamWriter = new(cryptoStream))
                {

                    streamWriter.Write(plainBytes);
                }
                encryptedString = Encoding.UTF8.GetString(memoryStream.ToArray());

            }

            return encryptedString;

        }

        public string EncryptRsa(string plainString)
        {
            var encryptedString = _cryptoServiceProvider.Encrypt(Encoding.UTF8.GetBytes(plainString), false);
            return Encoding.UTF8.GetString(encryptedString, 0, encryptedString.Length);
        }

        private string GetPath()
        {
            var path = Path.Combine(".", _keyFilePath);
            return Path.Combine(path, _keyFileName);
        }
    }
}
