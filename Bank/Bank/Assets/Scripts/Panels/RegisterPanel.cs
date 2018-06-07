using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = System.Random;


public class RegisterPanel : BasePanel {
    private UIInput mIDNumberInput, mNameInput, mAddressInput, mPasswordInput, mConfirmPasswordInput;
    private string mRandomAccountNumber;
    void Awake() {
        transform.Find("Pop/Button_Close").GetComponent<UIButton>().onClick.Add(new EventDelegate(CloseButtonOnClick));
        transform.Find("Pop/Button_Submit").GetComponent<UIButton>().onClick.Add(new EventDelegate(SubmitButtonOnClick));
        mIDNumberInput = transform.Find("Pop/Input_IDNumber").GetComponent<UIInput>();
        mNameInput = transform.Find("Pop/Input_Name").GetComponent<UIInput>();
        mAddressInput = transform.Find("Pop/Input_Address").GetComponent<UIInput>();
        mPasswordInput = transform.Find("Pop/Input_Password").GetComponent<UIInput>();
        mConfirmPasswordInput = transform.Find("Pop/Input_ConfirmPassword").GetComponent<UIInput>();

    }

    void OnEnable() {
        ClearAllInput();
    }

    private void SubmitButtonOnClick() {

        if (mIDNumberInput.value.Trim().Length == 0 || mNameInput.value.Trim().Length == 0 || mAddressInput.value.Trim().Length == 0 || mPasswordInput.value.Trim().Length == 0 || mConfirmPasswordInput.value.Trim().Length == 0) {

            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("请检查是否有空输入", null);
            return;
        }
        if (!GlobalManager.CheckStringLength(mPasswordInput.value, 6)) {
            ClearAllInput();
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("请输入6位数字密码", null);
            return;
        }

        if (!GlobalManager.CheckStringLength(mIDNumberInput.value, 18)) {
            ClearAllInput();
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("请输入18位身份证号码", null);
            return;
        }
        if (mPasswordInput.value != mConfirmPasswordInput.value) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("两次输入的密码不一样!", null);
            mPasswordInput.value = "";
            mConfirmPasswordInput.value = "";
            return;
        }

        //todo 提交申请
        Debug.Log("数据无误, 提交申请");
        mRandomAccountNumber = RandomAccountNumber();
        Debug.Log("randomAccountNumber: " + mRandomAccountNumber);
        string userStatus = SqlConnectionManager.Instance.Register(mRandomAccountNumber, mIDNumberInput.value, mNameInput.value,
            mAddressInput.value, mPasswordInput.value);
        if (userStatus == null) {
            GlobalManager.RefreshUserInformation(mNameInput.value, mPasswordInput.value, mRandomAccountNumber, 0);
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("开户成功, 账号为: " + mRandomAccountNumber,SwitchToHomePanel,  null);
            ClearAllInput();
            
        }
        else {
            ClearAllInput();
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("开户失败, \n 该身份证的账号状态为: " + userStatus,null);
        }
    }

    private void SwitchToHomePanel() {
        //GlobalManager.balance = "normal";
        //GlobalManager.userAccountNumber = mRandomAccountNumber;
        //GlobalManager.userName = mNameInput.value;
        //GlobalManager.balance = 0.ToString();
        //GlobalManager.RefreshUserInformation(mNameInput.value, mPasswordInput.value, mRandomAccountNumber, 0.ToString());
        UIManager.Instance.PopAllPanel();
        UIManager.Instance.PushPanel(UIPanelType.HOME_PANEL);
    }

    /// <summary>
    /// 生成19位随机数
    /// </summary>
    /// <returns></returns>
    private string RandomAccountNumber() {
        int iSeed = 10;
        Random ro = new Random(10);
        long tick = DateTime.Now.Ticks;
        Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
        StringBuilder sb = new StringBuilder();
        int Nums = 19;
        while (Nums > 0) {
            //Random ran = new Random();
            int i = ran.Next(0, 9);
            //string i = Guid.NewGuid().ToString();// 9>=a>=1
            if (sb.Length < 19) {
                sb.Append(i);
            }
            Nums -= 1;
        }

        return sb.ToString();
    }

    private void ClearAllInput() {
        mIDNumberInput.value = "";
        mNameInput.value = "";
        mAddressInput.value = "";
        mPasswordInput.value = "";
        mConfirmPasswordInput.value = "";
    }
    private void CloseButtonOnClick() {
        OnClosePanel();
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
