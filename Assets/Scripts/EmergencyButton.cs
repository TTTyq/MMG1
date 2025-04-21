using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class EmergencyButton : MonoBehaviour
{
    [SerializeField]
    private List<string> roomButtonNames; // 房间按钮的名称列表

    private Button emergencyButton;       // 紧急集合按钮
    private VisualElement emerInfoBox;    // 紧急集合信息框

    private HashSet<string> clickedRooms = new HashSet<string>(); // 记录已点击的房间按钮

    private static bool emergencyButtonShown = false; // 记录紧急集合按钮是否已显示过

    void Start()
    {
        // 获取 UIDocument 的根节点
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 获取紧急集合按钮和信息框
        emergencyButton = root.Q<Button>("EmergencyButton");
        emerInfoBox = root.Q<VisualElement>("Emergency");

        // 初始化：隐藏紧急集合按钮
        emergencyButton.style.display = DisplayStyle.None;

        // 如果紧急集合按钮已经显示过，确保它保持隐藏状态
        if (emergencyButtonShown)
        {
            Debug.Log("Emergency button already shown. Hiding it.");
            return;
        }

        // 动态绑定房间按钮
        foreach (var roomName in roomButtonNames)
        {
            Button roomButton = root.Q<Button>(roomName);
            if (roomButton != null)
            {
                roomButton.clicked += () => OnRoomButtonClicked(roomName);
            }
            else
            {
                Debug.LogWarning($"Room button '{roomName}' not found.");
            }
        }

        // 绑定紧急集合按钮点击事件
        emergencyButton.clicked += OnEmergencyButtonClicked;
    }

    private void OnRoomButtonClicked(string roomName)
    {
        if (!clickedRooms.Contains(roomName))
        {
            clickedRooms.Add(roomName);
            Debug.Log($"Room '{roomName}' clicked. Total clicked: {clickedRooms.Count}/{roomButtonNames.Count}");

            // 检查是否所有房间按钮都已点击
            if (clickedRooms.Count == roomButtonNames.Count && !emergencyButtonShown)
            {
                ShowEmergencyButton();
            }
        }
    }

    private void ShowEmergencyButton()
    {
        Debug.Log("All rooms clicked. Showing Emergency Button.");
        emergencyButton.style.display = DisplayStyle.Flex;
        emergencyButtonShown = true; // 标记按钮已显示
    }

    private void OnEmergencyButtonClicked()
    {
        Debug.Log("Emergency button clicked.");
        emergencyButton.style.display = DisplayStyle.None;
    }
}
