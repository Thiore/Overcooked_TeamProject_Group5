using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddIngredientSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] AddIngrePrefabs;
    [SerializeField] private Crate_Data Ingredients_Info;

    private Ingredient Ingre;

    private List<Recipe> recipes;
    private List<GameObject> IngreList = new List<GameObject>();

    private void Awake()
    {
        TryGetComponent(out Ingre);
        recipes = DataManager.Instance.StageRecipeData(1);

        for(int i = 0; i < Ingredients_Info.Info.Length; i++)
        {
           for(int j = 0; j < AddIngrePrefabs.Length; i++)
            {
                if (AddIngrePrefabs[j].name.StartsWith(Ingredients_Info.Info[i].Ingredients.ToString()))
                {
                    IngreList.Add(AddIngrePrefabs[j]);
                }
            }
                
        }
    }

    public bool SetAddIngredient(GameObject targetObj) //false가 반환되면 합치지못함
    {
        if(Ingre.OnPlate)
        {
            int recipe_Num = 0;
            if (targetObj.TryGetComponent(out Ingredient targetIngre))
            {

                if (!targetIngre.OnPlate)
                {
                    Debug.Log($"{targetObj.name}이 아직 손질되지 않았습니다.");
                    return false;
                }

                if (targetObj.name.Contains(gameObject.name))
                {
                    Debug.Log("동일한 재료입니다.");
                    return false;
                }

                for (int i = 0; i < recipes.Count; i++)
                {
                    for (int j = 0; j < recipes[i].ingredient.Count; j++)
                    {
                        if (targetObj.name.Contains(recipes[i].ingredient[j]))
                        {
                            if (recipes[i].ingredient.Contains(gameObject.name))
                            {
                                recipe_Num = i;
                                i = recipes.Count;
                                break;
                            }
                            else
                            {
                                Debug.Log("레시피가 다릅니다.");
                                return false;
                            }

                        }

                    }
                }
                GameObject Obj = new GameObject($"{targetObj.name}_{gameObject.name}");
                
                TryGetComponent(out SphereCollider targetcol);
                SphereCollider newcol = Obj.AddComponent<SphereCollider>();
                newcol.center = targetcol.center;
                newcol.radius = targetcol.radius;
                newcol.isTrigger = targetcol.isTrigger;
                targetObj.TryGetComponent(out AddIngredientSpawn targetadd);
                for (int i = 0; i < targetadd.IngreList.Count; i++)
                {
                    if (targetadd.IngreList[i].name.StartsWith(Obj.name))
                    {
                        GameObject childObj = Instantiate(IngreList[i], Vector3.zero, Quaternion.identity, Obj.transform);
                        if (!childObj.name.Equals(Obj.name))
                        {
                            childObj.SetActive(false);
                        }
                    }
                }
                targetIngre.Die();
                if (recipes[recipe_Num].ingredient.Count.Equals(2))
                {
                    for (int i = 0; i < recipes[recipe_Num].ingredient.Count; i++)
                    {
                        if (!Obj.name.Contains(recipes[recipe_Num].ingredient[i]))
                        {
                            break;
                        }
                        if (i == recipes[recipe_Num].ingredient.Count - 1)
                        {
                            Obj.name = recipes[recipe_Num].recipe;
                            break;
                        }
                    }
                }
                return true;

            }
            else
            {
                
                for (int i = 0; i < recipes.Count; i++)
                {
                    for (int j = 0; j < recipes[i].ingredient.Count; j++)
                    {
                        if (targetObj.name.Contains(recipes[i].ingredient[j]))
                        {
                            if (recipes[i].ingredient.Contains(gameObject.name))
                            {
                                i = recipes.Count;
                                recipe_Num = i;
                                break;
                            }
                            else
                            {
                                Debug.Log("레시피가 다릅니다.");
                                return false;
                            }
                        }
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
                for (int i = 0; i < targetObj.transform.childCount; i++)
                {
                    if (targetObj.transform.GetChild(i).name.Equals(targetObj.name))
                    {
                        targetObj.transform.GetChild(i).gameObject.SetActive(true);
                        for(int j = 0; j < recipes[recipe_Num].ingredient.Count;j++)
                        {
                            if(!targetObj.name.Contains(recipes[recipe_Num].ingredient[j]))
                            {
                                break;
                            }
                            if(j == recipes[recipe_Num].ingredient.Count-1)
                            {
                                targetObj.name = recipes[recipe_Num].recipe;
                                break;
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
