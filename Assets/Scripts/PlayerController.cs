using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Added DOTween

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float force = 10f;
    public float rotationSpeed = 5f;
    public float maxRotation = 30f;

    private Rigidbody2D rb;
    private bool inputEnabled = false;
    private Vector3 initialScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;
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
        rb.linearVelocity = Vector2.up * force;

        // Juice: Squash and Stretch animation on jump
        transform.DOKill(true);
        transform.localScale = initialScale;
        transform.DOPunchScale(new Vector3(-0.25f, 0.35f, 0f), 0.15f, 10, 1f);

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
            float targetZ = 0f;
            if (rb.linearVelocity.y > 0)
            {
                targetZ = maxRotation;
            }
            else if (rb.linearVelocity.y < -2)
            {
                targetZ = -maxRotation;
            }

            // Juice: Smooth tilt rotation tween
            transform.DORotate(new Vector3(0, 0, targetZ), 0.15f).SetEase(Ease.OutQuad);
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
            // Juice: Camera shake on crash
            if (Camera.main != null)
            {
                Camera.main.DOShakePosition(0.35f, 0.4f, 25, 90f).SetUpdate(true);
            }

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
        else if (other.CompareTag("Coin"))
        {
            // Call the DOTween collect animation on the coin
            Coin coin = other.GetComponent<Coin>();
            if (coin != null)
            {
                coin.Collect();
            }

            // Add extra points for coins (e.g., +2 points) or track a separate coin currency
            GameManager.instance.currentScore += 2; 
            
            // Re-use your UpdateScoreUI logic here (you may need to make UpdateScoreUI public in GameManager)
            if (GameManager.instance.scoreText != null)
            {
                GameManager.instance.scoreText.text = GameManager.instance.currentScore.ToString();
                GameManager.instance.scoreText.transform.DOPunchScale(Vector3.one * 0.4f, 0.2f, 8, 1f);
            }

            // Optional: Add a specific coin pickup sound here
        }
    }
}