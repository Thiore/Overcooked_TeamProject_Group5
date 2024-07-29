using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Ingredient
{
    //[SerializeField] private Mesh[] Change_Mesh;
    //[SerializeField] private Material[] Change_Material;

    [SerializeField] private Transform[] CotrolBone;
    [SerializeField] private Transform[] JointBone;
    private Transform[] CopyBone;

    private Renderer Ingredient_renderer;
    private MeshFilter Ingredient_Mesh;
    private MeshCollider Ingredient_Col;

    private void Awake()
    {
        //TryGetComponent(out Ingredient_renderer);
        //TryGetComponent(out Ingredient_Mesh);
        //TryGetComponent(out Ingredient_Col);
        if (Chop_Anim)
        {
            CopyBone = JointBone;
        }


        //for (int i = 0; i < playerAnim.Length; i++)
        //{
        //    playerAnim[i] = null;
        //}
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

    private void Update()
    {
        //if (transform.parent != null)
        //{
        //    if (transform.parent.CompareTag("ChoppingBoard"))
        //    {
        //        if (isChopping)
        //        {
        //            if (cooking.Equals(eCooked.Normal))
        //            {
        //                cooking = eCooked.Chopping;
        //            }
        //            if (cooking.Equals(eCooked.Chopping))
        //            {
        //                for (int i = 0; i < playerAnim.Length; i++)
        //                {
        //                    if (playerAnim[i] != null)
        //                    {
        //                        AnimInfo[i] = playerAnim[i].GetCurrentAnimatorStateInfo(0);
        //                        if (AnimInfo[i].IsName("New_Chef@Chop"))
        //                        {
        //                            if (playerAnim[i] != null)
        //                                ChopTime += Time.deltaTime;
        //                            Debug.Log($"잘리는중{ChopTime}");
        //                            if (ChopTime > FinishChopTime)
        //                            {
        //                                ChopTime = 0;
        //                                Change_Ingredient(eCooked.Cooking);
        //                                playerAnim[i].SetTrigger("Finish");
        //                                playerAnim[i].transform.GetComponent<Player_StateController>().CleaverOb.SetActive(false);
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //        return;
        //    }
        //}

      
                            ChopTime += Time.deltaTime;
                        Debug.Log($"잘리는중{ChopTime}");
                        if (ChopTime > FinishChopTime)
                        {
                            ChopTime = 0;
                            Change_Ingredient(eCooked.Cooking);
                            
                        }
            
        
    }

    //private void ChopAnim()
    //{
    //    int LastTime = 1;
    //    if(ChopTime*2f>Mathf.Ceil(ChopTime*2f))
    //        {

    //    }
    //}

    //private IEnumerator ChopAnim_co(int)
    //{

    //}

    private void Change_Ingredient(eCooked cooked)
    {
        cooking = cooked;
        int CookEnum = (int)cooked;
        if (CookEnum > 0)
            CookEnum -= 1;
        //Ingredient_Mesh.mesh = Change_Mesh[CookEnum];
        //Ingredient_renderer.material = Change_Material[CookEnum];
        //Ingredient_Col.sharedMesh = Change_Mesh[CookEnum];
        
    }

    public override void Die()
    {

    }
}
