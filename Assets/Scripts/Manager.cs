using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent monsterAgent;
    [SerializeField]
    private MonsterMovement monster;
    [SerializeField]
    private TMP_Text questionText;
    [SerializeField]
    private GameObject questionTextBox;
    [SerializeField]
    private GameObject GameOverUI;
    [SerializeField]
    private int totalNumOfQuestions;
    [SerializeField]
    private TMP_Text[] LeftPlatformsAnswerTxts;
    [SerializeField]
    private TMP_Text[] RightPlatformsAnswerTxts;
    [SerializeField]
    private InputReciever Ireciever;

    private List<Question> questions = new List<Question>();
    private List<Question> usedQuestions = new List<Question>();
    private int currentQuestionNum = 0;
    protected bool leftCorrect;
    void Start()
    {
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
            
        }

        monster.reachedQuestiontile.AddListener(ShowQuestion);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(monster.reachedEnd())
        {
            GameOverUI.SetActive(true);
        } 
    }
    private void ShowQuestion()
    {
        if (monster.reachedEnd())
        {
            return;
        }
        questionTextBox.SetActive(true);
        int rando = 0;
        if (questions.Count > 0)
        {
            rando = Random.Range(0, questions.Count); 

        }
        else
        {
            questions = usedQuestions;
        }
         

        questionText.text = questions[rando].questionText;

        int fiftyfifty = Random.Range(0, 2);
        if(fiftyfifty == 0)
        {
            PlatformAnswersTxtsEnabled(true);
            LeftPlatformsAnswerTxts[currentQuestionNum].text = questions[rando].CorrectAnswer;
            RightPlatformsAnswerTxts[currentQuestionNum].text = questions[rando].WrongAnswer;
            leftCorrect = true;
        }
        else
        {
            PlatformAnswersTxtsEnabled(true);
            LeftPlatformsAnswerTxts[currentQuestionNum].text = questions[rando].WrongAnswer;
            RightPlatformsAnswerTxts[currentQuestionNum].text = questions[rando].CorrectAnswer;
            leftCorrect = false;
        }
        usedQuestions.Add(questions[rando]);
        questions.Remove(questions[rando]);

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
    public void ChooseOption(int intChoice)
    {
        Choice chosen = (Choice)intChoice;
        Ireciever.gameObject.SetActive(false);

        questionTextBox.SetActive(false);
        LeftPlatformsAnswerTxts[currentQuestionNum-1].gameObject.transform.parent.gameObject.SetActive(false);
        RightPlatformsAnswerTxts[currentQuestionNum-1].gameObject.transform.parent.gameObject.SetActive(false);
        if ((chosen == Choice.LEFT && leftCorrect)|| (chosen == Choice.RIGHT && !leftCorrect))
        {
            if(leftCorrect) monster.makeRightChoice(Choice.LEFT);
            else monster.makeRightChoice(Choice.RIGHT);

        }
        else 
        {
            if (leftCorrect) monster.makeWrongChoice(Choice.RIGHT);
            else monster.makeWrongChoice(Choice.LEFT);
        }
    }

    private void PlatformAnswersTxtsEnabled(bool enabled) {
        LeftPlatformsAnswerTxts[currentQuestionNum].gameObject.transform.parent.gameObject.SetActive(enabled);
        RightPlatformsAnswerTxts[currentQuestionNum].gameObject.transform.parent.gameObject.SetActive(enabled);
    }
}

public enum Choice
{
    LEFT,
    RIGHT
}