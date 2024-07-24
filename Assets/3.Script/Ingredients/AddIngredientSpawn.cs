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

    public bool SetAddIngredient(GameObject targetObj) //false가 반환되면 합치지못함
    {// targetObj는 접시 또는 counter위에 올라가있는 재료입니다.(tag가 ingredient인것을 넣어주면 될것같습니다.)
        if(Ingre.OnPlate)
        {
            if (targetObj.TryGetComponent(out Ingredient targetIngre))
            {
                targetObj.TryGetComponent(out AddIngredientSpawn targetadd);
                for (int i = 0; i < targetadd.recipe_Num.Count; i++)
                {
                    if (targetadd.recipes[recipe_Num[i]].recipe.Equals(targetObj.name))
                    {
                        Debug.Log($"{targetObj.name}은(는) 이미 완성된 레시피 입니다.");
                        return false;
                    }
                }
                for(int i = 0; i < recipe_Num.Count; i++)
                {
                    if(recipes[recipe_Num[i]].recipe.Equals(gameObject.name))
                    {
                        Debug.Log($"{gameObject.name}은(는) 이미 완성된 레시피 입니다.");
                    }
                }

                if (!targetIngre.OnPlate)
                {
                    Debug.Log($"{targetObj.transform.parent.name}위에 있는 {targetObj.name}이 아직 손질되지 않았습니다.");
                    return false;
                }

                if (targetObj.name.Contains(gameObject.name))
                {
                    Debug.Log("동일한 재료입니다.");
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
            else // targetobj가 조합된 재료라면 여기로 들어옵니다.
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
                                    Debug.Log($"{recipes[i].recipe}에는 {targetObj.name}이 들어가지 않습니다.");
                                    break;
                                }
                                else
                                {
                                    Debug.Log("조합이 맞지 않습니다.");
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
