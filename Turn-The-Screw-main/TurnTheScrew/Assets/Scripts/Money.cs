using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.NiceVibrations;

public class Money : MonoBehaviour
{
    public static Money Current;
    [SerializeField] PlayerScriptable playerData = null;
    [SerializeField] private Animator moneyPickText;
    [SerializeField] private TextMeshProUGUI pick;
    [SerializeField] private Canvas canvas;
    public Animator coinAnim;
    private LevelController levelCon;
    private bool HapticsAllowed=true;


    // Start is called before the first frame update
    void Start()
    {
        Current = this;
        levelCon = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
        MMVibrationManager.SetHapticsActive(HapticsAllowed);
        //canvas.transform.LookAt(Camera.main.transform);

    }


    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        


        if (other.gameObject.CompareTag("Player"))
        {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            pick.text = "$" + playerData.GetUnitOfMoney.ToString().Replace(",", ".");
            moneyPickText.SetTrigger("Pick");
            //coinAnim.SetBool("Coin", true);
            coinAnim.SetTrigger("CoinT");

            playerData.GetMoney += playerData.GetUnitOfMoney;
            PlayerPrefs.SetFloat("totalMoney", playerData.GetMoney);
            //Debug.Log(PlayerPrefs.GetFloat("totalMoney"));
            
            levelCon.totalMoneyText.text = Mathf.Floor(playerData.GetMoney).ToString();
            //other.gameObject.GetComponent<PlayerController>().totalMoneyText.text = Mathf.Floor(playerData.GetMoney).ToString();
            //pickMoney.text = "$" + playerData.GetUnitOfMoney;
            //pickMoneyAnimator.SetTrigger("Pick");

        }
    }
}
