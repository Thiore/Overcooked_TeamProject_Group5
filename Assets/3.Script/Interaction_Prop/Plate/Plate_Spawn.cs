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

    private WaitForSeconds SpawnTime = new WaitForSeconds(1f);
    

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

    public bool PassPlate(Plate plate)//false�� ��ȯ�Ǹ� ���ø� ��������
    {
        for(int i = 0; i< plates.Length; i++)
        {
            if(plate == plates[i])
            {
                if(!plate.isWash)
                {
                    if(plates[i].transform.childCount>0)
                    {
                        Transform childobj = plates[i].transform.GetChild(0);
                        CheckRecipe.CheckRecipe($"{childobj.gameObject.name}_Food");
                        plates[i].transform.SetParent(null);
                        DestroyPlateNum.Enqueue(i);
                        if (childobj.TryGetComponent(out Ingredient Ingre))
                        {
                            Ingre.Die();
                        }
                        else
                            Destroy(childobj.gameObject);

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
