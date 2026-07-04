using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        if (door == null)
        {
            Debug.LogError("未设置绑定的门");
        }
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
        }
    }
}
