using System;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine;

namespace GPHive.Game.Upgrade
{
    public class Upgrade : ScriptableObject
    {
        public string Name;
        [ReadOnly] [SerializeField] protected int level;
        public Sprite activeCart, pasifCart;
        public int Level => level;


        public void SetLevel()
        {
            level = PlayerPrefs.GetInt(Name, 0);
            PlayerPrefs.SetInt(Name, level);
        }

        public virtual void BuyUpgrade()
        {
            level++;
            PlayerPrefs.SetInt(Name, level);
        }

        public bool IsBuyable()
        {
            return PlayerEconomy.Instance.CheckEnoughMoney(GetPrice());
        }

        public virtual float GetPrice()
        {
            return 0;
        }

        public virtual bool IsMaxLevel()
        {
            return false;
        }
    }
}