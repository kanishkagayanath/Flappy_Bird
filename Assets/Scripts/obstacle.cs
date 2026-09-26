using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    public float speed = 5f;
    
    [Header("Destruction Settings")]
    // Adjust this value in the Inspector to make it destroy further off-screen
    public float destroyXPosition = -12f; 

    [Header("Score Trigger")]
    public GameObject scoreTrigger;

    void Start()
    {
        if (scoreTrigger == null)
        {
            CreateScoreTrigger();
        }
    }

    void Update()
    {
        if (GameManager.instance.isGameOver == false)
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        }

        // Now uses the variable instead of a hardcoded -6f
        if (transform.position.x < destroyXPosition) 
        {
            Destroy(gameObject);
        }
    }

    private void CreateScoreTrigger()
    {
        GameObject trigger = new GameObject("ScoreTrigger");
        trigger.transform.SetParent(transform);
        trigger.transform.localPosition = Vector3.zero;
        trigger.layer = gameObject.layer;

        BoxCollider2D triggerCollider = trigger.AddComponent<BoxCollider2D>();
        triggerCollider.isTrigger = true;
        triggerCollider.size = new Vector2(0.5f, 8f); 

        trigger.tag = "ScoreTrigger";
        scoreTrigger = trigger;
    }
}