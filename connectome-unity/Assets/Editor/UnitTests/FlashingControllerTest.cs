using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Assets.UITestComponents;


[TestFixture]
public class FlashingControllerTest {

    FlashingController fc;

    [SetUp]
    public void init()
    {
        fc = new FlashingController();
        fc.high = new Connectome.Unity.UI.FlashingHighlighter();
        fc.text = new DummyText();
    }

	[Test]
    [Category("Update Value Tests")]
    public void UpdateValueTest() {
        fc.UpdateValue(new DummySlider());
        Assert.AreEqual(1, fc.high.Frequency);
        Assert.AreEqual("1", fc.text.text);
	}
}
