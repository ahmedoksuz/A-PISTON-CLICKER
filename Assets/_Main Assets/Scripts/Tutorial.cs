using System;
using System.Collections;
using System.Collections.Generic;
using GPHive.Game;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject clickHand, giveHand, takeHand, upgrades, tapTap, tapFasterToEarn;
    [SerializeField] private Button spawnButton;
    [SerializeField] private List<GameEventListener> Listeners = new();
    [SerializeField] private List<GameObject> otherSlots = new();

    private void Awake()
    {
        if (PlayerPrefs.GetInt("Tutorial", 0) >= 6)
        {
            upgrades.SetActive(true);
            enabled = false;
            foreach (var listener in Listeners) listener.enabled = false;
            foreach (var slot in otherSlots) slot.SetActive(true);
        }
        else
        {
            PlayerPrefs.DeleteAll();
            clickHand.SetActive(true);
            upgrades.SetActive(false);
            foreach (var slot in otherSlots) slot.SetActive(false);
        }
    }

    private void Update()
    {
        if (PlayerEconomy.Instance.GetMoney() >= 5 && PlayerPrefs.GetInt("Tutorial", 0) == 2)
        {
            PlayerPrefs.SetInt("Tutorial", 3);
            clickHand.SetActive(true);
            tapTap.SetActive(false);
            tapFasterToEarn.SetActive(false);
        }
    }


    public void EventTriggered()
    {
        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
        {
            PlayerPrefs.SetInt("Tutorial", 1);
            clickHand.SetActive(false);
            giveHand.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("Tutorial", 0) == 3)
        {
            PlayerPrefs.SetInt("Tutorial", 4);

            clickHand.SetActive(false);
            takeHand.SetActive(true);
        }
    }

    public void EventTriggeredMerge()
    {
        if (PlayerPrefs.GetInt("Tutorial", 0) == 4)
        {
            PlayerPrefs.SetInt("Tutorial", 5);
            takeHand.SetActive(false);
            giveHand.SetActive(true);
        }
    }

    public void EventTriggeredSlot()
    {
        if (PlayerPrefs.GetInt("Tutorial", 0) == 1)
        {
            giveHand.SetActive(false);
            PlayerPrefs.SetInt("Tutorial", 2);
            tapTap.SetActive(true);
            tapFasterToEarn.SetActive(true);
        }

        else if (PlayerPrefs.GetInt("Tutorial", 0) == 5)
        {
            PlayerPrefs.SetInt("Tutorial", 6);

            giveHand.SetActive(false);
            upgrades.SetActive(true);
            enabled = false;
            foreach (var listener in Listeners) listener.enabled = false;
            foreach (var slot in otherSlots) slot.SetActive(true);
        }
    }
}