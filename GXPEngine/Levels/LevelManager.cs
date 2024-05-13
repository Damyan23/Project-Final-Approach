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
        Level level1 = new Level(this);

        // Initialize objects for level 1
        level1.InitializeObjects(new LevelObjectParams[] {
            new LevelObjectParams(LevelObjectType.Box, "", new Vector2(game.width / 2, 600), new Vector2 (), 500, 60, 45, 1f, 0.8f, 0, true, settings),
            new LevelObjectParams(LevelObjectType.Spawnpoint, "spawnPoint.png", new Vector2(200, 200), new Vector2(), 0, 0, 0, 0, 0, 0, false, settings),
            new LevelObjectParams(LevelObjectType.Teleporter, "", new Vector2 (200, 400), new Vector2 (600, 400), 0, 0, 0, 0, 0, 0, false, settings),
            new LevelObjectParams(LevelObjectType.Explosive, "", new Vector2(game.width/2, game.height/4), new Vector2 (), 0, 0, 0, 0, 0, 0, true, settings),
            new LevelObjectParams(LevelObjectType.Spikes, "square.png", new Vector2(600, 350), new Vector2(), 0, 0, 0, 0, 0, 0, true, settings)
        }) ;

        // Set object limits for level 1
        level1.SetObjectLimits(new Dictionary<LevelObjectType, int> {
            { LevelObjectType.Box, 1 },
        });

        levels.Add(level1);
    }

    private void LoadLevel(int levelIndex)
    {
        // Clear the screen
        if (levelIndex != 0)
        {
            ClearLevel();
        }

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

            playerAddedObjects.Add (obj);
        }
    }

    public void RemoveObject (GameObject obj)
    {
        Level currentLevel = levels[currentLevelIndex];
        if (obj != null)
        {
            playerAddedObjects.Remove (obj);
            currentLevel.objects.Remove(obj);
            game.RemoveChild(obj);
            
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

        foreach (GameObject obj in currentLevel.objects)
        {
            obj.LateDestroy();
            currentLevel.objects.Remove (obj);
        }

        playerAddedObjects.Clear();
        teleporters.Clear();
    }

    public void SwitchToNextLevel()
    {
        currentLevelIndex++;
        this.settings.phase = 1;
        this.settings.ghostSpawned = false;
        LoadLevel(currentLevelIndex);
    }
}