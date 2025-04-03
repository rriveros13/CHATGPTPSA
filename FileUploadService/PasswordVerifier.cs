#region Copyright (c) 2000-2018 Developer Express Inc.
/*
{*******************************************************************}
{                                                                   }
{       Developer Express .NET Component Library                    }
{       eXpressApp Framework                                        }
{                                                                   }
{       Copyright (c) 2000-2018 Developer Express Inc.              }
{       ALL RIGHTS RESERVED                                         }
{                                                                   }
{   The entire contents of this file is protected by U.S. and       }
{   International Copyright Laws. Unauthorized reproduction,        }
{   reverse-engineering, and distribution of all or any portion of  }
{   the code contained in this file is strictly prohibited and may  }
{   result in severe civil and criminal penalties and will be       }
{   prosecuted to the maximum extent possible under the law.        }
{                                                                   }
{   RESTRICTIONS                                                    }
{                                                                   }
{   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           }
{   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          }
{   SECRETS OF DEVELOPER EXPRESS INC. THE REGISTERED DEVELOPER IS   }
{   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    }
{   CONTROLS AS PART OF AN EXECUTABLE PROGRAM ONLY.                 }
{                                                                   }
{   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      }
{   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        }
{   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       }
{   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  }
{   AND PERMISSION FROM DEVELOPER EXPRESS INC.                      }
{                                                                   }
{   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       }
{   ADDITIONAL RESTRICTIONS.                                        }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2000-2018 Developer Express Inc.

using System;
using System.ComponentModel;
using System.Security.Cryptography;

namespace FileUploadService {
	public delegate string HashPasswordDelegate(string password);
	public delegate bool VerifyHashedPasswordDelegate(string hashedPassword, string password);
	public class PasswordCryptographer {
		private static bool _enableRfc2898 = false; 
		private static bool _supportLegacySha512 = true;
		private static HashPasswordDelegate _hashPasswordDelegate = HashPassword;
		private static VerifyHashedPasswordDelegate _verifyHashedPasswordDelegate = VerifyHashedPassword;
		const int saltLength = 6;
		const string delim = "*";
		static string SaltPassword(string password, string salt) {
			SHA512 hashAlgorithm = SHA512.Create();
			return Convert.ToBase64String(hashAlgorithm.ComputeHash(System.Text.Encoding.UTF8.GetBytes(salt + password)));
		}
		static string HashPassword(string password) {
			if(EnableRfc2898) {
				return Rfc2898PasswordCryptographer.HashPassword(password);
			}
			if(string.IsNullOrEmpty(password))
				return password;
			byte[] randomSalt = new byte[saltLength];
			new RNGCryptoServiceProvider().GetBytes(randomSalt);
			string salt = Convert.ToBase64String(randomSalt);
			return salt + delim + SaltPassword(password, salt);
		}
		static bool VerifyHashedPassword(string saltedPassword, string password) {
			if(EnableRfc2898) {
				bool result = false;
				result = Rfc2898PasswordCryptographer.VerifyHashedPassword(saltedPassword, password);
				if(!result && SupportLegacySha512) { 
					try {
						result = AreEqualSHA512(saltedPassword, password);
					}
					catch {
					}
				}
				return result;
			}
			return AreEqualSHA512(saltedPassword, password);
		}
		static bool AreEqualSHA512(string saltedPassword, string password) {
			if(string.IsNullOrEmpty(saltedPassword))
				return string.IsNullOrEmpty(password);
			if(string.IsNullOrEmpty(password)) {
				return false;
			}
			int delimPos = saltedPassword.IndexOf(delim);
			if(delimPos <= 0) {
				return saltedPassword.Equals(password);
			}
			else {
				string calculatedSaltedPassword = SaltPassword(password, saltedPassword.Substring(0, delimPos));
				string expectedSaltedPassword = saltedPassword.Substring(delimPos + delim.Length);
				if(expectedSaltedPassword.Equals(calculatedSaltedPassword)) {
					return true;
				}
				return expectedSaltedPassword.Equals(SaltPassword(password, "System.Byte[]"));
			}
		}
		public static bool EnableRfc2898 {
			get { return PasswordCryptographer._enableRfc2898; }
			set { PasswordCryptographer._enableRfc2898 = value; }
		}
		public static bool SupportLegacySha512 {
			get { return PasswordCryptographer._supportLegacySha512; }
			set { PasswordCryptographer._supportLegacySha512 = value; }
		}
		public static VerifyHashedPasswordDelegate VerifyHashedPasswordDelegate {
			get { return PasswordCryptographer._verifyHashedPasswordDelegate; }
			set { PasswordCryptographer._verifyHashedPasswordDelegate = value; }
		}
		public static HashPasswordDelegate HashPasswordDelegate {
			get { return PasswordCryptographer._hashPasswordDelegate; }
			set { PasswordCryptographer._hashPasswordDelegate = value; }
		}
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public virtual string GenerateSaltedPassword(string password) {
			return HashPasswordDelegate(password);
		}
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool AreEqual(string saltedPassword, string password) {
			return VerifyHashedPasswordDelegate(saltedPassword, password);
		}
	}
}
