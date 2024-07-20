using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RecipeObject : MonoBehaviour
{
    public float activationTime;
    public int Point;
    private string recipe_name;
    public string Recipe_name { get { return recipe_name; } }

    // 자식 Image 오브젝트의 이름을 기준으로 이미지를 설정하는 메소드
    public void SetImagesByName(Dictionary<string, string> imageNameMap)
    {
        Image[] images = GetComponentsInChildren<Image>();

        foreach (Image image in images)
        {
            if (imageNameMap.ContainsKey(image.name))
            {
                // Resources 폴더에서 이미지 파일을 로드
                Sprite sprite = Resources.Load<Sprite>(imageNameMap[image.name]);
                if (sprite != null)
                {
                    image.sprite = sprite;
                }
            }
        }
    }
}
