using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeySoundMapping
{
    public KeyCode key = KeyCode.Alpha1;
    public string soundId = "";
}

public class SoundTest : MonoBehaviour
{
    // 在 Inspector 中配置按键与音效的映射
    public List<KeySoundMapping> mappings = new List<KeySoundMapping>()
    {
        new KeySoundMapping { key = KeyCode.Alpha1, soundId = "1" },
        new KeySoundMapping { key = KeyCode.Alpha2, soundId = "2" },
        new KeySoundMapping { key = KeyCode.Alpha3, soundId = "3" },
        new KeySoundMapping { key = KeyCode.Alpha4, soundId = "2" }
    };

    // 停止所有的按键（可在 Inspector 修改）
    public KeyCode stopKey = KeyCode.Space;

    void Start()
    {
        Debug.Log("SoundTest 启动了！");
        Debug.Log("在 Inspector 中可自定义按键与音效映射。按配置的键播放对应音效，按停止键停止所有音效。");
    }

    void Update()
    {
        if (SoundManager.SoundManager.Instance == null)
            return;

        // 遍历映射并处理按键事件
        foreach (var m in mappings)
        {
            if (m == null || string.IsNullOrEmpty(m.soundId))
                continue;

            if (Input.GetKeyDown(m.key))
            {
                Debug.Log($"按键 {m.key} 播放音效: {m.soundId}");
                // 统一调用 Play，当前 SoundManager 中 SFX 已实现覆盖播放逻辑
                SoundManager.SoundManager.Instance.Play(m.soundId);
            }
        }

        // 停止所有
        if (Input.GetKeyDown(stopKey))
        {
            Debug.Log("停止所有");
            SoundManager.SoundManager.Instance.StopAll();
        }
    }
}
