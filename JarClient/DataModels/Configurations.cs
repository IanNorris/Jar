using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Jar.Model;
using JarPluginApi;
using Sodium;

namespace Jar.DataModels
{
	public class Configurations
	{
		public Configurations(EventBus eventBus)
		{
		}

		public void SetDatabase(Database database, string password)
		{
			_database = database;

			byte[] saltBytes;

			//Key derivation salt
			var kdfSalt = GetConfigurationValues(KeyDerivation_PluginName, null, KeyDerivation_KeyName);
			if (kdfSalt.Any())
			{
				saltBytes = Utilities.Base64ToBinary(kdfSalt.First().Value, "");
			}
			else
			{
				saltBytes = PasswordHash.ScryptGenerateSalt();
				
				var salt = Utilities.BinaryToBase64(saltBytes);

				UpsertConfiguration(KeyDerivation_PluginName, null, KeyDerivation_KeyName, 0, salt, false);
			}

			_credentialKey = PasswordHash.ScryptHashBinary(Encoding.UTF8.GetBytes(password), saltBytes, PasswordHash.Strength.Medium, KeyDerivation_KeyLength);
		}

		public IEnumerable<Configuration> GetConfigurationValues(string pluginName, string account, string name)
		{
			var results = _database.Connection.Table<Configuration>().Where(x => x.Name == name && x.Plugin == pluginName && x.Account == account ).ToList();

			foreach(var result in results)
			{
				if(result.IsCredential)
				{
					result.Value = Decrypt(result.Value);
				}
			}
			
			return results;
		}

		public int UpsertConfiguration(string pluginName, string account, string name, int arrayIndex, string value, bool isCredential)
		{
			var newValue = isCredential ? Encrypt(value) : value;

			var existing = _database.Connection.Table<Configuration>().Where(x => x.Name == name && x.Plugin == pluginName && x.Account == account && x.ArrayIndex == arrayIndex).ToList();

			if(existing.Any())
			{
				var existingConfig = existing.First();
				existingConfig.Value = newValue;
				existingConfig.IsCredential = isCredential;

				_database.Connection.Update(existingConfig);

				return existingConfig.Id;
			}
			
			var newConfig = new Configuration
			{
				Account = account,
				ArrayIndex = arrayIndex,
				IsCredential = isCredential,
				Name = name,
				Plugin = pluginName,
				Value = newValue
			};

			_database.Connection.Insert(newConfig);


			var configId = (int)_database.GetLastInsertedRowId();

			return configId;
		}

		private string Encrypt(string plainText)
		{
			var nonce = SecretBox.GenerateNonce();
			var nonceB64 = Utilities.BinaryToBase64(nonce);

			var encryptedBytes = SecretBox.Create(plainText, nonce, _credentialKey);
			var encryptedBytesB64 = Utilities.BinaryToBase64(encryptedBytes);

			return $"{nonceB64}:{encryptedBytesB64}";
		}

		private string Decrypt(string cipherText)
		{
			var splitCipher = cipherText.Split(':');
			if(splitCipher.Length != 2)
			{
				throw new InvalidDataException("Attempting to decrypt a value that has no nonce.");
			}

			var nonceBytes = Utilities.Base64ToBinary(splitCipher[0], "");
			var cipherBytes = Utilities.Base64ToBinary(splitCipher[1], "");

			var plainTextBytes = SecretBox.Open(cipherBytes, nonceBytes, _credentialKey);
			return Encoding.UTF8.GetString(plainTextBytes);
		}

		private const string KeyDerivation_PluginName = "$System$";
		private const string KeyDerivation_KeyName = "KDFSalt";
		private const int KeyDerivation_KeyLength = 32;

		private Database _database;
		private byte[] _credentialKey;
	}
}
