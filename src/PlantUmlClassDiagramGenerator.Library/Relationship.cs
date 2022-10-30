namespace PlantUmlClassDiagramGenerator.Library
{
    public class Relationship
    {
        protected TypeNameText baseTypeName;
        protected TypeNameText subTypeName;
        protected string baseLabel;
        protected string subLabel;
        protected string centerLabel;
        private readonly string symbol;

        public Relationship(TypeNameText baseTypeName, TypeNameText subTypeName, string symbol, string baseLabel = "", string subLabel = "", string centerLabel="")
        {
            this.baseTypeName = baseTypeName;
            this.subTypeName = subTypeName;
            this.symbol = symbol;
            this.baseLabel = string.IsNullOrWhiteSpace(baseLabel) ? "" : $" \"{baseLabel}\"";
            this.subLabel = string.IsNullOrWhiteSpace(subLabel) ? "" : $" \"{subLabel}\"";
            this.centerLabel = string.IsNullOrWhiteSpace(centerLabel) ? "" : $" : \"{centerLabel}\"";
        }

        public override string ToString()
        {
            return $"{baseTypeName.Identifier}{baseLabel} {symbol}{subLabel} {subTypeName.Identifier}{centerLabel}";
        }
    }
}