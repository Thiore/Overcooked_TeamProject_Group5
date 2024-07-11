using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Recipe
{
    public int id;
    public int stage;
    public string recipe;
    public string ingredient;

}

public class Recipe_List
{
    public List<Recipe> recipe_list;
}
