using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using UnityEngine.UI;
using Connectome.Emotiv.Enum;
using Assets.UITestComponents;

[TestFixture]
public class CommandRateEmotivInterpreterTest {

    CommandRateEmotivInterpreter cre;
    GameObject slider1;
    GameObject slider2;
    GameObject slider3;

    [SetUp]
    public void init()
    {
        slider1 = new GameObject();
        slider2 = new GameObject();
        slider3 = new GameObject();
        cre = new CommandRateEmotivInterpreter();

        cre.ReachRate = .7f;
        cre.TargetCommand = EmotivCommandType.PUSH;
        cre.OnReached = new UnityEngine.Events.UnityEvent();
        cre.AutoUpdateIndicators = true;
       
        cre.ActivitySlider = slider1.AddComponent<DummySlider>() as DummySlider;
        cre.IndicatorSlider = slider2.AddComponent<DummySlider>() as DummySlider;
    }

	[Test]
    [Category("Update Slider ScaleSlider False Tests")]
    public void UpdateSliderScaleSliderFalseTest() {

        cre.ScaleSlider = false;
        cre.Interpeter(.6f);
        Assert.AreEqual(.6f,cre.ActivitySlider.value);
        Assert.AreEqual(.7f,  cre.IndicatorSlider.value);

	}

    [Test]
    [Category("Update Slider ScaleSlider True Tests")]
    public void UpdateSliderScaleSliderTrueTest()
    {

        cre.ScaleSlider = true;
        cre.Interpeter(.6f);
        Assert.AreEqual(.6f/.7f, cre.ActivitySlider.value);
        Assert.AreEqual(1, cre.IndicatorSlider.value);

    }
}
