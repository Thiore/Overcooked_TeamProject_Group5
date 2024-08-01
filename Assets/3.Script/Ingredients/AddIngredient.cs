//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AddIngredient : MonoBehaviour
//{
//    public bool isPlate { get; private set; }
//    public string LastName;
    
//    private List<Recipe> recipes;
//    public spawn_Ingredient crate;
//    private GameObject[] AddIngrePrefabs;
//    private Ingredient Ingre;
//    public Crate_Info[] info;

//    private bool ChangeRecipe = false;

//    private bool isTrash;

//    //private void Awake()
//    //{
//    //    GameObject AddObj = new GameObject("AddObj");
//    //    AddObj.transform.SetParent(transform);
//    //    AddObj.transform.position = transform.position;
//    //    recipes = DataManager.Instance.StageRecipeData(GameManager.Instance.stage_index);
//    //    crate = FindObjectOfType<spawn_Ingredient>();
//    //    info = crate.Data.Info;
//    //    AddIngrePrefabs = crate.AddIngredient_Prefabs;
//    //    for (int i = 0; i < crate.Data.Info.Length;i++)
//    //    {
//    //        for(int j = 0; j < AddIngrePrefabs.Length; j ++)
//    //        {
//    //            if(AddIngrePrefabs[j].name.StartsWith(info[i].Ingredients.ToString()))
//    //            {
//    //                GameObject obj = Instantiate(AddIngrePrefabs[j], transform.GetChild(0).position, transform.GetChild(0).rotation, transform.GetChild(0));
//    //                obj.SetActive(false);
//    //                RecipeList.Add(obj);
//    //            }
//    //        }
//    //    }

//    //}

//    private void OnEnable()
//    {
//        isPlate = false;
//        isTrash = false;
//    }
//    private void Start()
//    {
//        if(!transform.childCount.Equals(0))
//        {
//            transform.GetChild(0).TryGetComponent(out Ingre);
//        }
//    }
    

//    //public bool OnIngredient(AddIngredient Ontarget)//어딘가에 올라가있는 재료
//    //{
//    //    if (!Addreturn(Ontarget))
//    //        return false;


//    //    if (Ingre.cooking.Equals(eCooked.ReadyCook))
//    //    {
//    //            string[] target = Ontarget.transform.name.Split('_');
//    //            string[] This = gameObject.name.Split('_');


//    //            for (int i = 0; i < target.Length; i++)
//    //            {
//    //                for (int j = 0; j < This.Length; j++)
//    //                {
//    //                    if (target[i].Equals(This[j])) // 동일한 재료가 있다면 false 반환
//    //                    {
//    //                        return false;
//    //                    }
//    //                }
//    //            }

//    //            for (int i = 0; i < RecipeList.Count; i++)
//    //            {
//    //                string[] Recipe_ingre = RecipeList[i].name.Split('_');
//    //                if (Recipe_ingre.Length.Equals(target.Length + This.Length))
//    //                {
//    //                    int CollectCount = 0;
//    //                    for (int j = 0; j < Recipe_ingre.Length; j++)
//    //                    {
//    //                        for (int k = 0; k < target.Length; k++)
//    //                        {
//    //                            if (Recipe_ingre[j].Equals(target[k]))
//    //                            {
//    //                                CollectCount += 1;
//    //                            }
//    //                        }
//    //                        for (int k = 0; k < This.Length; k++)
//    //                        {
//    //                            if (Recipe_ingre[j].Equals(This[k]))
//    //                            {
//    //                                CollectCount += 1;
//    //                            }
//    //                        }
//    //                        if (Recipe_ingre.Length.Equals(CollectCount))
//    //                        {
//    //                            Transform targetIngre = Ontarget.transform.GetChild(1);
//    //                            transform.position = targetIngre.position;
//    //                            transform.rotation = targetIngre.rotation;
//    //                            for (int k = 0; k < transform.GetChild(1).childCount; k++)
//    //                            {
//    //                                transform.GetChild(1).GetChild(k).SetParent(targetIngre);
//    //                            }
//    //                            Ontarget.name = RecipeList[i].name;
//    //                            Ontarget.isPlate = true;
//    //                        Ingre.Off_Ingredient();
//    //                        targetIngre.GetChild(0).GetComponent<Ingredient>().Off_Ingredient();
//    //                        return true;
//    //                        }
//    //                    }
//    //                }
//    //                else
//    //                {
//    //                    continue;
//    //                }

//    //            }
//    //            return false;

            

//    //    }

//    //    return false;

//    //}

//    private bool Addreturn(AddIngredient Ontarget)
//    {
//        if (!Ontarget.transform.GetChild(1).GetChild(0).GetComponent<Ingredient>().cooking.Equals(eCooked.ReadyCook))
//        {
//            return false;
//        }



//        Transform currentParent = Ontarget.transform.parent;
//        while (currentParent != null)
//        {
//            currentParent = currentParent.parent;
//            if (currentParent.CompareTag("Tool"))
//            {
//                return false;
//            }
//        }
//        return true;
//    }

//    //public bool OnPlate(Plate targetPlate) // 접시위에 올라가있는 재료
//    //{
//    //    if(isPlate)
//    //        return false;

//    //    if (targetPlate.isWash)
//    //        return false;

//    //    if(Ingre.cooking.Equals(eCooked.ReadyCook))
//    //    {
//    //        if (targetPlate.transform.childCount.Equals(0))
//    //        {
//    //            isPlate = true;
//    //            if(transform.GetChild(1).childCount.Equals(1))
//    //                Ingre.Change_PlateIngredient();
                
//    //            return true;
//    //        }
//    //        else
//    //        {
//    //            string[] target = targetPlate.transform.GetChild(0).name.Split('_');
//    //            string[] This = gameObject.name.Split('_');
                
                
//    //            for(int i = 0; i < target.Length;i++)
//    //            {
//    //                for(int j = 0; j < This.Length;j++)
//    //                {
//    //                    if(target[i].Equals(This[j])) // 동일한 재료가 있다면 false 반환
//    //                    {
//    //                        return false;
//    //                    }
//    //                }
//    //            }
                
                
//    //            for(int i = 0; i < RecipeList.Count;i++)
//    //            {
//    //                string[] Recipe_ingre = RecipeList[i].name.Split('_');
//    //                if (Recipe_ingre.Length.Equals(target.Length + This.Length))
//    //                {
//    //                    int CollectCount = 0;
//    //                    for(int j = 0; j < Recipe_ingre.Length;j++)
//    //                    {
//    //                        for(int k = 0; k < target.Length;k++)
//    //                        {
//    //                            if(Recipe_ingre[j].Equals(target[k]))
//    //                            {
//    //                                CollectCount += 1;
//    //                            }
//    //                        }
//    //                        for (int k = 0; k < This.Length; k++)
//    //                        {
//    //                            if (Recipe_ingre[j].Equals(This[k]))
//    //                            {
//    //                                CollectCount += 1;
//    //                            }
//    //                        }
//    //                        if(Recipe_ingre.Length.Equals(CollectCount))
//    //                        {
                                
//    //                            Transform targetIngre = targetPlate.transform.GetChild(0).GetChild(1);
//    //                            transform.position = targetIngre.position;
//    //                            transform.rotation = targetIngre.rotation;
//    //                            for (int k = 0; k < transform.GetChild(1).childCount;k++)
//    //                            {
//    //                                transform.GetChild(1).GetChild(k).SetParent(targetIngre);
//    //                            }
//    //                            targetPlate.transform.GetChild(0).name = RecipeList[i].name;
//    //                            Ingre.Off_Ingredient();
//    //                            targetIngre.GetChild(0).GetComponent<Ingredient>().Off_Ingredient();

//    //                            for (int k = 0; k < recipes.Count; k++)
//    //                            {
//    //                                if(recipes[k].ingredient.Count.Equals(targetIngre.childCount))
//    //                                {
//    //                                    bool isDifferent = false;
//    //                                    foreach(string recipeIngre in recipes[i].ingredient)
//    //                                    {
//    //                                        foreach(string GetIngre in Recipe_ingre)
//    //                                        {
//    //                                            if(!recipeIngre.Equals(GetIngre))
//    //                                            {
//    //                                                isDifferent = true;
//    //                                                break;
//    //                                            }
//    //                                        }
//    //                                        if (isDifferent)
//    //                                            break;
//    //                                    }
//    //                                    targetPlate.transform.GetChild(0).name = recipes[k].recipe;
//    //                                }
//    //                            }
//    //                            return true;
//    //                        }
//    //                    }
//    //                }
//    //                else
//    //                {
//    //                    continue;
//    //                }
                   
//    //            }
//    //            return false;

//    //        }
                
//    //    }
        
//    //    return false;
//    //}



//    public void Die()
//    {
//        transform.parent = null;
//        gameObject.SetActive(false);
//        transform.position = crate.transform.position;
//        transform.localScale = new Vector3(1f, 1f, 1f);
//        //crate.DestroyAdd(gameObject);
//    }
//}
