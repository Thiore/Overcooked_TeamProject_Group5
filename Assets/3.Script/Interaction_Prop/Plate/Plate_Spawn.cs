using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate_Spawn : MonoBehaviour
{
    [SerializeField] private Transform[] Set_Pos;
    [SerializeField] private Plate_Return plateReturn;
    [SerializeField] private Plate PlatePrefabs;
    [SerializeField] private RecipePool CheckRecipe;

    [SerializeField] private bool isWash;

    private Plate[] plates;
    private Queue<int> DestroyPlateNum = new Queue<int>();

    private WaitForSeconds SpawnTime = new WaitForSeconds(4f);
    

    private void Start()
    {
        plates = new Plate[Set_Pos.Length];
        for(int i = 0;i<plates.Length;i++)
        {
            if (Set_Pos[i] == plateReturn.transform)
            {
                plates[i] = Instantiate(PlatePrefabs,plateReturn.transform.position,plateReturn.transform.rotation);
                plateReturn.SetPlate(plates[i]);
                plates[i].SetWash(false);

            }
            else
            {
                plates[i] = Instantiate(PlatePrefabs, Set_Pos[i].position, Set_Pos[i].rotation, Set_Pos[i]);
                CounterController counter = 
                    plates[i].transform.GetComponentInParent<CounterController>();
                counter.ChangePuton();
                counter.PutOnOb = plates[i].gameObject;
                plates[i].SetWash(false);
            }
            
        }
    }

    private void FixedUpdate()
    {
        if(DestroyPlateNum.Count>0)
        {
            Debug.Log(DestroyPlateNum.Count);
            Debug.Log((int)DestroyPlateNum.Peek());
            StartCoroutine(SpawnPlate(DestroyPlateNum.Dequeue()));
        }
    }

    public bool PassPlate(Plate plate)//false가 반환되면 접시를 내지못함
    {
        for(int i = 0; i< plates.Length; i++)
        {
            if(plate == plates[i])
            {
                if(!plate.isWash)
                {
                    if(!plates[i].name.Equals("Plate"))
                    {
                        CheckRecipe.CheckRecipe($"{plate.gameObject.name}_Food");
                        plates[i].transform.SetParent(null);
                        
                        for(int j = 0; j < plate.transform.childCount;j++)
                        {
                            plate.transform.GetChild(j).gameObject.SetActive(false);
                        }
                        DestroyPlateNum.Enqueue(i);
                        plates[i].gameObject.SetActive(false);
                        return true;
                    }  
                    else
                    {
                        plates[i].transform.SetParent(null);
                        DestroyPlateNum.Enqueue(i);
                        plates[i].gameObject.SetActive(false);
                        
                        return true;
                    }
                }
                return false;
            }
        }
        return false;
    }

    private IEnumerator SpawnPlate(int plateNum)
    {
        
        yield return SpawnTime;
        Debug.Log($"{DestroyPlateNum.Count}");
        plates[plateNum].SetWash(isWash);
        plateReturn.SetPlate(plates[plateNum]);
    }

}
