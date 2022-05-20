using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject[] QuestionPlatforms;
    [SerializeField]
    private GameObject[] leftPlatforms;
    [SerializeField]
    private GameObject[] rightPlatforms;
    [SerializeField]
    private GameObject monsterSpeechBubble;

    private GameObject[] fallenPlatforms;

    private Vector3[] fallenPlatformOgPOsitions;

    private GameObject chosenPlatform;

    public bool readyForNextQuestion = true;

    private bool MovingToChosenPlatform = false;

    public bool onQuestionTile;

    private bool makingRightChoice;


    private NavMeshAgent Monster;
    private int currentQuestionNum = 0;
    


    // Start is called before the first frame update
    void Start()
    {
        currentQuestionNum = 0;
        Monster = GetComponent<NavMeshAgent>();
        Monster.SetDestination(QuestionPlatforms[currentQuestionNum].transform.position);

        
    }

    
    //IEnumerator Sequence()
    //{

    //    yield return MoveToNextTile();

    //    Debug.Log("");

    //}

    //IEnumerator MoveToNextTile(Transform targetTile)
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        FaceTarget(new Vector3(-15,0,10));
        if (Monster.remainingDistance == 0 && !MovingToChosenPlatform)
        {
            onQuestionTile = true;
        }
        if (Monster.remainingDistance == 0 && MovingToChosenPlatform)
        {
            MovingToChosenPlatform = false;
            StartCoroutine(PreformResultAction(makingRightChoice)) ;
            Debug.Log("update calling preform");
            
        }
    }



    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
    }


    public void makeRightChoice(int platformToGo)
    {
        makingRightChoice = true;
        MoveToChosenPlatform(platformToGo);
        
        //move to current correct platform
        //play celebration animation 
        //move to next question platform

    }
    public void makeWrongChoice(int platformToGo)
    {
        makingRightChoice = false;
        MoveToChosenPlatform(platformToGo);
       
        //move to current wrong platform
        //play falling in animation sad
        //jump out of water to next question platform
    }

    IEnumerator PreformResultAction(bool correct)
    {
        if (correct)
        {
            Debug.Log("doing correct");
            yield return DoCelebration();
            GoToNextQuestion();
        }
        else
        {
            Debug.Log("doing incorrect");
            //do fall in water and jump out
            yield return FallInWater();
            GetOutOfWater();
            GoToNextQuestion();
        }
    }

    public bool reachedEnd()
    {
        if (QuestionPlatforms.Length <= currentQuestionNum+1 && Monster.remainingDistance == 0)
        {
            return true;
        }
        else return false;
    }
    public void GetOutOfWater()
    {
        StartCoroutine(revertFellInWater());
    }
    IEnumerator revertFellInWater()
    {
        Monster.gameObject.transform.GetChild(0).GetComponent<ConstantForce>().enabled = true;
        yield return new WaitForSeconds(1.2f);
        Monster.gameObject.transform.GetChild(0).GetComponent<ConstantForce>().enabled = false;
        Monster.gameObject.transform.GetChild(0).GetComponent<Rigidbody>().velocity = Vector3.zero;

    }
    IEnumerator DoCelebration()
    {
        //do celebration animation
        monsterSpeechBubble.transform.GetChild(0).GetComponent<TMP_Text>().text = "Correct, YAY!";
        monsterSpeechBubble.SetActive(true);
        yield return new WaitForSeconds(1);
    }
    IEnumerator FallInWater()
    {
        //fall in water
        monsterSpeechBubble.transform.GetChild(0).GetComponent<TMP_Text>().text = "Oh no :(";
        monsterSpeechBubble.SetActive(true);
        Monster.gameObject.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
        chosenPlatform.gameObject.GetComponent<Rigidbody>().useGravity = true;

        Debug.Log("falling");
        
        
        yield return new WaitForSeconds(1.2f);
        Monster.gameObject.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
        Monster.gameObject.transform.GetChild(0).GetComponent<Rigidbody>().velocity = Vector3.zero;
        Debug.Log("GravityReset");
    }
    private void MoveToChosenPlatform(int p2g)
    {
        
        onQuestionTile = false;
        if (p2g == 1)
        {
            Debug.Log("goleft");
            //go left
            Monster.SetDestination(leftPlatforms[currentQuestionNum].transform.position);
            chosenPlatform = leftPlatforms[currentQuestionNum];
        }
        else
        {
            //go right
            Debug.Log("goright");
            Monster.SetDestination(rightPlatforms[currentQuestionNum].transform.position);
            chosenPlatform = rightPlatforms[currentQuestionNum];
        }
        MovingToChosenPlatform = true;
        
    }
    private void GoToNextQuestion()
    {
        currentQuestionNum++;
        monsterSpeechBubble.SetActive(false);
        Monster.SetDestination(QuestionPlatforms[currentQuestionNum].transform.position);
        readyForNextQuestion = true;
        MovingToChosenPlatform = false;
        
    }
}
