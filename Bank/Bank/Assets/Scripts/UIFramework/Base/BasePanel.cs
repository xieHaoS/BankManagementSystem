using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour {

    /// <summary>
    /// 开启交互,页面显示
    /// </summary>
    public virtual void OnEnter() {

    }

    /// <summary>
    /// 界面暂停,关闭交互
    /// </summary>
    public virtual void OnPause() {

    }

    /// <summary>
    /// 界面继续,恢复交互
    /// </summary>
    public virtual void OnResume() {
        
    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关闭
    /// </summary>
    public virtual void OnExit() {
        
    }

}
