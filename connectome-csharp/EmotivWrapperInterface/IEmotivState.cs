namespace EmotivWrapperInterface
{
    public interface IEmotivState
    {
       EmotivStateType command { set; get; }
       float power { set; get; }
       long time { set; get; }
    }
}
