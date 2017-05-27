using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Connectome.Unity.Plugin;
using Connectome.Emotiv.Implementation;
/***
 *  Need to create Dummy Device to correctly complete test.
 ***/
[TestFixture]
[Category("Device Tests")]
public class EPOCDevicePluginTest {

    EPOCDevicePlugin device;

    [SetUp]
    public void Init()
    {
        device = new EPOCDevicePlugin();
    }

    [Test]
    [Category("Device Creation")]
    public void DeviceCreationTest() {

        device.Username = "jschuck";
        device.Password = "Js6js6js6";
        device.Profile = "jschuck_SSVEP";

        device.Setup();
        Assert.IsInstanceOf<EPOCEmotivDevice>(device.Content);
        Assert.IsNotNull(device.Content);
	}

    [Test]
    [Category("Device Connection")]
    public void IsConnectedTest()
    {
        device.Setup();
        Assert.IsFalse(device.IsConnected);
    }

    [Test]
    [Category("Batterty Level")]
    public void BatteryLevelTest()
    {
        device.Setup();
        Assert.AreEqual(0,device.BatteryLevel);
    }

    [Test]
    [Category("Wireless Signal Strength")]
    public void WirelessSignalStrengthTest()
    {
        device.Setup();
        Assert.AreEqual(0,device.WirelessSignalStrength);
    }
    /*
      [Test]
       [Category("Connect Disconnect")]
       //Need to find method to properly test. Placeholder code
       public void ConnectDisconnectTest()
       {
           device.Setup();
           device.Connect();
           device.Disconnect();
       }

          [Test]
          [Category("Read Emotiv State")]
          public void ReadTest()
          {
              device.Setup();
              Assert.AreEqual(0, device.Read(1));
          }
      */
}
