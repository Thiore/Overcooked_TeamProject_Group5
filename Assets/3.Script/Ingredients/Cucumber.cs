using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Ingredient
{
    [SerializeField] private Transform[] JointBone;

    [SerializeField] private SkinnedMeshRenderer ChopIngre_renderer;
    [SerializeField] private MeshCollider ChopIngre_Col;

    

    [SerializeField] private Mesh Chop_Mesh;

    [SerializeField] private Mesh Plate_Mesh;
    [SerializeField] private Material Plate_Material;
    [SerializeField] private GameObject[] Ingre;
    private float LastTime;

    protected override void Awake()
    {
        TryGetComponent(out ChopIngre_renderer);
        Chop_Anim = true;
        
        if (Chop_Anim)
        {
            ChopIngre_renderer.bones = JointBone;
        }
        LastTime = 0;
    }

    protected override void OnEnable()
    {
        ChopTime = 0;
        isChop = false;
        isCook = false;
        
        //ChopIngre_renderer.sharedMesh = null;
        //ChopIngre_Col.enabled = false;
        //if (!ChopIngre_renderer.sharedMesh.Equals(Change_Mesh[0]))
        //{
        //    Change_Ingredient(eCooked.Normal);
        //}
    }

    public virtual void Change_Ingredient(eCooked cooked)
    {
        cooking = cooked;
        int CookEnum = (int)cooked;
        if (CookEnum > 0)
            CookEnum -= 1;


        ChopIngre_renderer.sharedMesh = Change_Mesh[CookEnum];
        ChopIngre_renderer.material = Change_Material[CookEnum];
        ChopIngre_Col.sharedMesh = Change_Mesh[CookEnum];

    }

    protected override void ChildChopAnim(float chopTime)
    {
        if (ChopTime * 2f > LastTime)
        {
            LastTime = Mathf.CeilToInt(ChopTime * 2f);
            StartCoroutine(ChopAnim_co(Mathf.FloorToInt(ChopTime * 2f)));
        }
        if(LastTime>6f)
        {
            LastTime = 0f;
        }
    }

    private IEnumerator ChopAnim_co(int BoneIndex)
    {
        //float ChoppingTime = 0;
        //while(ChoppingTime<1f)
        //{
        //    ChoppingTime += Time.deltaTime;

        //    JointBone[BoneIndex].RotateAround(CotrolBone[BoneIndex].position, CotrolBone[BoneIndex].right, ChoppingTime);
        //    yield return null;
        //}
        yield return null;
    }

    protected override void Chop_Change_obj()
    {
        Ingredient_Mesh.mesh = null;
        Ingredient_Col.enabled = false;
        ChopIngre_renderer.sharedMesh = Chop_Mesh;
        ChopIngre_Col.enabled = true;
    }

    //public override void Change_PlateIngredient()
    //{
    //    OnPlate = true;

    //    if(ChopIngre_renderer.sharedMesh != null)
    //    {
    //        ChopIngre_renderer.sharedMesh = null;
    //        ChopIngre_Col.enabled = false;
    //    }

    //    Ingredient_Mesh.mesh = Plate_Mesh;
    //    Ingredient_renderer.material = Plate_Material;
    //    Ingredient_Col.sharedMesh = Plate_Mesh;
    //}

    protected override void OnDisable()
    {
        //JointBone = CopyBone;
        base.OnDisable();
    }
}
