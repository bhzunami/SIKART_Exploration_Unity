using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

	public Dropdown DropDownX;
	public Dropdown DropDownY;
	public Dropdown DropDownZ;
    public Dropdown DropDownColor;
    public Dropdown DropDownSize;
    public Dropdown DropdownFixScale;

    public ParseDB ParseDB;
    public MakeViz MakeViz;
    
    // Use this for initialization
    void Start () {
        DropDownX.onValueChanged.AddListener(act => MakeViz.DataColumnX = DropDownX.value + 1);
        DropDownY.onValueChanged.AddListener(act => MakeViz.DataColumnY = DropDownY.value + 1);
        DropDownZ.onValueChanged.AddListener(act => MakeViz.DataColumnZ = DropDownZ.value + 1);
        DropDownColor.onValueChanged.AddListener(act => MakeViz.DataColumnColour = DropDownColor.value + 1);
        DropDownSize.onValueChanged.AddListener(act => MakeViz.DataColumnSize = DropDownSize.value + 1);
        DropdownFixScale.onValueChanged.AddListener(act =>
        {
            MakeViz.InstanceScale = float.Parse(DropdownFixScale.options[DropdownFixScale.value].text);
        });
    }

	public void updateDropDownValues(int nCol) {
		DropDownX.options.Clear ();
		DropDownY.options.Clear ();
		DropDownZ.options.Clear ();
        DropDownColor.options.Clear();
        DropDownSize.options.Clear();

        for (int i = 1; i <= nCol; i++) {
			DropDownX.options.Add(new Dropdown.OptionData(ParseDB.Header[i-1]));
			DropDownY.options.Add(new Dropdown.OptionData(ParseDB.Header[i - 1]));
			DropDownZ.options.Add(new Dropdown.OptionData(ParseDB.Header[i - 1]));
            DropDownColor.options.Add(new Dropdown.OptionData(ParseDB.Header[i - 1]));
            DropDownSize.options.Add(new Dropdown.OptionData(ParseDB.Header[i - 1]));
        }

        DropDownX.value = MakeViz.DataColumnX - 1;
		DropDownY.value = MakeViz.DataColumnY - 1;
		DropDownZ.value = MakeViz.DataColumnZ - 1;
        DropDownColor.value = MakeViz.DataColumnColour - 1;
        DropDownSize.value = MakeViz.DataColumnSize - 1;
        DropdownFixScale.value = 2;

	}

}
