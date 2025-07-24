using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class QuizData
{
    public string question;
    public string[] answers;
    public int correctIndex;
}

[System.Serializable]
public class QuizDataList
{
    public QuizData[] quizList;
}

public class QuizManager : MonoBehaviour
{
    public Text questionText;
    public Button[] answerButtons;

    private List<QuizData> quizDataList;
    private int currentQuestion = 0;

    void Start()
    {
        LoadQuestions();
        ShowQuestion();
    }

    void LoadQuestions()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("questions"); // no .json extension
        string wrappedJson = "{\"quizList\":" + jsonFile.text + "}"; // wrap array
        QuizDataList data = JsonUtility.FromJson<QuizDataList>(wrappedJson);
        quizDataList = new List<QuizData>(data.quizList);
    }

    void ShowQuestion()
    {
        if (quizDataList == null || quizDataList.Count == 0) return;

        var q = quizDataList[currentQuestion];
        questionText.text = q.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<Text>().text = q.answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    void CheckAnswer(int selected)
    {
        var q = quizDataList[currentQuestion];
        if (selected == q.correctIndex)
            Debug.Log("Correct!");
        else
            Debug.Log("Wrong!");

        currentQuestion = (currentQuestion + 1) % quizDataList.Count;
        ShowQuestion();
    }
}
