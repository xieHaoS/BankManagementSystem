using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : BasePanel {
    private UIInput mAccountNumberInput, mPasswordInput;
    void Awake() {
        transform.Find("Pop/Button_Login").GetComponent<UIButton>().onClick.Add(new EventDelegate(LoginButtonOnClick));
        transform.Find("Pop/Button_Close").GetComponent<UIButton>().onClick.Add(new EventDelegate(CloseButtonOnClick));
        transform.Find("Pop/Button_Forget").GetComponent<UIButton>().onClick.Add(new EventDelegate(ForgetButtonOnClick));
        mAccountNumberInput = transform.Find("Pop/Input_AccountNumber").GetComponent<UIInput>();
        mPasswordInput = transform.Find("Pop/Input_Password").GetComponent<UIInput>();
    }

    private void ForgetButtonOnClick() {
        OnClosePanel();
        UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
        AlertPanel.Instance.Show("请联系管理员", null);
    }

    void OnEnable() {
        mAccountNumberInput.value = "";
        mPasswordInput.value = "";
    }

    private void CloseButtonOnClick() {
        OnClosePanel();
    }

    private void LoginButtonOnClick() {
        //todo 检查账号密码
        //UIManager.Instance.PopAllPanel();
        //UIManager.Instance.PushPanel(UIPanelType.HOME_PANEL);
        if (mAccountNumberInput.value.Trim().Length == 0 || mPasswordInput.value.Trim().Length == 0) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("账号或密码为空，请检查后重新登录", null);
        }
        else {
            //todo 数据库检查账号密码
            var userStatus = SqlConnectionManager.Instance.UserLogin(mAccountNumberInput.value, mPasswordInput.value);
            Debug.Log("userStatus : " + userStatus);

            if (userStatus == "normal") {
                UIManager.Instance.PopAllPanel();
                UIManager.Instance.PushPanel(UIPanelType.HOME_PANEL);
            }
            else if (userStatus == "cancel" ) {
                UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
                AlertPanel.Instance.Show("该账户已注销, 请联系管理员", null);
                mAccountNumberInput.value = "";
                mPasswordInput.value = "";
            }
            else if (userStatus == null) {
                UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
                AlertPanel.Instance.Show("账户不存在!", null);
                mAccountNumberInput.value = "";
                mPasswordInput.value = "";
            }
            else if (userStatus == "wrong") {
                UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
                AlertPanel.Instance.Show("密码错误!", null);
                mAccountNumberInput.value = "";
                mPasswordInput.value = "";
            }

        }
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
