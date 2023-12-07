namespace PlantUmlClassDiagramGenerator.Library
{
    public class Relationship(TypeNameText baseTypeName, TypeNameText subTypeName, string symbol, string baseLabel = "", string subLabel = "", string centerLabel = "")
    {
        protected TypeNameText baseTypeName = baseTypeName;
        protected TypeNameText subTypeName = subTypeName;
        protected string baseLabel = string.IsNullOrWhiteSpace(baseLabel) ? "" : $" \"{baseLabel}\"";
        protected string subLabel = string.IsNullOrWhiteSpace(subLabel) ? "" : $" \"{subLabel}\"";
        protected string centerLabel = string.IsNullOrWhiteSpace(centerLabel) ? "" : $" : \"{centerLabel}\"";
        private readonly string symbol = symbol;

        public override string ToString()
        {
            return $"{baseTypeName.Identifier}{baseLabel} {symbol}{subLabel} {subTypeName.Identifier}{centerLabel}";
        }
    }
}