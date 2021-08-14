using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private const string SAVE_LOAD_PATH = "file.ktoprocheltotsdoh";
    private const string GAMESTATS_PSTH = "SaveSystem";

    private int _money;
    private int _curScore;
    private int _maxScore;

    private static SaveSystem _instance;

    public static SaveSystem instance
    {
        get
        {
            if (_instance == null)
            {
                var prefab = Resources.Load<SaveSystem>(GAMESTATS_PSTH);

                _instance = Instantiate(prefab);
                _instance.LoadData();

                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private void SetData(GameData data)
    {
        _money    = data.Money;
        _curScore = data.CurScore;
        _maxScore = data.MaxScore;
    }

    private void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + SAVE_LOAD_PATH;
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(_instance);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    private void LoadData()
    {
        string path = Application.persistentDataPath + SAVE_LOAD_PATH;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data;

            try
            {
                data = formatter.Deserialize(stream) as GameData;
            }
            catch
            {
                Debug.Log("Формат класса \"GameData\" был изменён!");

                stream.Close();
                File.Delete(path);
                stream = new FileStream(path, FileMode.Open);

                SaveData();
                data = formatter.Deserialize(stream) as GameData;
            }

            SetData(data);

            stream.Close();
        }
        else
        {
            SaveData();
            LoadData();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        SaveData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public int Money
    {
        get { return _money; }
    }

    public int CurScore
    { 
        get { return _curScore; }
    }

    public int MaxScore
    {
        get { return _maxScore; }
    }

    public void AddMoney(int value)
    {
        _money += value;
    }

    public void UpdateScore(int value)
    {
        _curScore = value;
        _maxScore = Mathf.Max(_maxScore, _curScore);
    }
}