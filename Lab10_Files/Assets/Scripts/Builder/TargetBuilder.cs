using UnityEngine;

/*
 * Concrete builder: collects target settings one step at a time and creates the finished target from a single prefab when Build is called
*/

public class TargetBuilder : ITargetBuilder
{
    private readonly Target prefab;
    private readonly Transform parent;

    private Sprite sprite;
    private Color color;
    private float size;
    private float speed;
    private int pointValue;
    private Vector3 position;
    private float direction;

    // Sets up the builder with the base target prefab and the transform new targets are parented to
    public TargetBuilder(Target prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
        Reset();
    }

    // Clears every setting back to its default so the next target starts fresh
    public ITargetBuilder Reset()
    {
        sprite = null;
        color = Color.white;
        size = 1f;
        speed = 1f;
        pointValue = 0;
        position = Vector3.zero;
        direction = 1f;
        return this;
    }

    // Sets the target's sprite
    public ITargetBuilder SetSprite(Sprite sprite)
    {
        this.sprite = sprite;
        return this;
    }

    // Sets the target's tint color
    public ITargetBuilder SetColor(Color color)
    {
        this.color = color;
        return this;
    }

    // Sets the target's uniform scale
    public ITargetBuilder SetSize(float size)
    {
        this.size = size;
        return this;
    }

    // Sets how fast the target moves across its lane
    public ITargetBuilder SetSpeed(float speed)
    {
        this.speed = speed;
        return this;
    }

    // Sets how many points the target is worth
    public ITargetBuilder SetPointValue(int pointValue)
    {
        this.pointValue = pointValue;
        return this;
    }

    // Sets where the target spawns
    public ITargetBuilder SetPosition(Vector3 position)
    {
        this.position = position;
        return this;
    }

    // Sets which way the target moves: 1 for right, -1 for left
    public ITargetBuilder SetDirection(float direction)
    {
        this.direction = direction;
        return this;
    }

    // Instantiates the prefab, raises it so its bottom sits on the shelf at any size, and applies every collected setting to produce the finished target
    public Target Build()
    {
        if (prefab == null)
        {
            Debug.LogError($"{nameof(TargetBuilder)} has no target prefab to build from.");
            return null;
        }

        Vector3 spawnPosition = position;
        if (sprite != null)
        {
            spawnPosition.y -= sprite.bounds.min.y * size;
        }

        Target target = Object.Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
        target.transform.localScale = Vector3.one * size;
        target.Initialize(sprite, color, speed, pointValue, direction);
        return target;
    }
}