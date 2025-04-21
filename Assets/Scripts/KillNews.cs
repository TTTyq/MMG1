using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class KillNews : MonoBehaviour
{
    private VisualElement infoBox;
    private Label infoLabel;
    private Button leftArrowButton;
    private Button rightArrowButton;
    private Button returnBackButton;

    // 信息框文本
    public List<string> infoTexts;
    private int currentIndex = 0;

    void Start()
    {
        // 获取 UIDocument 的根节点
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 获取按钮和信息框引用
        infoBox = root.Q<VisualElement>("News");
        infoLabel = root.Q<Label>("InfoLabel");
        leftArrowButton = root.Q<Button>("Left");
        rightArrowButton = root.Q<Button>("Right");
        returnBackButton = root.Q<Button>("Return");

        // 初始时隐藏信息框
        infoBox?.SetVisible(false);
        leftArrowButton?.SetVisible(false);
        rightArrowButton?.SetVisible(false);
        returnBackButton?.SetVisible(false);

        // 注册按钮事件
        leftArrowButton?.RegisterCallback<ClickEvent>(evt => OnLeftArrowClicked());
        rightArrowButton?.RegisterCallback<ClickEvent>(evt => OnRightArrowClicked());
        returnBackButton?.RegisterCallback<ClickEvent>(evt => OnReturnButtonClicked());

        // 设置 Label 的换行属性
        if (infoLabel != null)
        {
            infoLabel.style.whiteSpace = WhiteSpace.Normal;
        }

        // 显示初始信息（如果需要）
        ShowInfo(currentIndex);
    }

    private void ShowInfo(int index)
    {
        if (infoBox != null && infoLabel != null && infoTexts != null && infoTexts.Count > 0)
        {
            infoBox.SetVisible(true);
            infoLabel.text = infoTexts[index];

            // 设置按钮的可见性
            leftArrowButton?.SetVisible(index > 0); // 非第一句显示左箭头
            rightArrowButton?.SetVisible(index < infoTexts.Count - 1); // 非最后一句显示右箭头
            returnBackButton?.SetVisible(index == infoTexts.Count - 1); // 最后一句显示Return按钮
        }
    }

    private void OnLeftArrowClicked()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowInfo(currentIndex);
        }
    }

    private void OnRightArrowClicked()
    {
        if (currentIndex < infoTexts.Count - 1)
        {
            currentIndex++;
            ShowInfo(currentIndex);
        }
    }

    private void OnReturnButtonClicked()
    {
        Debug.Log("Returning to the main scene...");
        SceneManager.LoadScene("GameScene1"); // 替换为你的场景名称
    }
}

// 扩展方法，方便控制元素的可见性
public static class VisualElementExtensions
{
    public static void SetVisible(this VisualElement element, bool isVisible)
    {
        if (element != null)
        {
            element.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}