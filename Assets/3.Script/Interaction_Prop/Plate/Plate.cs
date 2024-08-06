using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum eWash
{
    outSink = 0,
    inSink
}

public class Plate : MonoBehaviour
{
    private Transform[] Sink_Pos = new Transform[2];
    [SerializeField] private Mesh[] Plate_Mesh;
    [SerializeField] private GameObject[] RecipeList;
    [SerializeField] private Sink Sink;

    [SerializeField] private Crate_Data Data;

    private MeshFilter mesh;
    private Renderer _renderer;
    private MeshCollider meshcol;

    public bool isComplete = false;

    public bool isPlate = false;
    //[field: SerializeField] public bool isWash { get; private set; }
    public bool isWash = false;

    private List<Recipe> recipes;

    protected Animator playerAnim;
    protected AnimatorStateInfo AnimInfo;
    private float washtime;
    private float finishWashtime = 4f;
    private Slider slide = null;


    private void Awake()
    {
        if(Sink!=null)
        {
            Sink_Pos[0] = Sink.transform.GetChild(0).GetChild(0);
            Sink_Pos[1] = Sink.transform.GetChild(0);
        }
       
        AnimInfo = new AnimatorStateInfo();

        TryGetComponent(out mesh);
        TryGetComponent(out _renderer);
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
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            playerAnim = null;
        }

    }

    private void Start()
    {
        recipes = DataManager.Instance.StageRecipeData(GameManager.Instance.stage_index);



        for (int i = 0; i < RecipeList.Length; i++)
        {

            GameObject obj = Instantiate(RecipeList[i], transform.position, transform.rotation, transform);
            obj.name = RecipeList[i].name;
            obj.SetActive(false);
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
                _renderer.material.SetFloat("_DetailAlbedoMapScale", 0f);
                meshcol.sharedMesh = Plate_Mesh[0];
            }
            else
            {
                mesh.mesh = Plate_Mesh[0];
                _renderer.material.SetFloat("_DetailAlbedoMapScale", 1f);
                meshcol.sharedMesh = Plate_Mesh[0];
            }
        }
        else
        {
            mesh.mesh = Plate_Mesh[1];
            _renderer.material.SetFloat("_DetailAlbedoMapScale", 1f);
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
                            if(transform.GetChild(j).name.Equals($"{transform.name}_Food"))
                            {
                                transform.GetChild(j).gameObject.SetActive(true);
                                transform.name = transform.name + "_Food";
                                Ingre.Die();
                                break;
                            }
                        }
                        isComplete = true;
                        isPlate = true;
                        return true;
                    }
                }
                for (int j = 0; j < transform.childCount; j++)
                {
                    if (transform.GetChild(j).name.Equals(Ingre.name))
                    {
                        transform.GetChild(j).gameObject.SetActive(true);
                        Ingre.Die();
                        break;
                    }
                }

                isPlate = true;
                return true;
            }
            else
            {
                string checkName = $"{transform.name}_{Ingre.name}";
                ThisName = checkName.Split('_');
                string[] RecipeArray = default;
                string recipe = default;
                for (int j = 0; j < recipes.Count; j++)
                {
                    recipe = recipes[j].recipe;
                    RecipeArray = recipes[j].ingredient.ToArray();
                    bool isSelect = true;
                    for (int k = 0; k < ThisName.Length; k++)
                    {
                        if (!RecipeArray.Contains(ThisName[k]))
                        {
                            isSelect = false;
                            break;
                        }
                    }
                    if(isSelect)
                    {
                        break;
                    }
                    if(j.Equals(recipes.Count-1))
                    {
                        return false;
                    }
                    
                }
                if (new HashSet<string>(RecipeArray).SetEquals(ThisName))
                {

                    isComplete = true;
                    transform.name = recipe;
                    for (int j = 0; j < transform.childCount; j++)
                    {
                        if (transform.GetChild(j).name.Equals(recipe))
                        {
                            transform.GetChild(j).gameObject.SetActive(true);
                        }
                        else
                        {
                            transform.GetChild(j).gameObject.SetActive(false);
                        }
                    }
                    return true;
                }

                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).name.Equals(recipe))
                        continue;
                    string[] IngreArray = transform.GetChild(i).name.Split('_');

                    if (IngreArray.Length.Equals(ThisName.Length))
                    {
                        if (new HashSet<string>(IngreArray).SetEquals(ThisName))
                        {
                            transform.GetChild(i).gameObject.SetActive(true);
                            transform.name = transform.GetChild(i).name;
                        }
                        else
                        {
                            transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                

                return true;
                    

            }
        }
        return false;

    }


    public void Wash(Animator Anim, GameObject SlideObj)
    {
        if (isWash)
        {
            if (Anim != null)
            {
                AnimInfo = Anim.GetCurrentAnimatorStateInfo(0);
                if (AnimInfo.IsName("New_Chef@Wash"))
                {
                    if(!SlideObj.activeSelf||slide == null)
                    {
                        SlideObj.TryGetComponent(out slide);
                        slide.maxValue = finishWashtime;
                        slide.value = washtime;
                        SlideObj.SetActive(true);

                    }

                    washtime += Time.deltaTime;
                    slide.value = washtime;
                    Debug.Log(washtime);

                    if (washtime > finishWashtime)
                    {
                        isWash = false;
                        Change_Plate(isWash,eWash.outSink);
                        SlideObj.SetActive(false);
                        slide = null;
                        washtime = 0;
                        if(Sink.InPlate.Count.Equals(0))
                            Anim.SetTrigger("Finish");
                        Sink.sinkAnim = null;
                        AnimInfo = default;
                    }
                }
            }
        }
    }
}
