namespace EmotivWrapperInterface
{
    public interface IEmotivDevice
    {
       bool Connect(out string errorMsg);
       void Disconnect();

       bool ConnectionSetUp(out string errorMsg);
       bool DisconnectionSetUp();

       IEmotivState Read();
    }
}
