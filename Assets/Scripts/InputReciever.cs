using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReciever : MonoBehaviour
{
    [SerializeField]
    private RiverGameManager MonsterManager;
    [SerializeField]
    private StartingArea1 Area1Manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (Area1Manager.gameObject.activeSelf)
            {
                Area1Manager.ProceedFromArea1();
            }
            else
            {
                MonsterManager.ChooseOption(0);
            }
            
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MonsterManager.ChooseOption(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MonsterManager.ChooseOption(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            MonsterManager.ChooseOption(3);
        }
    }
}
