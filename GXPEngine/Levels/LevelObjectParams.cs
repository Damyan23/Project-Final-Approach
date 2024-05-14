// Contains all the parameters an object can have
using GXPEngine;
using GXPEngine.Core;

public class LevelObjectParams
{
    public LevelObjectType type;
    public Vector2 position;
    public Vector2 position2;
    public FanDirection fanDirection;
    public float width;
    public float height;
    public float rotation;
    public float density;
    public float bounciness;
    public int mode;
    public bool isStatic;

    public string imageName;

    public LevelObjectParams(LevelObjectType type, string imageName, Vector2 position, Vector2 position2,float width, float height, float rotation, float density, float bounciness, int mode, bool isStatic, FanDirection fanDirection = FanDirection.None)
    {
        this.type = type;
        this.imageName = imageName;
        this.position = position;
        this.position2 = position2;
        this.width = width;
        this.height = height;
        this.rotation = rotation;
        this.density = density;
        this.bounciness = bounciness;
        this.mode = mode;
        this.isStatic = isStatic;
        this.fanDirection = fanDirection;
        
    }
}

public enum LevelObjectType
{
    Box,
    Spawnpoint,
    Teleporter,
    Explosive,
    Spikes,
    Fan,
    Exit,
    Halfpipe
}
