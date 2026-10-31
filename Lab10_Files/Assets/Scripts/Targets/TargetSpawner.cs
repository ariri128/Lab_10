using UnityEngine;
using System;
using System.Collections.Generic;

/*
 * Spawns a random premade target type on a random lane at a set interval, using the director and builder to put each one together
 * It is also saveable: it saves every duck on screen and rebuilds them when a save is loaded
*/

public class TargetSpawner : SaveableBehaviour
{
    [SerializeField] private Target targetPrefab;
    [SerializeField] private TargetConfig[] targetTypes;
    [SerializeField] private float[] laneHeights = { 2f, 0.25f, -1.5f };
    [SerializeField, Min(0.1f)] private float spawnInterval = 1.2f;
    [SerializeField] private float spawnMargin = 1f;

    private readonly List<SpawnedTarget> spawnedTargets = new List<SpawnedTarget>();
    private TargetDirector director;
    private float spawnTimer;

    // Saves every duck currently on screen: its type, its lane and its transform
    public override string SavedData
    {
        get
        {
            spawnedTargets.RemoveAll(spawned => spawned.Target == null);

            SpawnerSaveData saveData = new SpawnerSaveData();
            foreach (SpawnedTarget spawned in spawnedTargets)
            {
                saveData.targets.Add(new TargetSaveData
                {
                    typeIndex = spawned.TypeIndex,
                    laneIndex = spawned.LaneIndex,
                    transform = TransformSaver.Capture(spawned.Target.transform)
                });
            }

            return JsonUtility.ToJson(saveData);
        }
    }

    // Checks the inspector setup and creates the director with a target builder
    private void Awake()
    {
        if (targetPrefab == null || targetTypes == null || targetTypes.Length == 0 || laneHeights == null || laneHeights.Length == 0)
        {
            Debug.LogError($"{nameof(TargetSpawner)} on {name} needs a target prefab, at least one target type and at least one lane.", this);
            enabled = false;
            return;
        }

        director = new TargetDirector(new TargetBuilder(targetPrefab, transform));
    }

    // Counts down and spawns a target each time the interval passes
    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer > 0f)
        {
            return;
        }

        SpawnTarget();
        spawnTimer = spawnInterval;
    }

    // Removes the ducks on screen and rebuilds the saved ones with the builder, then puts each back where it was
    public override void LoadFromData(string data)
    {
        if (director == null || string.IsNullOrEmpty(data))
        {
            return;
        }

        SpawnerSaveData saveData = JsonUtility.FromJson<SpawnerSaveData>(data);
        if (saveData == null || saveData.targets == null)
        {
            return;
        }

        ClearTargets();

        foreach (TargetSaveData saved in saveData.targets)
        {
            if (saved == null || saved.transform == null || saved.typeIndex < 0 || saved.typeIndex >= targetTypes.Length || saved.laneIndex < 0 || saved.laneIndex >= laneHeights.Length)
            {
                continue;
            }

            Target target = BuildTarget(saved.typeIndex, saved.laneIndex, saved.transform.localPosition.x);
            if (target != null)
            {
                TransformSaver.Apply(target.transform, saved.transform);
            }
        }
    }

    // Picks a random lane and target type and builds that target just off the edge of the screen
    private void SpawnTarget()
    {
        int lane = UnityEngine.Random.Range(0, laneHeights.Length);
        float startX = -LaneDirection(lane) * (ScreenBounds.HalfWidth + spawnMargin);
        BuildTarget(UnityEngine.Random.Range(0, targetTypes.Length), lane, startX);
    }

    // Asks the director to build one target and keeps track of it so it can be saved later
    private Target BuildTarget(int typeIndex, int lane, float x)
    {
        spawnedTargets.RemoveAll(spawned => spawned.Target == null);

        Vector3 position = new Vector3(x, laneHeights[lane], 0f);
        Target target = director.Construct(targetTypes[typeIndex], position, LaneDirection(lane));
        if (target != null)
        {
            spawnedTargets.Add(new SpawnedTarget(target, typeIndex, lane));
        }

        return target;
    }

    // Destroys every duck currently on screen
    private void ClearTargets()
    {
        foreach (SpawnedTarget spawned in spawnedTargets)
        {
            if (spawned.Target != null)
            {
                Destroy(spawned.Target.gameObject);
            }
        }

        spawnedTargets.Clear();
    }

    // Even lanes move right and odd lanes move left
    private static float LaneDirection(int lane)
    {
        return lane % 2 == 0 ? 1f : -1f;
    }

    // A duck on screen together with the type and lane it was built with
    private class SpawnedTarget
    {
        public Target Target { get; }
        public int TypeIndex { get; }
        public int LaneIndex { get; }

        public SpawnedTarget(Target target, int typeIndex, int laneIndex)
        {
            Target = target;
            TypeIndex = typeIndex;
            LaneIndex = laneIndex;
        }
    }

    // The saved data for one duck
    [Serializable]
    private class TargetSaveData
    {
        public int typeIndex;
        public int laneIndex;
        public TransformData transform;
    }

    // The saved data for every duck
	// JsonUtility needs a class around the list
    [Serializable]
    private class SpawnerSaveData
    {
        public List<TargetSaveData> targets = new List<TargetSaveData>();
    }
}