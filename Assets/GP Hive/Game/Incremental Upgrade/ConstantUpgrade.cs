using GPHive.Game.Upgrade;
using UnityEngine;

[CreateAssetMenu(menuName = "GP Hive Objects/Upgrades/Constant Upgrade")]
public class ConstantUpgrade : Upgrade
{
    [SerializeField] private float startPrice;
    [SerializeField] private float pricePerLevel;

    [Tooltip("-1 is infinity")] [SerializeField]
    private int maxLevel = -1;

    public override float GetPrice()
    {
        return startPrice + pricePerLevel * Level;
    }

    public override bool IsMaxLevel()
    {
        if (Level == -1) return false;

        return Level == maxLevel;
    }
}