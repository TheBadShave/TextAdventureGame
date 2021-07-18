using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextQuest/Item")]
public class Item : ScriptableObject{

    public string itemName; //name of item used as dictionary reference
	public string itemDescription; //description of item when it is on floor
	public string itemExamine; //description of item when it is examined
	public bool lootable; //can the item be picked up.
}
