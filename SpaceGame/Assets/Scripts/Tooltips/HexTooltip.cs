﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HexScript))]
public class HexTooltip : MonoBehaviour {
	
	// code learned from following example below so some similarities may appear
	// http://answers.unity3d.com/questions/44811/tooltip-when-mousing-over-a-game-object.html
	
	public string toolTipText = "";
	private string currentToolTipText = ""; 
	private string toolTipAdditionalText = ""; // extra feature help info
	private GUIStyle guiStyleFore;
	private GUIStyle guiStyleBack;
	private int delay; // the delay time for the tooltip to appear

	public void Start()
	{
		// set up
		delay = 0;
		HexScript thisHex = this.GetComponent<HexScript>();
		guiStyleFore = new GUIStyle();
		guiStyleFore.normal.textColor = Color.white;
		guiStyleFore.alignment = TextAnchor.UpperCenter ;
		guiStyleFore.wordWrap = true;
		guiStyleBack = new GUIStyle();
		guiStyleBack.normal.textColor = Color.black;
		guiStyleBack.alignment = TextAnchor.UpperCenter ;
		guiStyleBack.wordWrap = true;

		// adds the type info
		string terrain = "TYPE:   " + thisHex.terrainType;

		// adds the cost info
		string cost = "\nCOST:   ";
		if (thisHex.terrainType.ToString().Equals("Lake") || thisHex.terrainType.ToString().Equals("Mountains")) {
			cost =  cost +  "Impassable";
		}
		else {
			cost =  cost +  HexScript.TerrainTypeToVal(thisHex.terrainType);
		}

		// adds the hex feature info
		string feature = "";
		if (thisHex.hexFeature.ToString ().Equals("Portal")) {
			feature = "\nFEATURE:   Crash site";
			toolTipAdditionalText = "\nThe shattered remains of your poor rocket lie here.";
		}
		else if (thisHex.hexFeature.ToString ().Equals("Glade")) {
			feature = "\nFEATURE:   Glade";
			toolTipAdditionalText = "\nYou can do so and so here";
		}
		else if (thisHex.hexFeature.ToString ().Equals("Town")) {
			feature = "\nFEATURE:   Town";
			toolTipAdditionalText = "\n";
		}
		else if (thisHex.hexFeature.ToString ().Equals("Monastary")) {
			feature = "\nFEATURE:   Monastary";
			toolTipAdditionalText = "\n";
		}
		else if (thisHex.hexFeature.ToString ().Equals("Base")) {
			feature = "\nFEATURE:   Base";
			toolTipAdditionalText = "\n";
		}
		else if (thisHex.hexFeature.ToString ().Equals("DarkMatterResearch")) {
			feature = "\nFEATURE:   Dark Matter Research";
			toolTipAdditionalText = "\n";
		}
		else if (thisHex.hexFeature.ToString ().Equals("Maze")) {
			feature = "\nFEATURE:   Maze";
			toolTipAdditionalText = "\n";
		}
		else if (thisHex.hexFeature.ToString ().Equals("Labyrinth")) {
			feature = "\nFEATURE:   Labyrinth";
			toolTipAdditionalText = "\n";
		}
		else if (thisHex.hexFeature.ToString ().Equals ("RampageGreen") &&
		    thisHex.hexFeature.ToString ().Equals ("RampageRed") ) {
			feature = "\nFEATURE:   Rampaging Alien";
			toolTipAdditionalText = "\n";
		}
		else if (thisHex.hexFeature.ToString ().Equals ("CityWhite") &&
		         thisHex.hexFeature.ToString ().Equals ("CityRed") &&
		         thisHex.hexFeature.ToString ().Equals ("CityGreen") &&
		         thisHex.hexFeature.ToString ().Equals ("CityBlue")) {
			feature = "\nFEATURE:   City";
			toolTipAdditionalText = "\n";
		}
		else if (thisHex.hexFeature.ToString ().Equals ("MineBlue") &&
		         thisHex.hexFeature.ToString ().Equals ("MineRed") &&
		         thisHex.hexFeature.ToString ().Equals ("MineGreen") &&
		         thisHex.hexFeature.ToString ().Equals ("MineWhite") &&
		         thisHex.hexFeature.ToString ().Equals ("MineDeep")) {
			feature = "\nFEATURE:   Energy Mine";
			toolTipAdditionalText = "\n";
		} 
		else {
		}

		toolTipText = terrain + cost + feature;
	}

	// Shows the tool tip after a delay because we may not want to see hex data all the time
	public void OnMouseOver() {
		delay++;
		if (delay > 150) { // if user is looking at this long enough, add help text
			currentToolTipText = toolTipText + toolTipAdditionalText;
		}
		else if (delay > 75) { // else just show regular text
			currentToolTipText = toolTipText;
		}
	}

	// resets the delay and tool text when mouse event finishes
	public void OnMouseExit ()
	{
		delay = 0;
		currentToolTipText = "";
	}

	// makes the tool tip
	public void OnGUI()
	{
		if (currentToolTipText != "")
		{
			float x = Event.current.mousePosition.x;
			float y = Event.current.mousePosition.y;
			GUI.Label (new Rect (x-149,y+21,300,60), currentToolTipText, guiStyleBack);
			GUI.Label (new Rect (x-150,y+20,300,60), currentToolTipText, guiStyleFore);
		}
	}
}
