using UnityEngine;
using System.Collections.Generic;

/*
 * Object Pool: creates a set of bullets up front and hands them out and takes them back instead of instantiating and destroying them
*/

public class BulletPool : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int initialSize = 20;

    private readonly Queue<Bullet> availableBullets = new Queue<Bullet>();

    // Fills the pool with inactive bullets before the game starts
    private void Awake()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError($"{nameof(BulletPool)} on {name} has no bullet prefab assigned.", this);
            enabled = false;
            return;
        }

        for (int i = 0; i < initialSize; i++)
        {
            availableBullets.Enqueue(CreateBullet());
        }
    }

    // Takes a bullet out of the pool, making a new one only if every pooled bullet is already in use
    public Bullet Get()
    {
        if (bulletPrefab == null)
        {
            return null;
        }

        return availableBullets.Count > 0 ? availableBullets.Dequeue() : CreateBullet();
    }

    // Deactivates a bullet and puts it back in the pool so it can be reused
    public void Return(Bullet bullet)
    {
        if (bullet == null || !bullet.gameObject.activeSelf)
        {
            return;
        }

        bullet.gameObject.SetActive(false);
        availableBullets.Enqueue(bullet);
    }

    // Instantiates one inactive bullet that knows which pool it belongs to
    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform);
        bullet.SetPool(this);
        bullet.gameObject.SetActive(false);
        return bullet;
    }
}