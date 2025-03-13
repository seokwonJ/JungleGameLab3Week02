using UnityEngine;

public class StateManager : MonoBehaviour
{
    static StateManager _instance;
    public static StateManager Instance { get { return _instance; } private set { } }

    static float _relodaingUpgradeValue = 0.2f;
    static float _reloadingTime = 1;
    static float _hp = 5f;
    static int spearCoin = 30;
    static int powerUpCoin = 10;
    static int hpCoin = 5;

    [field: SerializeField] public int SpearCount { get; set; }
    int _reloadUpgradeCount = 1;
    [field: SerializeField] public int MyCoin { get; private set; } = 0;
    [field: SerializeField] public float maxHP { get; private set; }


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // �� �̵��ص� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool BuySpear()
    {
        if (UseCoin(spearCoin))
        {
            SpearCount++;

            return true;
        }
        return false;
    }
    public bool ReroadingUpgrade()
    {
        if (UseCoin(powerUpCoin))
        {
            _reloadUpgradeCount++;
            return true;
        }
        return false;
    }
    public bool HpUpgrade()
    {
        if (UseCoin(hpCoin))
        {
            maxHP += _hp;
            return true;
        }
        return false;
    }


    public float ReloadingTime()
    {
        return _reloadingTime + (_relodaingUpgradeValue * _reloadUpgradeCount); // Spear.cs , isReturn �϶��� �ӵ��� �����ϵ��� �����ؾ��� 
    }

    public void CoinPlus()
    {
        MyCoin++;
    }
    public bool UseCoin(int coin)
    {
        if (MyCoin >= coin)
        {
            MyCoin -= coin;
            ShopUiManager shopUiManager = GameObject.Find("ShopUIManager").GetComponent<ShopUiManager>();
            if (shopUiManager != null) shopUiManager.UpdatePurchase();
            return true;
        }
        return false;
    }
}
