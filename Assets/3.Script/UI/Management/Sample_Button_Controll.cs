using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_Button_Controll : MonoBehaviour
{
    RecipePool recipePool;
    private void Start()
    {
        recipePool = GameObject.Find("Ingame_UI/Recipe_Pool").GetComponent<RecipePool>();
    }
    public void Correct()
    {
        recipePool.CheckRecipe("Fish_Food");
    }
    public void Incorrect()
    {
        recipePool.CheckRecipe("Prawn_Food");
    }
    public void Wrong()
    {
        recipePool.CheckRecipe("Wrong");
    }
}
