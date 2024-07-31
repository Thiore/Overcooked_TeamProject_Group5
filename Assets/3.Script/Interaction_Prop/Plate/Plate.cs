using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Plate : MonoBehaviour
{
    [SerializeField] private Transform[] Sink_Pos;
    [SerializeField] private Mesh[] Plate_Mesh;
    [SerializeField] private GameObject[] RecipeList;//해당맵에 사용되는 모든 재료묶음들

    private Crate_Data Data;

    private MeshFilter mesh;
    private Renderer renderer;
    private MeshCollider meshcol;

    private bool isComplete = false;

    public bool isPlate { get; private set; }
    public bool isWash { get; private set; }//true면 설거지를 해야하는상태 false면 올릴 수 있는상태

    private List<Recipe> recipes;


    private void Awake()
    {
        
        TryGetComponent(out mesh);
        TryGetComponent(out renderer);
        TryGetComponent(out meshcol);
    }

    private void OnEnable()
    {
        isPlate = false;
        Change_Plate(isWash);
        transform.name = "Plate";
        
    }

    private void Start()
    {
        recipes = DataManager.Instance.StageRecipeData(GameManager.Instance.stage_index);
        

        for (int j = 0; j < Data.Info.Length; j++)
        {
            for (int i = 0; i < RecipeList.Length; i++)
            {
                if(RecipeList[i].name.StartsWith(Data.Info[j].Ingredients.ToString()))
                {
                    Instantiate(RecipeList[i], transform.position, transform.rotation, transform);
                }
            }
        }

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

    public bool OnPlate(Ingredient Ingre) // 접시위에 올라가있는 재료
    {
        if (isWash)
            return false;

        if (isComplete)
            return false;

        if (transform.name.Contains(Ingre.name))
            return false;

        string[] ThisName = null;

        if (Ingre.cooking.Equals(eCooked.ReadyCook))
        {
            if (transform.name.Equals("Plate"))
            {
                transform.name = Ingre.name;
                Ingre.gameObject.SetActive(false);
                for (int i = 0; i < recipes.Count; i++)
                {
                    if (recipes[i].ingredient.Count.Equals(1) && recipes[i].ingredient[0].Equals(transform.name))
                    {
                        isComplete = true;
                        isPlate = true;
                        return true;
                    }
                }
            }
            else
            {
                string checkName = $"{transform.name}_{Ingre.name}";
                ThisName = checkName.Split('_');
                int isDisable = transform.childCount;
                int isEnable = transform.childCount;
                for (int i = 0; i < transform.childCount; i++)
                {
                    bool isPass = false;
                    if (transform.GetChild(i).gameObject.activeSelf)
                    {
                        isDisable = i;
                    }
                    else
                    {
                        if (!isEnable.Equals(transform.childCount))
                        {
                            if (transform.GetChild(i).name.Split('_').Length.Equals(ThisName.Length))
                            {

                                for (int j = 0; j < ThisName.Length; j++)
                                {

                                    if (!transform.GetChild(i).name.Contains(ThisName[j]))
                                    {
                                        isPass = true;
                                        break;
                                    }
                                }
                                if (!isPass)
                                {
                                    isEnable = i;

                                }
                            }
                        }


                    }

                    if (!isEnable.Equals(transform.childCount) && !isDisable.Equals(transform.childCount))
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                        transform.name = transform.GetChild(i).name;

                        for (int j = 0; j < recipes.Count; j++)
                        {
                            if (recipes[j].ingredient.Count.Equals(ThisName.Length))
                            {
                                for (int k = 0; k < ThisName.Length; k++)
                                {
                                    if (!recipes[j].ingredient.Contains(ThisName[k]))
                                    {
                                        return true;
                                    }
                                }
                                isComplete = true;
                                transform.name = recipes[j].recipe;
                                return true;
                            }
                        }
                    }

                }
                if (isEnable.Equals(transform.childCount))
                {
                    return false;
                }

            }
        }
        return false;
            
    }

}
