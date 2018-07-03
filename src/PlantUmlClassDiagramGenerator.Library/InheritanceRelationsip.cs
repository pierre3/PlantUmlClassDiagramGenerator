namespace PlantUmlClassDiagramGenerator.Library
{
    public class InheritanceRelationsip
    {
        private TypeNameText _baseTypeName;
        private TypeNameText _subTypeName;

        public InheritanceRelationsip(TypeNameText baseTypeName, TypeNameText subTypeName)
        {
            _baseTypeName = baseTypeName;
            _subTypeName = subTypeName;
        }

        public override string ToString()
        {
            var typeArgs = string.IsNullOrWhiteSpace(_baseTypeName.TypeArguments) ?
                "" :
                " \"" + _baseTypeName.TypeArguments + "\"";
            return $"{_baseTypeName.Identifier}{typeArgs} <|-- {_subTypeName.Identifier}";
        }
    }
}