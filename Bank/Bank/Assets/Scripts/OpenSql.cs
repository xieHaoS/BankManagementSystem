using System;
using UnityEngine;
using MySql.Data.MySqlClient;


public class OpenSql : MonoBehaviour {

    //数据库名称
    private string mDataname = "labery_3215001452";
    //数据库Ip地址
    private string mIpAddress = "localhost";
    //用户名
    private string mRoot = "root";
    //密码
    private string mPaw = "root";
    //创建数据库
    MySqlConnection mMySqlConnection;

    void Start() {
        var connectionStr = string.Format("Server={0};Database={1};User ID={2};Password={3}", mIpAddress, mDataname,
            mRoot, mPaw);
        Debug.Log(connectionStr);
        mMySqlConnection = new MySqlConnection(connectionStr);
    }

    void OnGUI() {
        //打开数据库
        if (GUILayout.Button("OpenMySql")) {
            if (mMySqlConnection != null) {
                mMySqlConnection.Open();
                Debug.Log("打开数据库成功");
            }
        }
        //显示查询到的信息
        string selectCommand = "call borr_book(\'小C\', @borr_cno, @borr_borrow_date, @borr_title);\r\nselect @borr_cno, @borr_borrow_date, @borr_title;\r\n";
        if (GUILayout.Button("SelectMySql")) {
            //创建操作数据库的实例（参数是查询语句，数据库）
            try {

                MySqlCommand command = new MySqlCommand(selectCommand, mMySqlConnection);
                if (command != null) {
                    MySqlDataReader msdr = command.ExecuteReader();
                    while (msdr.Read()) {
                        //输出8列信息，，，
                        Debug.Log(msdr[0] + "\t" + msdr[1] + "\t" + msdr[2]);
                    }
                    command.Dispose(); //释放内存
                }

            }
            catch (Exception e) {
                Debug.Log(e.Message);
                throw;
            }
        }
    }
}