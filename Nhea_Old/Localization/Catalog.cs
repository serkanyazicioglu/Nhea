using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhea.Localization
{
    public class Catalog
    {
        public Catalog()
        {
        }

        public string LanguageTitle { get; set; }

        public int LanguageId { get; set; }

        public string Key { get; set; }

        public string Translation { get; set; }

        public string SearchText
        {
            get
            {
                return Nhea.Text.StringHelper.ReplaceTurkishCharacters(Translation.Trim()).ToLower();
            }
        }

        public string Culture { get; set; }

        public string TwoLetterIsoLanguageName { get; set; }
    }
}
