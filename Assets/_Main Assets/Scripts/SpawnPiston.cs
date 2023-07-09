using System;
using System.Collections;
using System.Collections.Generic;
using GPHive.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPiston : Singleton<SpawnPiston>
{
    [SerializeField] private List<GameObject> pistonReferansPrefebs = new();
    [SerializeField] private List<Slot> mainSlots = new();
    [SerializeField] private float buyingStartPrice, buyingPrice;
    [SerializeField] private float buyingPriceIncrease;
    [SerializeField] private TextMeshProUGUI priceText;

    [SerializeField] private Button spawnButton;
    [SerializeField] private GameEvent tutorialEvent;

    private PlayerEconomy _playerEconomy;

    public void ButtonStuation()
    {
        var temp = false;

        foreach (var slot in mainSlots)
            if ((slot.SlotIsNull() && _playerEconomy.GetMoney() >= buyingPrice) || (slot.SlotIsNull() &&
                    _playerEconomy.ConvertToKBM(_playerEconomy.GetMoney()) ==
                    _playerEconomy.ConvertToKBM(buyingPrice)))
            {
                spawnButton.interactable = true;
                temp = true;
                break;
            }

        if (!temp) spawnButton.interactable = false;
    }

    private void Awake()
    {
        _playerEconomy = PlayerEconomy.Instance;
    }

    private void Start()
    {
        PlayerPrefs.GetInt("BuyingLevel", 1);
        CalculatePrice();
    }

    public void CalculatePrice()
    {
        var defaultLevel = Convert.ToInt32(PlayerPrefs.GetInt("BuyingLevel"));

        if (defaultLevel == 0)
        {
            buyingPrice = 0;
            priceText.text = "$ " + buyingPrice;
            return;
        }

        buyingPrice = buyingStartPrice * Mathf.Pow(buyingPriceIncrease, defaultLevel - 1);
        priceText.text = "$ " + PlayerEconomy.Instance.ConvertToKBM(buyingPrice);
    }


    public void SpawnButton()
    {
        foreach (var slot in mainSlots)
            if ((slot.SlotIsNull() && _playerEconomy.GetMoney() >= buyingPrice) || (slot.SlotIsNull() &&
                    _playerEconomy.ConvertToKBM(_playerEconomy.GetMoney()) ==
                    _playerEconomy.ConvertToKBM(buyingPrice)))
            {
                if (buyingPrice > PlayerEconomy.Instance.GetMoney())
                    PlayerEconomy.Instance.SpendMoney(PlayerEconomy.Instance.GetMoney());
                else
                    PlayerEconomy.Instance.SpendMoney(buyingPrice);

                NewPistonSpawn(slot, 0);
                PlayerPrefs.SetInt("BuyingLevel", PlayerPrefs.GetInt("BuyingLevel") + 1);
                CalculatePrice();
                SlotSpawnAndManage.Instance.SaveMainSlot();
                ButtonStuation();
                tutorialEvent.Raise();
                break;
            }
    }

    public void NewPistonSpawn(Slot slot, int level)
    {
        if (level >= pistonReferansPrefebs.Count) level = pistonReferansPrefebs.Count - 1;
        var go = ObjectPooling.Instance.GetFromPool(pistonReferansPrefebs[level]);
        go.SetActive(true);
        var goPistonSc = go.GetComponent<Piston>();
        goPistonSc.level = level;
        slot.SetPiston(goPistonSc, true);
    }
}