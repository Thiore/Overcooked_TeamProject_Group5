using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate_Spawn : MonoBehaviour
{
    [SerializeField] private Transform[] Set_Pos;
    [SerializeField] private BoxCollider ReturnCol;
    [SerializeField] private Transform Plate_Return;
    [SerializeField] private Plate PlatePrefabs;
    [SerializeField] private RecipePool CheckRecipe;

    [SerializeField] public bool isWash;

    private Plate[] plates;
    private Queue<int> DestroyPlateNum = new Queue<int>();

    private WaitForSeconds SpawnTime = new WaitForSeconds(5f);

    private void Awake()
    {
        plates = new Plate[Set_Pos.Length];
        for(int i = 0;i<plates.Length;i++)
        {
            plates[i] = Instantiate(PlatePrefabs);
            plates[i].transform.SetParent(Set_Pos[i]);
        }
    }

    private void Update()
    {
        if(DestroyPlateNum.Count>0)
        {
            StartCoroutine(SpawnPlate());
        }
    }

    public void PassPlate(GameObject plate)
    {
        for(int i = 0; i< plates.Length; i++)
        {
            if(plate == plates[i].gameObject)
            {
                for (int j = 0; j < plate.transform.childCount; j++)
                {
                    Transform childobj = plate.transform.GetChild(i);
                    if (childobj.gameObject.activeSelf)
                    {
                        CheckRecipe.CheckRecipe($"{childobj.gameObject.name}_Food");
                        DestroyPlateNum.Enqueue(i);
                        plates[i].gameObject.SetActive(false);
                        return;
                    }
                }
                
            }
        }
    }

    private IEnumerator SpawnPlate()
    {
        yield return SpawnTime;
        //plates[DestroyPlateNum.Dequeue()]
    }

}
