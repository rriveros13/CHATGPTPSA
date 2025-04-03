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
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace FileUploadService {
	[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
	public static class Rfc2898PasswordCryptographer {
		private static object lockObject = new object();
		private static Dictionary<string, byte[]> hashPassworDictionary = new Dictionary<string, byte[]>();
		private const int iterationsCountToCache = 1;
		private const int subkeyLength = 256 / 8;
		private const int iterationsCount = 20000;
		private const int saltSize = 128 / 8;
		[DefaultValue(false)]
		public static bool CanUseCachePassword { get; set; }
		public static string HashPassword(string password) {
			if(string.IsNullOrEmpty(password)) {
				return password;
			}
			byte[] salt;
			byte[] subkey;
			using(var deriveBytes = new Rfc2898DeriveBytes(password, saltSize, iterationsCount)) {
				salt = deriveBytes.Salt;
				subkey = deriveBytes.GetBytes(subkeyLength);
			}
			byte[] dst = new byte[1 + saltSize + subkeyLength];
			Buffer.BlockCopy(salt, 0, dst, 1, saltSize);
			Buffer.BlockCopy(subkey, 0, dst, 1 + saltSize, subkeyLength);
			return Convert.ToBase64String(dst);
		}
		public static bool VerifyHashedPassword(string hashedPassword, string password) {
			if(string.IsNullOrEmpty(hashedPassword))
				return string.IsNullOrEmpty(password);
			if(string.IsNullOrEmpty(password)) {
				return false;
			}
			byte[] hashedPasswordBytes;
			try { 
				hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
			}
			catch(FormatException) {
				return false;
			}
			if(hashedPasswordBytes.Length != (1 + saltSize + subkeyLength) || hashedPasswordBytes[0] != 0x00) {
				return false;
			}
			byte[] salt = new byte[saltSize];
			byte[] subkey = new byte[subkeyLength];
			Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, saltSize);
			Buffer.BlockCopy(hashedPasswordBytes, 1 + saltSize, subkey, 0, subkeyLength);
			byte[] newSubkey;
			if(CanUseCachePassword) {
				lock(lockObject) {
					byte[] subkeyToCache = CreateSubKey(password, salt, iterationsCountToCache);
					string subkeyToCacheString = Convert.ToBase64String(subkeyToCache);
					if(!hashPassworDictionary.TryGetValue(subkeyToCacheString, out newSubkey)) {
						newSubkey = CreateSubKey(password, salt, iterationsCount);
						hashPassworDictionary.Add(subkeyToCacheString, newSubkey);
					}
				}
			}
			else {
				newSubkey = CreateSubKey(password, salt, iterationsCount);
			}
			return IsEqual(subkey, newSubkey);
		}
		private static byte[] CreateSubKey(string password, byte[] salt, int iterationsCount) {
			byte[] newSubkey;
			using(var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterationsCount)) {
				newSubkey = deriveBytes.GetBytes(subkeyLength);
			}
			return newSubkey;
		}
		[MethodImpl(MethodImplOptions.NoOptimization)]
		private static bool IsEqual(byte[] x, byte[] y) {
			if(ReferenceEquals(x, y)) {
				return true;
			}
			if(x == null || y == null || x.Length != y.Length) {
				return false;
			}
			bool result = true;
			for(int i = 0; i < x.Length; i++) {
				result &= (x[i] == y[i]);
			}
			return result;
		}
		public static string GenerateSalt(int length = saltSize) {
			return Convert.ToBase64String(GenerateSaltCore(length));
		}
		private static byte[] GenerateSaltCore(int length) {
			byte[] buf = new byte[length];
			using(var rng = new RNGCryptoServiceProvider()) {
				rng.GetBytes(buf);
			}
			return buf;
		}
		public static void ClearHashPasswordCache() {
			lock(lockObject) {
				hashPassworDictionary.Clear();
			}
		}
	}
}
