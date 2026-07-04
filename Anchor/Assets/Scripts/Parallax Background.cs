using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float parallaxEffect;
    private GameObject mainCamera;
    private float xPosition;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera=GameObject.Find("Main Camera");
        Debug.Log(mainCamera);

        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToMove=mainCamera.transform.position.x*parallaxEffect;

        transform.position=new Vector2(xPosition + distanceToMove,transform.position.y);
    }
}
