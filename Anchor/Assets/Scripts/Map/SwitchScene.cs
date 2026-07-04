using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    private int currentSceneCount;
    private void Start()
    {
        currentSceneCount = SceneManager.GetActiveScene().buildIndex;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            SceneManager.LoadScene((currentSceneCount + 1) % SceneManager.sceneCountInBuildSettings);
        }
    }
}
