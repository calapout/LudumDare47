using Bytes;
public class GameTimerData : Bytes.Data
{
    private float _pourcent;
    public float pourcent
    {
        get => _pourcent;
        private set => _pourcent = value;
    }
    public GameTimerData(float TimerPourcent)
    {
        this.pourcent = TimerPourcent;
    }
}