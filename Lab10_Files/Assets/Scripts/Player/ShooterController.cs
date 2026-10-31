using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Moves the shooter left and right along the bottom of the screen and fires bullets taken from the bullet pool
*/

public class ShooterController : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference fireAction;
    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float fireCooldown = 0.2f;
    [SerializeField] private float edgePadding = 0.5f;

    private float nextFireTime;

    /// Enables the move and fire actions
    private void OnEnable()
    {
        if (moveAction != null)
        {
            moveAction.action.Enable();
        }

        if (fireAction != null)
        {
            fireAction.action.Enable();
        }
    }

    // Disables the move and fire actions
    private void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.action.Disable();
        }

        if (fireAction != null)
        {
            fireAction.action.Disable();
        }
    }

    // Handles movement every frame and fires while the fire button is held and the cooldown is over
    private void Update()
    {
        Move();

        if (fireAction != null && fireAction.action.IsPressed() && Time.time >= nextFireTime)
        {
            Fire();
        }
    }

    // Reads horizontal input and keeps the shooter inside the camera's view
    private void Move()
    {
        if (moveAction == null)
        {
            return;
        }

        float input = moveAction.action.ReadValue<Vector2>().x;
        float limit = ScreenBounds.HalfWidth - edgePadding;
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x + input * moveSpeed * Time.deltaTime, -limit, limit);
        transform.position = position;
    }

    // Gets a bullet from the pool and launches it from the fire point
    private void Fire()
    {
        if (bulletPool == null)
        {
            return;
        }

        Bullet bullet = bulletPool.Get();
        if (bullet == null)
        {
            return;
        }

        bullet.Launch(firePoint != null ? firePoint.position : transform.position);
        nextFireTime = Time.time + fireCooldown;
    }
}