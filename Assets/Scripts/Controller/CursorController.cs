using UnityEngine;

public class CursorController : MonoBehaviour
{
    private RectTransform crosshairRect;

    void Start()
    {
        crosshairRect = GetComponent<RectTransform>();
        Cursor.visible = false; // 마우스 커서 숨기기 (선택 사항)
    }

    void Update()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform, // 부모 Canvas의 RectTransform
            Input.mousePosition, // 마우스 위치
            null, // UI가 Screen Space - Overlay이면 null
            out mousePos
        );

        crosshairRect.anchoredPosition = mousePos; // UI 이동
    }
}
