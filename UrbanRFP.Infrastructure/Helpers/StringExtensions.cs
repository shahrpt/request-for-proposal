namespace UrbanRFP.Infrastructure.Helpers
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        /// <summary>
        ///     Encryption Key for the passwords
        /// </summary>
        private const string PASSWORD_ENCRYPTION_KEY = "!?A*6%#f@5$;}|";
        
        /// <summary>
        /// URL Query String Encryption Key
        /// </summary>
        private static byte[] key = { };
        private static readonly byte[] Iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private static readonly string StringEncryptionKey = "!@%$^*(~#de<?|";

        #region Public Methods

        /// <summary>
        /// Converts a string into a Base64 encoded string
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <returns>The base64 encoded string</returns>
        public static string Base64Encode(this string source)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
        }

        /// <summary>
        /// Decodes a base64 encoded string
        /// </summary>
        /// <param name="encodedValue">The encoded string.</param>
        /// <returns>
        /// The base64 encoded string
        /// </returns>
        public static string Base64Decode(this string encodedValue)
        {
            string decodedValue = string.Empty;

            try
            {
                decodedValue = Encoding.UTF8.GetString(Convert.FromBase64String(encodedValue));
            }
            catch (FormatException)
            {
                // The string we attempted to decode was not in correct base64 format
                decodedValue = string.Empty;
            }

            return decodedValue;
        }

        /// <summary>
        /// Splits the string into an array using the specified delimiterzz
        /// </summary>
        /// <param name="value">The value to split into an array</param>
        /// <param name="delimiter">The delimiter for array items</param>
        /// <returns>The array</returns>
        public static string[] ToArray(this string value, char delimiter)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new string[0];
            }

            return value.Split(delimiter);
        }

        /// <summary>
        /// Appends the time stamp to file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The file name with appended time stamp</returns>
        public static string AppendTimeStampToFile(this string fileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString("yyyyMMddHHmmss"),
                Path.GetExtension(fileName));
        }

        /// <summary>
        ///     Extension method to check for null or not.Returns true if object is not null
        /// </summary>
        /// <param name="source">Object</param>
        /// <returns>True if object is not null</returns>
        public static bool IsNotNull(this object source)
        {
            return !(source == null || source == DBNull.Value);
        }

        /// <summary>
        ///     Method to check whether 2 strings are equal or not
        /// </summary>
        /// <param name="currentString">String to be compared</param>
        /// <param name="stringToCompareWith">String to be compared with</param>
        /// <returns>Returns true if strings are equal</returns>
        public static bool IsStringEqualsTo(this string currentString, string stringToCompareWith)
        {
            return string.Equals(currentString, stringToCompareWith, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region Encrypt

        /// <summary>
        ///     Function to get the encrypted String
        /// </summary>
        /// <param name="aStrArg">String to be encrypted</param>
        /// <returns>Encrypted string</returns>
        public static string GetEncryptedString(this string aStrArg)
        {
            string encryptPassword;
            try
            {
                encryptPassword = EncryptPassword(aStrArg, true);
            }
            catch
            {
                throw;
            }
            return encryptPassword;
        }

        /// <summary>
        ///     Function to get the Decrypted String
        /// </summary>
        /// <param name="aStrArg">String to be decrypted</param>
        /// <returns>Encrypted string</returns>
        public static string GetDecryptedString(this string aStrArg)
        {
            string decryptPassword;
            try
            {
                decryptPassword = DecryptPassword(aStrArg, true);
            }
            catch
            {
                throw;
            }

            return decryptPassword;
        }

        /// <summary>
        ///     Function to encrypt the password
        /// </summary>
        /// <param name="aToEncrypt"></param>
        /// <param name="aUseHashing"></param>
        /// <returns></returns>
        public static string EncryptPassword(string aToEncrypt, bool aUseHashing)
        {
            byte[] arrKey;
            byte[] arrToEncrypt = Encoding.UTF8.GetBytes(aToEncrypt);

            // Get the key from config file
            string key = PASSWORD_ENCRYPTION_KEY;

            //If hashing use get hashcode regards to your key
            if (aUseHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                arrKey = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));

                /*Always release the resources and flush data
                of the Cryptographic service provide. Best Practice*/
                hashmd5.Clear();
            }
            else
                arrKey = Encoding.UTF8.GetBytes(key);

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = arrKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            //set the secret key for the tripleDES algorithm

            /*mode of operation. there are other 4 modes.
            We choose ECB(Electronic code Book)*/

            //padding mode(if any extra byte added)
            ICryptoTransform cTransform = tdes.CreateEncryptor();

            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock(arrToEncrypt, 0, arrToEncrypt.Length);

            //Release resources held by TripleDes Encryptor
            tdes.Clear();

            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        #endregion

        #region Decrypt

        /// <summary>
        ///     Function to decrypt the password
        /// </summary>
        /// <param name="cipherString"></param>
        /// <param name="useHashing"></param>
        /// <returns></returns>
        public static string DecryptPassword(string cipherString, bool useHashing)
        {
            byte[] arrKey;

            //get the byte code of the string
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //Get your key from config file to open the lock!
            string key = PASSWORD_ENCRYPTION_KEY; // (string)settingsReader.GetValue("SecurityKey",typeof(String));

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                var hashmd5 = new MD5CryptoServiceProvider();
                arrKey = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));

                //release any resource held by the MD5CryptoServiceProvider
                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                arrKey = Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = arrKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            //set the secret key for the tripleDES algorithm

            /*mode of operation. there are other 4 modes. 
            We choose ECB(Electronic code Book)*/

            using (var transform = tdes.CreateDecryptor())
            {
                try
                {
                    var resultArray = transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                    // return the Clear decrypted TEXT
                    return Encoding.UTF8.GetString(resultArray);
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        #endregion

        # region "Decrypt Querstring Value"

        /// <summary>
        ///     Decrypt Querstring Value
        /// </summary>
        /// <param name="aStringToDecrypt">String to decrypt</param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(this string aStringToDecrypt)
        {
            try
            {
                key = Encoding.UTF8.GetBytes(StringEncryptionKey.Substring(0, 8));
                using (var des = new DESCryptoServiceProvider())
                {
                    Byte[] inputByteArray = Convert.FromBase64String(aStringToDecrypt);
                    using (var memorystream = new MemoryStream())
                    {
                        using (
                            var crptostream = new CryptoStream(memorystream, des.CreateDecryptor(key, Iv),
                                                               CryptoStreamMode.Write))
                        {
                            crptostream.Write(inputByteArray, 0, inputByteArray.Length);
                            crptostream.FlushFinalBlock();
                            Encoding encoding = Encoding.UTF8;
                            return encoding.GetString(memorystream.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        # endregion

        # region "Encrypt QueryString Value"

        /// <summary>
        ///     Encrypt QueryString Value
        /// </summary>
        /// <param name="aStringToEncrypt">String to encrypt</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(this string aStringToEncrypt)
        {
            try
            {
                key = Encoding.UTF8.GetBytes(StringEncryptionKey.Substring(0, 8));
                using (var des = new DESCryptoServiceProvider())
                {
                    Byte[] inputByteArray = Encoding.UTF8.GetBytes(aStringToEncrypt);
                    using (var memorystream = new MemoryStream())
                    {
                        var cs = new CryptoStream(memorystream, des.CreateEncryptor(key, Iv), CryptoStreamMode.Write);
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        return Convert.ToBase64String(memorystream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        # endregion

        public static string RemoveSpecialChars(this string aString)
        {
            return Regex.Replace(aString, "[^0-9A-Za-z]+", "");
        }
    }
}
