using System.Collections;
using System.Collections.Generic;
using GPHive.Game;
using UnityEngine;

public class PistonAnimationEventController : MonoBehaviour
{
    [SerializeField] private Piston myPiston;
    [SerializeField] private GameEvent addMoneyAndPumpEvent;

    public void AddMoney()
    {
        if (myPiston.animator.enabled)
        {
            var baseEarn = myPiston.baseStartValue * Mathf.Pow(myPiston.baseEarnMultiplier, myPiston.level);
            float boostEarn;

            if (myPiston.boostLevel > 1)
                boostEarn = baseEarn * ((myPiston.boostLevel - 1) * 0.1f + 1);
            else
                boostEarn = baseEarn;

            var totalEarn = boostEarn + myPiston.IncomeUpgrade.Level * myPiston.ıncomeEarnMultiplier;
            PlayerEconomy.Instance.AddMoneyWithAnimation(transform.position, totalEarn);
        }
    }


    public void AddMoneyAndPump()
    {
        addMoneyAndPumpEvent.Raise();
    }
}