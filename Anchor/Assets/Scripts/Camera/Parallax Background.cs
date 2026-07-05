using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float parallaxEffect;
    private GameObject mainCamera;
    private float xPosition;
    private float yPosition;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera=GameObject.Find("Main Camera");

        xPosition = transform.position.x;
        yPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoveX=mainCamera.transform.position.x*parallaxEffect;
        float distanceMoveY=mainCamera.transform.position.y*parallaxEffect;

        transform.position=new Vector2(xPosition + distanceMoveX,yPosition+distanceMoveY);
    }
}
