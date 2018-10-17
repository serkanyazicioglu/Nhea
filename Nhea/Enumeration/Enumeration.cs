namespace Nhea.Enumeration
{
    public struct Enumeration
    {
        public Enumeration(string name, string value, string detail)
            : this()
        {
            this.Name = name;
            this.Value = value;
            this.Detail = detail;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Detail { get; set; }
    }
}