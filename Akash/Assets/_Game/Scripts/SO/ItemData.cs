using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    public string ItemId;
    public Sprite ItemIcon;
    public GameObject ItemPrefab;
    public float ItemCookTime;
    public int ItemPrice;
}