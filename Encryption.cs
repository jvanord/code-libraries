using System;
using System.Data;
using System.Security.Cryptography;
using System.IO;

namespace Indasys.Encryption
{
	public class Cryptor
	{
		private RijndaelManaged _rijndael = new RijndaelManaged();
		private ICryptoTransform _encryptor, _decryptor;

		public System.Text.Encoding _utfEncoder = new System.Text.UTF8Encoding();

		public static byte[] RandomByteArray(int length)
		{
			byte[] array = new byte[length];
			Random random = new Random();
			random.NextBytes(array);
			return array;
		}

		public Cryptor() { initializeCryptor(GenerateEncryptionKey(), GenerateEncryptionVector()); }

		public Cryptor(byte[] encryptionKey) { initializeCryptor(encryptionKey, GenerateEncryptionVector()); }

		public Cryptor(byte[] encryptionKey, byte[] encryptionVector) { initializeCryptor(encryptionKey, encryptionVector); }

		private void initializeCryptor(byte[] encryptionKey, byte[] encryptionVector)
		{
			_encryptor = _rijndael.CreateEncryptor(encryptionKey, encryptionVector);
			_decryptor = _rijndael.CreateDecryptor(encryptionKey, encryptionVector);
		}

		public byte[] EncryptionKey { get { return _rijndael.Key; } }

		public byte[] EncryptionVector { get { return _rijndael.IV; } }

		public byte[] GenerateEncryptionKey()
		{
			_rijndael.GenerateKey();
			return _rijndael.Key;
		}

		public byte[] GenerateEncryptionVector()
		{
			_rijndael.GenerateIV();
			return _rijndael.IV;
		}

		public string EncryptToString(string stringToEncrypt)
		{
			return EncodeUTF8(Encrypt(stringToEncrypt));
		}

		public byte[] Encrypt(string stringToEncrypt)
		{
			return Encrypt(_utfEncoder.GetBytes(stringToEncrypt));
		}

		public byte[] Encrypt(byte[] bytes)
		{
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cs = new CryptoStream(memoryStream, _encryptor, CryptoStreamMode.Write);
			cs.Write(bytes, 0, bytes.Length);
			cs.FlushFinalBlock();
			memoryStream.Position = 0;
			byte[] encrypted = new byte[memoryStream.Length];
			memoryStream.Read(encrypted, 0, encrypted.Length);
			cs.Close();
			memoryStream.Close();
			return encrypted;
		}

		public string DecryptString(string encryptedString)
		{
			return EncodeUTF8(Decrypt(DecodeUTF8(encryptedString)));
		}

		public byte[] Decrypt(byte[] encryptedValue)
		{
			MemoryStream encryptedStream = new MemoryStream();
			CryptoStream decryptStream = new CryptoStream(encryptedStream, _decryptor, CryptoStreamMode.Write);
			decryptStream.Write(encryptedValue, 0, encryptedValue.Length);
			decryptStream.FlushFinalBlock();
			encryptedStream.Position = 0;
			Byte[] decryptedBytes = new Byte[encryptedStream.Length];
			encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
			encryptedStream.Close();
			return decryptedBytes;
		}

		public byte[] DecodeUTF8(string str) { return _utfEncoder.GetBytes(str); }

		public string EncodeUTF8(byte[] bytes) { return _utfEncoder.GetString(bytes); }
	}
}