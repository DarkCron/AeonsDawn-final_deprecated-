using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using TBAGW.Scenes.Editor;

namespace TBAGW.Utilities.Editor.IO
{
    internal static class Encrypter
    {

        static internal void EncryptFile(string inputFile, string outputFile)
        {

            Console.WriteLine("Encryption called for: " + inputFile);
            FileStream fsCrypt = null;
            CryptoStream cs = null;
            FileStream fsIn = null;
            try
            {
                string input = inputFile;
                string password = @"1L1k3Y0u"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                if (!Directory.Exists(Path.GetDirectoryName(cryptFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(cryptFile));
                }
                if (File.Exists(cryptFile))
                    File.Delete(cryptFile);


                fsCrypt = File.Open(cryptFile, FileMode.OpenOrCreate);



                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.Padding = PaddingMode.PKCS7;

                cs = new CryptoStream(fsCrypt,
                   RMCrypto.CreateEncryptor(key, key),
                   CryptoStreamMode.Write);

                fsIn = File.Open(input, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                try
                {
                    if (inputFile.EndsWith(".cgmap") || inputFile.EndsWith(".cgmapc"))
                    {
                        MapBuilder.functionForm.Progress(75);
                    }
                }
                catch (Exception)
                {
                }


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                MessageBox.Show("Encryption failed!", "Error");
                try
                {
                    fsIn.Close();
                    cs.Close();
                    fsCrypt.Close();
                }
                catch (Exception)
                {
                }
            }
        }


        static internal String DecryptFile(string inputFile)
        {
            Console.WriteLine("Decryption called for: " + inputFile);
            {
                String outputFile = Path.GetTempFileName();

                string password = @"1L1k3Y0u"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                if (!Directory.Exists(Path.GetDirectoryName(outputFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                }
                if (File.Exists(outputFile))
                    File.Delete(outputFile);

                FileStream fsCrypt = File.Open(inputFile, FileMode.OpenOrCreate);

                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.Padding = PaddingMode.PKCS7;

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);
              

                FileStream fsOut = File.Open(outputFile, FileMode.OpenOrCreate);
              
                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();
                return outputFile;
            }
        }

        static internal String DecryptFileSong(string inputFile)
        {
            Console.WriteLine("Decryption called for: " + inputFile);
            {
                String outputFile = inputFile+"uc";

                string password = @"1L1k3Y0u"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                if (!Directory.Exists(Path.GetDirectoryName(outputFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                }
                if (File.Exists(outputFile))
                    File.Delete(outputFile);

                FileStream fsCrypt = File.Open(inputFile, FileMode.OpenOrCreate);

                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.Padding = PaddingMode.PKCS7;

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);


                FileStream fsOut = File.Open(outputFile, FileMode.OpenOrCreate);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();
                return outputFile;
            }
        }
    }

    internal class EncrypterSpawn
    {
        internal void EncryptFile(string inputFile, string outputFile)
        {

            Console.WriteLine("Encryption called for: " + inputFile);
            FileStream fsCrypt = null;
            CryptoStream cs = null;
            FileStream fsIn = null;
            try
            {
                string input = inputFile;
                string password = @"1L1k3Y0u"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                if (!Directory.Exists(Path.GetDirectoryName(cryptFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(cryptFile));
                }
                if (File.Exists(cryptFile))
                    File.Delete(cryptFile);


                fsCrypt = File.Open(cryptFile, FileMode.OpenOrCreate);



                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.Padding = PaddingMode.PKCS7;

                cs = new CryptoStream(fsCrypt,
                   RMCrypto.CreateEncryptor(key, key),
                   CryptoStreamMode.Write);

                fsIn = File.Open(input, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                try
                {
                    if (inputFile.EndsWith(".cgmap") || inputFile.EndsWith(".cgmapc"))
                    {
                        MapBuilder.functionForm.Progress(75);
                    }
                }
                catch (Exception)
                {
                }


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                MessageBox.Show("Encryption failed!", "Error");
                try
                {
                    fsIn.Close();
                    cs.Close();
                    fsCrypt.Close();
                }
                catch (Exception)
                {
                }
            }
        }

        internal String DecryptFile(string inputFile)
        {
            Console.WriteLine("Decryption called for: " + inputFile);
            {
                String outputFile = Path.GetTempFileName();

                string password = @"1L1k3Y0u"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                if (!Directory.Exists(Path.GetDirectoryName(outputFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                }
                if (File.Exists(outputFile))
                    File.Delete(outputFile);

                FileStream fsCrypt = File.Open(inputFile, FileMode.OpenOrCreate);

                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.Padding = PaddingMode.PKCS7;

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);


                FileStream fsOut = File.Open(outputFile, FileMode.OpenOrCreate);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();
                return outputFile;
            }
        }

        internal Stream DecryptFileToStream(string inputFile)
        {
            Console.WriteLine("Decryption called for: " + inputFile);
            {

                string password = @"1L1k3Y0u"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);


                FileStream fsCrypt = File.Open(inputFile, FileMode.OpenOrCreate);

                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.Padding = PaddingMode.PKCS7;

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                //fsCrypt.Close();
                return cs;
            }
        }
    }
}
