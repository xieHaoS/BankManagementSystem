using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPasswordPanel : BasePanel {
    private UIInput mNewPassordInput, mOldPasswordInput, mConfirmPasswordInput;

    // Use this for initialization
    void Awake() {
        mNewPassordInput = transform.Find("Pop/Input_NewPassword").GetComponent<UIInput>();
        mOldPasswordInput = transform.Find("Pop/Input_OldPassword").GetComponent<UIInput>();
        mConfirmPasswordInput = transform.Find("Pop/Input_ConfirmPassword").GetComponent<UIInput>();
        transform.Find("Pop/Button_Confirm").GetComponent<UIButton>().onClick.Add(new EventDelegate(ConfirmButtonOnClick));
        transform.Find("Pop/Button_Cancel").GetComponent<UIButton>().onClick.Add(new EventDelegate(CancelButtonOnClick));
        
    }
    void OnEnable() {
        ClearAllInput();
    }

    private void ConfirmButtonOnClick() {
        if (mNewPassordInput.value.Trim().Length == 0 || mOldPasswordInput.value.Trim().Length == 0 || mConfirmPasswordInput.value.Trim().Length == 0) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("请检查是否有空输入", null);
        }

        else if (mOldPasswordInput.value != GlobalManager.userPassword) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("旧密码错误, 请检查后重新输入", null);
            ClearAllInput();
        }
        else if (mNewPassordInput.value.Length != 6) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("新密码必须为6位数字", null);
            ClearAllInput();
        }
        else if (mNewPassordInput.value == mOldPasswordInput.value) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("新密码不能与旧密码一样", null);
            ClearAllInput();
        }
        else if (mNewPassordInput.value != mConfirmPasswordInput.value) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("两次输入的新密码不一样", null);
            ClearAllInput();

        }
        else {
            bool isSuccess = GlobalManager.ResetPassword(mNewPassordInput.value);
            OnClosePanel();
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show(isSuccess ? "重置密码成功" : "重置密码失败", null);
        }
    }


    private void CancelButtonOnClick() {
        OnClosePanel();
    }

    private void ClearAllInput() {
        mNewPassordInput.value = "";
        mOldPasswordInput.value = "";
        mConfirmPasswordInput.value = "";
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
