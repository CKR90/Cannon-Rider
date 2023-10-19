using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeVehicle : MonoBehaviour
{
    public GameObject Popup;
    public TextMeshProUGUI PopupText;
    public GameObject coinPointer;
    public Boxx speed;
    public Boxx damping;
    public Boxx tire;
    public Boxx maxrange;
    public Color DefaultColor;
    public Color LockedColor;


    public List<int> prices = new List<int>();


    void Start()
    {
        SetAllVehicleUpgrades();
    }
    private void SetAllVehicleUpgrades()
    {
        SetSpeed();
        SetDamping();
        SetTire();
        SetMaxrange();
    }
    private void SetSpeed()
    {
        if(User.info.velocitylevel >= 20)
        {
            speed.coinText.SetText("");
            speed.levelText.SetText("MAX");
            speed.coinBG.color = DefaultColor;
        }
        else
        {
            speed.coinText.SetText(prices[User.info.velocitylevel].ToString());
            speed.levelText.SetText(User.info.velocitylevel.ToString() + " / 20");

            if (prices[User.info.velocitylevel] > User.info.coin)
            {
                speed.coinBG.color = LockedColor;
            }
            else
            {
                speed.coinBG.color = DefaultColor;
            }
        }
    }
    private void SetDamping()
    {
        if (User.info.suspensionlevel >= 20)
        {
            damping.coinText.SetText("");
            damping.levelText.SetText("MAX");
            damping.coinBG.color = DefaultColor;
        }
        else
        {
            damping.coinText.SetText(prices[User.info.suspensionlevel].ToString());
            damping.levelText.SetText(User.info.suspensionlevel.ToString() + " / 20");

            if (prices[User.info.suspensionlevel] > User.info.coin)
            {
                damping.coinBG.color = LockedColor;
            }
            else
            {
                damping.coinBG.color = DefaultColor;
            }
        }
    }
    private void SetTire()
    {
        if (User.info.tirelevel >= 20)
        {
            tire.coinText.SetText("");
            tire.levelText.SetText("MAX");
            tire.coinBG.color = DefaultColor;
        }
        else
        {
            tire.coinText.SetText(prices[User.info.tirelevel].ToString());
            tire.levelText.SetText(User.info.tirelevel.ToString() + " / 20");

            if (prices[User.info.tirelevel] > User.info.coin)
            {
                tire.coinBG.color = LockedColor;
            }
            else
            {
                tire.coinBG.color = DefaultColor;
            }
        }
    }
    private void SetMaxrange()
    {
        if (User.info.cannonlevel >= 20)
        {
            maxrange.coinText.SetText("");
            maxrange.levelText.SetText("MAX");
            maxrange.coinBG.color = DefaultColor;
        }
        else
        {
            maxrange.coinText.SetText(prices[User.info.cannonlevel].ToString());
            maxrange.levelText.SetText(User.info.cannonlevel.ToString() + " / 20");

            if (prices[User.info.cannonlevel] > User.info.coin)
            {
                maxrange.coinBG.color = LockedColor;
            }
            else
            {
                maxrange.coinBG.color = DefaultColor;
            }
        }
    }

    public void ClickEvent(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 0: UpgradeSpeed(); break;
            case 1: UpgradeDamping(); break;
            case 2: UpgradeTire(); break;
            case 3: UpgradeMaxrange(); break;
        }
    }

    private void UpgradeSpeed()
    {
        if (User.info.velocitylevel >= 20)
        {
            return;
        }

        int need = prices[User.info.velocitylevel];

        if(need <= User.info.coin)
        {
            SpendCoin.Instance.SpendAnim(need);
            GameDatabase.Instance.PaySpeedUpgrade(need, User.info.velocitylevel + 1);
            SoundController.instance.Play(SFXList.CoinFalling);
            TopBarData.Instance.SetData();
            SetAllVehicleUpgrades();
        }
        else
        {
            SoundController.instance.Play(SFXList.Denied);
            Popup.SetActive(true);
            PopupText.SetText("You don't have enough coins.");
            coinPointer.SetActive(true);
        }
    }
    private void UpgradeDamping()
    {
        if (User.info.suspensionlevel >= 20) return;

        int need = prices[User.info.suspensionlevel];

        if (need <= User.info.coin)
        {
            SpendCoin.Instance.SpendAnim(need);
            GameDatabase.Instance.PayDampingUpgrade(need, User.info.suspensionlevel + 1);
            SoundController.instance.Play(SFXList.CoinFalling);
            TopBarData.Instance.SetData();
            SetAllVehicleUpgrades();
        }
        else
        {
            SoundController.instance.Play(SFXList.Denied);
            Popup.SetActive(true);
            PopupText.SetText("You don't have enough coins.");
            coinPointer.SetActive(true);
        }
    }
    private void UpgradeTire()
    {
        if (User.info.tirelevel >= 20) return;

        int need = prices[User.info.tirelevel];

        if (need <= User.info.coin)
        {
            SpendCoin.Instance.SpendAnim(need);
            GameDatabase.Instance.PayTireUpgrade(need, User.info.tirelevel + 1);
            SoundController.instance.Play(SFXList.CoinFalling);
            TopBarData.Instance.SetData();
            SetAllVehicleUpgrades();
        }
        else
        {
            SoundController.instance.Play(SFXList.Denied);
            Popup.SetActive(true);
            PopupText.SetText("You don't have enough coins.");
            coinPointer.SetActive(true);
        }
    }
    private void UpgradeMaxrange()
    {
        if (User.info.cannonlevel >= 20) return;

        int need = prices[User.info.cannonlevel];

        if (need <= User.info.coin)
        {
            SpendCoin.Instance.SpendAnim(need);
            GameDatabase.Instance.PayMaxrangeUpgrade(need, User.info.cannonlevel + 1);
            SoundController.instance.Play(SFXList.CoinFalling);
            TopBarData.Instance.SetData();
            SetAllVehicleUpgrades();
        }
        else
        {
            SoundController.instance.Play(SFXList.Denied);
            Popup.SetActive(true);
            PopupText.SetText("You don't have enough coins.");
            coinPointer.SetActive(true);
        }
    }




    [System.Serializable] public class Boxx
    {
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI coinText;
        public Image coinBG;
    }
}
