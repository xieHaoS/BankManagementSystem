using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLogPanel : BasePanel {
    private UIGrid mDetailGrid;


	// Use this for initialization
	void Awake () {
	    mDetailGrid = transform.Find("Pop/ScrollView/Grid").gameObject.GetComponent<UIGrid>();
        transform.Find("Pop/Button_Close").GetComponent<UIButton>().onClick.Add(new EventDelegate(CloseButtonOnClick));
        
	}
    void OnEnable() {
        //初始化页面
        InitPage();
        //显示作业详情
        ShowLog();

    }

    private void ShowLog() {
        GlobalManager.GetLog();
        //int count = 0;
        foreach (var tempLogItem in GlobalManager.logList) {
            var logItem = NGUITools.AddChild(mDetailGrid.gameObject, (GameObject)Resources.Load("Prefabs/DetailItem")).transform;
            string amount;

            if (tempLogItem[1].ToString().Contains("-")) {
                amount = tempLogItem[1].ToString();
            }
            else {
                amount = "+" + tempLogItem[1].ToString();
            }
            logItem.Find("Label_Time").GetComponent<UILabel>().text = tempLogItem[0].ToString();
            logItem.Find("Label_Amount").GetComponent<UILabel>().text = amount;
            //Debug.Log("tempOperateItem.operateTime: " + tempOperateItem.operateTime);
            //Debug.Log("tempOperateItem.operateAmount: " + tempOperateItem.operateAmount);
        }
        mDetailGrid.repositionNow = true;
    }

    private void InitPage() {
        //清空页面
        for (int i = 0; i < mDetailGrid.transform.childCount; i++) {
            Destroy(mDetailGrid.transform.GetChild(i).gameObject);
        }
    }

    private void CloseButtonOnClick() {
        OnClosePanel();
    }

    // Update is called once per frame
	void Update () {
		
	}

    public override void OnEnter() {
        gameObject.SetActive(true);
    }

    public override void OnExit() {
        gameObject.SetActive(false);
    }

    public void OnClosePanel() {
        UIManager.Instance.PopPanel();
    }
}
