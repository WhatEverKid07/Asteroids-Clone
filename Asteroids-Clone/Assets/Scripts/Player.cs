using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private bool player1;
    [SerializeField] private bool player2;

    [Header("Ship Parameteres")]
    [SerializeField] private float shipAcceleration = 10f;
    [SerializeField] private float shipMaxVelocity = 10f;
    [SerializeField] private float shipRotationSpeed = 180f;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private ParticleSystem exhaustParticles;

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
        gameManager.playersLeft = 2;
    }
    private void Update()
    {
        if (isAlive && player1)
        {
            HandleSipAcceleration();
            HandleShipRotation();
            HandleShooting();
        }
        if (isAlive && player2)
        {
            Player2HandleSipAcceleration();
            Player2HandleShipRotation();
            Player2HandleShooting();
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
            Debug.Log("P1 accelerating");
        }
        else if (!isAccelerating && exhaustParticles.isPlaying)
        {
            exhaustParticles.Stop();
        }
    }
    private void Player2HandleSipAcceleration()
    {
        isAccelerating = Input.GetKey(KeyCode.UpArrow);
        if (isAccelerating && !exhaustParticles.isPlaying)
        {
            exhaustParticles.Play();
            Debug.Log("P2 accelerating");
        }
        else if (!isAccelerating && exhaustParticles.isPlaying)
        {
            exhaustParticles.Stop();
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
    private void Player2HandleShipRotation()
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
    private void Player2HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

            Vector2 shipVelocity = shipRigibody.velocity;
            Vector2 shipDirection = transform.up;
            float shipForwardSpeed = Vector2.Dot(shipVelocity, shipDirection);

            if (shipForwardSpeed < 0)
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
            //GameManager gameManager = FindAnyObjectByType<GameManager>();
            gameManager.playersLeft-= 1;
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if(gameManager.playersLeft == 0)
        {
            gameManager.GameOver();
        }
    }
}
