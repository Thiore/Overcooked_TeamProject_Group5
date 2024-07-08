using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


class Recipe_DB
{
    public int stage;
    public string recipe;
    public string ingredient;

}
public class Recipe_DataManager
{
    Recipe_DB recipe = new Recipe_DB();

    private void Start()
    {
        
    }
}
