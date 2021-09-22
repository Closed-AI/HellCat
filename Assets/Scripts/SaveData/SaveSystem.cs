using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private const string SAVE_LOAD_PATH  = "file.ktoprocheltotsdoh";
    private const string SAVESYSTEM_PATH = "SaveSystem";

    private readonly int[] _price = { 100, 250, 500, 750, 1000, 1500, 2000, 3000, 4000, 5000 };
    private GameData _data;

    // синглтон
    private static SaveSystem _instance;

    private SaveSystem() { }

    public static SaveSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                var prefab = Resources.Load<SaveSystem>(SAVESYSTEM_PATH);

                _instance = Instantiate(prefab);

                _instance.LoadData();

                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private void SaveData()
    {
        if (_data == null)
            _data =  new GameData();

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + SAVE_LOAD_PATH;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, _data);
        stream.Close();
    }

    private void LoadData()
    {
        string path = Application.persistentDataPath + SAVE_LOAD_PATH;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            try
            {
                _data = formatter.Deserialize(stream) as GameData;
            }
            catch
            {
                Debug.Log("Формат класса \"GameData\" был изменён!");

                stream.Close();

                File.Delete(path);

                SaveData();

                stream = new FileStream(path, FileMode.Open);
                _data = formatter.Deserialize(stream) as GameData;
            }

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
    { get => _data.Money; set => _data.Money = value; }

    public int CurScore
    { get => _data.CurScore; set => _data.CurScore = value; }

    public int MaxScore
    { get => _data.MaxScore; set => _data.MaxScore = value; }

    public int[] Progress
    { get => _data.Progress; set => _data.Progress = value; }

    public int[] Price
    { get => _price; }

    public void AddMoney(int value)
    {
        _data.Money += value;
    }

    public void UpdateScore(int value)
    {
        _data.CurScore = value;
        _data.MaxScore = Mathf.Max(_data.CurScore, _data.MaxScore);
    }

    public void ResetProgress()
    {
        _data = new GameData();
    }
}