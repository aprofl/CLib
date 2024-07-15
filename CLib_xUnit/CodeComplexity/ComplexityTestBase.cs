using Xunit;
using System.IO;
using System.Linq;
using System;
using System.Diagnostics;
using Xunit.Abstractions;

public abstract class ComplexityTestBase
{
    protected abstract string GetCodeFilePath();
    private readonly ComplexityAnalyzer _complexityAnalyzer = new ComplexityAnalyzer();
    private readonly ITestOutputHelper _output;

    protected ComplexityTestBase(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void TestCyclomaticComplexity()
    {
        var code = GetCodeToAnalyze();

        var complexity = _complexityAnalyzer.CalculateComplexity(code);
        var name = Path.GetFileNameWithoutExtension(filePath);
        _output.WriteLine($"{name} Cyclomatic complexity: {complexity}");
        Assert.True(complexity <= 7, $"Cyclomatic complexity is too high: {complexity}");
    }

    string filePath = string.Empty;
    private string GetCodeToAnalyze()
    {
        filePath = GetCodeFilePath();

        var baseDirectory = AppContext.BaseDirectory;
        string? solutionDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
        var newPath = Path.Combine(solutionDirectory ?? string.Empty, "CLib", filePath);

        if (!File.Exists(newPath))
        {
            throw new InvalidOperationException($"Could not find code file at {newPath}");
        }

        return File.ReadAllText(newPath);
    }
}
