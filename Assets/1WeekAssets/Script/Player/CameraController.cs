using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 originalPosition;
    [field: SerializeField] public Vector2 ScreenArea { get; private set; } // 화면 크기
    void Start()
    {
        originalPosition = transform.position;

        // 카메라의 화면 경계를 월드 좌표로 변환
        ScreenArea = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, transform.position.z));
    }

    public void StartShake(float duration, float manitude)
    {
        StartCoroutine(ShakeCamera(duration, manitude));
    }

    // 카메라 화면 흔들기
    private IEnumerator ShakeCamera(float shakeDuration, float shakeMagnitude)
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            Camera.main.transform.position = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.position = originalPosition; // 원래 위치로 복귀
    }
}