using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    [SerializeField] Transform lookCamera;
    private Transform localTrans;
    void Start()
    {
        localTrans =GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lookCamera)
        {
            localTrans.LookAt(2 * localTrans.position - lookCamera.position);
        }
    }
}
