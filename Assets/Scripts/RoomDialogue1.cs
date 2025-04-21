using UnityEngine.UIElements;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomDialogue1 : MonoBehaviour
{
    [System.Serializable]
    public class RoomDialogue
    {
        public string roomName;
        public List<string> dialogues; // �÷���Ķ�ζԻ�
    }

    [SerializeField]
    private List<RoomDialogue> roomDialogues; // ���估���Ӧ�Ի����б�

    //private ScrollView dialogueLabel;    // ʵ����ʾ�Ի����ݵ� Label
    private Button nextButton;      // ��һ�ΰ�ť
    private VisualElement dialogueBox; // �����Ի�������� VisualElement
    private int currentDialogueIndex = 0; // ��ǰ�Ի���������
    private List<string> currentRoomDialogues = null; // ��ǰ����ĶԻ��б�
    private Label dialogueLabel;    // ʵ����ʾ�Ի����ݵ� Label

    void Start()
    {
        // ��ȡ UI ���ڵ�
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ��ȡ����� VisualElement���Ի����� Label �Ͱ�ť
        dialogueBox = root.Q<VisualElement>("dmtext");
        dialogueLabel = root.Q<Label>("taici2");
        nextButton = root.Q<Button>("dmnext2");

        // ������
        if (dialogueBox == null)
        {
            Debug.LogError("VisualElement 'dmtext' not found in the UI Document.");
            return;
        }
        if (dialogueLabel == null)
        {
            Debug.LogError("Label 'taici2' not found in the UI Document.");
            return;
        }
        if (nextButton == null)
        {
            Debug.LogError("Button 'dmnext2' not found in the UI Document.");
            return;
        }


        

        // ��ʼ���Ի���Ϊ����״̬
        //dialogueBox.style.display = DisplayStyle.None;

        // ��̬�󶨷��䰴ť
        foreach (var room in roomDialogues)
        {
            Button roomButton = root.Q<Button>(room.roomName);
            if (roomButton != null)
            {
                roomButton.clicked += () => ShowRoomDialogue(room.roomName);
            }
            else
            {
                Debug.LogWarning($"Button for room '{room.roomName}' not found.");
            }
        }

        // ����һ�ΰ�ť����¼�
        nextButton.clicked += ShowNextDialogue;

        nextButton.style.display = DisplayStyle.None;

        if (dialogueLabel != null)
        {
            dialogueLabel.style.whiteSpace = WhiteSpace.Normal; // ǿ�����û���
            Debug.Log($"WhiteSpace style after set: {dialogueLabel.style.whiteSpace}");
        }

    }

    public void ShowRoomDialogue(string roomName)
    {
        RoomDialogue room = roomDialogues.Find(r => r.roomName == roomName);
        if (room != null)
        {
            currentRoomDialogues = room.dialogues;
            currentDialogueIndex = 0;

            dialogueBox.style.display = DisplayStyle.Flex; // ��ʾ�Ի���
            nextButton.visible = currentRoomDialogues.Count > 1; // ����Ի�����һ�䣬��ʾ Next ��ť

            // ��������� Next ��ť������ǿ��������ʾ
            if (currentRoomDialogues.Count > 1)
            {
                nextButton.style.display = DisplayStyle.Flex;
            }

            // ��ʾ��һ�ζԻ�
            string dialogue = currentRoomDialogues[currentDialogueIndex].Replace("\n", "\n");
            dialogueLabel.text = dialogue;
            dialogueLabel.MarkDirtyRepaint(); // �ֶ�ˢ�¿ؼ�

            Debug.Log($"Original dialogue: {currentRoomDialogues[currentDialogueIndex]}");
            Debug.Log($"Processed dialogue: {dialogue}");

            // ����ı���ʽ�Ƿ���ȷ
            Debug.Log($"WhiteSpace style: {dialogueLabel.style.whiteSpace}");
            dialogueLabel.style.whiteSpace = WhiteSpace.Normal;

            currentDialogueIndex++;
        }
        else
        {
            Debug.LogError($"No dialogues found for room: {roomName}");
        }
    }


    void ShowNextDialogue()
    {
        if (currentRoomDialogues != null && currentDialogueIndex < currentRoomDialogues.Count)
        {
            // ���Ե�ǰ�Ի�����
            Debug.Log($"Next dialogue: {currentRoomDialogues[currentDialogueIndex]}");

            // ���� Label ����
            dialogueLabel.text = currentRoomDialogues[currentDialogueIndex].Replace("\n", "\n");

            currentDialogueIndex++;

            // ��������һ�䣬���� Next ��ť
            if (currentDialogueIndex == currentRoomDialogues.Count)
            {
                nextButton.style.display = DisplayStyle.None; // ʹ�� DisplayStyle ��������
                Debug.Log("Next button hidden");
            }
        }
    }
}
