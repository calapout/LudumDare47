using Bytes;
public class GameTimerData : Bytes.Data
{
    private float _pourcent;
    private float _timeLeft;
    public float pourcent
    {
        get => _pourcent;
        private set => _pourcent = value;
    }
    public float TimeLeft { get => _timeLeft; private set => _timeLeft = value; }
    public GameTimerData(float TimerPourcent, float timeLeft)
    {
        this.pourcent = TimerPourcent;
        this._timeLeft = timeLeft;
    }
}