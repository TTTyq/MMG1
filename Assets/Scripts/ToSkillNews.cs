using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ToSkillNews : MonoBehaviour
{
    private Button nextButton;     // Next ��ť
    private Button warningButton;  // Warning ��ť

    private static bool hasWarningButtonShown = false; // ��̬������ȷ��һ����Ϸֻ��ʾһ��

    void Start()
    {
        // ��ȡ UIDocument �ĸ��ڵ�
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ��ȡ Next ��ť�� Warning ��ť
        nextButton = root.Q<Button>("dmnext");
        warningButton = root.Q<Button>("Warning");

        // ��ʼ�� Warning ��ť
        if (warningButton != null)
        {
            warningButton.style.display = DisplayStyle.None; // ��ʼ����
            warningButton.clicked += OnWarningButtonClicked; // ��ӵ���¼�
        }
    }

    void Update()
    {
        // ��� Next ��ť�Ƿ��Ѿ�����
        if (nextButton != null && nextButton.style.display == DisplayStyle.None && !hasWarningButtonShown)
        {
            ShowWarningButton(); // ��ʾ Warning ��ť
        }
    }

    private void ShowWarningButton()
    {
        if (warningButton != null)
        {
            warningButton.style.display = DisplayStyle.Flex; // ��ʾ Warning ��ť
            hasWarningButtonShown = true; // ���Ϊ�Ѿ���ʾ
        }
    }

    private void OnWarningButtonClicked()
    {
        Debug.Log("Warning button clicked. Loading KillNews scene...");
        SceneManager.LoadScene("KillNews"); // ������һ������
    }
}
