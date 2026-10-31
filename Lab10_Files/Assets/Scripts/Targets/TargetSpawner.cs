using UnityEngine;

/*
 * Spawns a random premade target type on a random lane at a set interval, using the director and builder to put each one together
*/

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private Target targetPrefab;
    [SerializeField] private TargetConfig[] targetTypes;
    [SerializeField] private float[] laneHeights = { 3.5f, 2f, 0.5f };
    [SerializeField, Min(0.1f)] private float spawnInterval = 1.2f;
    [SerializeField] private float spawnMargin = 1f;

    private TargetDirector director;
    private float spawnTimer;

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

    // Picks a lane and target type, then asks the director to build that target just off the edge of the screen
    private void SpawnTarget()
    {
        int lane = Random.Range(0, laneHeights.Length);
        float direction = lane % 2 == 0 ? 1f : -1f;
        Vector3 spawnPosition = new Vector3(-direction * (ScreenBounds.HalfWidth + spawnMargin), laneHeights[lane], 0f);
        TargetConfig config = targetTypes[Random.Range(0, targetTypes.Length)];
        director.Construct(config, spawnPosition, direction);
    }
}