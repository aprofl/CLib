using Xunit.Abstractions;
using CLib.Infos;

public class SingleTonTest : ComplexityTestBase
{
    public SingleTonTest(ITestOutputHelper output) : base(output) { }

    protected override string GetCodeFilePath()
        => Path.Combine("Base", $"Singleton.cs");

    [Fact]
    public void TestSingletonInstance()
    {
        var instance1 = TestSingleton.Instance;
        var instance2 = TestSingleton.Instance;

        Assert.NotNull(instance1);
        Assert.Same(instance1, instance2);
    }

    [Fact]
    public void TestLoadReturnsNullWhenFileDoesNotExist()
    {
        var path = Singleton<TestSingleton>.GetPath();            
        var result = Singleton<TestSingleton>.Load();

        Assert.Null(result);
    }

    [Fact]
    public void TestSaveAndLoad()
    {
        var instance = TestSingleton.Instance;
        instance.Value = 42;
        instance.Save();

        var loadedInstance = Singleton<TestSingleton>.Load();

        Assert.NotNull(loadedInstance);
        Assert.Equal(42, loadedInstance.Value);

        var path = Singleton<TestSingleton>.GetPath();
        if (File.Exists(path))
            File.Delete(path);
    }
}

public class TestSingleton : Singleton<TestSingleton>
{
    public int Value { get; set; }
}
