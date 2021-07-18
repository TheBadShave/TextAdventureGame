using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLibrarian : MonoBehaviour
{
    public Room[] roomDirectory; //stores all the rooms in the inspector to sort into a library 
    public IDictionary<string, Room> roomDICT = new Dictionary<string, Room>(); //Dictionary for Rooms
    public Item[] itemDirectory; //stores all the items in the inspector to sort into a library 
    public IDictionary<string, Item> itemDICT = new Dictionary<string, Item>(); //Dictionary for items

    void Start()
    {
    	//Initialize a dictionary for rooms items
    	
    	//Index through directory, assigning each room to it's name.
        for(int i = 0; i < roomDirectory.Length; i++) {          
    		roomDICT.Add(roomDirectory[i].roomName, roomDirectory[i]);  
    	}
    	//Same for items
    	for(int i = 0; i < itemDirectory.Length; i++) {          
    		itemDICT.Add(itemDirectory[i].itemName, itemDirectory[i]);  
    	}
        
    }

}
