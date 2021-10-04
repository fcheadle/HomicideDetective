using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace HomicideDetective.Words
{
    public class PhysicalProperties : IPrintable
    {
        public int Mass { get; set; } //grams
        public int Volume { get; set; } //cm^3
        public string? QualityAdjective { get; set; }
        public string? SizeAdjective { get; set; }
        public string? AgeAdjective { get; set; }
        public string? ShapeAdjective { get; set; }
        public string? ColorAdjective { get; set; }
        public string? ProperAdjective { get; set; }
        
        public PhysicalProperties(int mass, int volume, string? quality = null, string? size = null, string? age = null, string? shape = null, string? color = null, string? proper = null)
        {
            Volume = volume;
            Mass = mass;
            QualityAdjective = quality;
            SizeAdjective = size;
            AgeAdjective = age;
            ShapeAdjective = shape;
            ColorAdjective = color;
            ProperAdjective = proper;
        }

        public string GetPrintableString()
        {
            var sb = new StringBuilder();

            if(QualityAdjective != null) sb.Append($"{QualityAdjective} ");
            if(SizeAdjective != null) sb.Append($"{SizeAdjective} ");
            if(AgeAdjective != null) sb.Append($"{AgeAdjective} ");
            if(ShapeAdjective != null) sb.Append($"{ShapeAdjective} ");
            if(ColorAdjective != null) sb.Append($"{ColorAdjective} ");
            if(ProperAdjective != null) sb.Append($"{ProperAdjective}");
            var answer = sb.ToString();
            return answer.Trim();
        }
    }
}