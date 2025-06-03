using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float force = 10f;
    public float rotationSpeed = 5f;
    public float maxRotation = 30f;

    private Rigidbody2D rb;
    private bool inputEnabled = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (inputEnabled && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Jump();
        }

        
        HandleRotation();
    }

    private void Jump()
    {
        rb.velocity = Vector2.up * force;

       
        if (SoundManager.instance != null && SoundManager.instance.PlayerFly != null)
        {
            GameObject soundObj = Instantiate(SoundManager.instance.PlayerFly);
            Destroy(soundObj, 2f);
        }
    }

    private void HandleRotation()
    {
        if (rb != null)
        {
           
            float rotation = 0f;
            if (rb.velocity.y > 0)
            {
                rotation = maxRotation;
            }
            else if (rb.velocity.y < -2)
            {
                rotation = -maxRotation;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rotation), rotationSpeed * Time.deltaTime);
        }
    }

    public void EnableInput(bool enable)
    {
        inputEnabled = enable;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Ground"))
        {
            
            if (SoundManager.instance != null && SoundManager.instance.gameoversound != null)
            {
                GameObject soundObj = Instantiate(SoundManager.instance.gameoversound);
                Destroy(soundObj, 2f);
            }

            
            GameManager.instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("ScoreTrigger"))
        {
            GameManager.instance.AddScore();

           
            if (SoundManager.instance != null && SoundManager.instance.scoreSound != null)
            {
                GameObject soundObj = Instantiate(SoundManager.instance.scoreSound);
                Destroy(soundObj, 2f);
            }
        }
    }
}