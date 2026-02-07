using Mirror;
using NetCore;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.UI
{
    public class NetworkManagerUI : MonoBehaviour
    {
        NetManager netManager;
        private void Start()
        {
            netManager = NetManager.Instance;
        }

        public void Connect()
        {
            netManager.StartClient();
        }

        public void Host()
        {
            netManager.StartHost();
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void SetIP(string value)
        {
            if(value == string.Empty)
            {
                PortTransport portTransport = (PortTransport)netManager.GetComponent(typeof(PortTransport));
                portTransport.Port = 7777;
            }
            if (value.Contains(":"))
            {
                string port = value.Split(":")[1];
                string ip = value.Split(":")[0];
                netManager.networkAddress = ip;
                PortTransport portTransport = (PortTransport)netManager.GetComponent(typeof(PortTransport));
                ushort.TryParse(port, out var usPort);
                portTransport.Port = usPort;
            }
            else
            {
                netManager.networkAddress = value;
            }
        }

        public void Disconect()
        {
            if (NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopClient();

            }

            if (NetworkServer.active)
            {
                NetworkManager.singleton.StopServer();
            }
            SceneManager.LoadScene(0);
        }
    }
}
