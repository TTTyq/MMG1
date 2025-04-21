using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class MainMenuManager : MonoBehaviour
{
    private Button startButton;
    private Button aboutButton;
    private VisualElement infoBox;
    private Label infoLabel;
    private Button leftArrowButton;
    private Button rightArrowButton;
    private Button enterBuildingButton;

    // 信息框文本
    public List<string> infoTexts;
    private int currentIndex = 0;


    void Start()
    {
        // 获取 UIDocument 的根节点
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 获取按钮和信息框引用
        startButton = root.Q<Button>("Start");
        aboutButton = root.Q<Button>("About");
        infoBox = root.Q<VisualElement>("Prologue");
        infoLabel = root.Q<Label>("InfoLabel");
        leftArrowButton = root.Q<Button>("Left");
        rightArrowButton = root.Q<Button>("Right");
        enterBuildingButton = root.Q<Button>("Enter");



        // 初始时隐藏信息框
        if (infoBox != null)
        {
            infoBox.visible = false;
        }

        if (leftArrowButton != null)
        {
            leftArrowButton.visible = false;
            leftArrowButton.clicked += OnLeftArrowClicked;
        }

        if (rightArrowButton != null)
        {
            rightArrowButton.visible = false;
            rightArrowButton.clicked += OnRightArrowClicked;
        }

        if (enterBuildingButton != null)
        {
            enterBuildingButton.visible = false;
            enterBuildingButton.clicked += OnEnterBuildingClicked;
        }



        // 设置 Label 的换行属性
        if (infoLabel != null)
        {
            infoLabel.style.whiteSpace = WhiteSpace.Normal;
        }



        // 添加按钮点击事件
        if (startButton != null)
        {
            startButton.clicked += OnStartButtonClicked;
        }

        if (aboutButton != null)
        {
            aboutButton.clicked += OnAboutButtonClicked;
        }
        // 如果是最后一段文本，显示进入按钮
        


    }
    private void HideButtons()
    {
        // 隐藏 Start 和 About 按钮
        if (startButton != null || aboutButton != null)
        {
            startButton.visible = false;
            aboutButton.visible = false;
        }
    }



    private void ShowInfo(int index)
    {
        // 显示信息框并设置当前文本
        if (infoBox != null && infoLabel != null && infoTexts != null && infoTexts.Count > 0)
        {
            infoBox.visible = true;
            infoLabel.text = infoTexts[index];

            // 显示箭头按钮
            if (leftArrowButton != null)
            {
                leftArrowButton.visible = index > 0;
            }

            if (rightArrowButton != null)
            {
                rightArrowButton.visible = index < infoTexts.Count - 1;
            }
            if (enterBuildingButton != null)
            {
                enterBuildingButton.visible = index == infoTexts.Count - 1;
            }

        }
    }


    private void OnStartButtonClicked()
    {
        Debug.Log("Start clicked!");
        HideButtons();
        currentIndex = 0; // 初始化为第一句
        ShowInfo(currentIndex);
    }

    private void OnAboutButtonClicked()
    {
        Debug.Log("About clicked!");
        HideButtons();
        currentIndex = 0; // 初始化为第一句
        ShowInfo(currentIndex);
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
    private void OnEnterBuildingClicked()
    {
        Debug.Log("Entering the building...");
        SceneManager.LoadScene("GameScene1"); // 替换为你的场景名称
    }

}