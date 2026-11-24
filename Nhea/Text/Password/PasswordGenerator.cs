using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Nhea.Text.Password
{
    internal class PasswordGenerator
    {
        internal int TotalCount { get; set; }

        internal int PasswordLength { get; set; }

        internal string CharSet { get; set; }

        internal List<ConstantChar> ConstantChars { get; set; }

        private const int MaxCreateAttempts = 1000;

        internal string CreatePassword()
        {
            for (int attempt = 0; attempt < MaxCreateAttempts; attempt++)
            {
                string password = BuildPassword();

                if (ValidatePassword(password))
                {
                    return password;
                }
            }

            throw new InvalidOperationException(String.Format("Could not generate a {0} character password from the given character set without exceeding {1} total or {2} sequential occurrences of the same character.", this.PasswordLength, MaxAllowedTotalDuplicateCharacter, MaxAllowedSequentalDuplicateCharacter));
        }

        private string BuildPassword()
        {
            string password = string.Empty;
            string randomChars = RandomNumberGenerator.GetString(this.CharSet, this.PasswordLength);

            for (int i = 0; i < this.PasswordLength; i++)
            {
                ConstantChar? constantChar = null;

                if (this.ConstantChars != null && this.ConstantChars.Count > 0 && ConstantChars.Any(p => p.CharIndex == i))
                {
                    constantChar = this.ConstantChars.Single(p => p.CharIndex == i);
                }

                if (constantChar.HasValue)
                {
                    password = string.Concat(password, constantChar.Value.Constant);
                }
                else
                {
                    password = string.Concat(password, randomChars[i]);
                }
            }

            return password;
        }

        private const int MaxAllowedTotalDuplicateCharacter = 4;

        private const int MaxAllowedSequentalDuplicateCharacter = 2;

        private static bool ValidatePassword(string password)
        {
            foreach (char character in password)
            {
                int characterCount = password.Count(c => c.Equals(character));

                if (characterCount > MaxAllowedTotalDuplicateCharacter)
                {
                    return false;
                }
                else if (characterCount > MaxAllowedSequentalDuplicateCharacter)
                {
                    string checkCharacters = new(character, MaxAllowedSequentalDuplicateCharacter + 1);

                    if (password.Contains(checkCharacters))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}