using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.Events;

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

    private GameObject chosenPlatform;

    public UnityEvent reachedQuestiontile = new UnityEvent();


    private bool makingRightChoice;


    private NavMeshAgent Monster;

    public int currentQuestionNum;

    private int counter =0;
    private bool waitingOnTile = false;

    


    // Start is called before the first frame update
    void Start()
    {
        currentQuestionNum = 0;
        Monster = GetComponent<NavMeshAgent>();
        Monster.SetDestination(QuestionPlatforms[currentQuestionNum].transform.position);
    }

    void Update()
    {
        
    }

    public void updateRiverGameMethods()
    {
        FaceTarget(new Vector3(-15, 0, 10));
        Debug.Log(currentQuestionNum);
        if (!waitingOnTile && Monster.remainingDistance == 0 && counter <= QuestionPlatforms.Length + 2)
        {
            waitingOnTile = true;
            counter++;
            if (counter % 2 == 0)
            {
                //on answer tile 
                StartCoroutine(PreformResultAction(makingRightChoice));
                Debug.Log("update calling preform");
            }
            else
            {
                //on question tile 
                reachedQuestiontile.Invoke();
            }

        }
    }




    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
    }


    public void makeRightChoice(Choice platformToGo)
    {
        makingRightChoice = true;
        MoveToChosenPlatform(platformToGo);
        
        //move to current correct platform
        //play celebration animation 
        //move to next question platform

    }
    public void makeWrongChoice(Choice platformToGo)
    {
        makingRightChoice = false;
        MoveToChosenPlatform(platformToGo);
       
        //move to current wrong platform
        //play falling in animation sad
        //jump out of water to next question platform
    }

    private void MoveToChosenPlatform(Choice chosenSide)
    {

        
        if (chosenSide == Choice.LEFT)
        {
            Debug.Log("goleft");
            //go left
            Monster.SetDestination(leftPlatforms[currentQuestionNum-1].transform.position);
            chosenPlatform = leftPlatforms[currentQuestionNum-1];
        }
        else
        {
            //go right
            Debug.Log("goright");
            Monster.SetDestination(rightPlatforms[currentQuestionNum-1].transform.position);
            chosenPlatform = rightPlatforms[currentQuestionNum-1];
        }
        waitingOnTile = false;

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
    IEnumerator FallInWater()
    {
        //fall in water is manipulation of the 3D model as we do not currently have animations but once 
        //those are finished this model manipulation will be replaced by them
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
    
    
    private void GoToNextQuestion()
    {
        monsterSpeechBubble.SetActive(false);
        Monster.SetDestination(QuestionPlatforms[currentQuestionNum].transform.position);
        waitingOnTile = false;
        
    }
    public bool reachedEnd()
    {
        if (QuestionPlatforms.Length <= currentQuestionNum+1 && Monster.remainingDistance == 0&& !waitingOnTile)
        {
            Debug.Log("reached end true ");
            return true;
        }
        else return false;
    }
}
