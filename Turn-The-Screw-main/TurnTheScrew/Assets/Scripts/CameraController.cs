using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
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
            transform.DOMove(new Vector3(0, 6.5f,-5f), 1.7f);
        }
    }
}
