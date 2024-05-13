
public class GameSettings
{
    public bool startGame = false;
    public bool isGameOver = false;
    public bool stuffDrawn = false;
    public bool levelSetup = false;
    public bool ghostSpawned = false;

    public int phase = 1;
    public GameSettings()
    {

    }

    void Update()
    {
        if(phase == 1 && ghostSpawned)
        {
            ghostSpawned = false;
        }
    }
}
