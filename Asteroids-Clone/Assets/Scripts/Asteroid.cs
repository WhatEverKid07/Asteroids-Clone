using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private AudioSource explosionSFX;
    [SerializeField] private ParticleSystem destroyedParticles;
    [SerializeField] private Vector2 rotationSpeedRange = new Vector2(-200f, 200f);
    public float size = 3;
    public GameManager gameManager;

    private void Start()
    {
        explosionSFX = GetComponent<AudioSource>();

        transform.localScale = 0.5f * size * Vector3.one;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 direction = new Vector2(Random.value, Random.value).normalized;
        float spawnSpeed = Random.Range(4f - size, 5f - size);
        rb.AddForce(direction * spawnSpeed, ForceMode2D.Impulse);
        float rotationSpeed = Random.Range(rotationSpeedRange.x, rotationSpeedRange.y);
        rb.angularVelocity = rotationSpeed;
        gameManager.asteroidCount++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            explosionSFX.Play();

            gameManager.asteroidCount--;
            Destroy(collision.gameObject);

            if (size > 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    Asteroid newAsteroid = Instantiate(this, transform.position, Quaternion.identity);
                    newAsteroid.size = size - 1;
                    newAsteroid.gameManager = gameManager;
                }
            }
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
