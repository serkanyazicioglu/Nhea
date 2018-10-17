using Nhea.Enumeration;

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
