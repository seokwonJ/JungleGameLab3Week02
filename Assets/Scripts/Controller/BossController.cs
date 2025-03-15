using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject Kraken;
    private bool isBossStart;

    public GameObject spawner;
    public GameObject clear;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBossStart && collision.tag == "Player")
        {
            isBossStart = true;
            StartCoroutine(BossStart());
        }
    }

    IEnumerator BossStart()
    {
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.StartBossCamera();
        cameraController.StartShake(1f, 1f);
        Kraken = Instantiate(Kraken, new Vector3(75, 19, 0), Quaternion.identity);
        Kraken.GetComponent<KrakenMove>().bossController = this;
        yield return new WaitForSeconds(1.6f);
        cameraController.EndBossCamera();
    }

    public void BossClear()
    {
        spawner.SetActive(false);
        clear.SetActive(true);
        TimeManager.Instance.Clear();
    }
}
