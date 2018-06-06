using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class VerifyPanel : BasePanel {

    private UIInput mIDNumberInput, mPasswordInput;
    private GameObject mIDNumberGameObject, mPasswordGameObject;
    // Use this for initialization
    void Awake() {
        mIDNumberInput = transform.Find("Pop/IDNumber/Input_IDNumber").GetComponent<UIInput>();
        mPasswordInput = transform.Find("Pop/Password/Input_Password").GetComponent<UIInput>();
        mIDNumberGameObject = transform.Find("Pop/IDNumber").gameObject;
        mPasswordGameObject = transform.Find("Pop/Password").gameObject;
        transform.Find("Pop/Button_Confirm").GetComponent<UIButton>().onClick.Add(new EventDelegate(ConfirmButtonOnClick));
        transform.Find("Pop/Button_Close").GetComponent<UIButton>().onClick.Add(new EventDelegate(CancelButtonOnClick));

    }
    void OnEnable() {
        ClearAllInput();
        if (GlobalManager.isPasswordInput == false) {
            mIDNumberGameObject.SetActive(true);
            mPasswordGameObject.SetActive(false);
        }
        else {
            mIDNumberGameObject.SetActive(false);
            mPasswordGameObject.SetActive(true);
        }
    }

    private void ConfirmButtonOnClick() {
        if (GlobalManager.isPasswordInput == false) {
            ConnectAdministrator();
        }
        else {
            CancelApply();
        }
    }

    private void ConnectAdministrator() {
        if (mIDNumberInput.value.Trim().Length == 0) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("请检查是否有空输入", null);
            return;
        }
        if (!GlobalManager.CheckStringLength(mIDNumberInput.value, 18)) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("请输入18位身份证号码", null);
            return;

        }
            OnClosePanel();
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("您的请求已经通知管理员", null);
    }

    private void CancelApply() {
        if ( mPasswordInput.value.Trim().Length == 0 ) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("密码输入为空", null);
        }
        else if (mPasswordInput.value != GlobalManager.userPassword) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("密码错误, 请检查后重新输入", null);
            ClearAllInput();
        }
        else {
            bool isSuccess = SqlConnectionManager.Instance.CancelAccount(GlobalManager.userAccountNumber);
            if (isSuccess) {
                UIManager.Instance.PopAllPanel();
                UIManager.Instance.PushPanel(UIPanelType.WELCOME_PANEL);
                UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
                AlertPanel.Instance.Show( "注销账户成功", null);
            }
            else {
                OnClosePanel();
                UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
                AlertPanel.Instance.Show("注销账户失败", null);
            }
        }
    }
    private void CancelButtonOnClick() {
        OnClosePanel();
    }

    private void ClearAllInput() {
        mIDNumberInput.value = "";
        mPasswordInput.value = "";
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
