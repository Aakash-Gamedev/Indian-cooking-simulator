using UnityEngine;
using UnityEngine.UI; // Import for UI elements
using TMPro; // Import for TextMeshPro

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public ItemData InventoryItem { get; private set; }

    [Header("UI Elements")] // Add a header for UI elements in the Inspector
    public Image itemIconImage; // Assign your icon Image UI element in the Inspector
    public TextMeshProUGUI itemIdText; // Assign your Item ID TextMeshPro UI element in the Inspector

    private void Start()
    {
        // Call UpdateInventoryUI() at start to initialize the UI
        UpdateInventoryUI();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StoreInInventory(ItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("Trying to store a null item.");
            return;
        }

        InventoryItem = item;

        // Update UI
        UpdateInventoryUI();

        Debug.Log(item.ItemId + " Stored");
    }

    public ItemData TakeFromInventory()
    {
        if (InventoryItem == null)
        {
            Debug.LogWarning("Inventory is empty.");
            return null;
        }

        ItemData temp = InventoryItem;
        InventoryItem = null;

        // Update UI
        UpdateInventoryUI();

        Debug.Log(temp.ItemId + " Taken");

        return temp;
    }

    private void UpdateInventoryUI()
    {
        if (itemIconImage != null && itemIdText != null)
        {
            if (InventoryItem != null)
            {
                itemIconImage.sprite = InventoryItem.ItemIcon;
                itemIdText.text = InventoryItem.ItemId;

                // Enable (or ensure they are enabled)
                itemIconImage.enabled = true;  // Make sure the Image is enabled
                itemIdText.enabled = true; // Make sure the Text is enabled
            }
            else
            {
                // Inventory is empty
                itemIconImage.sprite = null; // Or use a default "empty" sprite if you have one
                itemIdText.text = "Empty"; // Display "Empty" text

                itemIconImage.enabled = false; // Turn off the Image component
            }
        }
        else
        {
            Debug.LogWarning("Item Icon Image or Item ID Text is not assigned in the Inspector!");
        }
    }
}