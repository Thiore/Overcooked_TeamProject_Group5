using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object_UI_Controll : MonoBehaviour
{

    /*
     * 
     * 
     * 주의 사항 *********************************
     * 냄비에는 각자 모두 Pot 태그가 붙어있어야함
     * 소화기에도 Extinguisher 태그가 붙어있어야함
     * 재료 오브젝트가 생길 때 같이 호출해야하는 Method Public으로 선언되어있음
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
    private List<GameObject> ingredient_slider_List = new List<GameObject>();


    //현재 조리중인 pot index
    public List<int> cooking_pot_index;
    //현재 과조리되고있는 pot index
    public List<int> overcook_pot_index;
    //불 난 pot index
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
        if (GameManager.Instance.isFire > 0)
        {
            Extinguisher_UI_Active();
        }

    }

    private void Initialize()
    {
        //pot이랑 pot slider 리스트에 넣기
        GameObject[] pot_objects = GameObject.FindGameObjectsWithTag("Cooker");
        for (int i = 0; i < pot_objects.Length; i++)
        {
            pot_object_List.Add(pot_objects[i].transform);
            GameObject pot_slider = Instantiate(pot_slider_prefab, pot_objects[i].transform.position, Quaternion.identity, transform);
            pot_slider.SetActive(false);
            slider_List.Add(pot_slider);
            warning_img_List.Add(Instantiate(warning_img, pot_objects[i].transform.position, Quaternion.identity, transform));
            warning_img_List[i].SetActive(false);
            done_img_List.Add(Instantiate(done_img, pot_objects[i].transform.position, Quaternion.identity, transform));
            done_img_List[i].SetActive(false);
            pot_ingredient_img_List.Add(Instantiate(ingredient_img, pot_objects[i].transform.position, Quaternion.identity, transform));
        }
        hold_ext.SetActive(false);

    }

    //냄비 슬라이더
    private void PotSlider_Active(int pot_index)
    {
        if (!slider_List[pot_index].activeSelf)
            slider_List[pot_index].SetActive(true);
        slider_List[pot_index].transform.position = main_cam.WorldToScreenPoint(pot_object_List[pot_index].position + new Vector3(0, -1, 0));
        slider_List[pot_index].GetComponent<Slider>().value = pot_object_List[pot_index].GetComponent<Pot>().CookTime;
    }

    private void Pot_UI_Update()
    {
        for (int i = 0; i < pot_object_List.Count; i++)
        {
            Pot_Ingredient_Follow(i);
            if (pot_object_List[i].TryGetComponent(out Pot pot))
            {
                if (pot.CookTime < 0.1f)
                {
                    return;
                }//요리중일때
                else if (pot.CookTime < pot.FinishCookTime)
                {
                    PotSlider_Active(i);
                }
                else if (pot.CookTime < 4.5f)
                {
                    slider_List[i].SetActive(false);
                    Pot_Finish_Cook(i);
                }
                else if (pot.CookTime < 5.0f)
                {
                    done_img_List[i].SetActive(false);
                }
                //경고
                else if (pot.CookTime + 0.3f < (int)pot_object_List[i].GetComponent<Pot>().FireTime)
                {

                    Pot_Warning_Active(i);
                }//불남
                else if (pot.CookTime > pot.FireTime)
                {
                    pot.name = "Burn";
                    warning_img_List[i].SetActive(false);
                }
            }

        }
    }

    //요리 다 되면 알림
    private void Pot_Finish_Cook(int pot_index)
    {
        done_img_List[pot_index].SetActive(true);
        done_img_List[pot_index].transform.position =
            main_cam.WorldToScreenPoint(pot_object_List[pot_index].position + new Vector3(0, -1, 0));
    }

    private void Pot_Ingredient_Follow(int pot_index)
    {
        pot_ingredient_img_List[pot_index].transform.position = main_cam.WorldToScreenPoint(pot_object_List[pot_index].position + new Vector3(0, 2, 0));

    }


    //냄비 경고
    private void Pot_Warning_Active(int pot_index)
    {
        warning_img_List[pot_index].SetActive(true);
        warning_img_List[pot_index].transform.position = main_cam.WorldToScreenPoint(pot_object_List[pot_index].position + new Vector3(0, -1, 0));
    }


    //소화기 알림
    private void Extinguisher_UI_Active()
    {
        hold_ext.SetActive(true);
        hold_ext.transform.position = main_cam.WorldToScreenPoint(GameObject.FindGameObjectWithTag("Extinguisher").transform.position + new Vector3(0, 1, 0));
    }


    //재료 오브젝트 생길 때 같이 불러줘야하는 메소드
    public void Ingredient_UI_Init(GameObject ingredient)
    {
        ingredient_object_List.Add(ingredient);
        GameObject ingredient_img_object = Instantiate(ingredient_img, ingredient.transform.position, Quaternion.identity, transform);
        ingredient_img_object.GetComponent<Image>().sprite = Resources.Load<Sprite>($"{ingredient.gameObject.name}");
        ingredient_img_object.SetActive(false);
        ingredient_img_List.Add(ingredient_img_object);
        GameObject ingredient_slider_object = Instantiate(pot_slider_prefab, ingredient.transform.position, Quaternion.identity, transform);
        ingredient_slider_List.Add(ingredient_slider_object);
        ingredient_slider_object.SetActive(false);
    }

    //onplate 가능 이미지 생성
    private void Ingredient_UI_Active()
    {
        for (int i = 0; i < ingredient_object_List.Count; i++)
        {
            if (ingredient_object_List[i].GetComponent<Ingredient>().cooking.Equals(eCooked.ReadyCook))
            {
                ingredient_img_List[i].SetActive(true);
                ingredient_img_List[i].transform.position = main_cam.WorldToScreenPoint(ingredient_object_List[i].transform.position + new Vector3(0, 1, 0));
            }else if (ingredient_object_List[i].GetComponent<Ingredient>().cooking.Equals(eCooked.Chopping))
            {
                ingredient_slider_List[i].SetActive(true);
                ingredient_slider_List[i].transform.position = main_cam.WorldToScreenPoint(ingredient_object_List[i].transform.position + new Vector3(0, -2, 0));
                //Ingredient에서 써는거 시간 value로 넣어줘야함
                ingredient_slider_List[i].GetComponent<Slider>().value = ingredient_object_List[i].GetComponent<Ingredient>().ChopTime/2f;
                if (ingredient_object_List[i].GetComponent<Ingredient>().ChopTime >= 7.9f)
                {
                    ingredient_slider_List[i].SetActive(false);
                }
            }else
            {
                ingredient_img_List[i].SetActive(false);
            }
        }
    }

    //냄비 상단 UI
    private void Pot_Ingredient_Img()
    {
        for (int i = 0; i < pot_object_List.Count; i++)
        {
            if (pot_object_List[i].gameObject.name == "Pot")
            {
                pot_ingredient_img_List[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Empty");
            }
            else
            {
                pot_ingredient_img_List[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(pot_object_List[i].name);
            }

        }
    }




}
