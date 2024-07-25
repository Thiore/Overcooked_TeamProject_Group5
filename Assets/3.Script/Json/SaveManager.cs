using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveManager : MonoBehaviour
{
    private string filePath;

    private void Start()
    {
        //json형태로 gamedata 파일의 이름 정의 / 파일 경로 지정
        //C:\Users\[Username]\AppData\LocalLow\[CompanyName]\[ProductName]
        filePath = Application.persistentDataPath + "/gamedata.json";
    }

    //게임 데이터 저장
    public void SaveGame(GameSaveData data)
    {
        //GameSaveData 구조체를 json 문자열로 변환
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);

        //json 문자열을 파일에 저장
        File.WriteAllText(filePath, json);
    }

    //json파일에서 게임 데이터 로드
    public GameSaveData LoadGame()
    {
        if (File.Exists(filePath))
        {
            //파일에서 json문자열 읽기
            string json = File.ReadAllText(filePath);

            //json 문자열을 GameSaveData 구조체로 변환
            return JsonConvert.DeserializeObject<GameSaveData>(json);
        }
        //파일이 없는 경우 기본값 반환
        return new GameSaveData 
        {
            clearedStage = 0,
            targetScore =0,
            bestScore = 0
        };
    }
}
