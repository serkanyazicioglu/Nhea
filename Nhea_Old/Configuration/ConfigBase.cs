using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nhea.Configuration
{
    internal abstract class ConfigBase
    {
        internal abstract object GetValue(string parameterName);
    }
}
