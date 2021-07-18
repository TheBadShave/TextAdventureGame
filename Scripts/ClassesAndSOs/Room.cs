using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextQuest/Room")]
public class Room : ScriptableObject {

	public string roomName;
	public string roomDescription;
	public List<Exit> roomExits = new List<Exit>();
	public List<string> roomItems = new List<string>();
}
