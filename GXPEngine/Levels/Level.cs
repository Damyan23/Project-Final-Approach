using GXPEngine;
using System;
using System.Collections.Generic;

public class Level
{
    public List<GameObject> objects;
    public List<Teleporter> teleporters;
    private Dictionary<LevelObjectType, int> objectLimits; // Dictionary to store object limits
    public int spawnPointCount; // Track the number of spawn points in the level
    private LevelManager levelManager; // Reference to the LevelManager instance
    private GameSettings settings;

    public Level(LevelManager levelManager, GameSettings settings)
    {
        objects = new List<GameObject>();
        teleporters = new List<Teleporter>();
        spawnPointCount = 0;
        this.levelManager = levelManager;
        this.settings = settings;
    }

    public void InitializeObjects(LevelObjectParams[] objectParams)
    {
        foreach (LevelObjectParams param in objectParams)
        {
            GameObject obj = CreateObject(param);
            if (obj != null)
            {
                objects.Add(obj);

                // Update spawn point count
                if (param.type == LevelObjectType.Spawnpoint)
                {
                    spawnPointCount++;
                }

                if(param.type == LevelObjectType.Teleporter)
                {
                    teleporters.Add((Teleporter)obj);
                }
            }
        }
    }

    public void SetObjectLimits(Dictionary<LevelObjectType, int> limits)
    {
        objectLimits = limits;
    }

    public bool CanAddObject(GameObject obj)
    {
        LevelObjectType type;
        if (obj is Box)
        {
            type = LevelObjectType.Box;
        }
        else
        {
            // Unknown object type, disallow adding
            return false;
        }

        // Check if the object limit is reached
        if (objectLimits.ContainsKey(type))
        {
            int limit = objectLimits[type];
            int count = levelManager.playerAddedObjects.FindAll(o => o.GetType() == obj.GetType()).Count;
            return count < limit;
        }

        // No limit specified, allow adding
        return true;
    }

    public GameObject CreateObject(LevelObjectParams param)
    {
        switch (param.type)
        {
            case LevelObjectType.Spawnpoint:
                return new SpawnPoint(param.imageName, param.position);
            case LevelObjectType.Box:
                return new Box(param.width, param.height, param.position, param.density, param.bounciness, param.mode, param.isStatic, param.rotation);
            case LevelObjectType.Teleporter:
                return new Teleporter(param.position, param.position2, settings);
            case LevelObjectType.Explosive:
                return new Explosive(param.position, param.mode);
            case LevelObjectType.Spikes:
                return new Spikes(param.position, param.imageName, settings);
            case LevelObjectType.Exit:
                return new Exit(param.position, levelManager);
            case LevelObjectType.Fan:
                return new Fan(param.position, param.width, param.height, param.density, param.bounciness, param.fanDirection, param.mode);
            default:
                return null;
        }
    }

    public void RemoveExtraSpawnPoints()
    {
        if (spawnPointCount > 1)
        {
            int removedCount = 0;
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i] is SpawnPoint)
                {
                    objects[i].LateDestroy();
                    objects.RemoveAt(i);
                    removedCount++;
                    if (removedCount >= spawnPointCount - 1) // Keep one spawn point
                    {
                        break;
                    }
                }
            }
            spawnPointCount = 1; // Update spawn point count
        }
    }
}