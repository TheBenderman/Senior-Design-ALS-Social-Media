using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Connectome.Unity.Plugin;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;

[TestFixture]
public class BacisReaderPluginTest {

    BasicReaderPlugin brp;

    [SetUp]
    public void init()
    {
        brp = new BasicReaderPlugin();
        brp.AllowNull = true;
        brp.SetUp(new EPOCEmotivDevice("jschuck", "Js6js6js6", "jschuck_SSVEP"));

    }

    [Test]
    [Category("Setup Tests")]
    public void SetUpTest() {

        Assert.IsNotNull(brp.Content);
        brp.SetUp(new EPOCEmotivDevice("jschuck", "Js6js6js6", "jschuck_SSVEP"));
        Assert.IsInstanceOf<IEmotivReader>(brp.Content);

    }

    [Test]
    [Category("isReading Tests")]
    public void isReadingTest()
    {
        brp.SetUp(new EPOCEmotivDevice("jschuck", "Js6js6js6", "jschuck_SSVEP"));
        Assert.IsFalse(brp.IsReading);
    }

    [Test]
    [Category("Plug Device Tests")]
    public void PlugDeviceTest()
    {
        brp.PlugDevice(new BasicVirtualUnityDevice());
        Assert.IsInstanceOf<BasicVirtualUnityDevice>(brp.Content.Device);
    }
}
