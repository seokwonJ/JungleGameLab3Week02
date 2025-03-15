using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 originalPosition;
    [field: SerializeField] public Vector2 ScreenArea { get; private set; } // 화면 크기

    public Transform player;
    public float followSpeed;

    private bool _isDash;
    private Camera mainCamera;

    void Start()
    {
        
        mainCamera = transform.GetComponent<Camera>();
        // 카메라의 화면 경계를 월드 좌표로 변환
        ScreenArea = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, transform.position.z));
    }

    private void FixedUpdate()
    {
        if (_isDash)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 14, Time.deltaTime * followSpeed);
        }
        else 
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize,15, Time.deltaTime * followSpeed * 3);
        }
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y, transform.position.z), Time.deltaTime * followSpeed);
    }

    public void StartDashCamera()
    {
        _isDash = true;
    }

    public void EndDashCamera()
    {
        _isDash = false;
    }

    public void StartShake(float duration, float manitude)
    {
        StartCoroutine(ShakeCamera(duration, manitude));
    }

    // 카메라 화면 흔들기
    private IEnumerator ShakeCamera(float shakeDuration, float shakeMagnitude)
    {
        float elapsedTime = 0f;
        originalPosition = transform.position;
        while (elapsedTime < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            Camera.main.transform.position = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        Camera.main.transform.position = originalPosition; // 원래 위치로 복귀
    }
}