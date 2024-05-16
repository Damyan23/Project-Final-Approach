using GXPEngine;
using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Security;

public class LevelManager : GameObject
{
    public List<Level> levels;
    public int currentLevelIndex;
    public List<GameObject> playerAddedObjects;
    private GameSettings settings;
    public SpawnPoint currentLevelSpawnPoint;
    public List<Teleporter> teleporters;

    public LevelManager(GameSettings settings) : base()
    {
        // Initialize levels
        levels = new List<Level>();
        playerAddedObjects = new List<GameObject>();
        teleporters = new List<Teleporter>();
        this.settings = settings;
        InitializeLevels();

        // Set initial current level
        currentLevelIndex = 0;
    }

    public void Start()
    {
        // Load current level
        LoadLevel(currentLevelIndex);
    }
    
    public void Update ()
    {
        //Get the spawn point in the currnet level
        foreach (GameObject obj in levels[currentLevelIndex].objects) 
        {
            if (obj != null && obj is SpawnPoint && obj != currentLevelSpawnPoint)
            {
                currentLevelSpawnPoint = (SpawnPoint)obj;
            }

            if (obj != null && obj is Teleporter && !teleporters.Contains((Teleporter)obj))
            {
                teleporters.Add((Teleporter)obj);
            }
        }
    }

    private void InitializeLevels()
    {
        //Create levels
        Level level1 = new Level(1, this, settings);

        level1.InitializeObjects(new LevelObjectParams[] {
        new LevelObjectParams (LevelObjectType.Exit, new Vector2(1200, 400)),
        new LevelObjectParams (LevelObjectType.LogRight, new Vector2 (500, 485),"",new Vector2(),0,0,90),
        new LevelObjectParams (LevelObjectType.LogMid, new Vector2 (489, 330),"",new Vector2(),0,0,90),
        new LevelObjectParams (LevelObjectType.LogLeft, new Vector2 (489, 200),"",new Vector2(),0,0,90),
        new LevelObjectParams (LevelObjectType.Log, new Vector2 (60, 500),"",new Vector2(),0,0,45),
        new LevelObjectParams (LevelObjectType.Spawnpoint, new Vector2(100, 100)),
    });

        //Set object limits for level 1

        level1.SetObjectLimits(new Dictionary<LevelObjectType, int> {
        {LevelObjectType.Mushroom, 1},
        {LevelObjectType.HalfpipeLeft, 0},
        {LevelObjectType.HalfpipeRight, 0},
        {LevelObjectType.Leaf, 0},
        {LevelObjectType.Log, 0},
   });
        levels.Add(level1);

        Level level2 = new Level(2, this, settings);

        // Initialize objects for level 2
        level2.InitializeObjects(new LevelObjectParams[] {
     new LevelObjectParams (LevelObjectType.Exit, new Vector2(900, 100)),
     new LevelObjectParams (LevelObjectType.Log, new Vector2 (700, 600),"",new Vector2(),0,0,90),
     new LevelObjectParams (LevelObjectType.Log, new Vector2 (1000, 500),"",new Vector2(),0,0,90),
     new LevelObjectParams (LevelObjectType.Log, new Vector2 (700, 100),"",new Vector2(),0,0,90),
     new LevelObjectParams (LevelObjectType.HalfpipeLeft, new Vector2 (150, 400)),
     new LevelObjectParams (LevelObjectType.Spawnpoint, new Vector2(100, 100)),
     new LevelObjectParams (LevelObjectType.Log, new Vector2 (1200, 100),"",new Vector2(),0,0,-45),
     new LevelObjectParams (LevelObjectType.Leaf, new Vector2 (1100, 300),"",new Vector2(),0,0,-45)
 });

        //Set object limits for level 2

        level2.SetObjectLimits(new Dictionary<LevelObjectType, int> {
     {LevelObjectType.Mushroom, 1},
     {LevelObjectType.HalfpipeLeft, 0},
     {LevelObjectType.HalfpipeRight, 0},
     {LevelObjectType.Leaf,0},
     {LevelObjectType.Log, 1},
});

        levels.Add(level2);
        // Create levels
        Level level3 = new Level(3, this, settings);

        // Initialize objects for level 3
        level3.InitializeObjects(new LevelObjectParams[] {
     new LevelObjectParams (LevelObjectType.Exit, new Vector2(1100, 100)),
     new LevelObjectParams (LevelObjectType.Log, new Vector2 (200, 200),"",new Vector2(),0,0,90),
     new LevelObjectParams (LevelObjectType.Log, new Vector2 (50, 200),"",new Vector2(),0,0,90),
     new LevelObjectParams (LevelObjectType.Log, new Vector2 (230, 400),"",new Vector2(),0,0,64),
     new LevelObjectParams (LevelObjectType.Log, new Vector2 (80, 400),"",new Vector2(),0,0,64),
     new LevelObjectParams (LevelObjectType.Spawnpoint, new Vector2 (100, 50),"",new Vector2(),0,0,70),
     new LevelObjectParams (LevelObjectType.Log, new Vector2 (500, 600)),
     new LevelObjectParams (LevelObjectType.Log, new Vector2 (700, 600)),
     //new LevelObjectParams (LevelObjectType.Log, new Vector2 (900, 600)),
     new LevelObjectParams (LevelObjectType.Log, new Vector2 (700, 50)),
     new LevelObjectParams (LevelObjectType.Thorns, new Vector2 (900, 100),"",new Vector2(),0,0,90),
     new LevelObjectParams (LevelObjectType.Log, new Vector2 (800, 200),"",new Vector2(),0,0,90),
     new LevelObjectParams (LevelObjectType.Leaf, new Vector2 (1270, 200),"",new Vector2(),0,0,-90),
     new LevelObjectParams (LevelObjectType.Leaf, new Vector2 (1270, 400),"",new Vector2(),0,0,-90),
     new LevelObjectParams (LevelObjectType.Leaf, new Vector2 (1270, 600),"",new Vector2(),0,0,-90),
 });

        //Set object limits for level 1

        level3.SetObjectLimits(new Dictionary<LevelObjectType, int> {
     {LevelObjectType.Mushroom, 2},
     {LevelObjectType.HalfpipeLeft, 0},
     {LevelObjectType.HalfpipeRight, 0},
     {LevelObjectType.Leaf,0},
     {LevelObjectType.Log, 1},
});
        levels.Add(level3);
    }

    private void LoadLevel(int levelIndex)
    {
        // Instantiate objects for the current level
        Level currentLevel = levels[levelIndex];
        foreach (GameObject obj in currentLevel.objects)
        {
            // Add obj to the game
            game.AddChild(obj);
        }

        // Check and remove extra spawn points
        currentLevel.RemoveExtraSpawnPoints();
    }

    public void AddObject (GameObject obj)
    {
        Level currentLevel = levels[currentLevelIndex];
        if (obj != null && currentLevel.CanAddObject(obj))
        {
            currentLevel.objects.Add(obj);
            game.AddChild(obj);

            playerAddedObjects.Add(obj);

            switch (obj)
            {
                case Fan fan:
                    fan.level = this.currentLevelIndex + 1;
                    break;
                case HalfPipeLeft pipeLeft:
                    pipeLeft.level = this.currentLevelIndex + 1;
                    break;
                case HalfPipeRight pipeRight:
                    pipeRight.level = this.currentLevelIndex + 1;
                    break;
                case Log log:
                    log.Level = this.currentLevelIndex + 1;
                    break;
                case LogLeft logLeft:
                    logLeft.Level = this.currentLevelIndex + 1;
                    break;
                case LogRight logRight:
                    logRight.Level = this.currentLevelIndex + 1;
                    break;
                case LogMid logMid:
                    logMid.Level = this.currentLevelIndex + 1;
                    break;
                case Leaf leaf:
                    leaf.Level = this.currentLevelIndex + 1;
                    break;
                case Mushroom mushroom:
                    mushroom.Level = this.currentLevelIndex + 1;
                    break;
                case Thorns thorns:
                    thorns.Level = this.currentLevelIndex + 1;
                    break;
            }
        }
    }

    public void RemoveObject (GameObject obj)
    {
        Level currentLevel = levels[currentLevelIndex];
        if (obj != null)
        {
            playerAddedObjects.Remove (obj);
            currentLevel.objects.Remove(obj);
            
            foreach (GameObject obj2 in obj.GetChildren())
            {
                if(obj2 is RigidBody rb)
                {
                    World.RemoveBody(rb);
                }

                foreach (GameObject obj3 in obj2.GetChildren())
                {
                    if (obj3 is RigidBody rb2)
                    {
                        World.RemoveBody(rb2);
                    }
                }
            }

            obj.Destroy();
        }
    }

    private void ClearLevel()
    {
        Level currentLevel = levels[currentLevelIndex];

        List<GameObject> objectsToDelete = new List<GameObject>();

        foreach (GameObject obj in currentLevel.objects)
        {
            obj.LateDestroy();
            objectsToDelete.Add(obj);

            if (obj is HalfPipeLeft pipe)
            {
                foreach (Box box in pipe.boxColliders)
                {
                    World.bodyList.Remove (box.GetRigidBody());
                }
            }
        }
        // Remove objectsToDelete from currentLevel.objects
        foreach (GameObject objToDelete in objectsToDelete)
        {
            currentLevel.objects.Remove(objToDelete);
        }

        playerAddedObjects.Clear();
        teleporters.Clear();
    }


    public void SwitchToNextLevel()
    {
        ClearLevel();
        currentLevelIndex++;
        this.settings.phase = 1;
        this.settings.ghostSpawned = false;
        LoadLevel(currentLevelIndex);

    }
}