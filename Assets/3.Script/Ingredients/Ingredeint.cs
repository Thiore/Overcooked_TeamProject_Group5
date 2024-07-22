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
    Chopping
}

public class Ingredeint : MonoBehaviour
{
    public spawn_Ingredient crate;
    
    [SerializeField] private Mesh[] Change_Mesh;
    [SerializeField] private Material[] Change_Material;

    [SerializeField] private Transform[] CotrolBone;
    [SerializeField] private Transform[] JointBone;
    private Transform[] CopyBone;


    private Renderer Ingredient_renderer;
    private MeshFilter Ingredient_Mesh;

    public eCooked cooking { get; private set; }
    // public eIngredients myIngredients;
    private eCookingProcess CookProcess;

    [SerializeField] private bool Chop_Script;
    private bool Chop_Anim = false;

    private float ChopTime;
    private float FinishChopTime = 4f;
    public bool OnPlate { get; private set; }
    public bool OnCooker { get; private set; }

    private Animator[] playerAnim = new Animator[2];
    private AnimatorStateInfo[] AnimInfo = new AnimatorStateInfo[2];



    private void Awake()
    {
        TryGetComponent(out Ingredient_renderer);
        TryGetComponent(out Ingredient_Mesh);
        if (JointBone.Length.Equals(0))
        {
            CopyBone = JointBone;
        }


        for (int i = 0; i < playerAnim.Length; i++)
        {
            playerAnim[i] = null;
        }
        cooking = eCooked.Normal;
        OnIngredients();
    }
    private void OnEnable()
    {
        
        ChopTime = 0;
        

        if (!Ingredient_Mesh.mesh.Equals(Change_Mesh[0]))
        {
            Change_Ingredient(eCooked.Normal);
            Debug.Log("들어오면안됨");
        }
        

    }

    private void Update()
    {
        if (transform.parent != null)
        {
            if (transform.parent.CompareTag("ChoppingBoard"))
            {
                if (cooking.Equals(eCooked.Normal))
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
                                Debug.Log($"잘리는중{ChopTime}");
                                if (ChopTime > FinishChopTime)
                                {
                                    ChopTime = 0;
                                    Change_Ingredient(eCooked.Chopping);
                                }
                            }
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < playerAnim.Length; i++)
                    {
                        if (playerAnim[i] != null)
                        {
                            Debug.Log("어디에들어오니?");
                            AnimInfo[i] = playerAnim[i].GetCurrentAnimatorStateInfo(0);
                            if (AnimInfo[i].IsName("New_Chef@Chop"))
                            {
                                playerAnim[i].SetTrigger("Finish");
                            }
                        }
                    }
                }
                return;
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

    public void Change_Ingredient(eCooked cooked)
    {
        cooking = cooked;
        int CookEnum = (int)cooked;

        Ingredient_Mesh.mesh = Change_Mesh[CookEnum];
        Ingredient_renderer.material = Change_Material[CookEnum];
        OnIngredients();

    }

    public void SetCookProcess(eCookingProcess process, bool Anim)
    {
        CookProcess = process;
        Chop_Anim = Anim;
    }

    public void OnIngredients()
    {
        switch (CookProcess)
        {
            case eCookingProcess.Normal:
                OnPlate = true;
                OnCooker = false;
                break;
            case eCookingProcess.Chopping:
                if(cooking.Equals(eCooked.Normal))
                {
                    OnCooker = false;
                    OnPlate = false;
                }
                else
                {
                    OnCooker = false;
                    OnPlate = true;
                }
                break;
            case eCookingProcess.Chop_Cook:
                if(cooking.Equals(eCooked.Normal))
                {
                    OnCooker = false;
                    OnPlate = false;
                }
                else
                {
                    OnCooker = true;
                    OnPlate = false;
                }
                break;
            case eCookingProcess.Cook:
                if (cooking.Equals(eCooked.Normal))
                {
                    OnCooker = true;
                    OnPlate = false;
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
                    playerAnim[i].SetTrigger("Finish");
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
