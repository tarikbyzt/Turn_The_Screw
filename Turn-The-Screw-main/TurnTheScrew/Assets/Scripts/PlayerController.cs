using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    //MERT
    
    
    [SerializeField] PlayerScriptable playerData = null;
    public  List<ParticleSystem> particle;
    private LevelController levelControler;
    public Animator playerAnim;
    [SerializeField] GameObject playerDeath;
    [SerializeField] GameObject playerDeathPartc;
    [SerializeField] GameObject finishMenu;

    [Header("Speed")]

    [SerializeField] private int currentSpeedLevel = 0;   // Speed upgradesi level degeri.
    private int currentLevelText;                          //Textler bir level onde gitmesi gerektigi icin ayr? bir deger.
    public float levelUpFeeSpeed = 0;          //Speed upgradesi upgrade icin gereken money degeri.
    private float levelUpFeeSpeedText;
    [SerializeField] private float levelIncreaseFeeRateSpeed = 1.45f;     //Speed upgradesi speed artis orani.

    [Header("Stamina")]
    
    [SerializeField] private int currentStaminaLevel = 0;   // Stamina upgradesi level degeri.
    private int currentStaminaText;
    [SerializeField] private float levelUpFeeStamina = 0;          //Stamina upgradesi upgrade icin gereken money degeri.
    private float levelUpFeeStaminaText;
    [SerializeField] private float levelIncreaseFeeRateStamina = 1.45f;     //Stamina upgradesi stamina artis orani.


    [Header("Money")]

    [SerializeField] private float levelUpFeeMoney = 0;          //Money upgradesi upgrade icin gereken money degeri.
    private float levelUpFeeMoneyText;
    private int currentMoneyText;
    [SerializeField] private int currentMoneyLevel = 0;   // Money upgradesi level degeri.
    [SerializeField] private float levelIncreaseFeeRateMoney = 1.45f;     //Money upgradesi money artis orani.


    [SerializeField] private Transform screw;
    private float screwSpeed;
    private float hot;
    private float sweat;
    public bool gameActive;
    



    [Header("UnitOfMoney")]
    [SerializeField] private GameObject coll;
    [SerializeField] private GameObject colliders;
    public List<GameObject> colls;

    [Header("Particle")]
    [SerializeField] private Material shaderMat;
    [SerializeField] float hotValue;
    public ParticleSystem sweatParticle;
    public float particleRate = 0f;
    [SerializeField] private SkinnedMeshRenderer mesh;




    //[SerializeField] private TextMeshProUGUI pickMoney;
    //[SerializeField] private Animator pickMoneyAnimator;
    float startPush;
    float startSpeed;


    private void Awake()
    {
        PlayerPrefsSaveAndCheck();
        levelControler = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
        for (int i = 0; i < 30; i++)
        {
            GameObject newColl = Instantiate(coll, new Vector3(0, 1.75f, 0), Quaternion.identity);
            newColl.transform.parent = colliders.transform;
            colls.Add(newColl);

            if (i >= 1)
            {
                colls[i].transform.eulerAngles = new Vector3(0, colls[i - 1].transform.eulerAngles.y + 12, 0);
            }
            
            shaderMat.SetFloat("_TiredRange", 0);
            hotValue = shaderMat.GetFloat("_TiredRange");


            //y scale up to make it look like a circle.
            //colls[i].transform.localScale = new Vector3(colls[i].transform.localScale.x, colls[i].transform.localScale.y * 5, colls[i].transform.localScale.z * 10);
        }
    }
    

    void Start()
    {
        startSpeed = playerData.GetSpeed;
        playerData.GetResting = playerData.GetMaxStamina;
        playerData.GetStamina = playerData.GetResting;

        sweat = playerData.GetStamina * playerData.terlemeYuzde / 100;
        //Debug.Log("terlemeye baþlama seviyesi" + sweat);
        hot = playerData.GetStamina * playerData.kizarmaYuzde / 100;
        sweatParticle.Play();
    }


    
    void Update()
    {
        gameActive = levelControler.isGameActive;
        screwSpeed = playerData.GetSpeed * playerData.GetCircleDownRate * Time.deltaTime;
        float hotDecreasee = hot / (playerData.GetStaminaDecrease);
        float hotDecrease = playerData.GetStamina / playerData.GetStaminaDecrease;

        var ems = sweatParticle.emission;
        if (playerData.GetStamina <= sweat)
        {
            ems.rateOverTime = particleRate;
            //Debug.Log("zort");
        }else
        {
            particleRate = 0;
            ems.rateOverTime = 0;
        }

        
        if (playerData.GetStamina <=0)
        {
            levelControler.isGameActive = false;
            playerAnim.SetBool("Pushing", false);
            playerDeath.SetActive(false);
            playerDeathPartc.SetActive(true);
            StartCoroutine(RestartGame());
            
        }
        if (Input.touchCount > 0 && gameActive)
        {

            if (startPush <= 1.0f)
            {
                startPush += 2 * Time.deltaTime;
                if (startSpeed >= playerData.GetSpeed)
                {
                    playerData.GetSpeed += Time.deltaTime * 400f;
                }
            }
            StartCoroutine(IdleWait());
            playerAnim.SetBool("Pushing", true);
            if (playerData.GetStamina <= hot)
            {
                particleRate += 9f * Time.deltaTime;
                
                DOTween.To(DegiskeninDegeriniAl, DegiskeninDegeriniDegistir, 0.6f, hotDecrease);
                //Debug.Log("Kizarma");
            }
            else
            {
                particleRate += 2f * Time.deltaTime;
            }

        }


        if (Input.touchCount == 0 && gameActive)
        {
            if (particleRate >= 0)
            {
                particleRate -= 6f * Time.deltaTime;
            }
            playerData.GetSpeed = 0f;
            startPush = 0f;
            playerData.GetStamina += playerData.GetRestingRate*Time.deltaTime;
            playerAnim.SetBool("Pushing", false);
            DOTween.To(DegiskeninDegeriniAl, DegiskeninDegeriniDegistir, 0, hotDecreasee);
            //gameActive = true;
        }

        

        shaderMat.SetFloat("_TiredRange", hotValue);


        if (playerData.GetStamina <= hot)
        {
            StartCoroutine(MeshBreath());

        }

    }
    private void PlayerPrefsSaveAndCheck()
    {
        playerData.GetSpeedAnimLevel = PlayerPrefs.GetFloat("speedAnimLevel");


        playerData.GetSpeed = PlayerPrefs.GetFloat("speed");
        if (PlayerPrefs.GetFloat("speed") <= 20)
        {
            playerData.GetSpeed = playerData.GetLevelMinSpeed;
            PlayerPrefs.SetFloat("speed", playerData.GetSpeed);
        }


        currentSpeedLevel = PlayerPrefs.GetInt("currentSpeedLevel");

        levelUpFeeSpeedText = PlayerPrefs.GetFloat("levelUpFeeSpeedText");
        if (PlayerPrefs.GetFloat("levelUpFeeSpeedText") <= 0)
        {
            levelUpFeeSpeedText += 15f;
            PlayerPrefs.SetFloat("levelUpFeeSpeedText", levelUpFeeSpeedText);
        }

        levelUpFeeSpeed = PlayerPrefs.GetFloat("levelUpFee");
        if (PlayerPrefs.GetFloat("levelUpFee") == 0)
        {
            levelUpFeeSpeed += 15f;
            PlayerPrefs.SetFloat("levelUpFee", levelUpFeeSpeed);
        }

        levelIncreaseFeeRateSpeed = PlayerPrefs.GetFloat("speedFeeRate");
        
        if (PlayerPrefs.GetFloat("speedFeeRate") == 0)
        {
            levelIncreaseFeeRateSpeed = 1.85f;
            PlayerPrefs.SetFloat("speedFeeRate", levelIncreaseFeeRateSpeed);
        }
        currentLevelText = PlayerPrefs.GetInt("currentLevelText");


        playerData.GetMaxStamina = PlayerPrefs.GetFloat("maxStamina");
        if (playerData.GetMaxStamina == 0)
        {
            playerData.GetMaxStamina = playerData.GetLevelMinStamina;
            PlayerPrefs.SetFloat("maxStamina", playerData.GetMaxStamina);
        }


        levelUpFeeStamina = PlayerPrefs.GetFloat("levelUpFeeStamina");
        if (PlayerPrefs.GetFloat("levelUpFeeStamina") == 0)
        {
            levelUpFeeStamina += 15f;
            PlayerPrefs.SetFloat("levelUpFeeStamina", levelUpFeeStamina);
        }

        levelUpFeeStaminaText = PlayerPrefs.GetFloat("levelUpFeeStaminaText");
        if (PlayerPrefs.GetFloat("levelUpFeeStaminaText") == 0)
        {
            levelUpFeeStaminaText += 15f;
            PlayerPrefs.SetFloat("levelUpFeeStaminaText", levelUpFeeStaminaText);
        }

        levelIncreaseFeeRateStamina = PlayerPrefs.GetFloat("staminaFeeRate");
        if (PlayerPrefs.GetFloat("staminaFeeRate") == 0)
        {
            levelIncreaseFeeRateStamina = 1.85f;
            PlayerPrefs.SetFloat("staminaFeeRate", levelIncreaseFeeRateStamina);
        }

        currentStaminaText = PlayerPrefs.GetInt("currentStaminaText");

        currentStaminaLevel = PlayerPrefs.GetInt("currentStaminaLevel");


        levelUpFeeMoney = PlayerPrefs.GetFloat("levelUpFeeMoney");
        if (PlayerPrefs.GetFloat("levelUpFeeMoney") == 0)
        {
            levelUpFeeMoney += 15f;
            PlayerPrefs.SetFloat("levelUpFeeMoney", levelUpFeeMoney);
        }

        levelUpFeeMoneyText = PlayerPrefs.GetFloat("levelUpFeeMoneyText");
        if (PlayerPrefs.GetFloat("levelUpFeeMoneyText") == 0)
        {
            levelUpFeeMoneyText += 15f;
            PlayerPrefs.SetFloat("levelUpFeeMoneyText", levelUpFeeMoneyText);
        }

        levelIncreaseFeeRateMoney = PlayerPrefs.GetFloat("moneyFeeRate");
        if (PlayerPrefs.GetFloat("moneyFeeRate") == 0)
        {
            levelIncreaseFeeRateMoney = 1.85f;
            PlayerPrefs.SetFloat("moneyFeeRate", levelIncreaseFeeRateMoney);
        }

        currentMoneyText = PlayerPrefs.GetInt("currentMoneyText");
        currentMoneyLevel = PlayerPrefs.GetInt("currentMoneyLevel");
        
        playerData.GetUnitOfMoney = PlayerPrefs.GetFloat("unitOfMoney");
        if (PlayerPrefs.GetFloat("unitOfMoney") == 0)
        {
            playerData.GetUnitOfMoney = 1;
            PlayerPrefs.SetFloat("unitOfMoney", playerData.GetUnitOfMoney);
        }


        playerData.GetMoney = PlayerPrefs.GetFloat("totalMoney");
        if (PlayerPrefs.GetFloat("totalMoney")==0)
        {
            playerData.GetMoney = 3000;
            PlayerPrefs.SetFloat("totalMoney", playerData.GetMoney);
        }

        playerData.GetMoney = Mathf.Floor(playerData.GetMoney);
        PlayerPrefs.SetFloat("totalMoney", playerData.GetMoney);

       
    }



    public void IncrementSpeed()
    {
        if (playerData.GetMoney > levelUpFeeSpeedText)
        {
            particle[1].Play();
            currentLevelText += 1;
            PlayerPrefs.SetInt("currentLevelText", currentLevelText);

            

            if (currentSpeedLevel < 5 && currentSpeedLevel > 0)
            {
                levelUpFeeSpeed *= levelIncreaseFeeRateSpeed;
                levelUpFeeSpeed = Mathf.Ceil(levelUpFeeSpeed);
                PlayerPrefs.SetFloat("levelUpFee", levelUpFeeSpeed);
                Debug.Log("levelUpFeeSpeed: " + PlayerPrefs.GetFloat("levelUpFee"));
                levelIncreaseFeeRateSpeed -= 0.075f;
                PlayerPrefs.SetFloat("speedFeeRate", levelIncreaseFeeRateSpeed);

            }
            else if (currentSpeedLevel < 15 && currentSpeedLevel >= 5)
            {
                
                levelIncreaseFeeRateSpeed = 1.2f;       //Asagidaki else if içerisindeki //burada  yazan float degeri degistirin.
                PlayerPrefs.SetFloat("speedFeeRate", levelIncreaseFeeRateSpeed);
                levelUpFeeSpeed *= levelIncreaseFeeRateSpeed;
                levelUpFeeSpeed = Mathf.Ceil(levelUpFeeSpeed);
                PlayerPrefs.SetFloat("levelUpFee", levelUpFeeSpeed);
            }
            else if (currentSpeedLevel <= 70 && currentSpeedLevel >= 15)
            {
                if (currentSpeedLevel <= 30 && currentSpeedLevel >= 15)
                {
                    playerAnim.speed = 3.5f;
                }
                         
                levelIncreaseFeeRateSpeed = 1.05f;       //Asagidaki else if içerisindeki //burada  yazan float degeri degistirin.
                PlayerPrefs.SetFloat("speedFeeRate", levelIncreaseFeeRateSpeed);
                levelUpFeeSpeed *= levelIncreaseFeeRateSpeed;
                levelUpFeeSpeed = Mathf.Ceil(levelUpFeeSpeed);
                PlayerPrefs.SetFloat("levelUpFee", levelUpFeeSpeed);
            }

            if (currentLevelText < 5 && currentLevelText > 0)
            {
                float levelIncrease = levelIncreaseFeeRateSpeed;

                float carpim = levelUpFeeSpeed * (levelIncrease);
                levelUpFeeSpeedText = Mathf.Ceil(carpim);
                PlayerPrefs.SetFloat("levelUpFeeSpeedText", levelUpFeeSpeedText);

            }
            if (currentLevelText < 15 && currentLevelText >= 5)
            {

                float carpim = levelUpFeeSpeed * (1.2f);        //burada
                levelUpFeeSpeedText = Mathf.Ceil(carpim);
                PlayerPrefs.SetFloat("levelUpFeeSpeedText", levelUpFeeSpeedText);
            }
            if (currentLevelText <= 70 && currentLevelText >= 15)
            {
                float carpim = levelUpFeeSpeed * (1.05f);        //burada
                levelUpFeeSpeedText = Mathf.Ceil(carpim);
                PlayerPrefs.SetFloat("levelUpFeeSpeedText", levelUpFeeSpeedText);
            }

            //if (playerData.GetMoney > levelUpFeeMoney)
            //{

            //}


            playerData.GetMoney -= levelUpFeeSpeed;
            playerData.GetMoney = Mathf.Ceil(playerData.GetMoney);
            PlayerPrefs.SetFloat("totalMoney", playerData.GetMoney);

            playerData.GetSpeed += playerData.GetIncreaseToSpeed;
            PlayerPrefs.SetFloat("speed", playerData.GetSpeed);

            currentSpeedLevel += 1;
            PlayerPrefs.SetInt("currentSpeedLevel", currentSpeedLevel);

            playerData.GetSpeedAnimLevel++;
            PlayerPrefs.SetFloat("speedAnimLevel", playerData.GetSpeedAnimLevel);
            startSpeed = playerData.GetSpeed;
        }
    }

    public void IncrementStamina()
    {
        if (playerData.GetMoney > levelUpFeeStaminaText)
        {
            particle[0].Play();
            currentStaminaText += 1;
            PlayerPrefs.SetInt("currentStaminaText", currentStaminaText);
            if (currentStaminaLevel < 5 && currentStaminaLevel > 0)
            {
                levelUpFeeStamina *= levelIncreaseFeeRateStamina;
                levelUpFeeStamina = Mathf.Ceil(levelUpFeeStamina);
                
                PlayerPrefs.SetFloat("levelUpFeeStamina", levelUpFeeStamina);
                Debug.Log("levelUpFeeStamina : " + PlayerPrefs.GetFloat("levelUpFeeStamina"));
                levelIncreaseFeeRateStamina -= 0.075f;
                PlayerPrefs.SetFloat("staminaFeeRate", levelIncreaseFeeRateStamina);
            }
            else if (currentStaminaLevel < 15 && currentStaminaLevel >= 5)
            {
                levelIncreaseFeeRateStamina = 1.2f;       //Asagidaki else if içerisindeki //burada  yazan float degeri degistirin.
                PlayerPrefs.SetFloat("staminaFeeRate", levelIncreaseFeeRateStamina);
                levelUpFeeStamina *= levelIncreaseFeeRateStamina;
                levelUpFeeStamina = Mathf.Ceil(levelUpFeeStamina);
                PlayerPrefs.SetFloat("levelUpFeeStamina", levelUpFeeStamina);
            }
            else if (currentStaminaLevel <= 70 && currentStaminaLevel >= 15)
            {
                levelIncreaseFeeRateStamina = 1.05f;       //Asagidaki else if içerisindeki //burada  yazan float degeri degistirin.
                PlayerPrefs.SetFloat("staminaFeeRate", levelIncreaseFeeRateStamina);
                levelUpFeeStamina *= levelIncreaseFeeRateStamina;
                levelUpFeeStamina = Mathf.Ceil(levelUpFeeStamina);
                PlayerPrefs.SetFloat("levelUpFeeStamina", levelUpFeeStamina);
            }
            
            

            if (currentStaminaText < 5 && currentStaminaText > 0)
            {
                float levelIncrease = levelIncreaseFeeRateStamina;
                float carpim = levelUpFeeStamina * (levelIncrease);
                levelUpFeeStaminaText = Mathf.Ceil(carpim);
                PlayerPrefs.SetFloat("levelUpFeeStaminaText", levelUpFeeStaminaText);
            }
            if (currentStaminaText < 15 && currentStaminaText >= 5)
            {
                float carpim = levelUpFeeStamina * (1.2f);        //burada
                levelUpFeeStaminaText = Mathf.Ceil(carpim);
                PlayerPrefs.SetFloat("levelUpFeeStaminaText", levelUpFeeStaminaText);
            }
            
            if (currentStaminaText <= 70 && currentStaminaText >= 15)
            {
                float carpim = levelUpFeeStamina * (1.05f);        //burada
                levelUpFeeStaminaText = Mathf.Ceil(carpim);
                PlayerPrefs.SetFloat("levelUpFeeStaminaText", levelUpFeeStaminaText);
            }


            playerData.GetMoney -= levelUpFeeStamina;
            playerData.GetMoney = Mathf.Ceil(playerData.GetMoney);
            PlayerPrefs.SetFloat("totalMoney", playerData.GetMoney);

            playerData.GetMaxStamina += playerData.GetIncreaseToStamina;
            PlayerPrefs.SetFloat("maxStamina", playerData.GetMaxStamina);

            currentStaminaLevel += 1;
            PlayerPrefs.SetInt("currentStaminaLevel", currentStaminaLevel);


            playerData.GetResting = playerData.GetMaxStamina;
            playerData.GetStamina = playerData.GetResting;
            sweat = playerData.GetStamina * playerData.terlemeYuzde / 100;
            hot = playerData.GetStamina * playerData.kizarmaYuzde / 100;
        }

    }



    public void IncremenetMoney()
    {
        if (playerData.GetMoney > levelUpFeeMoneyText)
        {
            particle[2].Play();
            currentMoneyText += 1;
            
            PlayerPrefs.SetInt("currentMoneyText", currentMoneyText);

            if (currentMoneyLevel == 0 )
            {
                playerData.GetUnitOfMoney += 0.1f;
                playerData.GetUnitOfMoney = Mathf.Round(playerData.GetUnitOfMoney * 10) / 10;
                PlayerPrefs.SetFloat("unitOfMoney", playerData.GetUnitOfMoney);
            }
            if (currentMoneyLevel < 5 && currentMoneyLevel > 0)
            {
                
                levelUpFeeMoney *= levelIncreaseFeeRateMoney;
                levelUpFeeMoney = Mathf.Ceil(levelUpFeeMoney);
                PlayerPrefs.SetFloat("levelUpFeeMoney", levelUpFeeMoney);
                Debug.Log("levelUpFeeMoney: " + PlayerPrefs.GetFloat("levelUpFeeMoney"));
                levelIncreaseFeeRateMoney -= 0.075f;
                PlayerPrefs.SetFloat("moneyFeeRate", levelIncreaseFeeRateMoney);

                
                playerData.GetUnitOfMoney += 0.1f;
                playerData.GetUnitOfMoney = Mathf.Round(playerData.GetUnitOfMoney * 10) / 10;
                PlayerPrefs.SetFloat("unitOfMoney", playerData.GetUnitOfMoney);
            }
            else if (currentMoneyLevel < 15 && currentMoneyLevel >= 5)
            {
                if (currentMoneyLevel <=11)
                {
                    playerData.GetUnitOfMoney += 0.3f;
                    playerData.GetUnitOfMoney = Mathf.Round(playerData.GetUnitOfMoney * 10) / 10;
                    PlayerPrefs.SetFloat("unitOfMoney", playerData.GetUnitOfMoney);
                }else
                {
                    playerData.GetUnitOfMoney += 0.5f;
                    playerData.GetUnitOfMoney = Mathf.Round(playerData.GetUnitOfMoney * 10) / 10;
                    PlayerPrefs.SetFloat("unitOfMoney", playerData.GetUnitOfMoney);
                }
                levelIncreaseFeeRateMoney = 1.2f;       //Asagidaki else if içerisindeki //burada  yazan float degeri degistirin.
                PlayerPrefs.SetFloat("moneyFeeRate", levelIncreaseFeeRateMoney);
                levelUpFeeMoney *= levelIncreaseFeeRateMoney;
                levelUpFeeMoney = Mathf.Ceil(levelUpFeeMoney);
                PlayerPrefs.SetFloat("levelUpFeeMoney", levelUpFeeMoney);
            }
            else if (currentMoneyLevel <= 70 && currentMoneyLevel >= 15)
            {
                if (currentMoneyLevel <=17 )
                {
                    playerData.GetUnitOfMoney += 0.5f;
                    playerData.GetUnitOfMoney = Mathf.Round(playerData.GetUnitOfMoney * 10) / 10;
                    PlayerPrefs.SetFloat("unitOfMoney", playerData.GetUnitOfMoney);
                }
                else if(currentMoneyLevel <=25 && currentMoneyLevel >= 18)
                {
                    playerData.GetUnitOfMoney += 0.7f;
                    playerData.GetUnitOfMoney = Mathf.Round(playerData.GetUnitOfMoney * 10) / 10;
                    PlayerPrefs.SetFloat("unitOfMoney", playerData.GetUnitOfMoney);
                }
                else if(currentMoneyLevel<=34 && currentMoneyLevel >= 26)
                {
                    playerData.GetUnitOfMoney += 0.8f;
                    playerData.GetUnitOfMoney = Mathf.Round(playerData.GetUnitOfMoney * 10) / 10;
                    PlayerPrefs.SetFloat("unitOfMoney", playerData.GetUnitOfMoney);
                }else
                {
                    playerData.GetUnitOfMoney += 0.2f;
                    playerData.GetUnitOfMoney = Mathf.Round(playerData.GetUnitOfMoney * 10) / 10;
                    PlayerPrefs.SetFloat("unitOfMoney", playerData.GetUnitOfMoney);
                }


                levelIncreaseFeeRateMoney = 1.05f;       //Asagidaki else if içerisindeki //burada  yazan float degeri degistirin.
                PlayerPrefs.SetFloat("moneyFeeRate", levelIncreaseFeeRateMoney);
                levelUpFeeMoney *= levelIncreaseFeeRateMoney;
                levelUpFeeMoney = Mathf.Ceil(levelUpFeeMoney);
                PlayerPrefs.SetFloat("levelUpFeeMoney", levelUpFeeMoney);
            }


            if (currentMoneyText < 5 && currentMoneyText >0)
            {
                float levelIncrease = levelIncreaseFeeRateMoney;
                float carpim = levelUpFeeMoney * (levelIncrease);
                levelUpFeeMoneyText = Mathf.Ceil(carpim);
                PlayerPrefs.SetFloat("levelUpFeeMoneyText", levelUpFeeMoneyText);

            }
            if (currentMoneyText < 15 && currentMoneyText >= 5)
            {
                float carpim = levelUpFeeMoney * (1.2f);        //burada
                levelUpFeeMoneyText = Mathf.Ceil(carpim);
                PlayerPrefs.SetFloat("levelUpFeeMoneyText", levelUpFeeMoneyText);

            }
            if (currentMoneyText <= 70 && currentMoneyText >= 15)
            {
                float carpim = levelUpFeeMoney * (1.05f);        //burada
                levelUpFeeMoneyText = Mathf.Ceil(carpim);
                PlayerPrefs.SetFloat("levelUpFeeMoneyText", levelUpFeeMoneyText);
                
            }


            playerData.GetMoney -= levelUpFeeMoney;
            playerData.GetMoney = Mathf.Ceil(playerData.GetMoney);
            PlayerPrefs.SetFloat("totalMoney", playerData.GetMoney);

            currentMoneyLevel += 1;
            PlayerPrefs.SetInt("currentMoneyLevel", currentMoneyLevel);




        }
    }


    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    IEnumerator IdleWait()
    {
        yield return new WaitForSeconds(0.1f);
        transform.Rotate(0, playerData.GetSpeed * Time.deltaTime, 0);
        if (playerData.GetSpeed >= startSpeed)
        {
            playerData.GetStamina -= playerData.GetStaminaDecrease * Time.deltaTime;
            playerData.GetResting -= playerData.GetRestingDecrease * Time.deltaTime;
        }
        else
        {
            playerData.GetStamina -= (playerData.GetStaminaDecrease * Time.deltaTime) / 3f;
            playerData.GetResting -= (playerData.GetRestingDecrease * Time.deltaTime) / 3f;
        }
        screw.DOLocalMoveY(screw.localPosition.y - screwSpeed, 0.1f);
    }

    public void TapToStart()
    {
        if (playerData.GetSpeedAnimLevel <= 1 && playerData.GetSpeedAnimLevel >= 0)
        {
            playerAnim.speed = 1f;
        }
        else if (playerData.GetSpeedAnimLevel <= 3 && playerData.GetSpeedAnimLevel >= 2)
        {
            playerAnim.speed = 1.4f;
        }
        else if (playerData.GetSpeedAnimLevel <= 5 && playerData.GetSpeedAnimLevel >= 4)
        {
            playerAnim.speed = 1.8f;
        }
        else if (playerData.GetSpeedAnimLevel <= 8 && playerData.GetSpeedAnimLevel >= 6)
        {
            playerAnim.speed = 2.2f;
        }
        else if (playerData.GetSpeedAnimLevel <= 11 && playerData.GetSpeedAnimLevel >= 9)
        {
            playerAnim.speed = 2.8f;
        }
        else if (playerData.GetSpeedAnimLevel <= 15 && playerData.GetSpeedAnimLevel >= 12)
        {
            playerAnim.speed = 3f;
        }
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1f);
        finishMenu.SetActive(true);
    }

    public IEnumerator MeshBreath()
    {

        mesh.SetBlendShapeWeight(1, Mathf.PingPong(200f * Time.time, 100f));

        yield return new WaitForSeconds(7f);
        
        
    }

    float DegiskeninDegeriniAl()
    {
        return hotValue;
    }
    void DegiskeninDegeriniDegistir(float yeniDeger)
    {
        hotValue = yeniDeger;
    }
    
    
}
