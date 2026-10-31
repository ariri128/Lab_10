using UnityEngine;
using System;

/* 
 * A target that slides across its lane
 * It is the product of the builder and the subject of the observer pattern: it raises OnTargetHit when shot
*/

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Target : MonoBehaviour
{
    public static event Action<int> OnTargetHit;

    [SerializeField] private float despawnMargin = 1f;
    [SerializeField] private float bobHeight = 0.08f;
    [SerializeField] private float bobSpeed = 6f;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private float speed;
    private int pointValue;
    private float direction = 1f;
    private float baseY;
    private float bobPhase;
    private bool isHit;

    // Caches the renderer and collider the builder configures
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Applies the values chosen by the builder, fits the collider to the new sprite and picks a random starting point for the bob
    public void Initialize(Sprite sprite, Color color, float speed, int pointValue, float direction)
    {
        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
            boxCollider.size = sprite.bounds.size;
            boxCollider.offset = sprite.bounds.center;
        }

        spriteRenderer.color = color;
        spriteRenderer.flipX = direction < 0f;
        this.speed = speed;
        this.pointValue = pointValue;
        this.direction = direction;
        baseY = transform.position.y;
        bobPhase = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
    }

    // Moves the target along its shelf while bobbing up and down, and removes it after it passes the far edge of the screen
    private void Update()
    {
        Vector3 position = transform.position;
        position.x += direction * speed * Time.deltaTime;
        position.y = baseY + Mathf.Sin(Time.time * bobSpeed + bobPhase) * bobHeight;
        transform.position = position;

        if (position.x * direction > ScreenBounds.HalfWidth + despawnMargin)
        {
            Destroy(gameObject);
        }
    }

    // Notifies every observer of the points earned and removes the target. Ignores repeat hits in the same frame
    public void Hit()
    {
        if (isHit)
        {
            return;
        }

        isHit = true;
        OnTargetHit?.Invoke(pointValue);
        Destroy(gameObject);
    }
}