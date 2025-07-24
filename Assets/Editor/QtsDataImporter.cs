using UnityEngine;
using UnityEditor;
using System.IO;

public class QtsDataImporter : EditorWindow
{
    private TextAsset jsonFile;
    private QtsData targetAsset;

    [MenuItem("Tools/Import Question Data")]
    public static void ShowWindow()
    {
        GetWindow<QtsDataImporter>("Import Question Data");
    }

    private void OnGUI()
    {
        GUILayout.Label("Import Questions from JSON", EditorStyles.boldLabel);

        jsonFile = (TextAsset)EditorGUILayout.ObjectField("JSON File", jsonFile, typeof(TextAsset), false);
        targetAsset = (QtsData)EditorGUILayout.ObjectField("Target QtsData Asset", targetAsset, typeof(QtsData), false);

        if (GUILayout.Button("Import") && jsonFile != null && targetAsset != null)
        {
            ImportQuestions();
        }
    }

    void ImportQuestions()
    {
        QtsData.Question[] importedQuestions = JsonHelper.FromJson<QtsData.Question>(jsonFile.text);
        targetAsset.questions = importedQuestions;
        EditorUtility.SetDirty(targetAsset);
        AssetDatabase.SaveAssets();
        Debug.Log("Questions imported successfully.");
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string wrapped = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrapped);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
