using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		UIManager.Instance.PushPanel(UIPanelType.WELCOME_PANEL);
        
	}
	
	// Update is called once per frame
	void Update () {
	   
	}
}
