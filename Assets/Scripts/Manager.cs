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
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentQuestionNum>=4 && monsterAgent.remainingDistance == 0)
        {
            GameOverUI.SetActive(true);
        }
        if(currentQuestionNum >= 4)
        {
            return;
        }
        if(monsterAgent.remainingDistance == 0 && monster.onQuestionTile && currentQuestionNum <= totalNumOfQuestions&&monster.readyForNextQuestion)
        {
            monster.readyForNextQuestion = false;
            ShowQuestion();
            currentQuestionNum++;
            
        }
    }
    private void ShowQuestion()
    {
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
            LeftPlatformsAnswerTxts[currentQuestionNum].gameObject.transform.parent.gameObject.SetActive(true);
            RightPlatformsAnswerTxts[currentQuestionNum].gameObject.transform.parent.gameObject.SetActive(true);
            LeftPlatformsAnswerTxts[currentQuestionNum].text = questions[rando].CorrectAnswer;
            RightPlatformsAnswerTxts[currentQuestionNum].text = questions[rando].WrongAnswer;
            leftCorrect = true;
        }
        else
        {
            LeftPlatformsAnswerTxts[currentQuestionNum].gameObject.transform.parent.gameObject.SetActive(true);
            RightPlatformsAnswerTxts[currentQuestionNum].gameObject.transform.parent.gameObject.SetActive(true);
            LeftPlatformsAnswerTxts[currentQuestionNum].text = questions[rando].WrongAnswer;
            RightPlatformsAnswerTxts[currentQuestionNum].text = questions[rando].CorrectAnswer;
            leftCorrect = false;
        }
        usedQuestions.Add(questions[rando]);
        questions.Remove(questions[rando]);

        Ireciever.gameObject.SetActive(true);
        //choose random question
        //display question
        //display possible answers over the 2 following tiles randomly + set correct and inccorect 

        //read input from ar / manually for now
        // check if chosen input was correct or incorrect 
        //show result with methoids below
    }
    public void ChooseOption(int choice)
    {
        Ireciever.gameObject.SetActive(false);
        questionTextBox.SetActive(false);
        LeftPlatformsAnswerTxts[currentQuestionNum-1].gameObject.transform.parent.gameObject.SetActive(false);
        RightPlatformsAnswerTxts[currentQuestionNum-1].gameObject.transform.parent.gameObject.SetActive(false);
        if ((choice == 1 && leftCorrect)|| (choice == 2 && !leftCorrect))
        {
            if(leftCorrect) monster.makeRightChoice(1);
            else monster.makeRightChoice(2);

        }
        else 
        {
            if (leftCorrect) monster.makeWrongChoice(2);
            else monster.makeWrongChoice(1);
        }
    }
}
