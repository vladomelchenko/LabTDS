namespace LabTDS.Models
{
    public class SimpleStatement
    {
        public char Symbol { get; set; }
        public bool IsNegative { get; set; }
        public bool Inverse { get; set; }
        public SimpleStatement(string clause, bool inverse = false)
        {
            if (clause[0] == '~')
            {
                IsNegative = true;
                Symbol = clause[1];
            }
            else
            {
                Symbol = clause[0];
            }
        }

        public bool Equals(SimpleStatement obj)
        {
            return obj.IsNegative == this.IsNegative && obj.Symbol == this.Symbol;
        }

        public bool CanResolve(SimpleStatement obj)
        {
            return obj.IsNegative != this.IsNegative && obj.Symbol == this.Symbol;
        }

        public override string ToString()
        {
            return this.IsNegative ? $"~{Symbol}" : $"{Symbol}";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}