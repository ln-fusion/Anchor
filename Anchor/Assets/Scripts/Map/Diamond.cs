using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    private int currentSceneCount;
    [SerializeField] private GameObject dialogSystem;
    [SerializeField] private GameObject dialogRoot;
    private bool hasTriggered = false;
	private void Start()
    {
        currentSceneCount = SceneManager.GetActiveScene().buildIndex;
    }

	private void Update()
	{
		// 暴力检测对话框是否关闭，如果关闭则切换场景
		if (hasTriggered && !dialogRoot.activeSelf)
        {
            if (SceneManager.GetActiveScene().name == "3-Level")
            {
                QuitGame();
			}
			SceneManager.LoadScene((currentSceneCount + 1) % SceneManager.sceneCountInBuildSettings);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() )
        {
            if (dialogSystem == null)
            {
                Debug.LogError("dialogSystem is not assigned in the inspector.");
                return;
			}
			dialogSystem.GetComponent<PlayDialoguesOnCertainConditions>().WhenTheLevelHasBeenCompleted();
            hasTriggered = true;
		}
    }

	void QuitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
	}
}
