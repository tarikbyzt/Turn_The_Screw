using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = ("Player Data") , order =1)]
public class PlayerScriptable : ScriptableObject
{
    
    [SerializeField] private bool decreaseOrStabil;
    [SerializeField] private float circleDownRate;
    [Header ("Stamina")]
    
    [SerializeField] private float stamina;
    [SerializeField] private float increaseToStamina;  // Bunu guclendirerek gucu arttiriyoruz.
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaDecrease; //Basili tuttugumuz andaki azalis orani.

    [SerializeField] private float resting;
    [Range(0, 100)][SerializeField] public int terlemeYuzde;
    [Range(0, 100)][SerializeField] public int kizarmaYuzde;
    [SerializeField] private float restingRate;     //Elimizi cektigimizdeki dinlenme orani.
    [SerializeField] private float restingDecrease;
    [SerializeField] private float levelMinStamina;
    [SerializeField] private float levelStaminaDecrease;


    [Header("Speed")]
    [SerializeField] private float speed;
    [SerializeField] private float increaseToSpeed; // Bunu güçlendirerek Hýzý arttýrýyoruz.
    [SerializeField] private float maxSpeed;

    [SerializeField] private float levelMinSpeed;
    [SerializeField] private float levelSpeedDecrease;



    
    [Header("Money")]
  
    [SerializeField] private float increaseRateToMoney;     //Bunu guclendirerek her adimdaki parayi arttiriyoruz.
    [SerializeField] private float maxUnitOfMoney;  // Her levelin adim basi verdigi paraya maksimum sinir koyuyoruz.
    [SerializeField] private float finishMoney;
    private float totalMoney;          //UI'da gözüken kazandigimiz toplam para.
    private float unitOfMoney;   //Her adimda kazanilacak para.    
    private float speedAnimLevel;

    public float GetSpeedAnimLevel
    {
        get { return speedAnimLevel; }
        set { speedAnimLevel = value; }
        
    }

    public float GetCircleDownRate
    {
        get { return circleDownRate; }
        set { circleDownRate = value; }

    }

    public bool GetDecreaseOrStabil
    {
        get { return decreaseOrStabil; }
        
    }
    public float GetLevelMinStamina
    {
        get { return levelMinStamina; }
        set { levelMinStamina = value; }
    }
    
    public float  GetLevelDecreaseStamina
    {
        get { return levelStaminaDecrease; }
        set { levelStaminaDecrease = value; }
    }

    public float GetLevelDecreaseSpeed
    {
        get { return levelSpeedDecrease; }
        set { levelSpeedDecrease = value; }
    }
    public float GetRestingRate 
    { get { return restingRate; } 
      set { restingRate = value;
            if (restingRate < 0)
            {
                restingRate = 0;
            }
        }
    
    }
    public float GetRestingDecrease
    {
        get { return restingDecrease; }
        set
        {
            restingDecrease = value;
            if (restingDecrease < 0)
            {
                restingDecrease = 0;
            }
        }
    }
    public float GetStaminaDecrease
    {
        get { return staminaDecrease; }
        set { staminaDecrease = value;
            if (staminaDecrease <0)
            {
                staminaDecrease = 0;
            }
        }
    }

    public float GetLevelMinSpeed { get { return levelMinSpeed; } }
    public float GetIncreaseRateToMoney
    {
        get { return increaseRateToMoney; }
        set
        {
            increaseRateToMoney = value;

        }
    }
    public float GetUnitOfMoney
    {
        get { return unitOfMoney; }
        set
        {
            unitOfMoney = value;
            if (unitOfMoney > maxUnitOfMoney)
            {
                unitOfMoney = maxUnitOfMoney;
            }
        }
    }
    public float GetFinishMoney
    {
        get { return finishMoney; }
        set { finishMoney = value;}
    }
    public float GetSpeed
    {
        get { return speed; }
        set
        {
            speed = value;

            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
            if (speed < 0)
            {
                speed = 0;
            }
        }
    }

    public float GetIncreaseToSpeed
    {
        get { return increaseToSpeed; }
        set { increaseToSpeed = value; }
    }
    public float GetMaxSpeed { get { return maxSpeed; } }

    public float GetStamina
    {
        get { return stamina; }
        set
        {
            stamina = value;
            if (stamina <0)
            {
                stamina = 0f;
            }

            if (stamina > resting)
            {
                stamina = resting;
            }
        }
    }
    public float GetResting
    {
        get { return resting; }
        set { resting = value; }
    }
    public float GetIncreaseToStamina
    {
        get { return increaseToStamina; }
        set { increaseToStamina = value; }
    }

    public float GetMaxStamina
    {
        get { return maxStamina; }
        set { maxStamina = value; }
    }

    public float GetMoney
    {
        get { return totalMoney; }
        set
        {
            totalMoney = value;
            if (totalMoney < 0)
            {
                totalMoney = 1;
            }
        }
    }
}
