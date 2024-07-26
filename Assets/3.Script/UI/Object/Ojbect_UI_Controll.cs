using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ojbect_UI_Controll : MonoBehaviour
{

    /*
     * 
     * 
     * ���� ���� *********************************
     * ���񿡴� ���� ��� Pot �±װ� �پ��־����
     * ��ȭ�⿡�� Extinguisher �±װ� �پ��־����
     * ��� ������Ʈ�� ���� �� ���� ȣ���ؾ��ϴ� Method Public���� ����Ǿ�����
     * 
     * 
     * 
    */




    [SerializeField] private GameObject pot_slider_prefab;
    [SerializeField] private GameObject ingredient_img;
    [SerializeField] private GameObject warning_img;
    [SerializeField] private GameObject done_img;
    [SerializeField] private GameObject respawn_img;
    [SerializeField] private GameObject hold_ext;

    private List<Transform> pot_object_List = new List<Transform>();
    private List<GameObject> slider_List = new List<GameObject>();
    private List<GameObject> ingredient_img_List = new List<GameObject>();
    private List<GameObject> pot_ingredient_img_List = new List<GameObject>();
    private List<GameObject> warning_img_List = new List<GameObject>();
    private List<GameObject> done_img_List = new List<GameObject>();
    public List<GameObject> ingredient_object_List = new List<GameObject>();

    //���� �������� pot index
    public List<int> cooking_pot_index;
    //���� �������ǰ��ִ� pot index
    public List<int> overcook_pot_index;
    //�� �� pot index
    public List<int> fire_pot_index;



    private Camera main_cam;

    private void Start()
    {
        main_cam = Camera.main;
        Initialize();
        
    }
    private void Update()
    {
        Pot_UI_Update();
        Ingredient_UI_Active();
        Pot_Ingredient_Img();
        if (0 < GameManager.Instance.isFire)
        {
            Extinguisher_UI_Active();
        }

    }

    private void Initialize()
    {
        //pot�̶� pot slider ����Ʈ�� �ֱ�
        GameObject[] pot_objects = GameObject.FindGameObjectsWithTag("Pot");
        for (int i = 0; i < pot_objects.Length; i++)
        {
            pot_object_List.Add(pot_objects[i].transform);
            GameObject pot_slider = Instantiate(pot_slider_prefab, pot_objects[i].transform.position, Quaternion.identity, transform);
            pot_slider.SetActive(false);
            slider_List.Add(pot_slider);
            warning_img_List[i] = Instantiate(warning_img,pot_objects[i].transform.position, Quaternion.identity, transform);
            warning_img_List[i].SetActive(false);
            done_img_List[i] = Instantiate(done_img, pot_objects[i].transform.position, Quaternion.identity, transform);
            done_img_List[i].SetActive(false);
            
        }
        hold_ext.SetActive(false);

    }

    //���� �����̴�
    private void PotSlider_Active(int pot_index)
    {
            slider_List[pot_index].SetActive(true);
            slider_List[pot_index].transform.position = main_cam.WorldToScreenPoint(pot_object_List[pot_index].position + new Vector3(0, -1, 0));
            slider_List[pot_index].GetComponent<Slider>().value = pot_object_List[pot_index].GetComponent<Pot>().CookTime;
    }

    private void Pot_UI_Update()
    {
        for(int i = 0; i < pot_object_List.Count; i++)
        {
            if (pot_object_List[i].GetComponent<Pot>().CookTime==0)
            {
                return;
            }//�丮���϶�
            else if(pot_object_List[i].GetComponent<Pot>().CookTime > pot_object_List[i].GetComponent<Pot>().FinishCookTime)
            {
                PotSlider_Active(i);
            }//�˶� �층
            else if((int)pot_object_List[i].GetComponent<Pot>().CookTime == (int)pot_object_List[i].GetComponent<Pot>().FinishCookTime)
            {
                slider_List[i].SetActive(false);
                StartCoroutine(Pot_Finish_Cook(i));
            }//���
            else if ((int)pot_object_List[i].GetComponent<Pot>().CookTime+0.3f < (int)pot_object_List[i].GetComponent<Pot>().FireTime)
            {
                Pot_Waring_Active(i);
            }//�ҳ�
            else if((int)pot_object_List[i].GetComponent<Pot>().CookTime > (int)pot_object_List[i].GetComponent<Pot>().FireTime)
            {
                warning_img_List[i].SetActive(false);
            }
        }
    }

    //�丮 �� �Ǹ� �˸�
    private IEnumerator Pot_Finish_Cook(int pot_index)
    {
        done_img_List[pot_index].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        done_img_List[pot_index].SetActive(false);
    }


    //���� ���
    private void Pot_Waring_Active(int pot_index)
    {
            warning_img_List[pot_index].SetActive(true);
            warning_img_List[pot_index].transform.position= main_cam.WorldToScreenPoint(pot_object_List[pot_index].position + new Vector3(0, -1, 0));
    }


    //��ȭ�� �˸�
    private void Extinguisher_UI_Active()
    {
        hold_ext.SetActive(true);
        hold_ext.transform.position = main_cam.WorldToScreenPoint(GameObject.FindGameObjectWithTag("Extinguisher").transform.position+new Vector3(0,1,0));
    }


    //��� ������Ʈ ���� �� ���� �ҷ�����ϴ� �޼ҵ�
    public void Ingredient_UI_Int(GameObject ingredient)
    {
        ingredient_object_List.Add(ingredient);
        GameObject ingredient_img_object = Instantiate(ingredient_img, ingredient.transform.position, Quaternion.identity, transform);
        ingredient_img_object.GetComponent<Image>().sprite = Resources.Load<Sprite>($"{ingredient.gameObject.name}");
        ingredient_img_object.SetActive(false);
        ingredient_img_List.Add(ingredient_img_object);
    }

    //onplate ���� �̹��� ����
    private void Ingredient_UI_Active()
    {
        for(int i = 0; i < ingredient_object_List.Count; i++)
        {
            if (ingredient_object_List[i].GetComponent<Ingredient>().OnPlate)
            {
                ingredient_img_List[i].SetActive(true);
                ingredient_img_List[i].transform.position = main_cam.WorldToScreenPoint(ingredient_object_List[i].transform.position + new Vector3(0, -1, 0));
            }
        }
    }

    //���� ��� UI
    private void Pot_Ingredient_Img()
    {
        for(int i = 0; i < pot_object_List.Count; i++)
        {
            if (pot_object_List[i].name=="Pot")
            {
                pot_ingredient_img_List[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Empty");
            }
            else{
                pot_ingredient_img_List[i].GetComponent<Image>().sprite = Resources.Load<Sprite>($"{pot_object_List[i].name}");
            }
            
        }
    }

    


}
