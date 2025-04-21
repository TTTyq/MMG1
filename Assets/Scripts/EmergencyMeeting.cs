using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class EmergencyMeeting : MonoBehaviour
{
    [SerializeField]
    private List<string> dialogues; // 在 Inspector 中加载的多段对话

    private Button emergencyButton;   // Emergency 按钮
    private VisualElement emerInfoBox;    // 信息框
    private Label dialogueLabel;      // 显示对话内容的 Label
    private Button leftArrow;         // 左箭头按钮
    private Button rightArrow;        // 右箭头按钮

    private int currentDialogueIndex = 0; // 当前对话索引

    void Start()
    {
        // 获取 UIDocument 的根节点
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 获取 UI 元素
        emergencyButton = root.Q<Button>("EmergencyButton");
        emerInfoBox = root.Q<VisualElement>("Emergency");
        dialogueLabel = root.Q<Label>("EmergencyLabel");
        leftArrow = root.Q<Button>("Left");
        rightArrow = root.Q<Button>("Right");

        // 防错处理
        if (emergencyButton == null)
        {
            Debug.LogError("Button 'Emergency' not found in the UI Document.");
            return;
        }
        if (emerInfoBox == null)
        {
            Debug.LogError("VisualElement 'InfoBox' not found in the UI Document.");
            return;
        }
        if (dialogueLabel == null)
        {
            Debug.LogError("Label 'DialogueLabel' not found in the UI Document.");
            return;
        }
        if (leftArrow == null || rightArrow == null)
        {
            Debug.LogError("Arrow buttons not found in the UI Document.");
            return;
        }

        dialogueLabel.style.whiteSpace = WhiteSpace.Normal; // 启用换行

        // 初始化：隐藏信息框和箭头
        emerInfoBox.style.display = DisplayStyle.None;
        leftArrow.style.display = DisplayStyle.None;
        rightArrow.style.display = DisplayStyle.None;

        // 绑定按钮点击事件
        emergencyButton.clicked += OnEmergencyButtonClicked;
        leftArrow.clicked += OnLeftArrowClicked;
        rightArrow.clicked += OnRightArrowClicked;
    }

    private void OnEmergencyButtonClicked()
    {
        // 显示信息框和箭头（根据对话状态）
        emerInfoBox.style.display = DisplayStyle.Flex;
        UpdateDialogue();
    }

    private void OnLeftArrowClicked()
    {
        if (currentDialogueIndex > 0)
        {
            currentDialogueIndex--;
            UpdateDialogue();
        }
    }

    private void OnRightArrowClicked()
    {
        if (currentDialogueIndex < dialogues.Count - 1)
        {
            currentDialogueIndex++;
            UpdateDialogue();
        }
    }

    private void UpdateDialogue()
    {
        // 更新对话内容
        if (dialogues != null && dialogues.Count > 0)
        {
            dialogueLabel.text = dialogues[currentDialogueIndex];
        }

        // 更新箭头按钮显示状态
        leftArrow.style.display = currentDialogueIndex > 0 ? DisplayStyle.Flex : DisplayStyle.None;
        rightArrow.style.display = currentDialogueIndex < dialogues.Count - 1 ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
