namespace PlantUmlClassDiagramGenerator.Library
{
    public class Relationship
    {
        protected TypeNameText baseTypeName;
        protected TypeNameText subTypeName;
        protected string baseLabel;
        protected string subLabel;
        private readonly string symbol;

        public Relationship(TypeNameText baseTypeName, TypeNameText subTypeName, string symbol, string baseLabel = "", string subLabel = "")
        {
            this.baseTypeName = baseTypeName;
            this.subTypeName = subTypeName;
            this.symbol = symbol;
            this.baseLabel = string.IsNullOrWhiteSpace(baseLabel) ? "" : $" \"{baseLabel}\"";
            this.subLabel = string.IsNullOrWhiteSpace(subLabel) ? "" : $" \"{subLabel}\"";
        }

        public override string ToString()
        {
            return $"{baseTypeName.Identifier}{baseLabel} {symbol}{subLabel} {subTypeName.Identifier}";
        }
    }
}