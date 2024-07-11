using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

public class PlatformInfosTests
{
    private readonly string _filePath;

    public PlatformInfosTests()
    {
        _filePath = Singleton<PlatformInfos>.GetPath();
    }

    [Fact]
    public void TestInitAndSave()
    {
        var platformInfos = PlatformInfos.Instance;

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
        var platformInfos = PlatformInfos.Instance;
        Assert.True(File.Exists(_filePath));

        var loadedPlatformInfos = Singleton<PlatformInfos>.Load();
        loadedPlatformInfos?.Init();

        Assert.NotNull(loadedPlatformInfos);
        Assert.NotNull(loadedPlatformInfos.List);
        Assert.True(loadedPlatformInfos.List.Count > 0);

        Assert.True(loadedPlatformInfos.dicPlatform.Count > 0);
        foreach (var platform in loadedPlatformInfos.List)
            Assert.True(loadedPlatformInfos.dicPlatform.ContainsKey(platform.Type));
    }
}
