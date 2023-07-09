using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using NaughtyAttributes;


namespace GPHive.Game
{
    public class PlayerEconomy : Singleton<PlayerEconomy>
    {
        [SerializeField] private TextMeshProUGUI currencyText;
        private Camera cam;

        public float GetMoney()
        {
            return PlayerPrefs.GetFloat("Player Currency");
        }

        private void SetMoney(float amount)
        {
            PlayerPrefs.SetFloat("Player Currency", amount);
        }

        [SerializeField] private bool moneyTextAnimationEnabled;

        public GameEvent OnMoneyChange;

        [Button]
        private void Add10Coin()
        {
            AddMoney(100);
        }

        [Button]
        private void Add100Coin()
        {
            AddMoney(1000);
        }

        [Button]
        private void Add1000Coin()
        {
            AddMoney(10000000);
        }

        [SerializeField] private GameObject moneyAnimPoolReferance;
        [SerializeField] private Transform parentPointAnimObj;

        private void Start()
        {
            currencyText.text = ConvertToKBM(GetMoney());
            cam = Camera.main;
        }


        /// <summary>
        /// Returns true if player have enough currency.
        /// </summary>
        /// <param name="spendAmount">Currency amount to spend</param>
        /// <returns></returns>
        public bool SpendMoney(float spendAmount)
        {
            if (GetMoney() < spendAmount) return false;

            var _oldMoney = GetMoney();
            SetMoney(GetMoney() - spendAmount);

            if (GetMoney() < 0) SetMoney(0);

            if (moneyTextAnimationEnabled)
                MoneyTextAnimation(_oldMoney);
            else
                SetMoneyText();

            OnMoneyChange.Raise();
            return true;
        }

        public void AddMoney(float amount)
        {
            var _oldMoney = GetMoney();
            SetMoney(GetMoney() + amount);
            if (moneyTextAnimationEnabled)
                MoneyTextAnimation(_oldMoney);
            else
                SetMoneyText();

            OnMoneyChange.Raise();
        }

        public bool CheckEnoughMoney(float amount)
        {
            return GetMoney() >= amount;
        }

        private void MoneyTextAnimation(float _oldMoney)
        {
            DOTween.To(() => _oldMoney, x => _oldMoney = x, GetMoney(), 1)
                .OnUpdate(() => currencyText.text = ConvertToKBM(_oldMoney));
        }

        private void SetMoneyText()
        {
            currencyText.text = ConvertToKBM(GetMoney());
        }

        public void AddMoneyWithAnimation(Vector3 possitionReferance, float addMoneyAmount)
        {
            var go = ObjectPooling.Instance.GetFromPool(moneyAnimPoolReferance);
            go.transform.parent = parentPointAnimObj;
            go.transform.position = cam.WorldToScreenPoint(possitionReferance);
            go.GetComponent<TextMeshProUGUI>().text = "$ " + ConvertToKBM(addMoneyAmount);
            AddMoney(addMoneyAmount);
            go.SetActive(true);
            go.transform.DOMoveY(go.transform.position.y + 80, .5f).OnComplete(() => { go.SetActive(false); });
        }

        private static string[] _suffix = { "", "K", "M", "B", "T", "Q", "QU", "S" };

        public string ConvertToKBM(float value)
        {
            var _count = 0;
            while (value >= 1000f)
            {
                _count++;
                value /= 1000f;
            }

            return value < .01f && value != 0 ? $"{value:0.0}{_suffix[_count]}" : $"{value:0.0}{_suffix[_count]}";
        }
    }
}