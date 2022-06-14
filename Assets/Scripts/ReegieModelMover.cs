using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReegieModelMover : MonoBehaviour
{
    private bool floatingUp;

    public int upSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(floatingUp && transform.position.y< 0)
        {
            transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.zero;
            floatingUp = false;
        }
        
    }

    public void FloatUp() {
        floatingUp = true;
    
    }
}
