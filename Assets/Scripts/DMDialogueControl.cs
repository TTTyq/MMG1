using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class DMDialogueControl : MonoBehaviour
{
    [SerializeField]
    private List<string> dialogues; // 通过 Inspector 填写多段对话

    private Label dialogueLabel;    // 实际显示对话内容的 Label
    private Button nextButton;      // 下一段按钮
    private VisualElement dialogueBox; // 包含对话的最外层 VisualElement
    private int currentDialogueIndex = 0; // 当前对话段落索引
    private bool isTyping = false; // 是否正在逐字显示



    void Start()
    {
        // 获取 UI 根节点
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 获取最外层 VisualElement、对话内容 Label 和按钮
        dialogueBox = root.Q<VisualElement>("dmtext");
        dialogueLabel = root.Q<Label>("taici");
        nextButton = root.Q<Button>("dmnext");

        // 防错处理
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

        // 绑定按钮点击事件
        nextButton.clicked += ShowNextDialogue;

        // 显示第一段对话
        ShowNextDialogue();

    }


    void ShowNextDialogue()
    {
        if (isTyping) return; // 如果正在逐字显示，直接返回

        if (currentDialogueIndex < dialogues.Count)
        {
            // 获取当前对话内容
            string dialogue = dialogues[currentDialogueIndex];
            currentDialogueIndex++;

            // 启动逐字显示效果
            StartCoroutine(TypeText(dialogue));
        }

        else
        {
            // 清空对话框内容并隐藏按钮和对话框
            dialogueLabel.text = ""; // 清空文本
            nextButton.style.display = DisplayStyle.None; // 隐藏 Next 按钮
            dialogueLabel.style.display = DisplayStyle.None; // 隐藏对话框
        }
        // 如果是最后一句，隐藏 next 按钮
        //if (currentDialogueIndex == dialogues.Count)
        //{
            //dialogueLabel.text = ""; // 清空对话框内容
            //nextButton.style.display = DisplayStyle.None; // 隐藏 Next 按钮
            //dialogueBox.style.display = DisplayStyle.None; // 隐藏对话框
        //}
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueLabel.text = ""; // 清空对话框内容

        foreach (char c in text)
        {
            dialogueLabel.text += c; // 添加字符
            yield return new WaitForSeconds(0.02f); // 每个字符延迟 0.01 秒
        }

        isTyping = false;
    }
}
