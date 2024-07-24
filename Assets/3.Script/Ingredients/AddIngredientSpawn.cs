using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddIngredientSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] AddIngrePrefabs;
    [SerializeField] private Crate_Data Ingredients_Info;

    private Ingredient Ingre;

    private List<Recipe> recipes;
    private List<GameObject> AddIngreList = new List<GameObject>();
    public List<int> recipe_Num { get; private set; }

    private void Awake()
    {
        TryGetComponent(out Ingre);
        recipes = DataManager.Instance.StageRecipeData(1);
        recipe_Num = new List<int>();
        for(int i = 0; i < recipes.Count;i++)
        {
            if(recipes[i].ingredient.Contains(gameObject.name))
            {
                recipe_Num.Add(i);
            }
        }

        for (int i = 0; i < AddIngrePrefabs.Length; i++)
        {
            if (AddIngrePrefabs[i].name.StartsWith(gameObject.name))
            {
                AddIngreList.Add(AddIngrePrefabs[i]);
            }
        }
    }

    public bool SetAddIngredient(GameObject targetObj) //false�� ��ȯ�Ǹ� ��ġ������
    {// targetObj�� ���� �Ǵ� counter���� �ö��ִ� ����Դϴ�.(tag�� ingredient�ΰ��� �־��ָ� �ɰͰ����ϴ�.)
        if(Ingre.OnPlate)
        {
            if (targetObj.TryGetComponent(out Ingredient targetIngre))
            {
                targetObj.TryGetComponent(out AddIngredientSpawn targetadd);
                for (int i = 0; i < targetadd.recipe_Num.Count; i++)
                {
                    if (targetadd.recipes[recipe_Num[i]].recipe.Equals(targetObj.name))
                    {
                        Debug.Log($"{targetObj.name}��(��) �̹� �ϼ��� ������ �Դϴ�.");
                        return false;
                    }
                }
                for(int i = 0; i < recipe_Num.Count; i++)
                {
                    if(recipes[recipe_Num[i]].recipe.Equals(gameObject.name))
                    {
                        Debug.Log($"{gameObject.name}��(��) �̹� �ϼ��� ������ �Դϴ�.");
                    }
                }

                if (!targetIngre.OnPlate)
                {
                    Debug.Log($"{targetObj.transform.parent.name}���� �ִ� {targetObj.name}�� ���� �������� �ʾҽ��ϴ�.");
                    return false;
                }

                if (targetObj.name.Contains(gameObject.name))
                {
                    Debug.Log("������ ����Դϴ�.");
                    return false;
                }
               

                for (int i = 0; i < recipe_Num.Count; i++)
                {
                    if (targetadd.recipe_Num.Contains(recipe_Num[i]))
                    {
                        GameObject Obj = new GameObject($"{targetObj.name}_{gameObject.name}");
                        Obj.transform.tag = "Ingredients";
                        TryGetComponent(out SphereCollider targetcol);
                        SphereCollider newcol = Obj.AddComponent<SphereCollider>();
                        newcol.center = targetcol.center;
                        newcol.radius = targetcol.radius;
                        newcol.isTrigger = targetcol.isTrigger;
                        if (targetObj.transform.parent != null)
                        {
                            Obj.transform.SetParent(targetObj.transform.parent);
                        }
                        for (int j = 0; j < targetadd.AddIngreList.Count; j++)
                        {
                            if (targetadd.AddIngreList[i].name.StartsWith(Obj.name))
                            {
                                GameObject childObj = Instantiate(AddIngreList[i], Obj.transform.position, Obj.transform.rotation, Obj.transform);
                                if (!childObj.name.Equals(Obj.name))
                                {
                                    childObj.SetActive(false);
                                }
                            }
                        }
                        targetIngre.Die();
                        for(int j = 0; j < recipe_Num.Count;j++)
                        {
                            for (int k = 0; k < recipes[recipe_Num[j]].ingredient.Count; k++)
                            {
                                if (!Obj.name.Contains(recipes[recipe_Num[j]].ingredient[k]))
                                {
                                    break;
                                }
                                if (k == recipes[recipe_Num[j]].ingredient.Count - 1)
                                {
                                    Obj.name = recipes[recipe_Num[j]].recipe;
                                    break;
                                }
                            }
                            
                        }
                        
                        return true;
                    }
                }
                return false;
            }
            else // targetobj�� ���յ� ����� ����� ���ɴϴ�.
            { 
                for (int i = 0; i < recipe_Num.Count; i++)
                {
                    for (int j = 0; j < recipes[recipe_Num[i]].ingredient.Count; j++)
                    {
                        if (!targetObj.name.Contains(recipes[recipe_Num[i]].ingredient[j]))
                        {
                            if (j == recipes[recipe_Num[i]].ingredient.Count)
                            {
                                if(i != recipe_Num.Count)
                                {
                                    Debug.Log($"{recipes[i].recipe}���� {targetObj.name}�� ���� �ʽ��ϴ�.");
                                    break;
                                }
                                else
                                {
                                    Debug.Log("������ ���� �ʽ��ϴ�.");
                                    return false;
                                }
                            }
                        }
                        else
                            i = recipe_Num.Count;
                    }
                }
                for (int i = 0; i < targetObj.transform.childCount; i++)
                {
                    if (targetObj.transform.GetChild(i).name.Equals(targetObj.name))
                    {
                        targetObj.transform.GetChild(i).gameObject.SetActive(false);
                        break;
                    }
                }
                targetObj.name = $"{targetObj.name}_{gameObject.name}";
                int removeCount = recipe_Num.Count;
                for (int i = 0; i < recipe_Num.Count; i++)
                {
                    for (int j = 0; j < recipes[recipe_Num[i]].ingredient.Count; j++)
                    {
                        if (!targetObj.name.Contains(recipes[recipe_Num[i]].ingredient[j]))
                        {
                            break;
                        }
                        if (j == recipes[recipe_Num[i]].ingredient.Count)
                        {
                            recipe_Num.Add(i);
                        }
                    }
                    
                }

                recipe_Num.RemoveRange(0, removeCount);

                for (int i = 0; i < targetObj.transform.childCount; i++)
                {
                    if (targetObj.transform.GetChild(i).name.Equals(targetObj.name))
                    {
                        targetObj.transform.GetChild(i).gameObject.SetActive(true);
                        
                        for(int k = 0; k < recipe_Num.Count;k++)
                        {
                            for (int j = 0; j < recipes[recipe_Num[k]].ingredient.Count; j++)
                            {
                                if (!targetObj.name.Contains(recipes[recipe_Num[k]].ingredient[j]))
                                {
                                    break;
                                }
                                if (j == recipes[recipe_Num[k]].ingredient.Count - 1)
                                {
                                    targetObj.name = recipes[recipe_Num[k]].recipe;
                                    return true;
                                }

                            }
                        }
                        
                        return true;
                    }
                }
                
            }
            
        }
        return false;
    }
}
