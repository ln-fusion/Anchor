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
        if (other.GetComponent<Hook>())
        {
            Debug.Log("轮椅角色盗冠成功");
            SceneManager.LoadScene((currentSceneCount + 1) % SceneManager.sceneCountInBuildSettings);
        }
    }
}
