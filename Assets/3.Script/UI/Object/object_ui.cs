using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class object_ui : MonoBehaviour
{
    private Ingredient ingredient;
    private GameObject ingredient_ui;
    private Image ui_image;
    private void Start()
    {
        ingredient = GetComponent<Ingredient>();
        ingredient_ui = GameObject.Find("object_ui");
        ui_image = ingredient_ui.GetComponentInChildren<Image>();
        ui_image.sprite = Resources.Load<Sprite>($"{this.gameObject.name}");
        ingredient_ui.SetActive(false);
    }
    private void Update()
    {
        Active_UI();
    }
    private void Active_UI()
    {
        if (ingredient.OnPlate)
        {
            ingredient_ui.SetActive(true);
        }
        else
        {
            return;
        }
    }
}
