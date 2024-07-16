using CLib.Infos;
using Xunit.Abstractions;

public class DeviceInfosTests : ComplexityTestBase
{
    private readonly string _filePath;
    protected override string GetCodeFilePath()
        => Path.Combine("ExternalRef", $"{nameof(Devices)}.cs");

    public DeviceInfosTests(ITestOutputHelper output) : base(output)
    {
        _filePath = Singleton<Devices>.GetPath();
    }

    [Fact]
    public void TestInitAndSave()
    {
        var deviceInfos = Devices.Instance;

        if (File.Exists(_filePath))
            File.Delete(_filePath);        
        deviceInfos.Init();

        Assert.NotNull(deviceInfos.List);
        Assert.True(deviceInfos.List.Count > 0);

        if (File.Exists(_filePath))
            File.Delete(_filePath);
        deviceInfos.List.Clear();
    }

    [Fact]
    public void TestLoadReturnsInstanceWithDeviceInfo()
    {
        var deviceInfos = Devices.Instance;
        deviceInfos.Init();

        Assert.True(File.Exists(_filePath));

        var loadedDeviceInfos = Singleton<Devices>.Load();
        Assert.NotNull(loadedDeviceInfos);
        Assert.NotNull(loadedDeviceInfos.List);
        Assert.True(loadedDeviceInfos.List.Count > 0);
    }
}
