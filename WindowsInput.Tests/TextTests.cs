using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using ApprovalTests;
using ApprovalTests.Core;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using HtmlAgilityPack;
using NUnit.Framework;
using WindowsInput.Native;
using WindowsInput.Tests.UnicodeTestSurface;
using Approvals = ApprovalTests.Approvals;

namespace WindowsInput.Tests
{
    public class UnicodeRange
    {
        public string Name { get; set; }
        public int Low { get; set; }
        public int High { get; set; }

        public UnicodeRange(string name, int low, int high)
        {
            Name = name;
            Low = low;
            High = high;
        }
    }

    public class CustomTestNamer : UnitTestFrameworkNamer, IApprovalNamer
    {
        private readonly string _name;

        public CustomTestNamer(string name)
        {
            _name = name;
        }

        string IApprovalNamer.Name
        {
            get { return _name; }
        }
    }

    [TestFixture]
    //[UseReporter(typeof(DiffReporter))]
    public class TextTests
    {
        public TestCaseData[] UnicodeTestCases
        {
            get
            {
                var ranges = GetUnicodeRanges();
                var cases = Array.ConvertAll(ranges, BuildTestCase);
                return cases;
            }
        }

        private TestCaseData BuildTestCase(UnicodeRange input)
        {
            return new TestCaseData(input).SetName(input.Name);
        }

        public UnicodeRange[] GetUnicodeRanges()
        {
            return new[]
                       {
                           new UnicodeRange("Basic Latin", 0x0020, 0x007F),
                           new UnicodeRange("Block Elements", 0x2580, 0x259F),
                           new UnicodeRange("Latin-1 Supplement", 0x00A0, 0x00FF),
                           new UnicodeRange("Geometric Shapes", 0x25A0, 0x25FF),
                           new UnicodeRange("Latin Extended-A", 0x0100, 0x017F),
                           new UnicodeRange("Miscellaneous Symbols", 0x2600, 0x26FF),
                           new UnicodeRange("Latin Extended-B", 0x0180, 0x024F),
                           new UnicodeRange("Dingbats", 0x2700, 0x27BF),
                           new UnicodeRange("IPA Extensions", 0x0250, 0x02AF),
                           new UnicodeRange("Miscellaneous Mathematical Symbols-A", 0x27C0, 0x27EF),
                           new UnicodeRange("Spacing Modifier Letters", 0x02B0, 0x02FF),
                           new UnicodeRange("Supplemental Arrows-A", 0x27F0, 0x27FF),
                           new UnicodeRange("Combining Diacritical Marks", 0x0300, 0x036F),
                           new UnicodeRange("Braille Patterns", 0x2800, 0x28FF),
                           new UnicodeRange("Greek and Coptic", 0x0370, 0x03FF),
                           new UnicodeRange("Supplemental Arrows-B", 0x2900, 0x297F),
                           new UnicodeRange("Cyrillic", 0x0400, 0x04FF),
                           new UnicodeRange("Miscellaneous Mathematical Symbols-B", 0x2980, 0x29FF),
                           new UnicodeRange("Cyrillic Supplementary", 0x0500, 0x052F),
                           new UnicodeRange("Supplemental Mathematical Operators", 0x2A00, 0x2AFF),
                           new UnicodeRange("Armenian", 0x0530, 0x058F),
                           new UnicodeRange("Miscellaneous Symbols and Arrows", 0x2B00, 0x2BFF),
                           new UnicodeRange("Hebrew", 0x0590, 0x05FF),
                           new UnicodeRange("CJK Radicals Supplement", 0x2E80, 0x2EFF),
                           new UnicodeRange("Arabic", 0x0600, 0x06FF),
                           new UnicodeRange("Kangxi Radicals", 0x2F00, 0x2FDF),
                           new UnicodeRange("Syriac", 0x0700, 0x074F),
                           new UnicodeRange("Ideographic Description Characters", 0x2FF0, 0x2FFF),
                           new UnicodeRange("Thaana", 0x0780, 0x07BF),
                           new UnicodeRange("CJK Symbols and Punctuation", 0x3000, 0x303F),
                           new UnicodeRange("Devanagari", 0x0900, 0x097F),
                           new UnicodeRange("Hiragana", 0x3040, 0x309F),
                           new UnicodeRange("Bengali", 0x0980, 0x09FF),
                           new UnicodeRange("Katakana", 0x30A0, 0x30FF),
                           new UnicodeRange("Gurmukhi", 0x0A00, 0x0A7F),
                           new UnicodeRange("Bopomofo", 0x3100, 0x312F),
                           new UnicodeRange("Gujarati", 0x0A80, 0x0AFF),
                           new UnicodeRange("Hangul Compatibility Jamo", 0x3130, 0x318F),
                           new UnicodeRange("Oriya", 0x0B00, 0x0B7F),
                           new UnicodeRange("Kanbun", 0x3190, 0x319F),
                           new UnicodeRange("Tamil", 0x0B80, 0x0BFF),
                           new UnicodeRange("Bopomofo Extended", 0x31A0, 0x31BF),
                           new UnicodeRange("Telugu", 0x0C00, 0x0C7F),
                           new UnicodeRange("Katakana Phonetic Extensions", 0x31F0, 0x31FF),
                           new UnicodeRange("Kannada", 0x0C80, 0x0CFF),
                           new UnicodeRange("Enclosed CJK Letters and Months", 0x3200, 0x32FF),
                           new UnicodeRange("Malayalam", 0x0D00, 0x0D7F),
                           new UnicodeRange("CJK Compatibility", 0x3300, 0x33FF),
                           new UnicodeRange("Sinhala", 0x0D80, 0x0DFF),
                           new UnicodeRange("CJK Unified Ideographs Extension A", 0x3400, 0x4DBF),
                           new UnicodeRange("Thai", 0x0E00, 0x0E7F),
                           new UnicodeRange("Yijing Hexagram Symbols", 0x4DC0, 0x4DFF),
                           new UnicodeRange("Lao", 0x0E80, 0x0EFF),
                           new UnicodeRange("CJK Unified Ideographs", 0x4E00, 0x9FFF),
                           new UnicodeRange("Tibetan", 0x0F00, 0x0FFF),
                           new UnicodeRange("Yi Syllables", 0xA000, 0xA48F),
                           new UnicodeRange("Myanmar", 0x1000, 0x109F),
                           new UnicodeRange("Yi Radicals", 0xA490, 0xA4CF),
                           new UnicodeRange("Georgian", 0x10A0, 0x10FF),
                           new UnicodeRange("Hangul Syllables", 0xAC00, 0xD7AF),
                           new UnicodeRange("Hangul Jamo", 0x1100, 0x11FF),
                           new UnicodeRange("High Surrogates", 0xD800, 0xDB7F),
                           new UnicodeRange("Ethiopic", 0x1200, 0x137F),
                           new UnicodeRange("High Private Use Surrogates", 0xDB80, 0xDBFF),
                           new UnicodeRange("Cherokee", 0x13A0, 0x13FF),
                           new UnicodeRange("Low Surrogates", 0xDC00, 0xDFFF),
                           new UnicodeRange("Unified Canadian Aboriginal Syllabics", 0x1400, 0x167F),
                           new UnicodeRange("Private Use Area", 0xE000, 0xF8FF),
                           new UnicodeRange("Ogham", 0x1680, 0x169F),
                           new UnicodeRange("CJK Compatibility Ideographs", 0xF900, 0xFAFF),
                           new UnicodeRange("Runic", 0x16A0, 0x16FF),
                           new UnicodeRange("Alphabetic Presentation Forms", 0xFB00, 0xFB4F),
                           new UnicodeRange("Tagalog", 0x1700, 0x171F),
                           new UnicodeRange("Arabic Presentation Forms-A", 0xFB50, 0xFDFF),
                           new UnicodeRange("Hanunoo", 0x1720, 0x173F),
                           new UnicodeRange("Variation Selectors", 0xFE00, 0xFE0F),
                           new UnicodeRange("Buhid", 0x1740, 0x175F),
                           new UnicodeRange("Combining Half Marks", 0xFE20, 0xFE2F),
                           new UnicodeRange("Tagbanwa", 0x1760, 0x177F),
                           new UnicodeRange("CJK Compatibility Forms", 0xFE30, 0xFE4F),
                           new UnicodeRange("Khmer", 0x1780, 0x17FF),
                           new UnicodeRange("Small Form Variants", 0xFE50, 0xFE6F),
                           new UnicodeRange("Mongolian", 0x1800, 0x18AF),
                           new UnicodeRange("Arabic Presentation Forms-B", 0xFE70, 0xFEFF),
                           new UnicodeRange("Limbu", 0x1900, 0x194F),
                           new UnicodeRange("Halfwidth and Fullwidth Forms", 0xFF00, 0xFFEF),
                           new UnicodeRange("Tai Le", 0x1950, 0x197F),
                           new UnicodeRange("Specials", 0xFFF0, 0xFFFF),
                           new UnicodeRange("Khmer Symbols", 0x19E0, 0x19FF),
                           new UnicodeRange("Linear B Syllabary", 0x10000, 0x1007F),
                           new UnicodeRange("Phonetic Extensions", 0x1D00, 0x1D7F),
                           new UnicodeRange("Linear B Ideograms", 0x10080, 0x100FF),
                           new UnicodeRange("Latin Extended Additional", 0x1E00, 0x1EFF),
                           new UnicodeRange("Aegean Numbers", 0x10100, 0x1013F),
                           new UnicodeRange("Greek Extended", 0x1F00, 0x1FFF),
                           new UnicodeRange("Old Italic", 0x10300, 0x1032F),
                           new UnicodeRange("General Punctuation", 0x2000, 0x206F),
                           new UnicodeRange("Gothic", 0x10330, 0x1034F),
                           new UnicodeRange("Superscripts and Subscripts", 0x2070, 0x209F),
                           new UnicodeRange("Ugaritic", 0x10380, 0x1039F),
                           new UnicodeRange("Currency Symbols", 0x20A0, 0x20CF),
                           new UnicodeRange("Deseret", 0x10400, 0x1044F),
                           new UnicodeRange("Combining Diacritical Marks for Symbols", 0x20D0, 0x20FF),
                           new UnicodeRange("Shavian", 0x10450, 0x1047F),
                           new UnicodeRange("Letterlike Symbols", 0x2100, 0x214F),
                           new UnicodeRange("Osmanya", 0x10480, 0x104AF),
                           new UnicodeRange("Number Forms", 0x2150, 0x218F),
                           new UnicodeRange("Cypriot Syllabary", 0x10800, 0x1083F),
                           new UnicodeRange("Arrows", 0x2190, 0x21FF),
                           new UnicodeRange("Byzantine Musical Symbols", 0x1D000, 0x1D0FF),
                           new UnicodeRange("Mathematical Operators", 0x2200, 0x22FF),
                           new UnicodeRange("Musical Symbols", 0x1D100, 0x1D1FF),
                           new UnicodeRange("Miscellaneous Technical", 0x2300, 0x23FF),
                           new UnicodeRange("Tai Xuan Jing Symbols", 0x1D300, 0x1D35F),
                           new UnicodeRange("Control Pictures", 0x2400, 0x243F),
                           new UnicodeRange("Mathematical Alphanumeric Symbols", 0x1D400, 0x1D7FF),
                           new UnicodeRange("Optical Character Recognition", 0x2440, 0x245F),
                           new UnicodeRange("CJK Unified Ideographs Extension B", 0x20000, 0x2A6DF),
                           new UnicodeRange("Enclosed Alphanumerics", 0x2460, 0x24FF),
                           new UnicodeRange("CJK Compatibility Ideographs Supplement", 0x2F800, 0x2FA1F),
                           new UnicodeRange("Box Drawing", 0x2500, 0x257F),
                           new UnicodeRange("Tags", 0xE0000, 0xE007F)
                       };
        }

        [Test]
        [TestCaseSource("UnicodeTestCases")]
        public void TestUnicodeRanges(UnicodeRange range)
        {
            var tempFile = Path.GetTempFileName();
            using (
                var process =
                    Process.Start(
                        @"C:\dev\InputSimulator\WindowsInput.Tests.UnicodeTestSurface\bin\Debug\WindowsInput.Tests.UnicodeTestSurface.exe",
                        tempFile))
            {
                var sim = new InputSimulator();
                sim.Keyboard.Sleep(3000).SimulateUnicodeRange(range).Sleep(3000);
                process.CloseMainWindow();
                Thread.Sleep(3000);
                var text = File.ReadAllText(tempFile, Encoding.UTF32);
                Approvals.Verify(new ApprovalTextWriter(text), new CustomTestNamer(range.Name), Approvals.GetReporter());
            }
        }

        [Test]
        [Explicit]
        public void GetCharacterRanges()
        {
            using (var client = new WebClient())
            {
                var html = client.DownloadString("http://jrgraphix.net/r/Unicode/");
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                foreach (var link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    var a = link.GetAttributeValue("href", "unknown");
                    if (a.Contains("Unicode"))
                    {
                        a = "0x" + a.Replace("/r/Unicode/", "").Replace("-", ", 0x") + "));";
                        Console.WriteLine("ranges.Add(new UnicodeRange(\"" + link.InnerText + "\", " + a);
                    }

                }

            }
        }
    }

    public static class KeyboardSimulatorExtensions
    {
        public static IKeyboardSimulator SimulateUnicodeRange(this IKeyboardSimulator sim, UnicodeRange range)
        {
            var i = range.Low;
            var sb = new StringBuilder(range.High - range.Low + 50);
            while (i <= range.High)
            {
                sb.Append(char.ConvertFromUtf32(i));
                i++;
            }

            sim.TextEntry(sb.ToString());
            return sim;
        }
    }
}