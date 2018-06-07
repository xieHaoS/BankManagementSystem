using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHitDebug : MonoBehaviour {
    
    private string mText;
    public bool isDebug;
    void OnGUI() {
        GUIStyle style = new GUIStyle();
        style.normal.background = null;    //这是设置背景填充的
        style.normal.textColor = new Color(1, 0, 0);   //设置字体颜色的
        style.fontSize = 60;       //当然，这是字体大小
        GUI.Label(new Rect(0, 0, 200, 200), mText, style);

    }

	
	// Update is called once per frame
	void Update () {
        if (isDebug) {
            //射线检查
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f)) {
                mText = hit.collider.gameObject.transform.name;
            }
            else {
                mText = "NULL";
            }
        }
        else {
            mText = " ";
        }
	}
}
