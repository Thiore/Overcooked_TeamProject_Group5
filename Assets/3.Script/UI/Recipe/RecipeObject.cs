using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RecipeObject : MonoBehaviour
{
    public float activationTime;
    public int Point;
    [SerializeField] private Image Tool_img_1;
    [SerializeField] private Image Tool_img_2;
    private string recipe_name;
    public string Recipe_name { get { return recipe_name; } }
    public Recipe recipe;

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

    public void SetToolImage(int index,string toolname)
    {
        
        if(index == 0)
        {
            Tool_img_1.sprite = Resources.Load<Sprite>(path: toolname);
            if(Tool_img_1.sprite != null)
            {
                Debug.Log(toolname+"tool_1");

            }
            else
            {
                Debug.Log("�ȶ�");
            }
        }
        else
        {
            Tool_img_2.sprite = Resources.Load<Sprite>(toolname);
            if (Tool_img_2.sprite != null)
            {
                Debug.Log(toolname + "tool_2");

            }
            else
            {
                Debug.Log("�ȶ�");
            }
        }
    }
}
