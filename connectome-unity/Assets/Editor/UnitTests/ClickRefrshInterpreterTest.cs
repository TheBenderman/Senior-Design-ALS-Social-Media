using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using UnityEngine.UI;
using Assets.UITestComponents;

[TestFixture]
[Category("ClickRefrshInterpreter Tests")]
public class ClickRefrshInterpreterTest {

    ClickRefrshInterpreter CRI;
    GameObject slider1;
    GameObject slider2;
    GameObject slider3;

    [SetUp]
    public void init()
    {
        CRI = new ClickRefrshInterpreter();
        slider1 = new GameObject();
        slider2 = new GameObject();
        slider3 = new GameObject();

        CRI.RefreshIndicator = slider1.AddComponent<DummySlider>() as DummySlider;
        CRI.ActivitySlider = slider2.AddComponent<DummySlider>() as DummySlider;
        CRI.IndicatorSlider = slider3.AddComponent<DummySlider>() as DummySlider;

        CRI.ReachRate = .10F;
        CRI.OnRefresh = new UnityEngine.Events.UnityEvent();
        CRI.AutoUpdateIndicators = true;
        CRI.TargetCommand = Connectome.Emotiv.Enum.EmotivCommandType.PUSH;
    }

	[Test]
    [Category("Interpeter Tests")]
    public void InterpreterBelowRefreshRateScale() {

        CRI.ScaleSlider = true;
        CRI.Interpeter(.2F);
        Assert.AreEqual(CRI.ActivitySlider.value, 1);
        Assert.AreEqual(CRI.IndicatorSlider.value, 1);

    }

    [Test]
    [Category("UpdateSlider Tests")]
    public void UpdateSlidersScaleSlider()
    {

        CRI.ScaleSlider = true;
        CRI.UpdateSliders(.2F);
        Assert.AreEqual(CRI.RefreshIndicator.value, 0);
        Assert.AreEqual(CRI.IndicatorSlider.value, 1);

    }

    [Test]
    [Category("UpdateSlider Tests")]
    public void UpdateSlidersDoNotScaleSlider()
    {

        CRI.ScaleSlider = false;
        CRI.UpdateSliders(.2f);
        Assert.AreEqual(CRI.ActivitySlider.value, .2f);
        Assert.AreEqual(CRI.IndicatorSlider.value, .1f);

    }
}
