using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelController : MonoBehaviour
{
    [SerializeField] PlayerScriptable playerData = null;
    [SerializeField] PlayerScriptable playerDatas = null;
    private PlayerController playerController;
    private float currentLevel = 0;
    public bool isGameActive = false;
    public bool levelFinish = false;
    [SerializeField] private TextMeshProUGUI levelText;
    public GameObject moneyBag;
    public Animator finishMoneyAnimator;
    [Header("Speed")]
    public TextMeshProUGUI speedLevelText;
    public TextMeshProUGUI speedUpgradeFeeText;

    [Header("Stamina")]
    public TextMeshProUGUI staminaLevelText;
    public TextMeshProUGUI staminaUpgradeFeeText;

    
    [Header("Money")]
    public TextMeshProUGUI moneyLevelText;
    public TextMeshProUGUI moneyUpgradeFeeText;

    public TextMeshProUGUI totalMoneyText;       //Toplam money.

    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject levelVictoryMenu;
    [SerializeField] private GameObject circle;
    
    private int levelll = 0;
    



    public float GetCurrentLevel
    {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
        }
    }
    
    void Awake()
    {
        isGameActive = false;
        
    }

    void Start()
    {
        //StartCoroutine(FinisMoneyAnim());
        speedLevelText.text = PlayerPrefs.GetInt("currentSpeedLevel").ToString();
        speedUpgradeFeeText.text = (PlayerPrefs.GetFloat("levelUpFeeSpeedText")).ToString();

        
        staminaUpgradeFeeText.text = PlayerPrefs.GetFloat("levelUpFeeStaminaText").ToString();
        staminaLevelText.text = PlayerPrefs.GetInt("currentStaminaLevel").ToString();

        moneyUpgradeFeeText.text = PlayerPrefs.GetFloat("levelUpFeeMoneyText").ToString();
        moneyLevelText.text = PlayerPrefs.GetInt("currentMoneyLevel").ToString();

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        currentLevel = PlayerPrefs.GetFloat("currentLevel ");


        totalMoneyText.text = PlayerPrefs.GetFloat("totalMoney").ToString();
        //totalMoneyText.text = Mathf.Floor(playerData.GetMoney).ToString();

        levelText.text = (PlayerPrefs.GetFloat("currentLevel ") +1).ToString();

        if (SceneManager.GetActiveScene().name != "Level " + currentLevel)
        {


            SceneManager.LoadScene("Level " + currentLevel);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (circle.gameObject.transform.position.y <= -2.80f && levelll == 0 )
        {
            levelll++;
            playerData.GetSpeedAnimLevel = 0f;
            PlayerPrefs.SetFloat("speedAnimLevel", playerData.GetSpeedAnimLevel);
            isGameActive = false;
            currentLevel++;

            Debug.Log("Current Level" + currentLevel);
            PlayerPrefs.SetFloat("currentLevel ", currentLevel);
            levelText.text = (PlayerPrefs.GetFloat("currentLevel ") + 1).ToString();

            levelFinish = true;
            playerController.playerAnim.SetBool("Win", true);
            playerController.playerAnim.SetBool("Pushing", false);
            FeatureResetOnLevelNext();
            StartCoroutine(LoadNextLevelWait());
            playerController.sweatParticle.Stop();
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("prize"))
        {
            StartCoroutine(FinisMoneyAnim());
            other.gameObject.GetComponent<Animator>().SetBool("Opening", true);
        }
        if (other.gameObject.CompareTag("kumbara"))
        {
            StartCoroutine(FinisMoneyAnim());
            other.gameObject.GetComponent<MoneyBOx>().enabled = true;
        }
        if (other.gameObject.CompareTag("torba"))
        {
            StartCoroutine(FinisMoneyAnim());
            other.gameObject.GetComponent<Torba>().enabled = true;
        }
        if (other.gameObject.CompareTag("dolarbag"))
        {
            StartCoroutine(FinisMoneyAnim());
        }
    }
    private IEnumerator FinisMoneyAnim()
    {
        yield return new WaitForSeconds(1.4f);
        //DOTween.To(() => playerData.GetMoney, (yeniDeger) => playerData.GetMoney = yeniDeger, playerData.GetMoney+ playerData.GetFinishMoney, 3f);
        for (int i = 0; i < 10; i++)
        {

            


            Money.Current.coinAnim.SetTrigger("CoinT");
            finishMoneyAnimator.SetTrigger("finishMoney");
            moneyBag.transform.GetChild(i).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.4f);

            playerDatas.GetMoney += playerDatas.GetFinishMoney;
            playerData.GetMoney = Mathf.Floor(playerData.GetMoney);
            PlayerPrefs.SetFloat("totalMoney", playerDatas.GetMoney);
            totalMoneyText.text = PlayerPrefs.GetFloat("totalMoney").ToString();
            Debug.Log("para= " + PlayerPrefs.GetFloat("totalMoney"));
        }
    }
    IEnumerator LoadNextLevelWait()
    {
        yield return new WaitForSeconds(1.5f);
        playerController.particle[3].Play();
        
        yield return new WaitForSeconds(4.5f);
        
        levelVictoryMenu.SetActive(true);
        
    }

    public void FeatureResetOnLevelNext()
    {
        if (playerData.GetDecreaseOrStabil)
        {
            playerData.GetSpeed -= playerData.GetLevelDecreaseSpeed;
            PlayerPrefs.SetFloat("speed", playerData.GetSpeed);
            playerData.GetMaxStamina -= playerData.GetLevelDecreaseStamina;
            PlayerPrefs.SetFloat("maxStamina", playerData.GetMaxStamina);
        }
        else if (!playerData.GetDecreaseOrStabil)
        {
            playerData.GetSpeed = playerData.GetLevelMinSpeed;
            PlayerPrefs.SetFloat("speed", playerData.GetSpeed);
            playerData.GetMaxStamina = playerData.GetLevelMinStamina;
            PlayerPrefs.SetFloat("maxStamina", playerData.GetMaxStamina);
        }
        
    }

    public void IncrementSpeed()
    {
        speedLevelText.text = PlayerPrefs.GetInt("currentSpeedLevel").ToString();
        speedUpgradeFeeText.text =PlayerPrefs.GetFloat("levelUpFeeSpeedText").ToString();
        totalMoneyText.text = PlayerPrefs.GetFloat("totalMoney").ToString();
    }
    
    public void IncrementStamina()
    {
        staminaLevelText.text = PlayerPrefs.GetInt("currentStaminaLevel").ToString();
        staminaUpgradeFeeText.text = PlayerPrefs.GetFloat("levelUpFeeStaminaText").ToString();
        totalMoneyText.text = PlayerPrefs.GetFloat("totalMoney").ToString();
    }

    public void IncremenetMoney()
    {
        moneyLevelText.text = PlayerPrefs.GetInt("currentMoneyLevel").ToString();
        moneyUpgradeFeeText.text =PlayerPrefs.GetFloat("levelUpFeeMoneyText").ToString();
        totalMoneyText.text = PlayerPrefs.GetFloat("totalMoney").ToString();
    }

    public void PlayerPrefsDel()
    {
        PlayerPrefs.DeleteAll();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void TapToStart()
    {
        //playerController.gameActive = isGameActive;
        StartCoroutine(isgameWait());
        startMenu.SetActive(false);
        gameUI.SetActive(true);
        Debug.Log("tap");
        


    }

    IEnumerator isgameWait()
    {
        yield return new WaitForSeconds(0.2f);
        isGameActive = true;
    }
    

    public void LevelRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelVictory()
    {
        if (PlayerPrefs.GetFloat("currentLevel ")>=4)
        {
            currentLevel = 4;
            PlayerPrefs.SetFloat("currentLevel ", currentLevel);
        }
        SceneManager.LoadScene("Level " + PlayerPrefs.GetFloat("currentLevel "));
        playerController.sweatParticle.Stop();
    }
}
