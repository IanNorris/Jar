using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace Jar
{
	public class Crypto
	{
		private const string ProgramKey = @"^;Ssm5E8YGx|kY)gTR3e[5TK~*jYJ+J8TR3e[5TK~*jYJ+J8TR3e[5TK~*jYJ+J8";
		
		//Crypto functions based on answer by Brett
		// http://stackoverflow.com/questions/202011/encrypt-and-decrypt-a-string#2791259

		//HKLM function by Palani Kumar
		// http://stackoverflow.com/questions/9491958/registry-getvalue-always-return-null

		const uint SaltLength = 32;

		private static RegistryKey GetHKLM()
		{
			if( Environment.Is64BitOperatingSystem )
			{
				return RegistryKey.OpenBaseKey( RegistryHive.LocalMachine, RegistryView.Registry64 );
			}
			else
			{
				return RegistryKey.OpenBaseKey( RegistryHive.LocalMachine, RegistryView.Registry32 );
			}
		}

		private static Rfc2898DeriveBytes GetDeriveBytes()
		{
			RegistryKey HKLM = GetHKLM();

			//TODO: Move to ProtectedData API:
			// https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.protecteddata?view=net-5.0

			RegistryKey CryptoInfo = HKLM.OpenSubKey(@"Software\Microsoft\Cryptography", RegistryKeyPermissionCheck.ReadSubTree);
			string MachineGuid = (string)CryptoInfo.GetValue("MachineGuid", "");
			byte[] ProgramKeyBytes = Encoding.ASCII.GetBytes(ProgramKey);
			CryptoInfo.Close();

			HKLM.Close();

			return new Rfc2898DeriveBytes(MachineGuid, ProgramKeyBytes);
		}

		// NOTE: This function is not going to protect data
		// against a serious attacker. It is just designed to
		// prevent people grabbing the file and reading out the 
		// plain text password.
		public static string EncryptString( string Input )
		{
			var Key = GetDeriveBytes();
			
			RijndaelManaged AES = null;

			try
			{
				AES = new RijndaelManaged();
				AES.Key = Key.GetBytes( AES.KeySize / 8 );

				ICryptoTransform Encrypt = AES.CreateEncryptor( AES.Key, AES.IV );

				using( MemoryStream Stream = new MemoryStream() )
				{
					Stream.Write( BitConverter.GetBytes( AES.IV.Length ), 0, sizeof(int) );
					Stream.Write( AES.IV, 0, AES.IV.Length );
					using( CryptoStream CryptoStream = new CryptoStream( Stream, Encrypt, CryptoStreamMode.Write ) )
					{
						using( StreamWriter Writer = new StreamWriter( CryptoStream ) )
						{
							Writer.Write( Input );
						}
					}

					string Result = Convert.ToBase64String( Stream.ToArray() );
					return Result;
				}
			}
			finally
			{
				if( AES != null )
				{
					AES.Clear();
				}
			}
		}

		public static string DecryptString( string Input )
		{
			var Key = GetDeriveBytes();

			RijndaelManaged AES = null;

			try
			{
				byte[] Bytes = Convert.FromBase64String( Input );

				using( MemoryStream Stream = new MemoryStream( Bytes ) )
				{
					AES = new RijndaelManaged();
					AES.Key = Key.GetBytes( AES.KeySize / 8 );
					AES.IV = ReadByteArray( Stream );

					ICryptoTransform Decrypt = AES.CreateDecryptor( AES.Key, AES.IV );
					
					using( CryptoStream CryptoStream = new CryptoStream( Stream, Decrypt, CryptoStreamMode.Read ) )
					{
						using( StreamReader Reader = new StreamReader( CryptoStream ) )
						{
							return Reader.ReadToEnd();
						}
					}
				}
			}
			catch( System.Security.Cryptography.CryptographicException )
			{
				return "";
			}
			finally
			{
				if( AES != null )
				{
					AES.Clear();
				}
			}
		}

		private static byte[] ReadByteArray( Stream Stream )
		{
			byte[] LengthBytes = new byte[ sizeof(Int32) ];

			if( Stream.Read( LengthBytes, 0, LengthBytes.Length ) != LengthBytes.Length )
			{
				throw new SystemException("Unexpected end of stream.");
			}

			byte[] Buffer = new byte[ BitConverter.ToInt32( LengthBytes, 0 ) ];
			if( Stream.Read( Buffer, 0, Buffer.Length ) != Buffer.Length )
			{
				throw new SystemException("Not all bytes could be read.");
			}

			return Buffer;
		}
	}
}
