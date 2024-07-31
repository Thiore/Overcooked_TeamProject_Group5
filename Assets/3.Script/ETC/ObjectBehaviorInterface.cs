using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ObjectBehaviorInterface
{
    void OnPickUp(GameObject gameObject);
    void OnDropDown(GameObject gameObject);
    void OnChop(GameObject gameObject);
   
    //bool CanInteract
}

public abstract class InteractableObject : MonoBehaviour, ObjectBehaviorInterface
{
    public abstract void OnPickUp(GameObject gameObject);
    public abstract void OnDropDown(GameObject gameObject);
    public abstract void OnChop(GameObject gameObject);

   // public abstract bool CanInteract(InteractionType type);
}