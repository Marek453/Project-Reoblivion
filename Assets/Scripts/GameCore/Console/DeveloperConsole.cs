using GameCore.Player;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameCore.Console
{
    public class DeveloperConsole : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private GameObject consoleCanvas; 
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI logText; 
        [SerializeField] private ScrollRect scrollRect; 

        private Dictionary<string, System.Action<string[]>> commands = new Dictionary<string, System.Action<string[]>>();

        public static DeveloperConsole singleton;

        private List<string> commandHistory = new List<string>();
        private int historyIndex = 0; 

        private bool isConsoleOpen = false;

        private PlayerManager lockalPlayer;

        private void Awake()
        {
            singleton = this;
            RegisterCommand("help", Help);
            RegisterCommand("quit", (args) => Application.Quit());
            RegisterCommand("clear", (args) => logText.text = "");
            RegisterCommand("map", LoadMap); 

            Application.logMessageReceived += HandleLog;

            consoleCanvas.SetActive(false);
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
        }

        public void OnNavigate(InputValue inputValue)
        {
            if (inputValue.Get<Vector2>().y != 0)
            {
                NavigateHistory((int)-inputValue.Get<Vector2>().y);
            }
        }

        private void OnToggleConsole()
        {
            isConsoleOpen = !isConsoleOpen;
            consoleCanvas.SetActive(isConsoleOpen);

            if(SceneManager.GetActiveScene().name.Contains("Facility") && lockalPlayer == null)
            {
                lockalPlayer = PlayerManager.players.Find(pl => pl.isLocalPlayer);
            }
            if (lockalPlayer != null)
            {
                lockalPlayer.cursorManager.isConsole = isConsoleOpen;
            }

            if (isConsoleOpen)
            {
                inputField.ActivateInputField(); 
            }
        }

        public void ProcessCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return;

            commandHistory.Add(input);
            historyIndex = commandHistory.Count; 

            LogToConsole($"> {input}");

            string[] parts = input.Split(' ');
            string commandName = parts[0].ToLower();
            string[] args = parts.Skip(1).ToArray();

            if (commands.ContainsKey(commandName))
            {
                commands[commandName].Invoke(args);
            }
            else
            {
                LogToConsole($"<color=red>Unknown command: {commandName}</color>");
            }

            inputField.text = ""; 
            inputField.ActivateInputField();
        }

        private void NavigateHistory(int direction)
        {
            if (commandHistory.Count == 0) return;
            historyIndex += direction;
            if (historyIndex < 0) historyIndex = 0;

            if (historyIndex > commandHistory.Count) historyIndex = commandHistory.Count;

            if (historyIndex == commandHistory.Count)
            {
                inputField.text = ""; 
            }
            else
            {
                inputField.text = commandHistory[historyIndex];
                inputField.caretPosition = inputField.text.Length;
                inputField.ForceLabelUpdate();
            }
        }

        public void RegisterCommand(string name, System.Action<string[]> action)
        {
            if (!commands.ContainsKey(name))
            {
                commands.Add(name, action);
            }
        }

        private void LogToConsole(string message)
        {
            logText.text += message + "\n";

            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            string color = "white";
            if (type == LogType.Warning) color = "yellow";
            else if (type == LogType.Error || type == LogType.Exception) color = "red";

            LogToConsole($"<color={color}>[{type}] {logString}</color>");
        }

        private void Help(string[] args)
        {
            LogToConsole("Available commands:");
            foreach (var cmd in commands.Keys)
            {
                LogToConsole($"- {cmd}");
            }
        }

        private void LoadMap(string[] args)
        {
            if (args.Length > 0)
                LogToConsole($"Loading map: {args[0]}...");
            else
                LogToConsole("<color=red>Usage: map <map_name></color>");
        }
    }
}
