using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Connectome.Emotiv.Common;
using Connectome.Emotiv.Enum;
using System.Linq;
using Connectome.Emotiv.Interface;

[TestFixture]
public class IntervalEmotivSampleTest {

    IntervalEmotivSampler ies;
    EmotivCommandType tect;
    EmotivState register1;
    EmotivState register2;

    [SetUp]
    public void init()
    {
        tect = EmotivCommandType.PUSH;
        ies = new IntervalEmotivSampler();

        register1 = new EmotivState(tect, 1, 1);
        register2 = new EmotivState(tect, .5F, 2);
    }

	[Test]
    [Category("Register Tests")]
    public void RegisterTest() {

        ies.Clear();
        ies.Register(register1);
        ies.Register(register2);
        ies.Interval = 1;

        IEmotivState sample = ies.GetSample().First();
        Assert.AreEqual(register1.Command, sample.Command);
        Assert.AreEqual(register1.Power, sample.Power);
        Assert.AreEqual(register1.Time, sample.Time);
    }

    [Test]
    [Category("Clear Tests")]
    public void ClearTest()
    {

        ies.Clear();
        ies.Register(register1);
        ies.Register(register2);
        ies.Interval = 1;

        IEmotivState sample = ies.GetSample().First();
        Assert.AreEqual(register1.Command, sample.Command);
        Assert.AreEqual(register1.Power, sample.Power);
        Assert.AreEqual(register1.Time, sample.Time);

        ies.Clear();
        Assert.IsEmpty(ies.GetSample());
    }
}
