using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LookAtCamera : MonoBehaviour
{
    private LevelController levelController;
    void Start()
    {
        levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelController.levelFinish)
        {
            transform.DORotate(new Vector3(0, 0, 0), 1f);
        }
    }
}
