using System.IO;
using Xunit;

public class DeviceInfosTests
{
    private readonly string _filePath;

    public DeviceInfosTests()
    {
        _filePath = Singleton<DeviceInfos>.GetPath();
    }

    [Fact]
    public void TestInitAndSave()
    {
        var deviceInfos = DeviceInfos.Instance;

        if (File.Exists(_filePath))
            File.Delete(_filePath);
        
        deviceInfos.Init();

        Assert.NotNull(deviceInfos.Devices);
        Assert.True(deviceInfos.Devices.Count > 0);

        if (File.Exists(_filePath))
            File.Delete(_filePath);

        deviceInfos.Devices.Clear();
    }

    [Fact]
    public void TestLoadReturnsInstanceWithDeviceInfo()
    {
        var deviceInfos = DeviceInfos.Instance;
        deviceInfos.Init();

        Assert.True(File.Exists(_filePath));

        var loadedDeviceInfos = Singleton<DeviceInfos>.Load();

        Assert.NotNull(loadedDeviceInfos);
        Assert.NotNull(loadedDeviceInfos.Devices);
        Assert.True(loadedDeviceInfos.Devices.Count > 0);
    }
}
