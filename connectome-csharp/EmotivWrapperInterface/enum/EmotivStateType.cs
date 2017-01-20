namespace Connectome.Emotiv.Enum
{
    public enum EmotivCommandType
    {
        NULL = 0x0000,
        NEUTRAL = 0x0001,
        PUSH = 0x0002,
        PULL = 0x0004,
        LIFT = 0x0008,
        DROP = 0x0010,
        LEFT = 0x0020,
        RIGHT = 0x0040,
        ROTATE_LEFT = 0x0080,
        ROTATE_RIGHT = 0x0100,
        ROTATE_CLOCKWISE = 0x0200,
        ROTATE_COUNTER_CLOCKWISE = 0x0400,
        ROTATE_FORWARDS = 0x0800,
        ROTATE_REVERSE = 0x1000,
        DISAPPEAR = 0x2000
    }; 
}
