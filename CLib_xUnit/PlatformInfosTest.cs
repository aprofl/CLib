using Xunit.Abstractions;
using CLib.Infos;

public class PlatformInfosTests : ComplexityTestBase
{
    private readonly string _filePath;
    protected override string GetCodeFilePath()
        => Path.Combine("ExternalRef", $"{nameof(Platforms)}.cs");

    public PlatformInfosTests(ITestOutputHelper output) : base(output)
    {
        _filePath = Singleton<Platforms>.GetPath();
    }

    [Fact]
    public void TestInitAndSave()
    {
        var platformInfos = Platforms.Instance;

        if (File.Exists(_filePath))
            File.Delete(_filePath);
        platformInfos.Init();

        Assert.NotNull(platformInfos.List);
        Assert.True(platformInfos.List.Count > 0);
        Assert.True(platformInfos.dicPlatform.Count > 0);
        
        foreach (var platform in platformInfos.List)
            Assert.True(platformInfos.dicPlatform.ContainsKey(platform.Type));
        if (File.Exists(_filePath))
            File.Delete(_filePath);        
        platformInfos.List.Clear();
    }

    [Fact]
    public void TestLoadReturnsInstanceWithPlatformInfo()
    {
        var platformInfos = Platforms.Instance;
        Assert.True(File.Exists(_filePath));
        var loadedPlatformInfos = Singleton<Platforms>.Load();
        loadedPlatformInfos?.Init();

        Assert.NotNull(loadedPlatformInfos);
        Assert.NotNull(loadedPlatformInfos.List);
        Assert.True(loadedPlatformInfos.List.Count > 0);

        Assert.True(loadedPlatformInfos.dicPlatform.Count > 0);
        foreach (var platform in loadedPlatformInfos.List)
            Assert.True(loadedPlatformInfos.dicPlatform.ContainsKey(platform.Type));
    }
}
