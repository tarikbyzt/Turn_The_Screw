using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBOx : MonoBehaviour
{
    [SerializeField] private GameObject parcaliKumbara;
    [SerializeField] private GameObject moneyBoxMoney;
    void Start()
    {
        parcaliKumbara.SetActive(true);
        
        StartCoroutine(wait());
    }
    

    
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.3f);
        moneyBoxMoney.SetActive(true);
        gameObject.SetActive(false);
    }
}
