using GXPEngine;
using System;
using System.Collections.Generic;

public class Level
{
    public List<GameObject> objects;
    private Dictionary<LevelObjectType, int> objectLimits; // Dictionary to store object limits
    public int spawnPointCount; // Track the number of spawn points in the level
    private LevelManager levelManager; // Reference to the LevelManager instance

    public Level(LevelManager levelManager)
    {
        objects = new List<GameObject>();
        spawnPointCount = 0;
        this.levelManager = levelManager;
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
                return new Teleporper(param.position, param.position2);
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