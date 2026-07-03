using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _pressedSprite;
    [SerializeField] private int _sortingOrder = 5000;

    private SpriteRenderer _skinRenderer;

    private void Awake()
    {
        _skinRenderer = GetComponentInChildren<SpriteRenderer>();

        if (_skinRenderer != null)
        {
            _skinRenderer.sortingOrder = _sortingOrder;
        }

        Cursor.visible = false;
    }

    private void Update()
    {
        if (_skinRenderer == null || Camera.main == null) return;

        if (Input.GetMouseButtonDown(0))
        {
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
        Cursor.visible = !focus;
    }
}
