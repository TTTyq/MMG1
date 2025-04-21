using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class EmergencyButton : MonoBehaviour
{
    [SerializeField]
    private List<string> roomButtonNames; // ���䰴ť�������б�

    private Button emergencyButton;       // �������ϰ�ť
    private VisualElement emerInfoBox;    // ����������Ϣ��

    private HashSet<string> clickedRooms = new HashSet<string>(); // ��¼�ѵ���ķ��䰴ť

    private static bool emergencyButtonShown = false; // ��¼�������ϰ�ť�Ƿ�����ʾ��

    void Start()
    {
        // ��ȡ UIDocument �ĸ��ڵ�
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ��ȡ�������ϰ�ť����Ϣ��
        emergencyButton = root.Q<Button>("EmergencyButton");
        emerInfoBox = root.Q<VisualElement>("Emergency");

        // ��ʼ�������ؽ������ϰ�ť
        emergencyButton.style.display = DisplayStyle.None;

        // ����������ϰ�ť�Ѿ���ʾ����ȷ������������״̬
        if (emergencyButtonShown)
        {
            Debug.Log("Emergency button already shown. Hiding it.");
            return;
        }

        // ��̬�󶨷��䰴ť
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

        // �󶨽������ϰ�ť����¼�
        emergencyButton.clicked += OnEmergencyButtonClicked;
    }

    private void OnRoomButtonClicked(string roomName)
    {
        if (!clickedRooms.Contains(roomName))
        {
            clickedRooms.Add(roomName);
            Debug.Log($"Room '{roomName}' clicked. Total clicked: {clickedRooms.Count}/{roomButtonNames.Count}");

            // ����Ƿ����з��䰴ť���ѵ��
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
        emergencyButtonShown = true; // ��ǰ�ť����ʾ
    }

    private void OnEmergencyButtonClicked()
    {
        Debug.Log("Emergency button clicked.");
        emergencyButton.style.display = DisplayStyle.None;
    }
}
