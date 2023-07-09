using GPHive.Game.Upgrade;
using UnityEngine;

[CreateAssetMenu(menuName = "GP Hive Objects/Upgrades/Coefficient Upgrade")]
public class CoefficientUpgrade : Upgrade
{
    [SerializeField] private float startPrice;
    [SerializeField] private float coefficient;

    [Tooltip("-1 is infinity")] [SerializeField]
    private int maxLevel = -1;


    public override float GetPrice()
    {
        var _price = startPrice;

        for (int i = 0; i < Level; i++)
        {
            _price *= coefficient;
        }

        return _price;
    }

    public override bool IsMaxLevel()
    {
        if (Level == -1) return false;
        
        return Level == maxLevel;
    }
}