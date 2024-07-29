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

public enum eCookutensils
{
    Normal = 0,
    Pot,
    Pan,
    Fry
}

public enum eCooked
{
    Normal = 0,
    Chopping,
    Cooking,
    ReadyCook,
    trash
}

public class Ingredient : MonoBehaviour
{
    [SerializeField] protected Mesh[] Change_Mesh;
    [SerializeField] protected Material[] Change_Material;

    protected Renderer Ingredient_renderer;
    protected MeshFilter Ingredient_Mesh;
    protected MeshCollider Ingredient_Col;

    public spawn_Ingredient crate;
    
    public eCooked cooking;
    public eCookutensils utensils;
    public eCookingProcess CookProcess;

    protected bool Chop_Anim;

    public float ChopTime;
    protected readonly float FinishChopTime = 4f;


    public bool isChop { get; protected set; }
    public bool isCook { get; protected set; }
    public bool isPlate { get; protected set; }

    public bool OnPlate;

    protected Animator[] playerAnim = new Animator[2];
    protected AnimatorStateInfo[] AnimInfo = new AnimatorStateInfo[2];



    protected virtual void Awake()
    {
        TryGetComponent(out Ingredient_renderer);
        TryGetComponent(out Ingredient_Mesh);
        TryGetComponent(out Ingredient_Col);

        for (int i = 0; i < playerAnim.Length; i++)
        {
            playerAnim[i] = null;
        }
        cooking = eCooked.Normal;
        
    }
    protected virtual void OnEnable()
    {
        
        ChopTime = 0;
        isChop = false;
        isCook = false;
        OnPlate = false;
        if (!Ingredient_Mesh.mesh.Equals(Change_Mesh[0]))
        {
            Change_Ingredient(eCooked.Normal);
        }
        if(CookProcess.Equals(eCookingProcess.Normal))
        {
            cooking = eCooked.ReadyCook;
        }


    }

    private void Update()
    {
        if(isChop)
        {
            Chop();
        }


    }

    public void Change_Ingredient(eCooked cooked)
    {
        cooking = cooked;
        int CookEnum = (int)cooked;
        if (CookEnum > 0)
            CookEnum -= 1;
        Ingredient_Mesh.mesh = Change_Mesh[CookEnum];
        Ingredient_renderer.material = Change_Material[CookEnum];
        Ingredient_Col.sharedMesh = Change_Mesh[CookEnum];
    }

    public void Off_Ingredient()
    {
        Ingredient_Mesh.mesh = null;
        Ingredient_Col.enabled = false;
    }

    public virtual void Change_PlateIngredient()
    {
        OnPlate = true;
    }

    public void SetCookProcess(Crate_Info Info)
    {
        CookProcess = Info.CookingProcess;
        Chop_Anim = Info.Chop_Anim;
        utensils = Info.utensils;
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

    public bool Chopable()
    {
        if (transform.parent.parent.CompareTag("ChoppingBoard"))
        {
            if (cooking.Equals(eCooked.Chopping))
                return true;
            else if (cooking.Equals(eCooked.Normal))
            {
                cooking = eCooked.Chopping;
                isChop = true;
                return true;
            }
        }
        return false;
    }
    public bool Cookable()
    {
        if (transform.parent.parent.CompareTag("Cooker"))
        {
            if (cooking.Equals(eCooked.Cooking))
            {
                isCook = true;
                return true;
            }
            if (CookProcess.Equals(eCookingProcess.Cook))
            {
                if(cooking.Equals(eCooked.Normal))
                {
                    if (Ingredient_Mesh != null)
                        Ingredient_Mesh = null;
                    cooking = eCooked.Cooking;
                    isCook = true;
                    return true;
                }
            }
            if(CookProcess.Equals(eCookingProcess.Chop_Cook))
            {
                if(cooking.Equals(eCooked.Chopping))
                {
                    if (Ingredient_Mesh != null)
                        Ingredient_Mesh = null;
                    cooking = eCooked.Cooking;
                    isCook = true;
                    return true;
                }
            }
            
        }
        return false;
    }
    public void SetisCook() => isCook = !isCook;
    

    private void Chop()
    {
        if (cooking.Equals(eCooked.Chopping))
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

                        if(Chop_Anim)
                            ChildChopAnim(ChopTime);

                        Debug.Log($"잘리는중{ChopTime}");
                        if (ChopTime > FinishChopTime)
                        {
                            isChop = false;
                            ChopTime = 0;
                            if (CookProcess.Equals(eCookingProcess.Chopping))
                                Change_Ingredient(eCooked.ReadyCook);
                            else
                                Change_Ingredient(eCooked.Cooking);

                            playerAnim[i].SetTrigger("Finish");
                            playerAnim[i].transform.GetComponent<Player_StateController>().CleaverOb.SetActive(false);
                        }
                    }
                }

            }
        }
    }

    //private void Trash()
    //{
    //    transform.Rotate(Vector3.up, 2f);
    //    transform.localScale *= 0.98f;
    //    if (transform.localScale.x < 0.2f)
    //    {
    //        transform.parent.TryGetComponent(out CounterController counter);
    //        if (counter != null)
    //        {
    //            counter.ChangePuton();
    //            counter.PutOnOb = null;
    //        }
    //        Die();
    //    }
    //}
    protected virtual void ChildChopAnim(float chopTime)
    {

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

    public virtual void Die()
    {
        transform.parent = null;
        gameObject.SetActive(false);
        transform.position = crate.transform.position;
        transform.localScale = new Vector3(1f, 1f, 1f);
        crate.DestroyIngredient(this);
    }
}
