using GameCore.UI;
using TMPro;
using UnityEngine;

public class ConnectionUI : MonoBehaviour
{
    public static ConnectionUI GetConnectionUI;
    public MenuUI menuUI;
    public TMP_Text connInfo;
    public GameObject connPanel;
    private void Awake()
    {
        GetConnectionUI = this;
        menuUI = GameObject.Find("MainUI").GetComponent<MenuUI>();
    }

    public void Connect()
    {
        menuUI.Connect();
    }

    public void Disconnect(string reason)
    {
        menuUI.Disconnect();
        connPanel.SetActive(true);
        connInfo.text = $"CONNECTION FAILED: {reason}";
    }
}
