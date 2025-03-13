using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AIFoodManager : MonoBehaviour
{
    public List<ItemData> availableFoodItems;
    public ItemData WantedFood;
    public float WaitTime;
    public Slider timerSlider;

    public Image foodIcon;
    public TextMeshProUGUI foodText;

    private ItemData temp;
    private float timer;

    public GameObject UIObject;

    private GameObject DeliveredFood = null;

    public void Start()
    {
        if (availableFoodItems == null || availableFoodItems.Count == 0)
        {
            Debug.LogError("No food items assigned to AIFoodManager!");
            enabled = false;
            return;
        }

        if (timerSlider == null)
        {
            Debug.LogError("Timer slider not assigned!");
            enabled = false;
            return;
        }

        WantedFood = GetRandomFoodItem();
        foodIcon.sprite = WantedFood.ItemIcon;
        foodText.text = WantedFood.ItemId;
        UIObject.SetActive(true);
    }

    public void StartFoodWait() {
        StartWaitTime(WantedFood);
    }

    private ItemData GetRandomFoodItem()
    {
        if (availableFoodItems.Count > 0)
        {
            int randomIndex = Random.Range(0, availableFoodItems.Count);
            return availableFoodItems[randomIndex];
        }
        return null;
    }

    public void ServerFood()
    {
        temp = PlayerInventory.Instance.TakeFromInventory();

        if (temp != null)
        {
            if (WantedFood.ItemId == temp.ItemId)
            {
                Vector3 foodDeliveryPoint = GetComponent<AIController>().GetFoodServingSlot();

                

                if(foodDeliveryPoint!= Vector3.zero)
                {
                    DeliveredFood = Instantiate(temp.ItemPrefab,foodDeliveryPoint, Quaternion.identity);
                    Debug.Log( DeliveredFood.gameObject.name + "Delivered to " + foodDeliveryPoint);
                }
                CancelInvoke("UpdateTimerAndSlider");
                UIObject.SetActive(false);
                Invoke(nameof(WaitAndLeave), 5.0f);
                // Add Money
                GameManager.Instance.AddMoney(temp.ItemPrice);
                temp = null;
            }
            else
            {
                PlayerInventory.Instance.StoreInInventory(temp);
                temp = null;
            }
        }
    }

    private void WaitAndLeave()
    {
        if(DeliveredFood != null)
        {
            Destroy(DeliveredFood);
        }
       this.GetComponent<AIController>().LeaveRestuarent();
    }

    private void StartWaitTime(ItemData food)
    {
        Debug.Log("Starting wait time for " + food.ItemId + "..." + WaitTime);

        timer = WaitTime; // Initialize the timer
        UpdateTimerSlider(); // Update the slider at the start

        InvokeRepeating("UpdateTimerAndSlider", 0.1f, 0.1f); // Repeat every 0.1 seconds
    }

    private void UpdateTimerAndSlider()
    {
        timer -= 0.1f; // Decrement by the repeat interval

        if (timer <= 0f)
        {
            timer = 0f; // Ensure it doesn't go below 0
            CancelInvoke("UpdateTimerAndSlider"); // Stop repeating
            AfterWaitTime();
        }

        UpdateTimerSlider();
    }

    private void UpdateTimerSlider()
    {
        if (timerSlider != null)
        {
            timerSlider.value = timer / WaitTime; // Calculate and set the slider value
        }
    }

    private void AfterWaitTime()
    {
        Debug.Log("Wait time is over!");
        // Reduce Life
        GameManager.Instance.RemoveLife();
        this.GetComponent<AIController>().LeaveRestuarent();
    }

}