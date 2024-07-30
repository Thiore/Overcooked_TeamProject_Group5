using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Transform[] Sink_Pos;
    [SerializeField] private Mesh[] Plate_Mesh;
    [SerializeField] private List<GameObject> RecipeList = new List<GameObject>();//�ش�ʿ� ���Ǵ� ��� ��ṭ����

    private MeshFilter mesh;
    private Renderer renderer;
    private MeshCollider meshcol;

    public bool isWash { get; private set; }//true�� �������� �ؾ��ϴ»��� false�� �ø� �� �ִ»���

    private List<Recipe> recipes;


    private void Awake()
    {
        recipes = DataManager.Instance.StageRecipeData(GameManager.Instance.stage_index);
        TryGetComponent(out mesh);
        TryGetComponent(out renderer);
        TryGetComponent(out meshcol);
    }

    private void OnEnable()
    {
        
        Change_Plate(isWash);
        
    }

   
    public void SetWash(bool isWash)
    {
        this.isWash = isWash;
    }

    private void Change_Plate(bool isWash)
    {
        if (!isWash)
        {
            mesh.mesh = Plate_Mesh[0];
            renderer.material.SetFloat("_DetailAlbedoMapScale", 0f);
            meshcol.sharedMesh = Plate_Mesh[0];
        }
        else
        {
            mesh.mesh = Plate_Mesh[1];
            renderer.material.SetFloat("_DetailAlbedoMapScale", 1f);
            meshcol.sharedMesh = Plate_Mesh[1];
        }
    }

    //public bool OnPlate(Plate targetPlate) // �������� �ö��ִ� ���
    //{
    //    if (isPlate)
    //        return false;

    //    if (targetPlate.isWash)
    //        return false;

    //    if (Ingre.cooking.Equals(eCooked.ReadyCook))
    //    {
    //        if (targetPlate.transform.childCount.Equals(0))
    //        {
    //            isPlate = true;
    //            if (transform.GetChild(1).childCount.Equals(1))
    //                Ingre.Change_PlateIngredient();

    //            return true;
    //        }
    //        else
    //        {
    //            string[] target = targetPlate.transform.GetChild(0).name.Split('_');
    //            string[] This = gameObject.name.Split('_');


    //            for (int i = 0; i < target.Length; i++)
    //            {
    //                for (int j = 0; j < This.Length; j++)
    //                {
    //                    if (target[i].Equals(This[j])) // ������ ��ᰡ �ִٸ� false ��ȯ
    //                    {
    //                        return false;
    //                    }
    //                }
    //            }


    //            for (int i = 0; i < RecipeList.Count; i++)
    //            {
    //                string[] Recipe_ingre = RecipeList[i].name.Split('_');
    //                if (Recipe_ingre.Length.Equals(target.Length + This.Length))
    //                {
    //                    int CollectCount = 0;
    //                    for (int j = 0; j < Recipe_ingre.Length; j++)
    //                    {
    //                        for (int k = 0; k < target.Length; k++)
    //                        {
    //                            if (Recipe_ingre[j].Equals(target[k]))
    //                            {
    //                                CollectCount += 1;
    //                            }
    //                        }
    //                        for (int k = 0; k < This.Length; k++)
    //                        {
    //                            if (Recipe_ingre[j].Equals(This[k]))
    //                            {
    //                                CollectCount += 1;
    //                            }
    //                        }
    //                        if (Recipe_ingre.Length.Equals(CollectCount))
    //                        {

    //                            Transform targetIngre = targetPlate.transform.GetChild(0).GetChild(1);
    //                            transform.position = targetIngre.position;
    //                            transform.rotation = targetIngre.rotation;
    //                            for (int k = 0; k < transform.GetChild(1).childCount; k++)
    //                            {
    //                                transform.GetChild(1).GetChild(k).SetParent(targetIngre);
    //                            }
    //                            targetPlate.transform.GetChild(0).name = RecipeList[i].name;
    //                            Ingre.Off_Ingredient();
    //                            targetIngre.GetChild(0).GetComponent<Ingredient>().Off_Ingredient();

    //                            for (int k = 0; k < recipes.Count; k++)
    //                            {
    //                                if (recipes[k].ingredient.Count.Equals(targetIngre.childCount))
    //                                {
    //                                    bool isDifferent = false;
    //                                    foreach (string recipeIngre in recipes[i].ingredient)
    //                                    {
    //                                        foreach (string GetIngre in Recipe_ingre)
    //                                        {
    //                                            if (!recipeIngre.Equals(GetIngre))
    //                                            {
    //                                                isDifferent = true;
    //                                                break;
    //                                            }
    //                                        }
    //                                        if (isDifferent)
    //                                            break;
    //                                    }
    //                                    targetPlate.transform.GetChild(0).name = recipes[k].recipe;
    //                                }
    //                            }
    //                            return true;
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    continue;
    //                }

    //            }
    //            return false;

    //        }

    //    }

    //    return false;
    //}
}
