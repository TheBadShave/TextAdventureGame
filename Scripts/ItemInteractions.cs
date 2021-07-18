using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractions : MonoBehaviour 
{
    string resultText; //used for sending result strings back to Game Director

    public void PerformItemInteraction(string item, string target) {
    	Debug.Log ("Item interaction performed using "+ item +" on "+target);
    	//cycle through usable items
    	switch (item) {
    		case "doll": useDoll(target); break;
    	}
    }

    public void useDoll(string target) {
    	switch (target) {
    		case "self":
    			resultText = "You give the doll a hug. It makes a soft squeak.";
    			this.gameObject.GetComponent<GameDirector>().DisplayText(resultText);
    			break;
    		case "pedestal":
    			this.gameObject.GetComponent<GameDirector>().playerInventory.Remove("doll");
    			resultText = "You place the doll upon the pedestal. It seems to join with it. Complete in monke.";
    			this.gameObject.GetComponent<GameLibrarian>().itemDICT["pedestal"].itemDescription = "A pedestal, complete with monkey shrine, rests here.";
    			this.gameObject.GetComponent<GameLibrarian>().itemDICT["pedestal"].itemExamine = "You feel the urge to return to monke as you stare at this.";
    			this.gameObject.GetComponent<GameDirector>().DisplayText(resultText);
    			break;


    	}
    }
}
