using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Connectome.Emotiv.Enum;
using Assets.UITestComponents;
using System.Collections.Generic;
using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Common;
using Connectome.Unity.Plugin;

[TestFixture]
public class CRCommandTypeInterpreterTest {

    CRCommandTypeInterpreter ccrt;

    [SetUp]
    public void init()
    {
        var slider1 = new GameObject();
        var deviceGameObject = new GameObject();
        var readerGameObject = new GameObject();
        var device = deviceGameObject.AddComponent<BasicVirtualUnityDevice>() as BasicVirtualUnityDevice;
        var reader = readerGameObject.AddComponent<BasicReaderPlugin>() as BasicReaderPlugin;

        device.TargetPower = 1;
        device.AddForcedState();
        device.Setup();
        reader.SetUp(device);
        device.AddForcedState();
        device.Read(1);
        device.Read(2);
        device.Read(3);

        ccrt = new CRCommandTypeInterpreter();
        ccrt.TargetCommand = EmotivCommandType.PUSH;
        ccrt.RefreshThreshhold = .4f;
        ccrt.ClickThreshhold = .6f;
        ccrt.Interval = 8;
        ccrt.OnClick = new UnityEngine.Events.UnityEvent();
        ccrt.OnRefresh = new UnityEngine.Events.UnityEvent();
        ccrt.Slider = slider1.AddComponent<DummySlider>() as DummySlider;
        ccrt.Setup(device,reader);


    }

	[Test]
    [Category("Interpret Tests")]
    public void InterpretTest() {

        ccrt.Interpret();

        Assert.AreEqual(0, ccrt.Slider.value);

    }
}
