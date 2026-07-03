using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _pressedSprite;

    private SpriteRenderer _skinRenderer;

    private void Awake()
    {
        _skinRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (_skinRenderer == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.visible = false;
            _skinRenderer.sprite = _pressedSprite;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _skinRenderer.sprite = _normalSprite;
        }

        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.visible = false;
    }
}
