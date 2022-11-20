using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinishMoney : MonoBehaviour
{
    public Animator finishMoneyAnimator;
    public PlayerScriptable playerData = null;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FinisMoneyAnim());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FinisMoneyAnim()
    {
        //DOTween.To(() => playerData.GetMoney, (yeniDeger) => playerData.GetMoney = yeniDeger, playerData.GetMoney+ playerData.GetFinishMoney, 3f);
        for (int i = 0; i < 10; i++)
        {


            
            Money.Current.coinAnim.SetTrigger("CoinT");
            finishMoneyAnimator.SetTrigger("finishMoney");
            
            yield return new WaitForSeconds(0.4f);
            playerData.GetMoney += playerData.GetFinishMoney;
            PlayerPrefs.SetFloat("totalMoney", playerData.GetMoney);
            Debug.Log("para= " + PlayerPrefs.GetFloat("totalMoney"));
        }
    }
}
