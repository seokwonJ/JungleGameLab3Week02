using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopUiManager : MonoBehaviour
{
    int _myCoin;
    public Text coinCount;

    private void Start()
    {
        UpdatePurchase();
    }

    public void GoStart()
    {
        GameManager.Instance.GoInGameScene();
    }

    public void UpdatePurchase()
    {
        _myCoin = StateManager.Instance.MyCoin;
        coinCount.text = "Sharksfin : " + _myCoin.ToString();
    }
}
