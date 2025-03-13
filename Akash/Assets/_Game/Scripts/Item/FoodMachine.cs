using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodMachine : MonoBehaviour
{
    [Header("Settings")]
    public ItemData itemData; // Use ItemData Scriptable Object

    [Header("Internal Inventory")]
    public int foodPerCook = 5;
    public int maxStorage = 20;

    [Header("UI Elements")]
    public Slider cookProgressSlider;
    public Button takeFoodButton;
    public Image foodIcon; // Renamed for clarity
    public TextMeshProUGUI foodCountText;
    public TextMeshProUGUI foodText;

    public GameObject FoodObject;
    public AudioSource foodsound;


    private bool isCooking = false;
    private float cookTimer = 0.0f;
    private List<ItemData> cookedFoodList = new List<ItemData>(); // Store ItemData

    void Start()
    {
        if (cookProgressSlider != null)
        {
            cookProgressSlider.gameObject.SetActive(false);
        }
        UpdateUI(); // Combined UI update
        foodText.text = itemData.ItemId + " Maker";
        FoodObject.SetActive(false);
    }

    void Update()
    {
        if (isCooking)
        {
            cookTimer += Time.deltaTime;
            UpdateSlider(cookTimer / itemData.ItemCookTime);

            if (cookTimer >= itemData.ItemCookTime)
            {
                CookFood();
            }
        }
    }

    public void StartCooking()
    {
        if (!isCooking && cookedFoodList.Count + foodPerCook <= maxStorage)
        {
            foodsound.Play();
            isCooking = true;
            cookTimer = 0.0f;
            cookProgressSlider.gameObject.SetActive(true);
            UpdateSlider(0.0f);
            Debug.Log("Started cooking " + itemData.ItemId + "...");
        }
        else if (cookedFoodList.Count + foodPerCook > maxStorage)
        {
            Debug.Log("Storage limit reached. Cannot cook more " + itemData.ItemId + ".");
        }
    }

    private void CookFood()
    {
        foodsound.Stop();
        isCooking = false;
        cookTimer = 0.0f;
        cookProgressSlider.gameObject.SetActive(false);
        UpdateSlider(0.0f);

        int foodToCook = Mathf.Min(foodPerCook, maxStorage - cookedFoodList.Count);
        for (int i = 0; i < foodToCook; i++)
        {
            // Create a *copy* of the ItemData so each item in the list is unique
            ItemData cookedFoodData = Instantiate(itemData);
            cookedFoodList.Add(cookedFoodData);

            // Important: Make the instantiated ItemData not save with the scene
            cookedFoodData.hideFlags = HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;

        }

        UpdateUI();
        Debug.Log(itemData.ItemId + " is cooked!");
    }

    public ItemData GetCookedFood()
    {
        if (cookedFoodList.Count > 0)
        {
            ItemData cookedFood = cookedFoodList[0];
            cookedFoodList.RemoveAt(0);
            UpdateUI();
            Debug.Log("Retrieved cooked " + cookedFood.ItemId + "!");
            return cookedFood;
        }

        Debug.Log("No cooked " + itemData.ItemId + " available.");
        return null;
    }

    public void TakeFoodFromMachine()
    {
        if (PlayerInventory.Instance.InventoryItem == null)
        {
            ItemData foodToTake = GetCookedFood();
            if (foodToTake != null)
            {
                PlayerInventory.Instance.StoreInInventory(foodToTake);
                FoodObject.SetActive(false);


            }
        }
        else
        {
            Debug.LogWarning("Player inventory already contains an item.");
        }
    }

    private void UpdateSlider(float progress)
    {
        if (cookProgressSlider != null)
        {
            cookProgressSlider.value = progress;
        }
    }

    private void UpdateUI()
    {
        UpdateTakeFoodButton();
        UpdateFoodCountText();
        UpdateFoodIcon();
    }

    private void UpdateTakeFoodButton()
    {
        if (takeFoodButton != null)
        {
            takeFoodButton.interactable = cookedFoodList.Count > 0;
            FoodObject.SetActive(cookedFoodList.Count > 0);
            foodsound.Stop();
        }
    }

    private void UpdateFoodCountText()
    {
        if (foodCountText != null)
        {
            foodCountText.text = $"Food Left: {cookedFoodList.Count}/{maxStorage}";
        }
    }

    private void UpdateFoodIcon()
    {
        if (foodIcon != null && itemData != null) // Check if itemData is assigned
        {
            foodIcon.sprite = itemData.ItemIcon;
        }
    }
}