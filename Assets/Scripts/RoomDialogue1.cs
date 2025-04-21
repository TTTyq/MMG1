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
        public List<string> dialogues; // 该房间的多段对话
    }

    [SerializeField]
    private List<RoomDialogue> roomDialogues; // 房间及其对应对话的列表

    //private ScrollView dialogueLabel;    // 实际显示对话内容的 Label
    private Button nextButton;      // 下一段按钮
    private VisualElement dialogueBox; // 包含对话的最外层 VisualElement
    private int currentDialogueIndex = 0; // 当前对话段落索引
    private List<string> currentRoomDialogues = null; // 当前房间的对话列表
    private Label dialogueLabel;    // 实际显示对话内容的 Label

    void Start()
    {
        // 获取 UI 根节点
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 获取最外层 VisualElement、对话内容 Label 和按钮
        dialogueBox = root.Q<VisualElement>("dmtext");
        dialogueLabel = root.Q<Label>("taici2");
        nextButton = root.Q<Button>("dmnext2");

        // 防错处理
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


        

        // 初始化对话框为隐藏状态
        //dialogueBox.style.display = DisplayStyle.None;

        // 动态绑定房间按钮
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

        // 绑定下一段按钮点击事件
        nextButton.clicked += ShowNextDialogue;

        nextButton.style.display = DisplayStyle.None;

        if (dialogueLabel != null)
        {
            dialogueLabel.style.whiteSpace = WhiteSpace.Normal; // 强制启用换行
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

            dialogueBox.style.display = DisplayStyle.Flex; // 显示对话框
            nextButton.visible = currentRoomDialogues.Count > 1; // 如果对话多于一句，显示 Next 按钮

            // 如果隐藏了 Next 按钮，将其强制重新显示
            if (currentRoomDialogues.Count > 1)
            {
                nextButton.style.display = DisplayStyle.Flex;
            }

            // 显示第一段对话
            string dialogue = currentRoomDialogues[currentDialogueIndex].Replace("\n", "\n");
            dialogueLabel.text = dialogue;
            dialogueLabel.MarkDirtyRepaint(); // 手动刷新控件

            Debug.Log($"Original dialogue: {currentRoomDialogues[currentDialogueIndex]}");
            Debug.Log($"Processed dialogue: {dialogue}");

            // 检查文本样式是否正确
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
            // 调试当前对话内容
            Debug.Log($"Next dialogue: {currentRoomDialogues[currentDialogueIndex]}");

            // 更新 Label 内容
            dialogueLabel.text = currentRoomDialogues[currentDialogueIndex].Replace("\n", "\n");

            currentDialogueIndex++;

            // 如果是最后一句，隐藏 Next 按钮
            if (currentDialogueIndex == currentRoomDialogues.Count)
            {
                nextButton.style.display = DisplayStyle.None; // 使用 DisplayStyle 控制隐藏
                Debug.Log("Next button hidden");
            }
        }
    }
}
