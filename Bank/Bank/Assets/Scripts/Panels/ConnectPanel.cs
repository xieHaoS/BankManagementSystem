using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectPanel : BasePanel {

    void Awake() {
        transform.Find("Pop/Button_Close").GetComponent<UIButton>().onClick.Add(new EventDelegate(CloseButtonOnClick));
        transform.Find("Pop/Button_FreezeApply").GetComponent<UIButton>().onClick.Add(new EventDelegate(FreezeApplyButtonOnClick));
        transform.Find("Pop/Button_ThawApply").GetComponent<UIButton>().onClick.Add(new EventDelegate(ThawApplyButtonOnClick));
        transform.Find("Pop/Button_ResetApply").GetComponent<UIButton>().onClick.Add(new EventDelegate(ResetApplyButtonOnClick));
    }

    void OnEnable() {
        GlobalManager.isPasswordInput = false;
    }
    private void FreezeApplyButtonOnClick() {
        GlobalManager.connectType = ConnectType.FREEZE_APPLY;
        AlertOrVertify();

    }

    private void ThawApplyButtonOnClick() {
        GlobalManager.connectType = ConnectType.THAW_APPLY;
        AlertOrVertify();

    }

    private void ResetApplyButtonOnClick() {
        GlobalManager.connectType = ConnectType.RESET_APPLY;
        AlertOrVertify();
    }

    private void AlertOrVertify() {
        OnClosePanel();
        if (GlobalManager.isFromHome) {
            UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
            AlertPanel.Instance.Show("您的申请已经通知管理员", null);
        }
        else {
            UIManager.Instance.PushPanel(UIPanelType.VERIFY_PANEL);
        }
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
