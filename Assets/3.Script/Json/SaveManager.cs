using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveManager : MonoBehaviour
{
    private string filePath;

    private void Start()
    {
        //json���·� gamedata ������ �̸� ���� / ���� ��� ����
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
        return new GameSaveData//������ ���� ��� �⺻�� ��ȯ
        {
            clearedStage = 0,
            targetScore = new int[] { 40, 60, 100 },
            bestScore = 0
        };
    }
}
