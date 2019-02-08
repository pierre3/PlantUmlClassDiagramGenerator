namespace PlantUmlClassDiagramGenerator.Library
{
    public abstract class Relationship
    {
        protected TypeNameText _baseTypeName;
        protected TypeNameText _subTypeName;
        protected string _baseLabel;
        protected string _subLabel;
        private string _symbol;

        public Relationship(TypeNameText baseTypeName, TypeNameText subTypeName, string symbol, string baseLabel, string subLabel)
        {
            _baseTypeName = baseTypeName;
            _subTypeName = subTypeName;
            _symbol = symbol;
            _baseLabel = string.IsNullOrWhiteSpace(baseLabel) ? "" : $" \"{baseLabel}\"";
            _subLabel = string.IsNullOrWhiteSpace(subLabel) ? "" : $" \"{subLabel}\"";
        }

        public override string ToString()
        {
            return $"{_baseTypeName.Identifier}{_baseLabel} {_symbol} {_subTypeName.Identifier}";
        }
    }
}