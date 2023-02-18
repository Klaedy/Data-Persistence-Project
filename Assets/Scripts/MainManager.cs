using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public string input;
    public int record;
    private GameManager gameManagerScript;
    public Text placeHolderText;
    private bool scene1 = false;

    void Start()
    {
      
    }

    void Update()
    {
        CallGameManager();
        PlaceHolderName();
        
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ReadInputField(string s)
    {
        input = s;
        Debug.Log(s);
        SaveName();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void CallGameManager()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == 1 && scene1 == false)
        {
            gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
            scene1 = true;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string input;
        public int record;
    }

    public void SaveName()
    {
        SaveData data = new SaveData();
        data.input = input;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void SavePoints()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        record = gameManagerScript.m_Points;
        SaveData data = new SaveData();
        data.record = gameManagerScript.m_Points;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);       
    }

    public void LoadName()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            input = data.input;
        }
    }

    public void LoadPoints()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            record = data.record;
        }
    }
    public void PlaceHolderName()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == 0)
        {
            LoadName();
            placeHolderText.GetComponent<Text>();
            placeHolderText.text = $"{input}";
        }       
    }
}
