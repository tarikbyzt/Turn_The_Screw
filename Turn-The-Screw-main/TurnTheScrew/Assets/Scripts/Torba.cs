using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Torba : MonoBehaviour
{
    [SerializeField] private List<Animator> anim;
    private LevelController levelController;
    [SerializeField] SkinnedMeshRenderer skin;
    private float blendshapeValue;
    void Start()
    {
        anim[0].SetBool("Finish", true);
        StartCoroutine(WaitAnim());
        
        //levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {

        //skin.SetBlendShapeWeight(0, Mathf.Lerp(0, 100f, Time.time / 2));
        //if (levelController.levelFinish)
        //{

        //}
    }

    IEnumerator WaitAnim()
    {
        yield return new WaitForSeconds(1);
        anim[1].SetBool("Finish", true);
        yield return new WaitForSeconds(2);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);

    }
}
