using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class DMDialogueControl : MonoBehaviour
{
    [SerializeField]
    private List<string> dialogues; // ͨ�� Inspector ��д��ζԻ�

    private Label dialogueLabel;    // ʵ����ʾ�Ի����ݵ� Label
    private Button nextButton;      // ��һ�ΰ�ť
    private VisualElement dialogueBox; // �����Ի�������� VisualElement
    private int currentDialogueIndex = 0; // ��ǰ�Ի���������
    private bool isTyping = false; // �Ƿ�����������ʾ



    void Start()
    {
        // ��ȡ UI ���ڵ�
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ��ȡ����� VisualElement���Ի����� Label �Ͱ�ť
        dialogueBox = root.Q<VisualElement>("dmtext");
        dialogueLabel = root.Q<Label>("taici");
        nextButton = root.Q<Button>("dmnext");

        // ������
        if (dialogueBox == null)
        {
            Debug.LogError("VisualElement 'dmtext' not found in the UI Document.");
            return;
        }
        if (dialogueLabel == null)
        {
            Debug.LogError("Label 'taici' not found in the UI Document.");
            return;
        }
        if (nextButton == null)
        {
            Debug.LogError("Button 'dmnext' not found in the UI Document.");
            return;
        }

        // �󶨰�ť����¼�
        nextButton.clicked += ShowNextDialogue;

        // ��ʾ��һ�ζԻ�
        ShowNextDialogue();

    }


    void ShowNextDialogue()
    {
        if (isTyping) return; // �������������ʾ��ֱ�ӷ���

        if (currentDialogueIndex < dialogues.Count)
        {
            // ��ȡ��ǰ�Ի�����
            string dialogue = dialogues[currentDialogueIndex];
            currentDialogueIndex++;

            // ����������ʾЧ��
            StartCoroutine(TypeText(dialogue));
        }

        else
        {
            // ��նԻ������ݲ����ذ�ť�ͶԻ���
            dialogueLabel.text = ""; // ����ı�
            nextButton.style.display = DisplayStyle.None; // ���� Next ��ť
            dialogueLabel.style.display = DisplayStyle.None; // ���ضԻ���
        }
        // ��������һ�䣬���� next ��ť
        //if (currentDialogueIndex == dialogues.Count)
        //{
            //dialogueLabel.text = ""; // ��նԻ�������
            //nextButton.style.display = DisplayStyle.None; // ���� Next ��ť
            //dialogueBox.style.display = DisplayStyle.None; // ���ضԻ���
        //}
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueLabel.text = ""; // ��նԻ�������

        foreach (char c in text)
        {
            dialogueLabel.text += c; // ����ַ�
            yield return new WaitForSeconds(0.02f); // ÿ���ַ��ӳ� 0.01 ��
        }

        isTyping = false;
    }
}
