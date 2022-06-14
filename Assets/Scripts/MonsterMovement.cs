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
    [SerializeField] 
    private List<AudioSource> CorrectAnswerVoices = new List<AudioSource>();
    [SerializeField] 
    private List<AudioSource> WrongAnswerVoices = new List<AudioSource>();
    [SerializeField]
    ReegieModelMover floater;

    private GameObject chosenPlatform;
    private Animator ReegieAnimator;
    private NavMeshAgent Monster;
    private Vector3 currentTarget;
    private int counter = 0;
    private bool makingRightChoice;
    private bool waiting = false;
    private bool Reverting = false;
    

    public int currentQuestionNum;
    public UnityEvent reachedQuestiontile = new UnityEvent();
    public UnityEvent reachedDestination = new UnityEvent();
    




    // Start is called before the first frame update
    void Start()
    {
        currentQuestionNum = 0;
        ReegieAnimator = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        if (Monster.remainingDistance > 0)
        {
            ReegieAnimator.SetBool("Moving", true);
        }
        else
        {
            ReegieAnimator.SetBool("Moving", false);
        }
        if (Monster.isOnOffMeshLink)
        {
            ReegieAnimator.SetBool("Jumping", true);
        }
        else
        {
            ReegieAnimator.SetBool("Jumping", false);
        }


        if (transform.GetChild(0).GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            
            
        }
        else
        {
            
        }

    }
    
    //general methods
    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos =  transform.position- destination;
        lookPos.y = 0;
        if (lookPos != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
        }  
    }

    public void SetDestinationTransform(Transform destTransform)
    {
        Monster = GetComponent<NavMeshAgent>();
        Monster.SetDestination(destTransform.position);
        waiting = false;
        currentTarget = destTransform.position; 
    }

    public void GoToRandomDestination(List<Transform> destiantions)
    {
        Debug.Log("gotorandom called");
        int chosenDestination = Random.Range(0, destiantions.Count - 1);
        Monster.SetDestination(destiantions[chosenDestination].position);
        waiting = false;
        currentTarget = destiantions[chosenDestination].position;
       
    }
    //start area 1 methods
    public void UpdateStartingArea1Methods()
    {
        if (!waiting)
        {
            FaceTarget(currentTarget);
        }
        
        if ( !waiting && Monster.remainingDistance == 0 )
        {
            waiting = true;
            reachedDestination.Invoke();
            Debug.Log("reached dest hit in uypdate");
            
        }
    }


    //river game methods 

    public void startRiverGame()
    {
        Monster.SetDestination(QuestionPlatforms[currentQuestionNum].transform.position);
        currentTarget = QuestionPlatforms[currentQuestionNum].transform.position;
        waiting = false;
    }
    public void updateRiverGameMethods()
    {
        Monster = GetComponent<NavMeshAgent>();
        FaceTarget(currentTarget);
        //Debug.Log(currentQuestionNum);
        if (!waiting && Monster.remainingDistance == 0 && counter <= QuestionPlatforms.Length + 2)
        {
            waiting = true;
            counter++;
            if (counter % 2 == 0) // Optimeze ALL THIS propably why the bug happens//or not 
            {
                //on answer tile 
                FaceTarget(new Vector3(50, 0, 10));
                StartCoroutine(PreformResultAction(makingRightChoice));
                Debug.Log("update calling preform");
            }
            else
            {
                //on question tile 
                FaceTarget(new Vector3(50, 0, 10));
                reachedQuestiontile.Invoke();
            }

        }
        if (Reverting && Monster.gameObject.transform.GetChild(0).transform.position.y >= 0.0f)
        {
            stopReverting();
        }
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
            Monster.SetDestination(leftPlatforms[currentQuestionNum - 1].transform.position);
            currentTarget = leftPlatforms[currentQuestionNum - 1].transform.position;
            chosenPlatform = leftPlatforms[currentQuestionNum - 1];
        }
        else
        {
            //go right
            Debug.Log("goright");
            Monster.SetDestination(rightPlatforms[currentQuestionNum - 1].transform.position);
            currentTarget = rightPlatforms[currentQuestionNum - 1].transform.position;
            chosenPlatform = rightPlatforms[currentQuestionNum - 1];
        }
        waiting = false;

    }

    IEnumerator PreformResultAction(bool correct)
    {
        if (correct)
        {
            Debug.Log("doing correct");

            CorrectAnswerVoices[Random.Range(0, CorrectAnswerVoices.Count)].Play();
            yield return DoCelebration();
            
            GoToNextQuestion();
        }
        else
        {
            WrongAnswerVoices[Random.Range(0, WrongAnswerVoices.Count)].Play();
            Debug.Log("doing incorrect");
            //do fall in water and jump out
            //ReegieAnimator.SetBool();
            yield return FallInWater();
            revertFellInWater();
            yield return new WaitForSeconds(0.5f);
            GoToNextQuestion();
        }
    }
    IEnumerator DoCelebration()
    {
        //do celebration animation
        ReegieAnimator.SetBool("Celebrating", true);
        monsterSpeechBubble.transform.GetChild(0).GetComponent<TMP_Text>().text = "Correct, YAY!";
        monsterSpeechBubble.SetActive(true);
        yield return new WaitForSeconds(3);
        ReegieAnimator.SetBool("Celebrating", false);
    }
    IEnumerator FallInWater()
    {
        //fall in water is manipulation of the 3D model as we do not currently have animations but once 
        //those are finished this model manipulation will be replaced by them
        monsterSpeechBubble.transform.GetChild(0).GetComponent<TMP_Text>().text = "Oh no :(";
        monsterSpeechBubble.SetActive(true);
        Monster.gameObject.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
        chosenPlatform.gameObject.GetComponent<Rigidbody>().useGravity = true;
        ReegieAnimator.SetBool("Falling", true);
        Debug.Log("falling");
        yield return new WaitForSeconds(1.5f);
        ReegieAnimator.SetBool("Falling", false);
        Monster.gameObject.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
        Monster.gameObject.transform.GetChild(0).GetComponent<Rigidbody>().velocity = Vector3.zero;
        Debug.Log("GravityReset");
    }

    //IEnumerator revertFellInWater()
    //{
    //    //Monster.gameObject.transform.GetChild(0).GetComponent<ConstantForce>().enabled = true;
    //    floater.FloatUp();
    //    ReegieAnimator.SetBool("Jumping", true);
    //    //Reverting = true;
    //    yield return new WaitForSeconds(1.0f);
    //    //Monster.gameObject.transform.GetChild(0).GetComponent<ConstantForce>().enabled = false;
    //    //Monster.gameObject.transform.GetChild(0).GetComponent<Rigidbody>().velocity = Vector3.zero;
    //    //yield return new WaitForFixedUpdate();
    //    //Monster.gameObject.transform.GetChild(0).transform.position.Set(0,0,0);



//}
private void revertFellInWater()
{
    Reverting = true;
    Monster.gameObject.transform.GetChild(0).GetComponent<ConstantForce>().enabled = true;
    ReegieAnimator.SetBool("Jumping", true);
}
private void stopReverting()
{
    Reverting = false;
    Monster.gameObject.transform.GetChild(0).GetComponent<ConstantForce>().enabled = false;
    Monster.gameObject.transform.GetChild(0).GetComponent<Rigidbody>().velocity = Vector3.zero;
    Monster.gameObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
    ReegieAnimator.SetBool("Jumping", false);
}



private void GoToNextQuestion()
    {
        monsterSpeechBubble.SetActive(false);
        Monster.SetDestination(QuestionPlatforms[currentQuestionNum].transform.position);
        currentTarget = QuestionPlatforms[currentQuestionNum].transform.position;
        waiting = false;

    }
    public bool reachedEndOfRiver()
    {
        if (QuestionPlatforms.Length <= currentQuestionNum + 1 && Monster.remainingDistance == 0 && !waiting)
        {
            return true;
        }
        else return false;
    }




}
