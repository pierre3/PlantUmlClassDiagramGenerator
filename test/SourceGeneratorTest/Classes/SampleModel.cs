﻿using PlantUmlClassDiagramGenerator.SourceGenerator.Attributes;
using SourceGeneratorTest.Library.Logs;
using SourceGeneratorTest.Library.Types;

namespace SourceGeneratorTest.Classes;

internal class SampleModel
{
    private readonly ILogger logger;
    private readonly IList<StructA> structures;

    public IReadOnlyList<Item> Items { get; } = new List<Item>();

    public SampleModel(ILogger logger, IList<StructA> structures)
    {
        this.logger = logger;
        this.structures = structures;
    }

    public async ValueTask Execute(Parameters parameters)
    {
        await Task.Delay(1000);
    }
}
