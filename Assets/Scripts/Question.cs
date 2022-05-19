using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewQuestion",menuName = "question")]
public class Question : ScriptableObject
{


    public string questionText;
    public string CorrectAnswer;
    public string WrongAnswer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
