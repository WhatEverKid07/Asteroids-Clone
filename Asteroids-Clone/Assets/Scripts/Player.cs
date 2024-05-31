using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Ship Parameteres")]
    [SerializeField] private float shipAcceleration = 10f;
    [SerializeField] private float shipMaxVelocity = 10f;
    [SerializeField] private float shipRotationSpeed = 180f;
    [SerializeField] private float bulletSpeed = 8f;

    [Header("Object Referance")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private ParticleSystem destroyedParticles;

    private Rigidbody2D shipRigibody;
    private bool isAlive = true;
    private bool isAccelerating = false;

    private void Start()
    {
        shipRigibody = GetComponent<Rigidbody2D>();
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
        isAccelerating = Input.GetKey(KeyCode.UpArrow);
    }
    private void HandleShipRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(shipRotationSpeed * Time.deltaTime * transform.forward);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-shipRotationSpeed * Time.deltaTime * transform.forward);
        }
    }
    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
            isAlive = false;
            GameManager gameManager = FindAnyObjectByType<GameManager>();
            gameManager.GameOver();
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
