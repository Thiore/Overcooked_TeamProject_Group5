using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RecipeObject : MonoBehaviour
{
    public float activationTime;
    public int Point;
    private string recipe_name;
    public string Recipe_name { get { return recipe_name; } }

    // �ڽ� Image ������Ʈ�� �̸��� �������� �̹����� �����ϴ� �޼ҵ�
    public void SetImagesByName(Dictionary<string, string> imageNameMap)
    {
        Image[] images = GetComponentsInChildren<Image>();

        foreach (Image image in images)
        {
            if (imageNameMap.ContainsKey(image.name))
            {
                // Resources �������� �̹��� ������ �ε�
                Sprite sprite = Resources.Load<Sprite>(imageNameMap[image.name]);
                if (sprite != null)
                {
                    image.sprite = sprite;
                }
            }
        }
    }
}
