using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class RecipeDB : ScriptableObject
{
	public List<Recipe_data> RecipeData; // Replace 'EntityType' to an actual type that is serializable.
}
