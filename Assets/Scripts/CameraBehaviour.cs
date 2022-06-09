using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject monster;
    public float xMin;
    public float xMax;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.Clamp(monster.transform.position.x, xMin, xMax);
        gameObject.transform.position = new Vector3(x + 4, gameObject.transform.position.y, monster.transform.position.z);
    }
}