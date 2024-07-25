using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveManager : MonoBehaviour
{
    private string filePath;

    private void Start()
    {
        //json���·� gamedata ������ �̸� ���� / ���� ��� ����
        //C:\Users\[Username]\AppData\LocalLow\[CompanyName]\[ProductName]
        filePath = Application.persistentDataPath + "/gamedata.json";
    }

    //���� ������ ����
    public void SaveGame(GameSaveData data)
    {
        //GameSaveData ����ü�� json ���ڿ��� ��ȯ
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);

        //json ���ڿ��� ���Ͽ� ����
        File.WriteAllText(filePath, json);
    }

    //json���Ͽ��� ���� ������ �ε�
    public GameSaveData LoadGame()
    {
        if (File.Exists(filePath))
        {
            //���Ͽ��� json���ڿ� �б�
            string json = File.ReadAllText(filePath);

            //json ���ڿ��� GameSaveData ����ü�� ��ȯ
            return JsonConvert.DeserializeObject<GameSaveData>(json);
        }
        //������ ���� ��� �⺻�� ��ȯ
        return new GameSaveData 
        {
            clearedStage = 0,
            targetScore =0,
            bestScore = 0
        };
    }
}
