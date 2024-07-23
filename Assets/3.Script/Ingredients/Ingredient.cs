using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eIngredients
{
    Tomato = 0,
    Onion,
    Potato,
    Dough,
    Seaweed,
    Mushroom,
    Meat,
    Lettuce,
    Rice,
    Flour,
    Bun,
    Fish,
    Pepperoni,
    Cucumber,
    Egg,
    Chicken,
    Tortilla,
    Cheese,
    Carrot,
    Chocolate,
    Honey,
    Pasta,
    Prawn
}

public enum eCookingProcess
{
    Normal = 0,
    Chopping,
    Chop_Cook,
    Cook
}

public enum eCooked
{
    Normal = 0,
    Chopping,
    Cooking,
    ReadyCook
}

public class Ingredient : MonoBehaviour
{
    public spawn_Ingredient crate;
    
    [SerializeField] private Mesh[] Change_Mesh;
    [SerializeField] private Material[] Change_Material;

    [SerializeField] private Transform[] CotrolBone;
    [SerializeField] private Transform[] JointBone;
    private Transform[] CopyBone;


    private Renderer Ingredient_renderer;
    private MeshFilter Ingredient_Mesh;
    private MeshCollider Ingredient_Col;

    public eCooked cooking { get; private set; }
    public eIngredients myIngredients;
    private eCookingProcess CookProcess;

    [SerializeField] private bool Chop_Script;
    private bool Chop_Anim = false;

    private float ChopTime;
    private float FinishChopTime = 4f;
    public bool OnPlate { get; private set; }
    public bool OnChopping { get; private set; }
    public bool OnCooker { get; private set; }

    private Animator[] playerAnim = new Animator[2];
    private AnimatorStateInfo[] AnimInfo = new AnimatorStateInfo[2];



    private void Awake()
    {
        TryGetComponent(out Ingredient_renderer);
        TryGetComponent(out Ingredient_Mesh);
        TryGetComponent(out Ingredient_Col);
        if (JointBone.Length.Equals(0))
        {
            CopyBone = JointBone;
        }


        for (int i = 0; i < playerAnim.Length; i++)
        {
            playerAnim[i] = null;
        }
        cooking = eCooked.Normal;
        
    }
    private void OnEnable()
    {
        
        ChopTime = 0;
        

        if (!Ingredient_Mesh.mesh.Equals(Change_Mesh[0]))
        {
            Change_Ingredient(eCooked.Normal);
            Debug.Log("������ȵ�");
        }
        

    }

    private void Update()
    {
        if (transform.parent != null)
        {
            if (transform.parent.CompareTag("ChoppingBoard"))
            {
                if(OnChopping)
                {
                    if (cooking.Equals(eCooked.Normal))
                    {
                        cooking = eCooked.Chopping;
                    }
                    if(cooking.Equals(eCooked.Chopping))
                    { 
                        for (int i = 0; i < playerAnim.Length; i++)
                        {
                            if (playerAnim[i] != null)
                            {
                                AnimInfo[i] = playerAnim[i].GetCurrentAnimatorStateInfo(0);
                                if (AnimInfo[i].IsName("New_Chef@Chop"))
                                {
                                    if (playerAnim[i] != null)
                                        ChopTime += Time.deltaTime;
                                    Debug.Log($"�߸�����{ChopTime}");
                                    if (ChopTime > FinishChopTime)
                                    {
                                        ChopTime = 0;
                                        Change_Ingredient(eCooked.Cooking);
                                        playerAnim[i].SetTrigger("Finish");
                                    }
                                }
                            }

                        }
                    }
                }
                return;
            }
            else if(transform.parent.CompareTag("Cooker"))
            {
                if(transform.parent.parent != null)
                {
                    //if(transform.parent.parent.CompareTag(""))
                }
            }
            if(transform.parent.CompareTag("TrashCan"))
            {
                transform.Rotate(Vector3.up, 2f);
                transform.localScale *= 0.98f;
                if(transform.localScale.x < 0.2f)
                {
                    transform.parent.TryGetComponent(out CounterController counter);
                    if(counter != null)
                    {
                        counter.ChangePuton();
                        counter.PutOnOb = null;
                    }
                    Die();
                }

            }
        }

    }

    private void Change_Ingredient(eCooked cooked)
    {
        cooking = cooked;
        int CookEnum = (int)cooked;
        if (CookEnum > 0)
            CookEnum -= 1;
        Ingredient_Mesh.mesh = Change_Mesh[CookEnum];
        Ingredient_renderer.material = Change_Material[CookEnum];
        Ingredient_Col.sharedMesh = Change_Mesh[CookEnum];
        OnIngredients();

    }

    public void SetCookProcess(eCookingProcess process, bool Anim, eIngredients ingredient)
    {
        CookProcess = process;
        Chop_Anim = Anim;
        myIngredients = ingredient;
        OnIngredients();
    }

    public void OnIngredients()
    {
        switch (CookProcess)
        {
            case eCookingProcess.Normal:
                OnPlate = true;
                OnChopping = false;
                OnCooker = false;
                break;
            case eCookingProcess.Chopping:
                if(cooking.Equals(eCooked.Normal))
                {
                    OnCooker = false;
                    OnChopping = true;
                    OnPlate = false;
                }
                else if(cooking.Equals(eCooked.Chopping))
                {
                    OnCooker = false;
                    OnChopping = true;
                    OnPlate = false;
                }
                else
                {
                    OnCooker = false;
                    OnChopping = false;
                    OnPlate = true;
                }
                break;
            case eCookingProcess.Chop_Cook:
                if(cooking.Equals(eCooked.Normal))
                {
                    OnCooker = false;
                    OnChopping = true;
                    OnPlate = false;
                }
                else if(cooking.Equals(eCooked.Chopping))
                {
                    OnCooker = false;
                    OnChopping = true;
                    OnPlate = false;
                }
                else if(cooking.Equals(eCooked.Cooking))
                {
                    OnCooker = true;
                    OnChopping = false;
                    OnPlate = false;
                }
                break;
            case eCookingProcess.Cook:
                if (cooking.Equals(eCooked.Normal))
                {
                    OnCooker = true;
                    OnChopping = false;
                    OnPlate = false;
                }
                else
                {
                    OnCooker = false;
                    OnChopping = false;
                    OnPlate = true;
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            for (int i = 0; i < playerAnim.Length; i++)
            {
                if (playerAnim[i] == null)
                {
                    playerAnim[i] = other.gameObject.GetComponent<Animator>();
                    if (playerAnim[0] == playerAnim[1])
                        playerAnim[i] = null;
                    return;
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < playerAnim.Length; i++)
        {
            if (playerAnim[i] != null)
            {
                if (playerAnim[i].gameObject.Equals(other.gameObject))
                {
                    playerAnim[i] = null;
                    return;
                }
            }

        }

    }

    public void Die()
    {
        if (transform.parent != null)
            transform.parent = null;
        gameObject.SetActive(false);
        transform.position = crate.transform.position;
        transform.localScale = new Vector3(1f, 1f, 1f);
        crate.DestroyIngredient(this);
    }
}
