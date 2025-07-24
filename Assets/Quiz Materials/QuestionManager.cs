using System;
using System.Collections; 
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    // public TextMeshProUGUI questionText;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI FinalScore;
    public Button[] replyButtons;
    public Button nextButton;
    public Button startButton;
    public QtsData qtsData; // Reference to the scriptable object
    public GameObject Right;
    public GameObject Wrong;
    public GameObject GameFinished;
    public GameObject GameStart;
    private int currentQuestion = 0;
    private static int score = 0;

    void Start()
    {
        // Set up quiz board 
        // SetQuestion(currentQuestion);
        Right.gameObject.SetActive(false);
        Wrong.gameObject.SetActive(false);
        GameFinished.SetActive(false);
        nextButton.gameObject.SetActive(false);
        scoreText.text = $"Score: {score}";

        // Activate start scene
        GameStart.gameObject.SetActive(true);
        startButton.onClick.AddListener(StartGame);

        // GameStart.SetActive(false); // Hide the start screen
        // SetQuestion(currentQuestion); // Begin the quiz
    }

    void StartGame()
    {
        GameStart.SetActive(false); // Hide the start screen
        SetQuestion(currentQuestion); // Begin the quiz
    }

    void SetQuestion(int questionIndex)
    {
        questionText.text = qtsData.questions[questionIndex].questionText;

        // Remove previous listeners before adding new ones 
        foreach (Button r in replyButtons)
        {
            r.onClick.RemoveAllListeners();
        }

        // get Question Data 
        for (int i = 0; i < replyButtons.Length; i++)
        {
            replyButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = qtsData.questions[questionIndex].replies[i];
            int replyIndex = i;
            replyButtons[i].onClick.AddListener(() =>
            {
                CheckReply(replyIndex);
            });
        }
    }

    void CheckReply(int replyIndex)
    {
        if (replyIndex == qtsData.questions[currentQuestion].correctReplyIndex)
        {
            score++;
            scoreText.text = $"Score: {score}";

            //Enable Right reply panel 
            Right.gameObject.SetActive(true);
        }
        else
        {
            //Wrong reply
            Wrong.gameObject.SetActive(true);
        }

        //Set Active false all reply buttons 
        foreach (Button r in replyButtons)
        {
            r.interactable = false;
        }
        //Set next button to be active
        nextButton.gameObject.SetActive(true);
        nextButton.onClick.AddListener(() =>
        {
            //Next Question
            StartCoroutine(Next());
        });
    }

    IEnumerator Next()
    {
        yield return new WaitForSeconds(2);

        currentQuestion++;

        if (currentQuestion < qtsData.questions.Length)
        {
            // Reset the UI and enable all reply buttons 
            Reset();
        }
        else
        {
            GameFinished.SetActive(true);

            // Calculate Score Percentage
            float scorePercentage = (float)score / qtsData.questions.Length * 100;
            // Display the score percentage
            FinalScore.text = "You scored " + scorePercentage.ToString("FO") + "%";
            // Display the appropriate message based on the score percentage 
            if (scorePercentage < 80)
            {
                FinalScore.text += "\nWell Done!";
            }
            else
            {
            }
            FinalScore.text += "\nYou answered all questions correctly!";
        }
    }

    public void Reset()
    {
        // Hide panels 
        Right.SetActive(false);
        Wrong.SetActive(false);
        nextButton.gameObject.SetActive(false);

        // Enable all reply buttons
        foreach (Button r in replyButtons)
        {
            r.interactable = true;
        }

        // Set the next question 
        SetQuestion(currentQuestion);
    }

}