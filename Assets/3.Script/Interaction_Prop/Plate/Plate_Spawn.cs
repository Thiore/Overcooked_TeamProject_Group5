using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate_Spawn : MonoBehaviour
{
    [SerializeField] private Transform[] Set_Pos;
    [SerializeField] private BoxCollider ReturnCol;
    [SerializeField] private Plate_Return plateReturn;
    [SerializeField] private Plate PlatePrefabs;
    [SerializeField] private RecipePool CheckRecipe;

    [SerializeField] private bool isWash;

    private Plate[] plates;
    private Queue<int> DestroyPlateNum = new Queue<int>();

    private WaitForSeconds SpawnTime = new WaitForSeconds(5f);

    private void Awake()
    {
        plates = new Plate[Set_Pos.Length];
        for(int i = 0;i<plates.Length;i++)
        {
            plates[i] = Instantiate(PlatePrefabs);
            plates[i].gameObject.name = "Plate";
            plates[i].SetWash(isWash);
            plates[i].transform.SetParent(Set_Pos[i]);
            if(Set_Pos[i] == plateReturn.transform)
            {
                plateReturn.SetPlate(plates[i]);
            }
            else
            {
                CounterController counter = 
                    plates[i].transform.GetComponentInParent<CounterController>();
                counter.ChangePuton();
                counter.PutOnOb = plates[i].gameObject;
            }
            
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
                Transform childobj = plate.transform.GetChild(0);
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

    private IEnumerator SpawnPlate()
    {
        int plateNum = DestroyPlateNum.Dequeue();
        yield return SpawnTime;
        plates[plateNum].SetWash(isWash);
        plateReturn.SetPlate(plates[plateNum]);
    }

}
