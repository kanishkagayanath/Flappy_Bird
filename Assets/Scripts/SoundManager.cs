using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Sound Effects")]
    public GameObject PlayerFly; 
    public GameObject gameoversound;    
    public GameObject scoreSound; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    
    public void PlayFlySound()
    {
        if (PlayerFly != null)
        {
            GameObject soundObj = Instantiate(PlayerFly);
            Destroy(soundObj, 2f);
        }
    }

    public void PlayGameOverSound()
    {
        if (gameoversound != null)
        {
            GameObject soundObj = Instantiate(gameoversound);
            Destroy(soundObj, 2f);
        }
    }

    public void PlayScoreSound()
    {
        if (scoreSound != null)
        {
            GameObject soundObj = Instantiate(scoreSound);
            Destroy(soundObj, 2f);
        }
    }
}