using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class OperatePanel : BasePanel {
    private UIInput mAmountInput, mPasswordInput;
    private UILabel mTitleLabel;
    // Use this for initialization
    void Awake() {
        mAmountInput = transform.Find("Pop/Input_Amount").GetComponent<UIInput>();
        mPasswordInput = transform.Find("Pop/Input_Password").GetComponent<UIInput>();
        mTitleLabel = transform.Find("Pop/Label_Title").GetComponent<UILabel>();
        transform.Find("Pop/Button_Confirm").GetComponent<UIButton>().onClick.Add(new EventDelegate(ConfirmButtonOnClick));
        transform.Find("Pop/Button_Cancel").GetComponent<UIButton>().onClick.Add(new EventDelegate(CancelButtonOnClick));
    }

    void OnEnable() {
        mTitleLabel.text = GlobalManager.isDeposit ? "存款" : "取款";
        ClearAllInput();
    }

    private void ConfirmButtonOnClick() {
        if (mAmountInput.value.Trim().Length == 0 || mPasswordInput.value.Trim().Length == 0) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("金额或密码为空，请检查后重新输入", null);
        }
        else if (mAmountInput.value.Contains("-")) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("金额不能为负数", null);
            ClearAllInput();
        }

        else if (mPasswordInput.value != GlobalManager.userPassword) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("密码错误, 请检查后重新输入", null);
            ClearAllInput();
        }
        else {
            //todo 提交存款记录
            var tempAmount = Convert.ToDecimal(mAmountInput.value);
            Decimal amount = decimal.Round(tempAmount, 2);
            string temp = amount.ToString(CultureInfo.CurrentCulture);
            //Debug.Log("提交存款记录");
            //Debug.Log(balance);
            //Debug.Log(mPasswordInput.value);
            if (GlobalManager.isDeposit) {
                if (temp.Length > 9) {
                    UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
                    AlertPanel.Instance.Show("存款失败, 存款金额过大", null);
                    ClearAllInput();
                    return;
                }
                //mTitleLabel.text = "存款";
                bool isSuccess = SqlConnectionManager.Instance.Deposit(GlobalManager.userAccountNumber, amount);
                Debug.Log(isSuccess);
                if (isSuccess) {
                    OnClosePanel();
                    UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
                    AlertPanel.Instance.Show("存款成功", null);
                    ClearAllInput();
                }
            }
            else {
                if (amount >= GlobalManager.balance) {
                    UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
                    AlertPanel.Instance.Show("不能取出超过账户余额的钱", null);
                    return;
                }
                bool isSuccess = SqlConnectionManager.Instance.Withdraw(GlobalManager.userAccountNumber, amount);
                Debug.Log(isSuccess);
                if (isSuccess) {
                    //OnClosePanel();
                    OnClosePanel();

                    UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
                    AlertPanel.Instance.Show("取款成功", null);
                    ClearAllInput();

                }
            }
        }
        //HomePanel.Instance.RefreshInformationBar();
        GlobalManager.RefreshBalance();
    }


    private void CancelButtonOnClick() {
        OnClosePanel();
    }

    private void ClearAllInput() {
        mAmountInput.value = "";
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
