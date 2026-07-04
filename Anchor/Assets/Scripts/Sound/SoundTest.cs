using UnityEngine;

public class SoundTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("SoundTest 启动了！");
        Debug.Log("按 1 播放音效1 | 按 2 播放音效2 | 按 3 播放BGM | 按 4 切换音效2 | 按 空格 停止所有");
    }

    void Update()
    {
        // 按 1：播放音效1（叠加）
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("播放音效: 1");
            SoundManager.SoundManager.Instance.Play("1");
        }

        // 按 2：播放音效2（叠加）
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("播放音效: 2");
            SoundManager.SoundManager.Instance.Play("2");
        }

        // 按 3：播放 BGM（音乐）
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("播放 BGM: 3");
            SoundManager.SoundManager.Instance.Play("3");
        }

        // 按 4：切换音效2（覆盖播放）
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("切换音效: 2（覆盖播放）");
            SoundManager.SoundManager.Instance.PlaySFXReplace("2");
        }

        // 按空格：停止所有
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("停止所有");
            SoundManager.SoundManager.Instance.StopAll();
        }
    }
}