using System;
using GPHive.Game;
using TMPro;
using UnityEngine;

public class OflineEarning : MonoBehaviour
{
    [SerializeField] private GameObject oflineEarningUI;
    [SerializeField] private TextMeshProUGUI oflineEarningMoneyText;
    [SerializeField] private CoefficientUpgrade oflineEarningUpgrade;
    [SerializeField] private float startValueEarningMultiple, levelByAmountOfIncrease;
    private PlayerEconomy _playerEconomy;

    private void Awake()
    {
        _playerEconomy = PlayerEconomy.Instance;
    }

    private void Start()
    {
        var lastTime = Convert.ToDateTime(PlayerPrefs.GetString("LastTime", DateTime.Now.ToString()));
        var duration = (DateTime.Now - lastTime).TotalMinutes;

        if (duration > 1 && PlayerPrefs.GetInt("DidPlayAny", 0) > 0)
        {
            oflineEarningUI.SetActive(true);
            _playerEconomy.AddMoney(OflineEarningCalculateValue(duration));
            oflineEarningMoneyText.text =
                "$" + _playerEconomy.ConvertToKBM(OflineEarningCalculateValue(duration));
        }
        else
        {
            PlayerPrefs.SetInt("DidPlayAny", 1);
        }
    }

    private void OnApplicationQuit()
    {
        var time = DateTime.Now;
        PlayerPrefs.SetString("LastTime", time.ToString());
    }

    public float OflineEarningCalculateValue(double minute)
    {
        return (int)minute *
               (startValueEarningMultiple * Mathf.Pow(levelByAmountOfIncrease, oflineEarningUpgrade.Level));
    }

    public void Button()
    {
        oflineEarningUI.SetActive(false);
    }
}