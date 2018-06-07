using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomePanel : BasePanel {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake() {
        transform.Find("Button_UserLogin").GetComponent<UIButton>().onClick.Add(new EventDelegate(UserLoginButtonOnClick));
        transform.Find("Button_Register").GetComponent<UIButton>().onClick.Add(new EventDelegate(RegisterButtonOnClick));
        transform.Find("Button_ConnectAdministrator").GetComponent<UIButton>().onClick.Add(new EventDelegate(ConnectAdministratorButtonOnClick));
        transform.Find("Button_AdministratorLogin").GetComponent<UIButton>().onClick.Add(new EventDelegate(AdministratorLoginButtonOnClick));

    }

    private void RegisterButtonOnClick() {
        UIManager.Instance.PushPanel(UIPanelType.REGISTER_PANEL);
    }

    private void AdministratorLoginButtonOnClick() {
        UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
        AlertPanel.Instance.Show("程序正在开发中", null);
    }

    private void ConnectAdministratorButtonOnClick() {
        GlobalManager.isFromHome = false;
        UIManager.Instance.PushPanel(UIPanelType.CONNECT_PANEL);
    }

    private void UserLoginButtonOnClick() {
        UIManager.Instance.PushPanel(UIPanelType.LOGIN_PANEL);


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
