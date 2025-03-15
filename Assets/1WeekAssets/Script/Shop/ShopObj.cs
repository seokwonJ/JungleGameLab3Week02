using System.Collections;
using UnityEngine;

public enum UpgradeItem
{
    SpearCount = 0,
    HealthPower = 1,
    ReroadingTime = 2
}

public class ShopObj : MonoBehaviour
{
    public GameObject msgSuccess;
    public GameObject msgFailure;

    public UpgradeItem characterState;

    IEnumerator Success()
    {
        msgSuccess.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        msgSuccess.SetActive(false);
    }
    IEnumerator Failure()
    {
        msgFailure.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        msgFailure.SetActive(false);
    }

}