using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertPanel : BasePanel {

    private UIButton mConfirmButton;
    private UIButton mCancelButton;
    private UILabel mAlertLabel;


    void Awake() {
        mConfirmButton = transform.Find("Pop/Button_Confirm").GetComponent<UIButton>();
        mCancelButton = transform.Find("Pop/Button_Cancel").GetComponent<UIButton>();
        mAlertLabel = transform.Find("Pop/Label_Content").GetComponent<UILabel>();
        Instance = transform.GetComponent<AlertPanel>();
    }

    void Start() {

    }

    public static AlertPanel Instance { get; set; }

    /// <summary>
    /// 文字提示及确定按钮响应方法
    /// </summary>
    /// <param name="alertText">文字提示内容</param>
    /// <param name="confirmButtonOnClick">为空时,关闭通知窗口</param>
    public void Show(string alertText, Action confirmButtonOnClick = null) {
        mAlertLabel.text = alertText;
        mConfirmButton.gameObject.SetActive(true);
        mCancelButton.gameObject.SetActive(false);
        mConfirmButton.transform.localPosition = new Vector3(0, -140, 0);


        EventDelegate.Set(mConfirmButton.onClick, delegate
        {
            if (confirmButtonOnClick != null) {
                OnClosePanel();
                confirmButtonOnClick();
            }
            else {
                OnClosePanel();
            }
        });
    }

    /// <summary>
    /// 文字提示及确定按钮,取消按钮响应方法
    /// </summary>
    /// <param name="alertText">文字提示内容</param>
    /// <param name="confirmButtonOnClick">确定按钮响应</param>
    /// <param name="cancelButtonOnClick">为空时,关闭通知窗口</param>
    public void Show(string alertText, Action confirmButtonOnClick = null, Action cancelButtonOnClick = null) {
        mAlertLabel.text = alertText;
        mConfirmButton.gameObject.SetActive(true);
        mCancelButton.gameObject.SetActive(true);
        mConfirmButton.transform.localPosition = new Vector3(-140, -140, 0);
        mCancelButton.transform.localPosition = new Vector3(140, -140, 0);

        EventDelegate.Set(mConfirmButton.onClick, delegate
        {
            if (confirmButtonOnClick != null) {
                OnClosePanel();
                confirmButtonOnClick();
            }
            else {
                OnClosePanel();
            }
        });

        EventDelegate.Set(mCancelButton.onClick, delegate
        {
            if (cancelButtonOnClick != null) {
                OnClosePanel();
                cancelButtonOnClick();
            }
            else {
                OnClosePanel();
            }
        });
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
