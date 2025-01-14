using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beach_Spawn : MonoBehaviour
{
    [SerializeField] private Material crate;
    [SerializeField] private Material[] crate_Material;
    [SerializeField] private Crate_Data crate_Data;
    [SerializeField] private GameObject crate_Prefabs;
    [SerializeField] private Transform[] Crate_Pos1;
    [SerializeField] private Transform[] Crate_Pos2;
    [SerializeField] private Transform[] Crate_Pos3;
    [SerializeField] private Transform[] Section_Pos;
    private Vector3[] CopySection;

    private GameObject[][] CrateArray;
    

    private Queue<GameObject> Recipe_queue = new Queue<GameObject>();
    private Dictionary<eIngredients, Queue<Ingredient>> spawner = new Dictionary<eIngredients, Queue<Ingredient>>();


    private void Awake()
    {
        for(int i = 0; i < crate_Data.Info.Length;i++)
        {
            spawner.Add(crate_Data.Info[i].Ingredients, new Queue<Ingredient>());
        }

        CopySection = new Vector3[Section_Pos.Length];
        for(int i = 0; i < Section_Pos.Length;i++)
        {
            CopySection[i] = Section_Pos[i].position;

        }

        CrateArray = new GameObject[3][];
        
        CrateArray[0] = new GameObject[Crate_Pos1.Length];
        CrateArray[1] = new GameObject[Crate_Pos2.Length];
        CrateArray[2] = new GameObject[Crate_Pos3.Length];


        for (int i = 0; i < Crate_Pos1.Length; i++)
        {
            CrateArray[0][i] = New_Crate1(i);
        }
        for (int i = 0; i < Crate_Pos2.Length; i++)
        {
            CrateArray[1][i] = New_Crate2(i);
        }
        for (int i = 0; i < Crate_Pos3.Length; i++)
        {
            CrateArray[2][i] = New_Crate3(i);
        }

    }

    private void Update()
    {
        MoveingPlatform();
        

    }
    
    private void MoveingPlatform()
    {
        
        for(int i = 0; i < Section_Pos.Length;i++)
        {
            Section_Pos[i].Translate(Vector3.right * Time.deltaTime * 2f);
            //Debug.Log(Section_Pos[i].name);
            //Debug.Log(Section_Pos[i].position);
            if (Section_Pos[i].position.x>CopySection[2].x+13.8f)
            {
                
                Section_Pos[i].position = CopySection[0]+Vector3.left*13.8f;
                for(int j = 0; j<CrateArray[i].Length;j++)
                {
                    CrateArray[i][j].SetActive(false);
                    Crate_Tex_Offset(CrateArray[i][j]);
                    CrateArray[i][j].SetActive(true);
                }
                
            }
        }
    }    

    private GameObject New_Crate1(int num1)
    {
        GameObject obj = Instantiate(crate_Prefabs, Crate_Pos1[num1].position, Crate_Pos1[num1].rotation, Crate_Pos1[num1]);
        Crate_Tex_Offset(obj);
        return obj;
    }
    private GameObject New_Crate2(int num1)
    {
        GameObject obj = Instantiate(crate_Prefabs, Crate_Pos2[num1].position, Crate_Pos2[num1].rotation, Crate_Pos2[num1]);
        Crate_Tex_Offset(obj);
        return obj;
    }
    private GameObject New_Crate3(int num1)
    {
        GameObject obj = Instantiate(crate_Prefabs, Crate_Pos3[num1].position, Crate_Pos3[num1].rotation, Crate_Pos3[num1]);
        Crate_Tex_Offset(obj);
        return obj;
    }

    public void Crate_Tex_Offset(GameObject obj)
    {
        int rand = Random.Range(0, crate_Data.Info.Length);
        Crate_Info info = crate_Data.Info[rand];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {

                if (info.Ingredients.Equals((eIngredients)(i * 5 + j)))
                {
                    crate_Material[rand].mainTextureOffset = new Vector2(0.2f * j, 1 - 0.2f * (i + 1));

                    Renderer crate_renderer = obj.GetComponent<Renderer>();
                    Material[] newMat = new Material[2];

                    newMat[0] = crate;
                    newMat[1] = crate_Material[rand];
                    crate_renderer.materials = newMat;
                    obj.TryGetComponent(out spawn_Ingredient ingre);
                    ingre.Set_IngredientInfo(info, crate_Data);
                    ingre.Beach_ResetQueue(spawner[info.Ingredients]);
                    obj.GetComponent<spawn_Ingredient>().Set_IngredientInfo(info, crate_Data);

                    return;
                }

            }
        }
    }

}
