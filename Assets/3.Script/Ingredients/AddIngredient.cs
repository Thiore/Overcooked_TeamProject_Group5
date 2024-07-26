using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddIngredient : MonoBehaviour
{
    public bool isPlate { get; private set; }
    public string LastName;
    public List<string> RecipeList;
    private List<Recipe> recipes; 

    private void Awake()
    {
        isPlate = true;
        recipes = DataManager.Instance.StageRecipeData(GameManager.Instance.stage_index);
        for (int i = 0; i < recipes.Count; i++)
            RecipeList.Add(recipes[i].recipe);

    }

    private void Update()
    {
        if(isPlate&&transform.parent.CompareTag("Plate")&&RecipeList.Contains(gameObject.name))
        {
            isPlate = false;
        }
    }
}
