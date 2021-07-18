using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    public Text gamePane; //where game is displayed
    public Room firstRoom; //tells game where to start
    public Room currentRoom;
    public InputField inputField;
    public List<string> playerInventory = new List<string>(); //player inventory
    string errorResponse = ""; //string used for reporting failed actions


    // Start is called before the first frame update
    void Start()
    {
    	inputField.ActivateInputField();
		inputField.onEndEdit.AddListener (AcceptPlayerString);
        gamePane.text = " ";
        currentRoom = firstRoom;
        PrintRoom(currentRoom);
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKey ("escape")) Application.Quit();
    }

    void PrintRoom(Room roomToPrint) {
    	//first, collect room description. Add a space after it.                                         
    	string collectedDescription = roomToPrint.roomDescription + " "; 
    	
    	//Do the same for items in the room, using their floor descriptions by sending names to the itemDictionary
    	for(int i = 0; i < roomToPrint.roomItems.Count; i++) {          
    		collectedDescription += this.gameObject.GetComponent<GameLibrarian>().itemDICT[roomToPrint.roomItems[i]].itemDescription + " ";
    	}
    	//Next, index through all the exist descriptions, adding them to the collected total.
    	for(int i = 0; i < roomToPrint.roomExits.Count; i++) {          
    		collectedDescription += roomToPrint.roomExits[i].description + " ";
    	}
    	//Finish by printing the collect description (maybe add typing effect as seperate function instead)
    	DisplayText(collectedDescription);
    }

    public void DisplayText(string textToDisplay) {
    	gamePane.text += "\n\n" + textToDisplay;
    }

    void AcceptPlayerString (string playerInput) {
    	//reactivate input field as it turns off automatically
    	inputField.ActivateInputField();
    	//convert to lowercase
    	playerInput = playerInput.ToLower();
    	//clear input area
    	inputField.text = null;
    	//Break input into array using space bar as marker
    	string[] playerCommand;
    	playerCommand = playerInput.Split(char.Parse(" "));
    	
    	//THE BIG PARSING HUNT (remember to break;)
    	switch (playerCommand[0]) {
//BLANK ENTRY (REPRINT) =======================================================================================================
    		case "": PrintRoom(currentRoom); break;
//GO===========================================================================================================================
    		case "go": verbGo (playerCommand[1]); break;
    		case "move": verbGo (playerCommand[1]); break;
    		case "travel": verbGo (playerCommand[1]); break;
    		case "head": verbGo (playerCommand[1]); break;
//LOOK=========================================================================================================================
    		case "examine": verbExamine (playerCommand[1]); break;
    		case "inspect": verbExamine (playerCommand[1]); break;
//TAKE=========================================================================================================================
    		case "take": verbTake(playerCommand[1]); break;
    		case "grab": verbTake(playerCommand[1]); break;
    		case "loot": verbTake(playerCommand[1]); break;
//USE==========================================================================================================================
    		case "use": verbUse(playerCommand); break;
//ERROR CATCH==================================================================================================================
    		default: verbError(); break;
    	}
    }

    void verbError(){
    	errorResponse = "I'm sorry, but I don't understand.";
    	DisplayText(errorResponse);
    }

    void verbGo (string directionToGo) {
    	bool isDirectionValid = false; //first create bool to check if there's a valid path.
    	int correctIndex = 0;	
    	//Loop through the room exits and make the bool true if an exit matches the direction. Save the index number for use.
    	//NOTE this is done because I don't know how to get the index FROM the direction name for use later, since I have to refernce a specific Exit.
    	for(int i = 0; i < currentRoom.roomExits.Count; i++) {          	
    		if (currentRoom.roomExits[i].direction == directionToGo) {
    			isDirectionValid = true;
    			correctIndex = i;	
    		}
    	}
    	//If the direction is valid, move. If not, send an error message.
    	if (isDirectionValid == true) {
    		currentRoom = currentRoom.roomExits[correctIndex].destination;
    		PrintRoom(currentRoom);
    	}
    	else {
    		errorResponse = "There's nowhere to go";
    		DisplayText(errorResponse);
    	}
    }

    void verbTake(string itemToTake) {
    	bool isItemValid = false;
    	//check if item is in the room
    	//Unlike going between rooms, since the inventory is just a list of strings, I can manipulate it with just Add/Remove.
    	for(int i = 0; i < currentRoom.roomItems.Count; i++) {          	
    		if (currentRoom.roomItems[i] == itemToTake) {
    			isItemValid = true;
    		}
    	}

    	//If item is valid, add it to inventory and remove it from room.
    	if (isItemValid == true) {
    		//if item can be picked up
    		if (this.gameObject.GetComponent<GameLibrarian>().itemDICT[itemToTake].lootable){
    		playerInventory.Add(itemToTake);
    		currentRoom.roomItems.Remove(itemToTake);
    		string itemPickUp = "You pick up the "+itemToTake;
    		DisplayText(itemPickUp);
    		} else {
    			errorResponse = "You can't take that with you.";
    			DisplayText(errorResponse);
    		}	

    	}
    	else {
    		errorResponse = "There's nothing to take like that";
    		DisplayText(errorResponse);
    	}
    }

    void verbUse(string[] useDirections) {
    	//since Use is slightly more complicated (needing both an item and a target), it takes the whole string of command for use.
    	//first run a check if the item exists in the inventory and if so remove it from the inventory (reverese of Take script)
    	bool isItemValid = false;
    	for(int i = 0; i < playerInventory.Count; i++) {          	
    		if (playerInventory[i] == useDirections[1]) {
    			isItemValid = true;
    		}
    	}

    	if (isItemValid == true) {
    		//determine if the syntax USE ITEM ON TARGET is valid. The array should be 4 items long with 'on' as the third word
    		if (useDirections.Length == 4 && useDirections[2] == "on") {
    			//if item is correct, send instruction to the ItemInteractions script. Do not remove item to allow reuse if possible.
    			this.gameObject.GetComponent<ItemInteractions>().PerformItemInteraction(useDirections[1], useDirections[3]);
    		} else {
    			//if the syntax is incorrect, remind player
    			errorResponse = "Remember: use items by saying: use [item] on [target]. You can also use items on 'self'.";
    			DisplayText(errorResponse);
    		}
    		
    	}
    	else {
    		errorResponse = "You don't have anything like that.";
    		DisplayText(errorResponse);
    	}
    	

    }

    void verbExamine(string itemToInspect) {
    	bool isItemValid = false;
    	//check both the inventory and room for the item.
    	for(int i = 0; i < playerInventory.Count; i++) {          	
    		if (playerInventory[i] == itemToInspect) {
    			isItemValid = true;
    		}
    	}
    	for(int i = 0; i < currentRoom.roomItems.Count; i++) {          	
    		if (currentRoom.roomItems[i] == itemToInspect) {
    			isItemValid = true;
    		}
    	}

    	if (isItemValid == true) {
    		string examineResult = this.gameObject.GetComponent<GameLibrarian>().itemDICT[itemToInspect].itemExamine;
    		DisplayText(examineResult);
    	}
    	else {
    		errorResponse = "You don't have anything like that.";
    		DisplayText(errorResponse);
    	}
    }





    
}
