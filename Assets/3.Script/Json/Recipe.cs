using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Recipe
{
    //public int id;
    //public int stage;
    public string recipe;
    public List<string> ingredient;
    
}

public class Recipe_List
{
    public List<Recipe> recipe_list;
}
