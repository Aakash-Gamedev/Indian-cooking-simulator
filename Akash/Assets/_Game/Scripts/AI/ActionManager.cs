using System;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [Header("Settings")]
    public Transform[] Places;
    public Transform[] FoodSlots;
    public Transform GO_Position;
    public bool IsSittable;
    public bool foodServerd;


    public AIController AI_instance;

    private void Update()
    {
        if (foodServerd) { 
            foodServerd = false;
            AI_instance.LeaveRestuarent();
            AI_instance = null;
        }
    }

    public Transform ReturnWaitingPlace()
    {
        if (Places.Length == 0)
        {
            Debug.LogError("No sit places available.");
            return null;
        }

        int index = UnityEngine.Random.Range(0, Places.Length);
        return Places[index];
    }

    public Transform GetFoodSlotTransform(Transform sittingTrans)
    {
        int index = Array.IndexOf(Places, sittingTrans);
        return FoodSlots[index];
    }
    
}
