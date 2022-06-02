using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReciever : MonoBehaviour
{
    [SerializeField]
    private RiverGameManager MonsterManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MonsterManager.ChooseOption(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MonsterManager.ChooseOption(1);
        }
    }
}
