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

    // ��Ϣ���ı�
    public List<string> infoTexts;
    private int currentIndex = 0;

    void Start()
    {
        // ��ȡ UIDocument �ĸ��ڵ�
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ��ȡ��ť����Ϣ������
        infoBox = root.Q<VisualElement>("News");
        infoLabel = root.Q<Label>("InfoLabel");
        leftArrowButton = root.Q<Button>("Left");
        rightArrowButton = root.Q<Button>("Right");
        returnBackButton = root.Q<Button>("Return");

        // ��ʼʱ������Ϣ��
        infoBox?.SetVisible(false);
        leftArrowButton?.SetVisible(false);
        rightArrowButton?.SetVisible(false);
        returnBackButton?.SetVisible(false);

        // ע�ᰴť�¼�
        leftArrowButton?.RegisterCallback<ClickEvent>(evt => OnLeftArrowClicked());
        rightArrowButton?.RegisterCallback<ClickEvent>(evt => OnRightArrowClicked());
        returnBackButton?.RegisterCallback<ClickEvent>(evt => OnReturnButtonClicked());

        // ���� Label �Ļ�������
        if (infoLabel != null)
        {
            infoLabel.style.whiteSpace = WhiteSpace.Normal;
        }

        // ��ʾ��ʼ��Ϣ�������Ҫ��
        ShowInfo(currentIndex);
    }

    private void ShowInfo(int index)
    {
        if (infoBox != null && infoLabel != null && infoTexts != null && infoTexts.Count > 0)
        {
            infoBox.SetVisible(true);
            infoLabel.text = infoTexts[index];

            // ���ð�ť�Ŀɼ���
            leftArrowButton?.SetVisible(index > 0); // �ǵ�һ����ʾ���ͷ
            rightArrowButton?.SetVisible(index < infoTexts.Count - 1); // �����һ����ʾ�Ҽ�ͷ
            returnBackButton?.SetVisible(index == infoTexts.Count - 1); // ���һ����ʾReturn��ť
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
        SceneManager.LoadScene("GameScene1"); // �滻Ϊ��ĳ�������
    }
}

// ��չ�������������Ԫ�صĿɼ���
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