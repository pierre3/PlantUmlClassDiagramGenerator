using System;
using System.Collections.Generic;
using PlantUmlClassDiagramGenerator.Library;

namespace PlantUmlClassDiagramGenerator.Generator;

public interface IPlantUmlGenerator
{
    public bool GeneratePlantUml(Dictionary<string, string> parameters);
    
    public static Accessibilities GetIgnoreAccessibilities(Dictionary<string, string> parameters)
    {
        var ignoreAcc = Accessibilities.None;
        if (parameters.ContainsKey("-public"))
        {
            ignoreAcc = Accessibilities.Private | Accessibilities.Internal
                                                | Accessibilities.Protected | Accessibilities.ProtectedInternal;
        }
        else if (parameters.TryGetValue("-ignore", out string value))
        {
            var ignoreItems = value.Split(',');
            foreach (var item in ignoreItems)
            {
                if (Enum.TryParse(item, true, out Accessibilities acc))
                {
                    ignoreAcc |= acc;
                }
            }
        }
        return ignoreAcc;
    }


}