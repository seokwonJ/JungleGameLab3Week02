using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopUiManager : MonoBehaviour
{
    int _myCoin;
    public Text coinCount;

    public void GoStart()
    {
        GameManager.Instance.GameOver();
    }

}
