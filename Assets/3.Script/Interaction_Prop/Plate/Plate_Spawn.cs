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

    //private Plate[] plates;
    private Queue<Plate> DestroyPlate = new Queue<Plate>();

    private WaitForSeconds SpawnTime = new WaitForSeconds(4f);
    

    private void Start()
    {
        //plates = new Plate[Set_Pos.Length];
        for(int i = 0;i< Set_Pos.Length;i++)
        {
            if (Set_Pos[i] == plateReturn.transform)
            {
                Plate plate = Instantiate(PlatePrefabs,plateReturn.transform.position,plateReturn.transform.rotation);
                plateReturn.SetPlate(plate);
                plate.SetWash(false);

            }
            else
            {
                Plate plate = Instantiate(PlatePrefabs, Set_Pos[i].position, Set_Pos[i].rotation, Set_Pos[i]);
                CounterController counter =
                    plate.transform.GetComponentInParent<CounterController>();
                counter.ChangePuton();
                counter.PutOnOb = plate.gameObject;
                plate.SetWash(false);
            }
            
        }
    }

    private void FixedUpdate()
    {
        if(DestroyPlate.Count>0)
        {
            StartCoroutine(SpawnPlate(DestroyPlate.Dequeue()));
        }
    }

    public bool PassPlate(Plate plate)//false가 반환되면 접시를 내지못함
    {

        if (!plate.isWash)
        {
            if (!plate.name.Equals("Plate"))
            {
                CheckRecipe.CheckRecipe(plate.gameObject.name+"_Food");
                plate.transform.SetParent(null);

                for (int j = 0; j < plate.transform.childCount; j++)
                {
                    plate.transform.GetChild(j).gameObject.SetActive(false);
                }
                DestroyPlate.Enqueue(plate);
                plate.gameObject.SetActive(false);
                return true;
            }
            else
            {
                CheckRecipe.CheckRecipe(plate.gameObject.name);
                plate.transform.SetParent(null);
                DestroyPlate.Enqueue(plate);
                plate.gameObject.SetActive(false);

                return true;
            }
        }
        return false;
    }

    private IEnumerator SpawnPlate(Plate plate)
    {
        
        yield return SpawnTime;
        plate.SetWash(isWash);
        plateReturn.SetPlate(plate);
    }

}
