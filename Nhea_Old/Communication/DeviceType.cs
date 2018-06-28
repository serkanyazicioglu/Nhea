using Nhea.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhea.Communication
{
    public enum DeviceType
    {
        [Detail("iOS")]
        iOS = 1,
        [Detail("Android")]
        Android = 2,
        [Detail("Browser")]
        Browser = 3
    }
}
