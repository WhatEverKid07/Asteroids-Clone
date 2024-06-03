using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOne : MonoBehaviour
{
    [Header("Ship Parameteres")]
    [SerializeField] private float shipAcceleration = 10f;
    [SerializeField] private float shipMaxVelocity = 10f;
    [SerializeField] private float shipRotationSpeed = 180f;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private ParticleSystem exhaustParticles;

    [Header("Audio")]
    [SerializeField] private AudioSource exhaustSFX;
    [SerializeField] private AudioSource shootingSFX;
    [SerializeField] private AudioSource explosionSFX;

    [Header("Object Referance")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private ParticleSystem destroyedParticles;

    private Rigidbody2D shipRigibody;
    private bool isAlive = true;
    private bool isAccelerating = false;
    public GameManager gameManager;

    private void Start()
    {
        shipRigibody = GetComponent<Rigidbody2D>();
        //GameManager gameManager = FindAnyObjectByType<GameManager>();
    }
    private void Update()
    {
        if (isAlive)
        {
            HandleSipAcceleration();
            HandleShipRotation();
            HandleShooting();
        }
    }
    private void FixedUpdate()
    {
        if (isAlive && isAccelerating)
        {
            shipRigibody.AddForce(shipAcceleration * transform.up);
            shipRigibody.velocity = Vector2.ClampMagnitude(shipRigibody.velocity, shipMaxVelocity);
        }
    }
    private void HandleSipAcceleration()
    {
        isAccelerating = Input.GetKey(KeyCode.W);
        if (isAccelerating && !exhaustParticles.isPlaying)
        {
            exhaustParticles.Play();
            exhaustSFX.Play();
            Debug.Log("P1 accelerating");
        }
        else if (!isAccelerating && exhaustParticles.isPlaying)
        {
            exhaustParticles.Stop();
            exhaustSFX.Stop();
        }
    }
    private void HandleShipRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(shipRotationSpeed * Time.deltaTime * transform.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-shipRotationSpeed * Time.deltaTime * transform.forward);
        }
    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shootingSFX.Play();
            Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

            Vector2 shipVelocity = shipRigibody.velocity;
            Vector2 shipDirection = transform.up;
            float shipForwardSpeed = Vector2.Dot(shipVelocity, shipDirection);

            if(shipForwardSpeed < 0)
            {
                shipForwardSpeed = 0;
            }
            
            bullet.velocity = shipDirection * shipForwardSpeed;
            bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroids"))
        {
            explosionSFX.Play();
            isAlive = false;
            //GameManager gameManager = FindAnyObjectByType<GameManager>();
            gameManager.GameOver();
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
