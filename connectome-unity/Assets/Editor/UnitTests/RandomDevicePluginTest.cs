using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Connectome.Unity.Plugin;
using Connectome.Emotiv.Implementation;

[TestFixture]
public class RandomDevicePluginTest {

    RandomDevicePlugin rdp;

    [SetUp]
    public void init()
    {
        rdp = new RandomDevicePlugin();
        rdp.Setup();
    }

	[Test]
    [Category("Setup Tests")]
    public void SetupTest() {
        Assert.IsInstanceOf<RandomEmotivDevice>(rdp.Content);
	}

    [Test]
    [Category("Is Connected Tests")]
    public void IsConnectedTest()
    {
        Assert.IsFalse(rdp.IsConnected);
    }

    [Test]
    [Category("Battery Level Tests")]
    public void BatteryLevelTest()
    {
        Assert.AreEqual(0, rdp.BatteryLevel);
    }

    [Test]
    [Category("Wireless Signal Strength Tests")]
    public void WirelessSignalStrengthTest()
    {
        Assert.AreEqual(0, rdp.WirelessSignalStrength);
    }

    [Test]
    [Category("Connect Disconnect Tests")]
    public void ConnectDisconnectTest()
    {
        rdp.Connect();
        Assert.IsTrue(rdp.IsConnected);
        rdp.Disconnect();
        Assert.IsFalse(rdp.IsConnected);
    }

}
