using System;
using System.Collections.Generic;
using System.Text;

namespace Nhea.Localization
{
    public class Language
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Culture { get; set; }

        public string TwoLetterIsoLanguageName { get; set; }

        public string CurrencyCode { get; set; }

        public string CurrencyShortCode { get; set; }

        public int CurrencyCodeNo { get; set; }
    }
}