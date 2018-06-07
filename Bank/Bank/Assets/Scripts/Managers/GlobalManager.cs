using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ConnectType {
    FREEZE_APPLY,
    THAW_APPLY,
    RESET_APPLY
}
public class GlobalManager {

    public static string userName;
    public static string userPassword;
    public static string userAccountNumber;
    public static decimal balance;
    public static bool isDeposit;
    public static bool isPasswordInput;
    public static bool isFromHome;
    public static List<object[]> logList;
    public static ConnectType connectType;



    /// <summary>
    /// 退出应用程序
    /// </summary>
    public static void QuitApplication() {
        UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
        AlertPanel.Instance.Show("是否要退出银行管理系统", Application.Quit, null);
    }

    public static void RefreshUserInformation(string refreshUserName, string refreshUserPassword, string refreshUserAccountNumber, decimal refreshBalance) {
        userName = refreshUserName;
        userPassword = refreshUserPassword;
        userAccountNumber = refreshUserAccountNumber;
        balance = refreshBalance;
        //Debug.Log("userName: " + refreshUserName);
        //Debug.Log("userPassword: " + userPassword);
        //Debug.Log("userAccountNumber: " + userAccountNumber);
        //Debug.Log("balance: " + balance);
    }

    public static void RefreshBalance() {
        balance = SqlConnectionManager.Instance.GetBalance(userAccountNumber);
        Debug.Log("RefreshBalance: " + balance);
    }

    public static void ShowOperateHint() {
        UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
        AlertPanel.Instance.Show(isDeposit ? "存款成功" : "取款成功", null);
    }

    public static void GetLog() {
        if (logList == null) {
            logList = new List<object[]>();
        }

        logList = SqlConnectionManager.Instance.ShowLog(userAccountNumber);
        foreach (var temp in logList) {
            Debug.Log(temp[0] + "\t" + temp[1]);
            
        }
    }

    public static bool ResetPassword(string newPassword) {
       bool isSuccess = SqlConnectionManager.Instance.ResetPassword(userAccountNumber, newPassword);
        if (isSuccess) {
            userPassword = newPassword;
        }
        return isSuccess;
    }
    /// <summary>
    /// 检查字符串长度
    /// </summary>
    /// <param name="tempString">需要检验的字符串</param>
    /// <param name="length">要检验的长度</param>
    /// <returns></returns>
    public static bool CheckStringLength(string tempString, int length) {
        if (tempString.Length == length) {
            return true;
        }
        return false;
    }
}
