using System;
using System.Collections.Generic;
using System.Linq;

namespace Nhea.Text.Password
{
    internal class PasswordGenerator
    {
        internal int TotalCount { get; set; }

        internal int PasswordLength { get; set; }

        internal string CharSet { get; set; }

        internal List<ConstantChar> ConstantChars { get; set; }

        private Random currentRandom;
        private Random CurrentRandom
        {
            get
            {
                if (currentRandom == null)
                {
                    currentRandom = new Random();
                }

                return currentRandom;
            }
        }

        internal string CreatePassword()
        {
            string password = string.Empty;

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
                    password = string.Concat(password, this.CharSet[(int)(CurrentRandom.NextDouble() * this.CharSet.Length)]);
                }
            }

            if (ValidatePassword(password))
            {
                return password;
            }
            else
            {
                return CreatePassword();
            }
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