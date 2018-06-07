using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class HomePanel : BasePanel {
    private UILabel mNameLabel, mAccountNumberLabel, mBalanceLabel;
    void Awake() {
        transform.Find("Button_Logout").GetComponent<UIButton>().onClick.Add(new EventDelegate(LogoutButtonOnClick));
        transform.Find("Button_ShowLog").GetComponent<UIButton>().onClick.Add(new EventDelegate(ShowLogButtonOnClick));
        transform.Find("Button_Deposit").GetComponent<UIButton>().onClick.Add(new EventDelegate(DepositButtonOnClick));
        transform.Find("Button_Withdraw").GetComponent<UIButton>().onClick.Add(new EventDelegate(WithdrawButtonOnClick));
        transform.Find("Button_ResetPassword").GetComponent<UIButton>().onClick.Add(new EventDelegate(ResetPasswordButtonOnClick));
        transform.Find("Button_CancelAccount").GetComponent<UIButton>().onClick.Add(new EventDelegate(CancelAccountButtonOnClick));
        transform.Find("Button_ConnectAdministrator").GetComponent<UIButton>().onClick.Add(new EventDelegate(ConnectAdministratorButtonOnClick));
        mNameLabel = transform.Find("PersonalInformation/Label_Name").GetComponent<UILabel>();
        mAccountNumberLabel = transform.Find("PersonalInformation/Label_AccountNumber").GetComponent<UILabel>();
        mBalanceLabel = transform.Find("PersonalInformation/Label_Balance").GetComponent<UILabel>();
    }

    void OnEnable() {
        RefreshInformationBar();
    }
    public static HomePanel Instance { get; set; }

    public void RefreshInformationBar() {
        mNameLabel.text = GlobalManager.userName;
        mAccountNumberLabel.text = GlobalManager.userAccountNumber;
        mBalanceLabel.text = "余额: "+GlobalManager.balance.ToString(CultureInfo.InvariantCulture);

    }

    private void ShowLogButtonOnClick() {
        UIManager.Instance.PushPanel(UIPanelType.SHOW_LOG_PANEL);
        //SqlConnectionManager.Instance.GetLog(GlobalManager.userAccountNumber);
    }

    private void DepositButtonOnClick() {
        GlobalManager.isDeposit = true;
        UIManager.Instance.PushPanel(UIPanelType.OPERATE_PANEL);
    }

    private void WithdrawButtonOnClick() {
        GlobalManager.isDeposit = false;
        UIManager.Instance.PushPanel(UIPanelType.OPERATE_PANEL);

    }

    private void ResetPasswordButtonOnClick() {
        UIManager.Instance.PushPanel(UIPanelType.RESET_PASSWORD_PANEL);

    }

    private void CancelAccountButtonOnClick() {
        GlobalManager.isPasswordInput = true;
        UIManager.Instance.PushPanel(UIPanelType.VERIFY_PANEL);

    }

    private void ConnectAdministratorButtonOnClick() {
        GlobalManager.isFromHome = true;

        UIManager.Instance.PushPanel(UIPanelType.CONNECT_PANEL);
    }

    private void LogoutButtonOnClick() {
        OnClosePanel();
        UIManager.Instance.PushPanel(UIPanelType.WELCOME_PANEL);
    }

    public override void OnEnter() {
        gameObject.SetActive(true);
    }

    public override void OnExit() {
        gameObject.SetActive(false);
    }

    public override void OnResume() {
        RefreshInformationBar();
    }



    public void OnClosePanel() {
        UIManager.Instance.PopPanel();
    }
}
