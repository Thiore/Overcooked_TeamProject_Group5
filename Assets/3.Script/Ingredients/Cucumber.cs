using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Ingredient
{

    [SerializeField] private Transform[] CotrolBone;
    [SerializeField] private Transform[] JointBone;
    private Transform[] CopyBone;

    [SerializeField] private Mesh Plate_Mesh;
    [SerializeField] private Material Plate_Material;
    [SerializeField] private GameObject[] Ingre;
    private float LastTime;

    protected override void Awake()
    {
        if (Chop_Anim)
        {
            CopyBone = JointBone;
        }
        LastTime = 0;
    }

    protected override void OnEnable()
    {
        ChopTime = 0;
        isChop = false;
        isCook = false;
        OnPlate = false;
        if (!Ingredient_Mesh.mesh.Equals(Change_Mesh[0]))
        {
            Change_Ingredient(eCooked.Normal);
        }
        if (CookProcess.Equals(eCookingProcess.Normal))
        {
            cooking = eCooked.ReadyCook;
        }
    }

    protected override void ChildChopAnim(float chopTime)
    {
        if (ChopTime * 2f > LastTime)
        {
            LastTime = Mathf.CeilToInt(ChopTime * 2f);
            StartCoroutine(ChopAnim_co(Mathf.FloorToInt(ChopTime * 2f)));
        }
        if(LastTime>7f)
        {
            LastTime = 0f;
        }
    }

    private IEnumerator ChopAnim_co(int BoneIndex)
    {
        float ChoppingTime = 0;
        while(ChoppingTime<1f)
        {
            Vector3.Lerp(JointBone[BoneIndex].position, CotrolBone[BoneIndex].position, ChoppingTime);
            Vector3.Slerp(JointBone[BoneIndex].position, CotrolBone[BoneIndex].position, ChoppingTime);
            yield return null;
        }
    }

    public override void Change_PlateIngredient()
    {
        OnPlate = true;

        Ingredient_Mesh.mesh = Plate_Mesh;
        Ingredient_renderer.material = Plate_Material;
        Ingredient_Col.sharedMesh = Plate_Mesh;
    }

    public override void Die()
    {
        JointBone = CopyBone;
        base.Die();
    }
}
