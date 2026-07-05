using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door;
    bool doorIsOpen;
    // Start is called before the first frame update
    void Start()
    {
        doorIsOpen=false;
        if (door == null)
        {
            Debug.LogError("未设置绑定的门");
        }
        //未对SoundManager进行检查
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Hook>())
        {
            if (door == null)
            {
                return;
            }
            door.SetActive(false);
            if(!doorIsOpen){
                // 使用单例访问 SoundManager
                if (SoundManager.SoundManager.Instance != null)
                {
                    SoundManager.SoundManager.Instance.Play("DoorOpen");
                }
            }
            doorIsOpen=true;
        }
    }
}
