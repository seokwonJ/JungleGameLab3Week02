using UnityEngine;

public class CursorController : MonoBehaviour
{
    private RectTransform crosshairRect;

    void Start()
    {
        crosshairRect = GetComponent<RectTransform>();
        Cursor.visible = false; // ���콺 Ŀ�� ����� (���� ����)
    }

    void Update()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform, // �θ� Canvas�� RectTransform
            Input.mousePosition, // ���콺 ��ġ
            null, // UI�� Screen Space - Overlay�̸� null
            out mousePos
        );

        crosshairRect.anchoredPosition = mousePos; // UI �̵�
    }
}
