using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [SerializeField] private Texture2D normalCursor;
    [SerializeField] private Texture2D pressedCursor;
    [SerializeField] private Vector2 hotspot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
        SetNormal();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetPressed();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SetNormal();
        }
    }

    public void SetNormal()
    {
        Cursor.SetCursor(normalCursor, hotspot, CursorMode.Auto);
    }

    public void SetPressed()
    {
        Cursor.SetCursor(pressedCursor, hotspot, CursorMode.Auto);
    }
}
