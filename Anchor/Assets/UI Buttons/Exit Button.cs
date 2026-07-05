using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ExitButton : MonoBehaviour
{
    public void Tapped(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 在编辑器里停止运行
#else
        Application.Quit(); // 在打包后的游戏中退出
#endif
    }
}
