using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;
using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Common;
using Connectome.Emotiv.Enum;
using System.Linq;

[TestFixture]
public class CommandTypeCalculatorTest {

    CommandTypeCalculator ctc;

    [SetUp]
    public void init()
    {
        ctc = new CommandTypeCalculator();
        ctc.DefaultConfig = new EmotivCalculatorConfiguration();
        ctc.DefaultConfig.TargetCommand = EmotivCommandType.PUSH;
        ctc.DefaultConfig.MinSampleSize = 2;
    }

    [Test]
    [Category("Calculate Tests")]
    public void CalculateReductionTest()
    {
        IEmotivState[] sample = new EmotivState[3];

        sample[0] =new EmotivState(EmotivCommandType.PUSH, 1, 1);
        sample[1] = new EmotivState(EmotivCommandType.PUSH, .5f, 2);
        sample[2] = new EmotivState(EmotivCommandType.PUSH, .75f, 3);

        ctc.Reduction = true;
        sample.Count<IEmotivState>();
        float rate = ctc.Calculate(sample);

        Assert.AreEqual(1, rate);


    }

    [Test]
    [Category("Calculate Tests")]
    public void CalculateTest()
    {
        List<IEmotivState> sample = new List<IEmotivState>();

        sample.Add(new EmotivState(EmotivCommandType.PUSH, 1, 1));
        sample.Add(new EmotivState(EmotivCommandType.PUSH, .5F, 2));
        sample.Add(new EmotivState(EmotivCommandType.PUSH, .75F, 3));

        ctc.Reduction = false;
        float rate = ctc.Calculate(sample);

        Assert.AreEqual(1, rate);


    }

}
