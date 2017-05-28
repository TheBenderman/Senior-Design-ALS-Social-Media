using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Connectome.Unity.Plugin;
using UnityEngine.UI;
using Connectome.Emotiv.Enum;
using System.Collections.Generic;
using Assets.UITestComponents;

[TestFixture]
public class BasicVirtualUnityDeviceTest {

    BasicVirtualUnityDevice bvud;
    DummyToggle dt;
    DummyDropdown dd;
    DummyText dummyText;
    DummySlider dummySlider;

    [SetUp]
    public void init()
    {
        bvud = new BasicVirtualUnityDevice();
        dt = new DummyToggle();
        dd = new DummyDropdown();
        dummyText = new DummyText();
        dummySlider = new DummySlider();
        bvud.Setup();
        bvud.TargetPower = 1;
    }

	[Test]
    [Category("Add Forced State Tests")]
    public void AddForcedStateTest() {
        bvud.AddForcedState();
        Assert.AreEqual(10, bvud.ForceCount);
	}

    [Test]
    [Category("Set Force Target Tests")]
    public void SetForceTargetTest()
    {
        bvud.SetForceTarget(dt);
        Assert.AreEqual(true, bvud.IsTargetForced);
    }

    [Test]
    [Category("Set Target Command Tests")]
    public void SetTargetCommandTest()
    {
        bvud.SetTargetCommand(dd);
        bvud.TargetCommand = EmotivCommandType.PUSH;
        Assert.AreEqual(EmotivCommandType.PUSH, bvud.TargetCommand);
    }

    [Test]
    [Category("Update Slider Text Value Tests")]
    public void UpdateSliderTextValueTest()
    {
        dummyText.text = "new";
        bvud.UpdateSliderTextValue(dummyText);
    }

    [Test]
    [Category("Set Target Power Tests")]
    public void SetTargetPowerTest()
    {
        bvud.SetTargetPower(dummySlider);
        Assert.AreEqual(1, bvud.TargetPower);
    }

    [Test]
    [Category("Is Connected Tests")]
    public void isConnectedTest()
    {
        Assert.IsTrue(bvud.IsConnected);
    }

    [Test]
    [Category("Battery Level Tests")]
    public void BatteryLevelTest()
    {
        Assert.AreEqual(100, bvud.BatteryLevel);
    }

    [Test]
    [Category("Wireless Signal Strength Tests")]
    public void WirelessSignalStrengthTest()
    {
        Assert.AreEqual(2, bvud.WirelessSignalStrength);
    }
}
