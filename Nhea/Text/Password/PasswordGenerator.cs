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
            string password = String.Empty;

            for (int i = 0; i < this.PasswordLength; i++)
            {
                ConstantChar? constantChar = null;

                if (this.ConstantChars != null && this.ConstantChars.Count() > 0 && this.ConstantChars.Where(p => p.CharIndex == i).Count() > 0)
                {
                    constantChar = this.ConstantChars.Where(p => p.CharIndex == i).Single();
                }

                if (constantChar.HasValue)
                {
                    password = String.Concat(password, constantChar.Value.Constant);
                }
                else
                {
                    password = String.Concat(password, this.CharSet[(int)(CurrentRandom.NextDouble() * this.CharSet.Length)]);
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

        private bool ValidatePassword(string password)
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
                    string checkCharacters = new string(character, MaxAllowedSequentalDuplicateCharacter + 1);

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