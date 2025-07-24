using UnityEngine;

[CreateAssetMenu(fileName = "QuestionData", menuName = "QuestionData")]

public class QtsData : ScriptableObject
{
    [System.Serializable]
    public struct Question
    {
        public string questionText;
        public string[] replies;
        public int correctReplyIndex;
    }
    public Question[] questions;
}