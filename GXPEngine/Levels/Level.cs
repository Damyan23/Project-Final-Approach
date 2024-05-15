using GXPEngine;
using System;
using System.Collections.Generic;

public class Level
{
    public int index;
    public List<GameObject> objects;
    public List<Teleporter> teleporters;
    public Dictionary<LevelObjectType, int> objectLimits; // Dictionary to store object limits
    public int spawnPointCount; // Track the number of spawn points in the level
    private LevelManager levelManager; // Reference to the LevelManager instance
    private GameSettings settings;

    public Level(int index, LevelManager levelManager, GameSettings settings)
    {
        this.index = index;
        objects = new List<GameObject>();
        teleporters = new List<Teleporter>();
        spawnPointCount = 0;
        this.levelManager = levelManager;
        this.settings = settings;
        objectLimits = new Dictionary<LevelObjectType, int>();
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
        if (obj is LogLeft)
        {
            type = LevelObjectType.LogLeft;
        }
        else if (obj is LogMid)
        {
            type = LevelObjectType.LogMid;
        }
        else if (obj is LogRight)
        {
            type = LevelObjectType.LogRight;
        }
        else if (obj is Leaf)
        {
            type = LevelObjectType.Leaf;
        }
        else if (obj is Log)
        {
            type = LevelObjectType.Log;
        }
        else if (obj is Explosive)
        {
            type = LevelObjectType.Explosive;
        }
        else if (obj is Fan)
        {
            type = LevelObjectType.Fan; 
        }
        else if (obj is Mushroom)
        {
            type = LevelObjectType.Mushroom;
        }
        else if (obj is Teleporter)
        {
            type = LevelObjectType.Teleporter;
        }
        else if (obj is Thorns)
        {
            type = LevelObjectType.Thorns;
        }
        else if(obj is HalfPipeLeft)
        {
            type = LevelObjectType.HalfpipeLeft;
        }
        else if(obj is HalfPipeRight)
        {
            type = LevelObjectType.HalfpipeRight;
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
                return new SpawnPoint(param.position);
            case LevelObjectType.Teleporter:
                return new Teleporter(param.position, param.position2, settings);
            case LevelObjectType.Explosive:
                return new Explosive(param.position, param.mode);
            case LevelObjectType.Thorns:
                Thorns thorns = new Thorns(param.position, param.rotation, settings);
                thorns.Level = index;
                return thorns;
            case LevelObjectType.Exit:
                return new Exit(param.position, levelManager);
            case LevelObjectType.HalfpipeRight:
                HalfPipeRight pipeRight = new HalfPipeRight(param.position, settings);
                pipeRight.level = index;
                return pipeRight;
            case LevelObjectType.HalfpipeLeft:
                HalfPipeRight pipeLeft = new HalfPipeRight(param.position, settings);
                pipeLeft.level = index;
                return pipeLeft;
            case LevelObjectType.Fan:
                return new Fan(param.position, param.width, param.height, param.density, param.bounciness, param.fanDirection, param.mode);
            case LevelObjectType.Log:
                Log log = new Log (param.position, param.rotation);
                log.Level = index;
                return log;
            case LevelObjectType.LogLeft:
                LogLeft logLeft = new LogLeft(param.position, param.rotation);
                logLeft.Level = index;
                return logLeft;
            case LevelObjectType.LogRight:
                LogRight logRight = new LogRight(param.position, param.rotation);
                logRight.Level = index;
                return logRight;
            case LevelObjectType.LogMid:
                LogMid logMid = new LogMid(param.position, param.rotation);
                logMid.Level = index;
                return logMid;
            case LevelObjectType.Leaf:
                Leaf leaf = new Leaf (param.position, param.rotation);
                leaf.Level = index;
                return leaf;
            case LevelObjectType.Mushroom:
                Mushroom mushroom = new Mushroom(param.position, param.rotation);
                mushroom.Level = index;
                return mushroom;
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