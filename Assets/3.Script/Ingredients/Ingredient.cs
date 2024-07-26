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
    Pan
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
    public spawn_Ingredient crate;
    
    public eCooked cooking;
    public eIngredients myIngredients;
    public eCookutensils utensils;
    protected eCookingProcess CookProcess;

    protected bool Chop_Anim;

    public float ChopTime;
    protected readonly float FinishChopTime = 4f;
    public bool isPlate { get; private set; }
    public bool isChopping { get; private set; }
    public bool isCooker { get; private set; }

    protected Animator[] playerAnim = new Animator[2];
    protected AnimatorStateInfo[] AnimInfo = new AnimatorStateInfo[2];



    private void Awake()
    {
        for (int i = 0; i < playerAnim.Length; i++)
        {
            playerAnim[i] = null;
        }
        cooking = eCooked.Normal;
        
    }
    private void OnEnable()
    {
        
        ChopTime = 0;


        //if (!Ingredient_Mesh.mesh.Equals(Change_Mesh[0]))
        //{
        //    Change_Ingredient(eCooked.Normal);
        //    Debug.Log("들어오면안됨");
        //}


    }

    //private void Update()
    //{
    //    if (transform.parent != null)
    //    {
    //        if (transform.parent.CompareTag("ChoppingBoard"))
    //        {
    //            if (isChopping)
    //            {
    //                if (cooking.Equals(eCooked.Normal))
    //                {
    //                    cooking = eCooked.Chopping;
    //                }
    //                if (cooking.Equals(eCooked.Chopping))
    //                {
    //                    for (int i = 0; i < playerAnim.Length; i++)
    //                    {
    //                        if (playerAnim[i] != null)
    //                        {
    //                            AnimInfo[i] = playerAnim[i].GetCurrentAnimatorStateInfo(0);
    //                            if (AnimInfo[i].IsName("New_Chef@Chop"))
    //                            {
    //                                if (playerAnim[i] != null)
    //                                    ChopTime += Time.deltaTime;
    //                                Debug.Log($"잘리는중{ChopTime}");
    //                                if (ChopTime > FinishChopTime)
    //                                {
    //                                    ChopTime = 0;
    //                                    Change_Ingredient(eCooked.Cooking);
    //                                    playerAnim[i].SetTrigger("Finish");
    //                                    playerAnim[i].transform.GetComponent<Player_StateController>().CleaverOb.SetActive(false);
    //                                }
    //                            }
    //                        }

    //                    }
    //                }
    //            }
    //            return;
    //        }
    //        else if (transform.parent.CompareTag("Cooker") && Ingredient_Mesh != null)
    //        {
    //            Ingredient_Mesh = null;
    //        }
    //        if (transform.parent.CompareTag("TrashCan"))
    //        {
    //            transform.Rotate(Vector3.up, 2f);
    //            transform.localScale *= 0.98f;
    //            if (transform.localScale.x < 0.2f)
    //            {
    //                transform.parent.TryGetComponent(out CounterController counter);
    //                if (counter != null)
    //                {
    //                    counter.ChangePuton();
    //                    counter.PutOnOb = null;
    //                }
    //                Die();
    //            }

    //        }
    //    }

    //}

    public void Change_Ingredient(eCooked cooked)
    {
        cooking = cooked;
        int CookEnum = (int)cooked;
        if (CookEnum > 0)
            CookEnum -= 1;
        //Ingredient_Mesh.mesh = Change_Mesh[CookEnum];
        //Ingredient_renderer.material = Change_Material[CookEnum];
        //Ingredient_Col.sharedMesh = Change_Mesh[CookEnum];
        OnIngredients();
    }

    public void SetCookProcess(eCookingProcess process, bool Anim, eIngredients ingredient)
    {
        CookProcess = process;
        Chop_Anim = Anim;
        myIngredients = ingredient;
        OnIngredients();
    }

    protected void OnIngredients()
    {
        switch (CookProcess)
        {
            case eCookingProcess.Normal:
                isPlate = true;
                isChopping = false;
                isCooker = false;
                break;
            case eCookingProcess.Chopping:
                if(cooking.Equals(eCooked.Normal))
                {
                    isCooker = false;
                    isChopping = true;
                    isPlate = false;
                }
                else if(cooking.Equals(eCooked.Chopping))
                {
                    isCooker = false;
                    isChopping = true;
                    isPlate = false;
                }
                else
                {
                    isCooker = false;
                    isChopping = false;
                    isPlate = true;
                }
                break;
            case eCookingProcess.Chop_Cook:
                if(cooking.Equals(eCooked.Normal))
                {
                    isCooker = false;
                    isChopping = true;
                    isPlate = false;
                }
                else if(cooking.Equals(eCooked.Chopping))
                {
                    isCooker = false;
                    isChopping = true;
                    isPlate = false;
                }
                else if(cooking.Equals(eCooked.Cooking))
                {
                    isCooker = true;
                    isChopping = false;
                    isPlate = false;
                }
                else if(cooking.Equals(eCooked.ReadyCook))
                {
                    isCooker = false;
                    isChopping = false;
                    isPlate = true;
                }
                else
                {
                    isCooker = false;
                    isChopping = false;
                    isPlate = false;
                }
                break;
            case eCookingProcess.Cook:
                if (cooking.Equals(eCooked.Normal))
                {
                    isCooker = true;
                    isChopping = false;
                    isPlate = false;
                }
                else if(cooking.Equals(eCooked.ReadyCook))
                {
                    isCooker = false;
                    isChopping = false;
                    isPlate = true;
                }
                else if (cooking.Equals(eCooked.trash))
                {
                    isCooker = false;
                    isChopping = false;
                    isPlate = false;
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

    //public bool Chop()
    //{

    //}

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
        if (transform.parent != null)
            transform.parent = null;
        gameObject.SetActive(false);
        transform.position = crate.transform.position;
        transform.localScale = new Vector3(1f, 1f, 1f);
        crate.DestroyIngredient(this);
    }
}
