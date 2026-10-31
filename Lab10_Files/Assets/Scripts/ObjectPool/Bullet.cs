using UnityEngine;

/*
 * A pooled projectile that flies upward and returns itself to its pool when it hits a target or leaves the screen
*/

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float despawnMargin = 0.5f;

    private Rigidbody2D body;
    private BulletPool pool;

    // Caches the Rigidbody2D used to move the bullet
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Stores the pool this bullet should go back to
    public void SetPool(BulletPool owner)
    {
        pool = owner;
    }

    // Places the bullet at the fire point, activates it and sends it upward
    public void Launch(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        body.linearVelocity = Vector2.up * speed;
    }

    // Returns the bullet once it passes the top of the screen
    private void Update()
    {
        if (transform.position.y > ScreenBounds.HalfHeight + despawnMargin)
        {
            ReturnToPool();
        }
    }

    // Tells a target it was hit and then returns the bullet
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameObject.activeSelf || !other.TryGetComponent(out Target target))
        {
            return;
        }

        target.Hit();
        ReturnToPool();
    }

    // Stops the bullet and gives it back to the pool, or destroys it if it was never pooled
    private void ReturnToPool()
    {
        body.linearVelocity = Vector2.zero;

        if (pool != null)
        {
            pool.Return(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}