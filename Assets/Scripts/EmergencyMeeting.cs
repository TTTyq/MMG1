using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class EmergencyMeeting : MonoBehaviour
{
    [SerializeField]
    private List<string> dialogues; // �� Inspector �м��صĶ�ζԻ�

    private Button emergencyButton;   // Emergency ��ť
    private VisualElement emerInfoBox;    // ��Ϣ��
    private Label dialogueLabel;      // ��ʾ�Ի����ݵ� Label
    private Button leftArrow;         // ���ͷ��ť
    private Button rightArrow;        // �Ҽ�ͷ��ť

    private int currentDialogueIndex = 0; // ��ǰ�Ի�����

    void Start()
    {
        // ��ȡ UIDocument �ĸ��ڵ�
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ��ȡ UI Ԫ��
        emergencyButton = root.Q<Button>("EmergencyButton");
        emerInfoBox = root.Q<VisualElement>("Emergency");
        dialogueLabel = root.Q<Label>("EmergencyLabel");
        leftArrow = root.Q<Button>("Left");
        rightArrow = root.Q<Button>("Right");

        // ������
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

        dialogueLabel.style.whiteSpace = WhiteSpace.Normal; // ���û���

        // ��ʼ����������Ϣ��ͼ�ͷ
        emerInfoBox.style.display = DisplayStyle.None;
        leftArrow.style.display = DisplayStyle.None;
        rightArrow.style.display = DisplayStyle.None;

        // �󶨰�ť����¼�
        emergencyButton.clicked += OnEmergencyButtonClicked;
        leftArrow.clicked += OnLeftArrowClicked;
        rightArrow.clicked += OnRightArrowClicked;
    }

    private void OnEmergencyButtonClicked()
    {
        // ��ʾ��Ϣ��ͼ�ͷ�����ݶԻ�״̬��
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
        // ���¶Ի�����
        if (dialogues != null && dialogues.Count > 0)
        {
            dialogueLabel.text = dialogues[currentDialogueIndex];
        }

        // ���¼�ͷ��ť��ʾ״̬
        leftArrow.style.display = currentDialogueIndex > 0 ? DisplayStyle.Flex : DisplayStyle.None;
        rightArrow.style.display = currentDialogueIndex < dialogues.Count - 1 ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
