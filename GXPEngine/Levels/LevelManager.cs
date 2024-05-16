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
        // Create levels
        Level level1 = new Level(1, this, settings);

        // Initialize objects for level 1
        level1.InitializeObjects(new LevelObjectParams[] {
            //new LevelObjectParams (LevelObjectType.HalfpipeRight, new Vector2(200, 300)),
            new LevelObjectParams (LevelObjectType.Exit, new Vector2(500, 500)),
        });

        //Set object limits for level 1

       level1.SetObjectLimits(new Dictionary<LevelObjectType, int> {
            {LevelObjectType.Mushroom, 1},
            {LevelObjectType.HalfpipeLeft, 1},
            {LevelObjectType.HalfpipeRight, 1},
            {LevelObjectType.Leaf, 1},
            {LevelObjectType.Log, 1},
       });

       levels.Add(level1);

        Level level2 = new Level(2, this, settings);
        level2.InitializeObjects(new LevelObjectParams[]
        {
            //new LevelObjectParams(LevelObjectType.Box, "", new Vector2(game.width / 2, 500), new Vector2 (), 500, 60, 0, 1f, 0.8f, 0, true),
            new LevelObjectParams(LevelObjectType.Spawnpoint, new Vector2(200, 200)),
        });

        // Set object limits for level 2
        //level2.SetObjectLimits(new Dictionary<LevelObjectType, int> {
        //    { LevelObjectType.Box, 2 },
        //});

        levels.Add (level2);
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
                case Thorns tohrns:
                    tohrns.Level = this.currentLevelIndex + 1;
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
                if (obj2 is RigidBody)
                {
                    World.RemoveBody((RigidBody)obj2);
                }
            }
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