using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    private static UIManager sInstanceUiManager;
    private Dictionary<UIPanelType, string> mPanelPathDictionary;//存储所有面板Prefab的路径
    private Dictionary<UIPanelType, BasePanel> mPanelDictionary;//保存所有实例化面板的游戏物体身上的BasePanel组件
    private Stack<BasePanel> mPanelStack;

    public static UIManager GetInstance() {
        if (sInstanceUiManager == null) {
            var go = new GameObject("UIManager");
            sInstanceUiManager = go.AddComponent<UIManager>();
            DontDestroyOnLoad(go);
        }
        return sInstanceUiManager;
    }

    public static UIManager Instance {
        get { return GetInstance(); }
    }



    void Awake() {
        ParseUIPanelTypeJson();
    }



    private Transform mUIRootTransform;
    private Transform UIRootTransform {
        get {
            if (mUIRootTransform == null) {
                mUIRootTransform = GameObject.Find("UI Root").transform;
            }
            return mUIRootTransform;
        }
    }

    [Serializable]
    class UIPanelTypeJson {
        public List<UIPanelInformation> infoList;
    }

    private void ParseUIPanelTypeJson() {
        mPanelPathDictionary = new Dictionary<UIPanelType, string>();
        TextAsset textAsset = Resources.Load<TextAsset>("UIPanelType");
        UIPanelTypeJson jsonObject = JsonUtility.FromJson<UIPanelTypeJson>(textAsset.text);
        foreach (UIPanelInformation info in jsonObject.infoList) {
            //Debug.Log(info.path);
            mPanelPathDictionary.Add(info.panelType, info.path);
        }
    }



    private BasePanel GetPanel(UIPanelType panelType) {
        if (mPanelDictionary == null) {
            mPanelDictionary = new Dictionary<UIPanelType, BasePanel>();
        }

        BasePanel panel;
        mPanelDictionary.TryGetValue(panelType, out panel);//TODO

        if (panel == null) {
            mPanelDictionary.Remove(panelType);
            //如果找不到，那么就找这个面板的prefab的路径，然后去根据prefab去实例化面板
            string path;
            mPanelPathDictionary.TryGetValue(panelType, out path);
            GameObject instancePanel = Instantiate(Resources.Load("Prefabs/Panels/" + path)) as GameObject;
            if (instancePanel != null) {
                instancePanel.transform.SetParent(UIRootTransform, false);
                mPanelDictionary.Add(panelType, instancePanel.GetComponent<BasePanel>());
                return instancePanel.GetComponent<BasePanel>();
            }
        }
        return panel;
    }

   
    /// <summary>
    /// 显示指定的面板
    /// </summary>
    /// <param name="panelType"></param>
    public void PushPanel(UIPanelType panelType) {
        if (mPanelStack == null)
            mPanelStack = new Stack<BasePanel>();

        //判断一下栈里面是否有页面
        if (mPanelStack.Count > 0) {

            BasePanel topPanel = mPanelStack.Peek();
            //Debug.Log(topPanel);
            topPanel.OnPause();
            
        }

        BasePanel panel = GetPanel(panelType);
        //Debug.Log(panel);

        panel.OnEnter();
        mPanelStack.Push(panel);
       // GlobalManager.Instance.CurrentPanelType = panelType;
        
    }


    /// <summary>
    /// 隐藏当前面板
    /// </summary>
    public void PopPanel() {
        if (mPanelStack == null)
            mPanelStack = new Stack<BasePanel>();

        if (mPanelStack.Count <= 0) return;

        //关闭栈顶页面的显示
        BasePanel topPanel = mPanelStack.Pop();
        topPanel.OnExit();

        //判断一下栈里面是否有页面
        if (mPanelStack.Count <= 0) return;
        BasePanel topPanel2 = mPanelStack.Peek();
        topPanel2.OnResume();
    }

    /// <summary>
    /// 隐藏所有面板
    /// </summary>
    public void PopAllPanel() {
        if (mPanelStack == null)
            mPanelStack = new Stack<BasePanel>();

        if (mPanelStack.Count <= 0) return;

        //关闭栈里面所有页面的显示
        while (mPanelStack.Count > 0) {
            BasePanel topPanel = mPanelStack.Pop();
            topPanel.OnExit();
        }    
    }
}
