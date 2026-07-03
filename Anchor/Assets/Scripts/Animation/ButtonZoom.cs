using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float zoomScale = 1.15f;
    [SerializeField] private float moveRight = 20f;
    [SerializeField] private float animationSpeed = 12f;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Vector2 originalAnchoredPosition;
    private bool isHovered;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = transform.localScale;

        if (rectTransform != null)
        {
            originalAnchoredPosition = rectTransform.anchoredPosition;
        }
    }

    private void Update()
    {
        var targetScale = isHovered ? originalScale * zoomScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);

        if (rectTransform != null)
        {
            var targetPosition = isHovered
                ? originalAnchoredPosition + new Vector2(moveRight, 0f)
                : originalAnchoredPosition;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * animationSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
