using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skinned : MonoBehaviour
{
    SkinnedMeshRenderer skin;
    // Start is called before the first frame update
    void Start()
    {
        skin = gameObject.GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //mesh.SetBlendShapeWeight(1, Mathf.PingPong(200f*Time.time, 100f));
        //skin.SetBlendShapeWeight(1, Mathf.Lerp(0, 100f, Time.time / 2));
    }
}
