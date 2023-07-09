using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GPHive.Game.Upgrade
{
    public class IncrementalUpgrade : MonoBehaviour
    {
        public List<SerializableDictionary<Upgrade, UpgradeButton>> upgrades;


        private void OnEnable()
        {
            SetUpgrades();
        }

        private void SetUpgrades()
        {
            foreach (var _upgrade in upgrades)
            {
                _upgrade.Key.SetLevel();
                if (CheckMaxLevel(_upgrade)) continue;

                _upgrade.Value.LevelText.SetText($"{_upgrade.Key.Level + 1}");
                _upgrade.Value.PriceText.SetText($"${PlayerEconomy.Instance.ConvertToKBM(_upgrade.Key.GetPrice())}");
            }

            CheckUpgradesBuyable();
        }

        public void CheckUpgradesBuyable()
        {
            foreach (var _upgrade in upgrades.Where(upgrade => !CheckMaxLevel(upgrade)))
            {
                _upgrade.Value.Button.interactable = _upgrade.Key.IsBuyable();
                var textMeshProUGUI = _upgrade.Value.LevelText;
                var valuePriceText = _upgrade.Value.PriceText;
                if (_upgrade.Value.Button.interactable)
                {
                    _upgrade.Value.Image.sprite = _upgrade.Key.activeCart;
                    textMeshProUGUI.color = new Color(textMeshProUGUI.color.r,
                        textMeshProUGUI.color.g, textMeshProUGUI.color.b, 1);
                    valuePriceText.color = new Color(valuePriceText.color.r,
                        valuePriceText.color.g, valuePriceText.color.b, 1);
                }
                else
                {
                    _upgrade.Value.Image.sprite = _upgrade.Key.pasifCart;
                    textMeshProUGUI.color = new Color(textMeshProUGUI.color.r,
                        textMeshProUGUI.color.g, textMeshProUGUI.color.b, .2f);
                    valuePriceText.color = new Color(valuePriceText.color.r,
                        valuePriceText.color.g, valuePriceText.color.b, .2f);
                }
            }
        }

        private static bool CheckMaxLevel(SerializableDictionary<Upgrade, UpgradeButton> upgrade)
        {
            if (!upgrade.Key.IsMaxLevel()) return false;

            upgrade.Value.LevelText.SetText("MAX");
            upgrade.Value.PriceText.gameObject.SetActive(false);
            upgrade.Value.Button.interactable = false;
            return true;
        }

        public void Upgrade(Upgrade upgrade)
        {
            if (PlayerEconomy.Instance.GetMoney() >= upgrade.GetPrice() ||
                PlayerEconomy.Instance.ConvertToKBM(PlayerEconomy.Instance.GetMoney()) ==
                PlayerEconomy.Instance.ConvertToKBM(upgrade.GetPrice()))
            {
                if (upgrade.GetPrice() > PlayerEconomy.Instance.GetMoney())
                    PlayerEconomy.Instance.SpendMoney(PlayerEconomy.Instance.GetMoney());
                else
                    PlayerEconomy.Instance.SpendMoney(upgrade.GetPrice());

                upgrade.BuyUpgrade();
                SetUpgrades();
            }
        }
    }
}