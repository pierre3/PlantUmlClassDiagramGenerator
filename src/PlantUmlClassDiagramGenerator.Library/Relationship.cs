namespace PlantUmlClassDiagramGenerator.Library;

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

    private bool Equals(Relationship other)
    {
        return Equals(baseTypeName, other.baseTypeName) 
               && Equals(subTypeName, other.subTypeName) 
               && Equals(baseLabel, other.baseLabel)
               && Equals(subLabel, other.subLabel);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Relationship)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (baseTypeName != null ? baseTypeName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (subTypeName != null ? subTypeName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (baseLabel != null ? baseLabel.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (subLabel != null ? subLabel.GetHashCode() : 0);
            return hashCode;
        }
    }
}