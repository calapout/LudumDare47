using Bytes;
public class GameTimerData : Bytes.Data
{
    public float pourcent
    {
        get => pourcent;
        private set => pourcent = value;
    }
    public GameTimerData(float pourcent)
    {
        this.pourcent = pourcent;
    }
}