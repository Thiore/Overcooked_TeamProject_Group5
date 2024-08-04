using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum eWash
{
    outSink = 0,
    inSink
}

public class Plate : MonoBehaviour
{
    [SerializeField] private Transform[] Sink_Pos;
    [SerializeField] private Mesh[] Plate_Mesh;
    [SerializeField] private GameObject[] RecipeList;
    [SerializeField] private Sink Sink;

    [SerializeField] private Crate_Data Data;

    private MeshFilter mesh;
    private Renderer renderer;
    private MeshCollider meshcol;

    public bool isComplete { get; private set; }

    public bool isPlate { get; private set; }
    //[field: SerializeField] public bool isWash { get; private set; }
    public bool isWash { get; private set; }

    private List<Recipe> recipes;

    protected Animator playerAnim;
    protected AnimatorStateInfo AnimInfo;
    private float washtime;
    private float finishWashtime = 4f;


    private void Awake()
    {
        AnimInfo = new AnimatorStateInfo();

        TryGetComponent(out mesh);
        TryGetComponent(out renderer);
        TryGetComponent(out meshcol);
    }

    private void OnEnable()
    {
        isComplete = false;
        isPlate = false;
        Change_Plate(isWash,eWash.outSink);
        transform.name = "Plate";

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            playerAnim = other.gameObject.GetComponent<Animator>();
        }
    }

    private void Start()
    {
        recipes = DataManager.Instance.StageRecipeData(GameManager.Instance.stage_index);


        for (int j = 0; j < Data.Info.Length; j++)
        {
            for (int i = 0; i < RecipeList.Length; i++)
            {
                if (RecipeList[i].name.StartsWith(Data.Info[j].Ingredients.ToString()))
                {
                    GameObject obj = Instantiate(RecipeList[i], transform.position, transform.rotation, transform);
                    obj.name = RecipeList[i].name;
                    obj.SetActive(false);
                }
            }
        }

    }


    public void SetWash(bool isWash)
    {
        this.isWash = isWash;
    }

    public void Change_Plate(bool isWash, eWash sink)
    {
        if(sink.Equals(eWash.outSink))
        {
            if (!isWash)
            {
                mesh.mesh = Plate_Mesh[0];
                renderer.material.SetFloat("_DetailAlbedoMapScale", 0f);
                meshcol.sharedMesh = Plate_Mesh[0];
            }
            else
            {
                mesh.mesh = Plate_Mesh[0];
                renderer.material.SetFloat("_DetailAlbedoMapScale", 1f);
                meshcol.sharedMesh = Plate_Mesh[0];
            }
        }
        else
        {
            mesh.mesh = Plate_Mesh[1];
            renderer.material.SetFloat("_DetailAlbedoMapScale", 1f);
            meshcol.sharedMesh = Plate_Mesh[1];
        }
        
    }

    public bool OnPlate(Ingredient Ingre)
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
                
                for (int i = 0; i < recipes.Count; i++)
                {
                    if (recipes[i].ingredient.Count.Equals(1) && recipes[i].ingredient[0].Equals(transform.name))
                    {
                        for(int j = 0; j < transform.childCount;j++)
                        {
                            if(transform.GetChild(j).name.Equals(Ingre.name))
                            {
                                transform.GetChild(j).gameObject.SetActive(true);
                                Ingre.Die();
                                break;
                            }
                        }
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
                                Ingre.Die();
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


    public void Wash()
    {
        if (isWash)
        {
            Debug.Log("iswash");
            if (playerAnim != null)
            {
                AnimInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
                if (AnimInfo.IsName("New_Chef@Wash"))
                {
                    washtime += Time.deltaTime;

                    Debug.Log(washtime);

                    if (washtime > finishWashtime)
                    {
                        isWash = false;
                        Change_Plate(isWash,eWash.outSink);
                        washtime = 0;
                        if(Sink.InPlate.Count.Equals(0))
                            playerAnim.SetTrigger("Finish");
                    }
                }
            }
        }
    }
}
