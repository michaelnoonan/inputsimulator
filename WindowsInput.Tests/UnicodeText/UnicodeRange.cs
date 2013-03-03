using System.Text;

namespace WindowsInput.Tests.UnicodeText
{
    public class UnicodeRange
    {
        public string Name { get; set; }
        public int Low { get; set; }
        public int High { get; set; }

        public string Characters
        {
            get
            {
                var i = Low;
                var sb = new StringBuilder(High - Low + 10);
                while (i <= High)
                {
                    sb.Append(char.ConvertFromUtf32(i));
                    i++;
                }
                return sb.ToString();
            }
        }

        public UnicodeRange(string name, int low, int high)
        {
            Name = name;
            Low = low;
            High = high;
        }
    }
}