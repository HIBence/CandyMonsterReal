using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class RiverGameManager : MonoBehaviour
{
    [SerializeField]
    private MonsterMovement monster;
    [SerializeField]
    private TMP_Text questionText;
    [SerializeField]
    private GameObject questionTextBox;
    [SerializeField]
    private int totalNumOfQuestions;
    [SerializeField]
    private TMP_Text[] LeftPlatformsAnswerTxts;
    private TMP_Text CurrentLeftText;
    [SerializeField]
    private TMP_Text[] RightPlatformsAnswerTxts;
    private TMP_Text CurrentRightText;
    [SerializeField]
    private InputReciever Ireciever;
    [SerializeField]
    private List<Sprite> AnswerPropsIllustrations;
    [SerializeField]
    SceneControl sceneController;




    private List<Sprite> OrderedAnswerProps = new List<Sprite>();
    private List<Sprite> UsedAnswerPropsIllustrations = new List<Sprite>();
    [SerializeField] private List<AudioSource> riverGameVoiceLines = new List<AudioSource>();



    private List<Question> questions = new List<Question>();
    private List<Question> usedQuestions = new List<Question>();
    private int currentQuestionNum = 0;
    private Sprite randomCorrectPropIllustration ;
    private Sprite randomFalsePropIllustration ;
    protected bool leftCorrect;
    private bool ScanLock = false;

    void Start()
    {
        StartCoroutine(SayTutorialVoiceLine());
        Ireciever.gameObject.SetActive(false);
        if(totalNumOfQuestions == 0)
        {
            return;
        }
        int n = 1;
        while ( totalNumOfQuestions >= n)
        {
            questions.Add(Resources.Load<Question>($"ScriptableObjects/Questions/Question{n}"));
            Debug.Log($"Question {n} loaded");
            n++;
            
        }Debug.Log(AnswerPropsIllustrations.Count);
        OrderedAnswerProps.AddRange(AnswerPropsIllustrations);
        Debug.Log(OrderedAnswerProps.Count);
        monster.reachedQuestiontile.AddListener(ShowQuestion);
        monster.startRiverGame();
    }

    // Update is called once per frame
    void Update()
    {
        monster.updateRiverGameMethods();
        if(monster.reachedEndOfRiver())
        {
            //complete voice line play
            riverGameVoiceLines[1].Play();
            sceneController.proceedToStartArea2();
            
        } 
    }
    private void ShowQuestion()
    {
        if (monster.reachedEndOfRiver())
        {
            return;
        }


        questionTextBox.SetActive(true);
        int randomQuestion = 0;
        
        if (questions.Count > 0)
        {
            randomQuestion = Random.Range(0, questions.Count); 

        }
        else
        {
            questions.AddRange(usedQuestions);
            usedQuestions.Clear();
        }

        if (AnswerPropsIllustrations.Count > 1)
        {
            SelectRandomProps();
        }
        else
        {
            AnswerPropsIllustrations.AddRange(UsedAnswerPropsIllustrations);
            UsedAnswerPropsIllustrations.Clear();
            SelectRandomProps();
        }


        questionText.text = questions[randomQuestion].questionText;

        int fiftyfifty = Random.Range(0, 2);
        CurrentLeftText = LeftPlatformsAnswerTxts[currentQuestionNum];
        CurrentRightText = RightPlatformsAnswerTxts[currentQuestionNum];
        if (fiftyfifty == 0)
        {
            PlatformAnswersTxtsEnabled(true);
            CurrentLeftText.text = questions[randomQuestion].CorrectAnswer;
            CurrentLeftText.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = randomCorrectPropIllustration;
            CurrentRightText.text = questions[randomQuestion].WrongAnswer;
            CurrentRightText.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = randomFalsePropIllustration;
            leftCorrect = true;
        }
        else
        {
            PlatformAnswersTxtsEnabled(true);
            CurrentLeftText.text = questions[randomQuestion].WrongAnswer;
            CurrentLeftText.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = randomFalsePropIllustration;
            CurrentRightText.text = questions[randomQuestion].CorrectAnswer;
            CurrentRightText.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = randomCorrectPropIllustration;
            leftCorrect = false;
        }
        usedQuestions.Add(questions[randomQuestion]);
        questions.Remove(questions[randomQuestion]);

        Ireciever.gameObject.SetActive(true);
        currentQuestionNum++;
        monster.currentQuestionNum = currentQuestionNum;
        //choose random question
        //display question
        //display possible answers over the 2 following tiles randomly + set correct and inccorect 

        //read input from ar / manually for now
        // check if chosen input was correct or incorrect 
        //show result with methoids below
    }
    private void SelectRandomProps()
    {
        randomCorrectPropIllustration = AnswerPropsIllustrations[Random.Range(0, AnswerPropsIllustrations.Count)];
        UsedAnswerPropsIllustrations.Add(randomCorrectPropIllustration);
        AnswerPropsIllustrations.Remove(randomCorrectPropIllustration);

        randomFalsePropIllustration = AnswerPropsIllustrations[Random.Range(0, AnswerPropsIllustrations.Count)];
        UsedAnswerPropsIllustrations.Add(randomFalsePropIllustration);
        AnswerPropsIllustrations.Remove(randomFalsePropIllustration);
        //cycle prop illsutrations to not get doubles and randomly select all of them once and then repeat.
    }
    public void ChooseOption(int intChoice)
    {
        //Choice chosen = (Choice)intChoice;
        if(ScanLock == true)
        {

            return;
        }
        LockScan();

        Debug.Log(OrderedAnswerProps.Count); 
        if (OrderedAnswerProps[intChoice] == randomCorrectPropIllustration && leftCorrect)    {  monster.makeRightChoice(Choice.LEFT); }
        else if (OrderedAnswerProps[intChoice] == randomCorrectPropIllustration)              { monster.makeRightChoice(Choice.RIGHT); }
        else if (leftCorrect && OrderedAnswerProps[intChoice] == randomFalsePropIllustration) { monster.makeWrongChoice(Choice.RIGHT); }
        else if(OrderedAnswerProps[intChoice] == randomFalsePropIllustration) { monster.makeWrongChoice(Choice.LEFT); }
        else { return; }

        Ireciever.gameObject.SetActive(false);
        questionTextBox.SetActive(false);
        LeftPlatformsAnswerTxts[currentQuestionNum - 1].gameObject.transform.parent.gameObject.SetActive(false);
        RightPlatformsAnswerTxts[currentQuestionNum - 1].gameObject.transform.parent.gameObject.SetActive(false);
        

        //if ((chosen == Choice.LEFT && leftCorrect)|| (chosen == Choice.RIGHT && !leftCorrect))
        //{
        //    if(leftCorrect) 
        //    else monster.makeRightChoice(Choice.RIGHT);

        //}
        //else 
        //{
        //    if (leftCorrect) monster.makeWrongChoice(Choice.RIGHT);
        //    else monster.makeWrongChoice(Choice.LEFT);
        //}
    }

    private void PlatformAnswersTxtsEnabled(bool enabled) {
        CurrentLeftText.gameObject.transform.parent.gameObject.SetActive(enabled);
        CurrentRightText.gameObject.transform.parent.gameObject.SetActive(enabled);
    }

    IEnumerator LockScan()
    {
        ScanLock = true;
        yield return new WaitForSeconds(5.0f);
        ScanLock = false;
    }
    IEnumerator SayTutorialVoiceLine()
    {
        yield return new WaitForSeconds(2.0f);
        riverGameVoiceLines[0].Play();
    }
}

public enum Choice
{
    LEFT,
    RIGHT
}