using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    public float speed = 5f;

    [Header("Score Trigger")]
    public GameObject scoreTrigger;
    private bool hasScored = false;

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

       
        CheckForScore();

        
        if (transform.position.x < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void CheckForScore()
    {
       
        if (!hasScored && transform.position.x < -2f)
        {
            hasScored = true;
            GameManager.instance.AddScore();

           
            if (SoundManager.instance != null && SoundManager.instance.scoreSound != null)
            {
                GameObject soundObj = Instantiate(SoundManager.instance.scoreSound);
                Destroy(soundObj, 2f);
            }
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