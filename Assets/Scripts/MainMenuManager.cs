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

    // ��Ϣ���ı�
    public List<string> infoTexts;
    private int currentIndex = 0;


    void Start()
    {
        // ��ȡ UIDocument �ĸ��ڵ�
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ��ȡ��ť����Ϣ������
        startButton = root.Q<Button>("Start");
        aboutButton = root.Q<Button>("About");
        infoBox = root.Q<VisualElement>("Prologue");
        infoLabel = root.Q<Label>("InfoLabel");
        leftArrowButton = root.Q<Button>("Left");
        rightArrowButton = root.Q<Button>("Right");
        enterBuildingButton = root.Q<Button>("Enter");



        // ��ʼʱ������Ϣ��
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



        // ���� Label �Ļ�������
        if (infoLabel != null)
        {
            infoLabel.style.whiteSpace = WhiteSpace.Normal;
        }



        // ��Ӱ�ť����¼�
        if (startButton != null)
        {
            startButton.clicked += OnStartButtonClicked;
        }

        if (aboutButton != null)
        {
            aboutButton.clicked += OnAboutButtonClicked;
        }
        // ��������һ���ı�����ʾ���밴ť
        


    }
    private void HideButtons()
    {
        // ���� Start �� About ��ť
        if (startButton != null || aboutButton != null)
        {
            startButton.visible = false;
            aboutButton.visible = false;
        }
    }



    private void ShowInfo(int index)
    {
        // ��ʾ��Ϣ�����õ�ǰ�ı�
        if (infoBox != null && infoLabel != null && infoTexts != null && infoTexts.Count > 0)
        {
            infoBox.visible = true;
            infoLabel.text = infoTexts[index];

            // ��ʾ��ͷ��ť
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
        currentIndex = 0; // ��ʼ��Ϊ��һ��
        ShowInfo(currentIndex);
    }

    private void OnAboutButtonClicked()
    {
        Debug.Log("About clicked!");
        HideButtons();
        currentIndex = 0; // ��ʼ��Ϊ��һ��
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
        SceneManager.LoadScene("GameScene1"); // �滻Ϊ��ĳ�������
    }

}