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

    [SerializeField] protected Renderer Ingredient_renderer = null;
    [SerializeField] protected MeshFilter Ingredient_Mesh = null;
    [SerializeField] protected MeshCollider Ingredient_Col = null;

    public spawn_Ingredient crate;
    
    public eCooked cooking { get; protected set; }
    public eCookutensils utensils { get; protected set; }
    public eCookingProcess CookProcess { get; protected set; }

    protected bool Chop_Anim;

    public float ChopTime;
    protected readonly float FinishChopTime = 8f;


    public bool isChop { get; protected set; }
    public bool isCook { get; protected set; }
    public bool isPlate { get; protected set; }

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
       
       if(crate != null)
        {
            Change_Ingredient(eCooked.Normal);

            if (CookProcess.Equals(eCookingProcess.Normal))
            {
                cooking = eCooked.ReadyCook;
            }
        }

       
       
    }

    protected virtual void Start()
    {

        ChopTime = 0;
        isChop = false;
        isCook = false;


        Change_Ingredient(eCooked.Normal);

        if (CookProcess.Equals(eCookingProcess.Normal))
        {
            cooking = eCooked.ReadyCook;
        }

        
    }

    protected void Update()
    {
        if (isChop)
        {
            Chop();
        }
       
    }

    public virtual void Change_Ingredient(eCooked cooked)
    {
        cooking = cooked;
        int CookEnum = (int)cooked;
        if (CookEnum > 0)
            CookEnum -= 1;
        if (CookProcess.Equals(eCookingProcess.Chopping))
        {
            if(cooking. Equals(eCooked.ReadyCook))
            {
                CookEnum = 1;
            }
        }
        Ingredient_Mesh.mesh = Change_Mesh[CookEnum];
        Ingredient_renderer.material = Change_Material[CookEnum];
        Ingredient_Col.sharedMesh = Change_Mesh[CookEnum];
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
        Debug.Log("1");
        if (CookProcess.Equals(eCookingProcess.Chopping)||CookProcess.Equals(eCookingProcess.Chop_Cook))
        {
            Debug.Log("2");
            if (cooking.Equals(eCooked.Chopping))
                return true;
            else if (cooking.Equals(eCooked.Normal))
            {
                Debug.Log("3");
                if (Chop_Anim)
                {
                    Chop_Change_obj();
                }
                cooking = eCooked.Chopping;
                isChop = true;
                return true;
            }
        }
        Debug.Log("4");

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

    public void SetReadyCook()
    {
        cooking = eCooked.ReadyCook;
    }

    public void SetTrash()
    {
        cooking = eCooked.trash;
    }

    protected virtual void Chop_Change_obj()
    {

    }
    

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

                        Debug.Log($"�߸�����{ChopTime}");
                        if (ChopTime > FinishChopTime)
                        {
                            isChop = false;
                            playerAnim[i].SetTrigger("Finish");
                            playerAnim[i].transform.GetComponent<PlayerStateControl>().Cleaver.SetActive(false);
                            ChopTime = 0;
                            if (!Chop_Anim)
                            {
                                if (CookProcess.Equals(eCookingProcess.Chopping))
                                    Change_Ingredient(eCooked.ReadyCook);
                                else
                                    Change_Ingredient(eCooked.Cooking);
                            }
                            else
                            {
                                if (CookProcess.Equals(eCookingProcess.Chopping))
                                    cooking = eCooked.ReadyCook;
                                else
                                    cooking = eCooked.Cooking;
                            }
                               

                            
                            
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
                    playerAnim[i].transform.GetComponent<PlayerStateControl>().Cleaver.SetActive(false);
                    playerAnim[i] = null;
                    return;
                }
            }

        }

    }

    public void Die()
    {
        ChopTime = 0f;
        cooking = eCooked.Normal;
        gameObject.SetActive(false);
        transform.SetParent(null);
        
        transform.position = crate.transform.position;
        transform.localScale = new Vector3(1f, 1f, 1f);
        Ingredient_Col.enabled = true;
        crate.DestroyIngredient(this);
    }
}
