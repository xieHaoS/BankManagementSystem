using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnityEngine;

public class SqlConnectionManager : MonoBehaviour {

    private static SqlConnectionManager sSqlConnectionManager;
    //数据库名称
    private string mDataname = "bank";
    //数据库Ip地址
    private string mIpAddress = "localhost";
    //用户名
    private string mRoot = "root";
    //密码
    private string mPaw = "biack0";
    //创建数据库
    static MySqlConnection sMySqlConnection;

    private List<object[]> mLogList;

    void Awake() {
        mLogList = new List<object[]>();
        var connectionStr = string.Format("Server={0};Database={1};User ID={2};Password={3}", mIpAddress, mDataname,
            mRoot, mPaw);
       // Debug.Log(connectionStr);
        sMySqlConnection = new MySqlConnection(connectionStr);
        //if (mMySqlConnection != null) {
        //    mMySqlConnection.Open();
        //    Debug.Log("打开数据库成功");
        //}
    }


    public static SqlConnectionManager GetInstance() {
        if (sSqlConnectionManager == null) {
            var go = new GameObject("UIManager");
            sSqlConnectionManager = go.AddComponent<SqlConnectionManager>();
            DontDestroyOnLoad(go);
        }
        return sSqlConnectionManager;
    }

    public static SqlConnectionManager Instance {
        get { return GetInstance(); }
    }


    /// <summary>
    /// 用户登录连接数据库
    /// </summary>
    /// <param name="userAccountNumber">用户输入的账号</param>
    /// <param name="userPassword">用户输入的密码</param>
    /// <returns></returns>
    public string UserLogin(string userAccountNumber, string userPassword) {
        string selectCommand = "select user_password, user_status, user_name, balance\r\nfrom bank_user\r\nwhere user_account_number = \'" +userAccountNumber + "\'";
        string userStatus = null;
        using (sMySqlConnection) {
            sMySqlConnection.Open();
            try {
                MySqlCommand command = new MySqlCommand(selectCommand, sMySqlConnection);
                if (command != null) {
                    MySqlDataReader msdr = command.ExecuteReader();
                    using (msdr) {
                        while (msdr.Read()) {
                            Debug.Log(msdr[0] + "\t" + msdr[1] + "\t" + msdr[2] + "\t" + msdr[3]);
                            userStatus = msdr[1] as string;
                            if (userPassword != (string) msdr[0]) {
                                Debug.Log("密码错误!");
                                return "wrong";
                            }
                            if (userStatus == "normal") {
                                var userName = (string)msdr[2];
                                var balance = (decimal)msdr[3];
                                //Debug.Log(balance);
                                GlobalManager.RefreshUserInformation(userName, userPassword, userAccountNumber, balance); 
                            }
                        }
                    }
                    return userStatus;
                }
            }
            catch (Exception e) {
                Debug.Log(e.Message);
                throw;
            }
            return null;
        }
    }

    public string Register(string userAccountNumber, string IDNumber, string name, string address, string userPassword) {
        string userStatus = CheckAccountStatus(IDNumber);
        switch (userStatus) {
            case null:
                Debug.Log("该身份证号码没有注册过账号");
                break;
            case "normal":
                Debug.Log("该身份证号码已经注册过账号, 状态为normal");
                return userStatus;
                break;
            case "cancel":
                Debug.Log("该身份证号码已经注册过账号, 状态为cancel");
                return userStatus;
                break;
            case "frozen":
                Debug.Log("该身份证号码已经注册过账号, 状态为frozen");
                return userStatus;
                break;  
        }
        string cmdText = string.Format("call procedure_register ('{0}','{1}','{2}','{3}','{4}')", userAccountNumber, name,IDNumber,
            address, userPassword);
        using (sMySqlConnection) {
            sMySqlConnection.Open();
            //string userStatus = null;
            try {
                MySqlCommand command = new MySqlCommand(cmdText, sMySqlConnection);
                if (command != null) {
                    MySqlDataReader msdr = command.ExecuteReader();
                    using (msdr) {
                        if (msdr.Read() == false) {
                            Debug.Log("成功!");
                        }
                    }
                }
            }
            catch (Exception e) {
                Debug.Log(e.Message);
                throw;
                
            }

            return null;
        }
    }

    public string CheckAccountStatus(string IDNumber) {
        string selectCommand = "select user_status from bank_user where id_number = '" + IDNumber + "'";//todo
        using (sMySqlConnection) {
            sMySqlConnection.Open();
            string userStatus = null;
            try {
                MySqlCommand command = new MySqlCommand(selectCommand, sMySqlConnection);
                if (command != null) {
                    MySqlDataReader msdr = command.ExecuteReader();
                    using (msdr) {
                        if (msdr.Read() == false) {
                            Debug.Log("该身份证号码没有注册过账号");
                            userStatus = null;
                        }
                        else {
                            while (msdr.Read()) {
                                Debug.Log("userStatus: " + msdr[0]);
                                userStatus = (string) msdr[0] ;
                            }
                        }
                        return userStatus;
                    }
                }
            }
            catch (Exception e) {
                Debug.Log(e.Message);
                throw;
            }
        }
        return null;
    }

    public decimal GetBalance(string accountNumber) {
        string selectCommand = "select balance\r\nfrom bank_user\r\nwhere user_account_number = \'" + accountNumber + "\'";
        string userStatus = null;
        using (sMySqlConnection) {
            sMySqlConnection.Open();
            try {
                Decimal balance = 0;
                MySqlCommand command = new MySqlCommand(selectCommand, sMySqlConnection);
                if (command != null) {

                    MySqlDataReader msdr = command.ExecuteReader();
                    using (msdr) {
                        while (msdr.Read()) {
                            
                            Debug.Log(msdr[0] );
                            balance = (decimal) msdr[0];
                        }
                        }
                    }
                    return balance;
                
            }
            catch (Exception e) {
                Debug.Log(e.Message);
                throw;
            }
            return 0;
        }
    }

    public List<object[]> ShowLog(string accountNumber) {
        if (mLogList.Count != 0) {
            mLogList.Clear();
        }
        string cmdText = string.Format("call procedure_record ('{0}')", accountNumber);
        using (sMySqlConnection) {  
            sMySqlConnection.Open();
            try {
                MySqlCommand command = new MySqlCommand(cmdText, sMySqlConnection);
                if (command != null) {
                    MySqlDataReader msdr = command.ExecuteReader();
                    using (msdr) {
                        while (msdr.Read()) {
                            Debug.Log(msdr[0] + "\t" + msdr[1]);
                            var objects = new[] {msdr[0], msdr[1]};
                            mLogList.Add(objects);
                        }
                        return mLogList;
                    }
                }
            }
            catch (Exception e) {
                Debug.Log(e.Message);
                throw;
            }
        }
        return null;
    }

    public bool Deposit(string accountNumber, decimal amount) {
        string cmdText = string.Format("call procedure_deposit ('{0}','{1}')", accountNumber, amount);
        using (sMySqlConnection) {
            sMySqlConnection.Open();
            try {
                MySqlCommand command = new MySqlCommand(cmdText, sMySqlConnection);
                if (command != null) {
                    MySqlDataReader msdr = command.ExecuteReader();
                    using (msdr) {
                        if (msdr.Read() == false) {
                            Debug.Log("存款成功");
                            return true;
                        }
                    }
                }
            }
            catch (Exception e) {
                Debug.Log(e.Message);
                throw;
            }
        }
        return false;

    }

    public bool Withdraw(string accountNumber, decimal amount) {
        string cmdText = string.Format("call procedure_withdraw ('{0}','{1}')", accountNumber, amount);
        using (sMySqlConnection) {
            sMySqlConnection.Open();
            try {
                MySqlCommand command = new MySqlCommand(cmdText, sMySqlConnection);
                if (command != null) {
                    MySqlDataReader msdr = command.ExecuteReader();
                    using (msdr) {
                        if (msdr.Read() == false) {
                            Debug.Log("取款成功");
                            return true;
                        }
                    }
                }
            }
            catch (Exception e) {
                Debug.Log(e.Message);
                throw;
            }
        }
        return false;

    }
    public bool ResetPassword(string accountNumber, string newPassword) {
        string cmdText = string.Format("update bank_user set user_password = '{0}' where user_account_number ='{1}' ", newPassword, accountNumber);
        using (sMySqlConnection) {
            sMySqlConnection.Open();
            try {
                MySqlCommand command = new MySqlCommand(cmdText, sMySqlConnection);
                if (command != null) {
                    MySqlDataReader msdr = command.ExecuteReader();
                    using (msdr) {
                        if (msdr.Read() == false) {
                            Debug.Log("修改密码成功");
                            return true;
                        }
                    }
                }
            }
            catch (Exception e) {
                Debug.Log(e.Message);
                throw;
            }
        }
        return false;

    }

    public bool CancelAccount(string accountNumber) {
        string cmdText = string.Format("call procedure_cancel('{0}')", accountNumber);
        using (sMySqlConnection) {
            sMySqlConnection.Open();
            try {
                MySqlCommand command = new MySqlCommand(cmdText, sMySqlConnection);
                if (command != null) {
                    MySqlDataReader msdr = command.ExecuteReader();
                    using (msdr) {
                        if (msdr.Read() == false) {
                            Debug.Log("注销账户成功");
                            return true;
                        }
                    }
                }
            }
            catch (Exception e) {
                Debug.Log(e.Message);
                throw;
            }
        }
        return false;

    }
}
