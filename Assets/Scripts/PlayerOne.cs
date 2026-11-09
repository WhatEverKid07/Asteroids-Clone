using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOne : MonoBehaviour
{
    [Header("Ship Parameters")]
    [SerializeField] private float shipAcceleration = 10f;
    [SerializeField] private float shipMaxVelocity = 10f;
    [SerializeField] private float shipRotationSpeed = 180f;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private ParticleSystem exhaustParticles;

    [Header("Audio")]
    [SerializeField] private AudioSource exhaustSFX;
    [SerializeField] private AudioSource shootingSFX;
    [SerializeField] private AudioSource explosionSFX;

    [Header("Object Reference")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private ParticleSystem destroyedParticles;

    private Rigidbody2D shipRigidbody;
    private bool isAlive = true;
    private bool isAccelerating = false;
    public GameManager gameManager;

    private PlayerInputActions inputActions;
    private InputAction moveAction;
    private InputAction shootAction;

    private void Awake()
    {
        shipRigidbody = GetComponent<Rigidbody2D>();
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        moveAction = inputActions.Player1.Move;
        shootAction = inputActions.Player1.Shoot;

        moveAction.Enable();
        shootAction.Enable();

        shootAction.performed += ctx => HandleShooting();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        shootAction.Disable();
    }

    private void Update()
    {
        if (isAlive)
        {
            HandleShipAcceleration();
            HandleShipRotation();
        }
    }

    private void FixedUpdate()
    {
        if (isAlive && isAccelerating)
        {
            shipRigidbody.AddForce(shipAcceleration * transform.up);
            shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
        }
    }

    private void HandleShipAcceleration()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        isAccelerating = moveInput.y > 0;
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
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        if (moveInput.x != 0)
        {
            transform.Rotate(-moveInput.x * shipRotationSpeed * Time.deltaTime * Vector3.forward);
        }
    }

    private void HandleShooting()
    {
        if (isAlive)
        {
            shootingSFX.Play();
            Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

            Vector2 shipVelocity = shipRigidbody.velocity;
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
            explosionSFX.Play();
            isAlive = false;
            // GameManager gameManager = FindAnyObjectByType<GameManager>();
            gameManager.GameOver();
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
