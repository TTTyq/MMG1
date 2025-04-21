using UnityEngine;
using UnityEngine.TextCore.Text;

public class FontPreloader : MonoBehaviour
{
    public FontAsset fontAsset;

    void Start()
    {
        string charactersToAdd = "��ҪԤ���ص��ַ�"; // �滻Ϊ��Ҫ��ʾ���ַ�
        if (fontAsset.TryAddCharacters(charactersToAdd, out string missingCharacters))
        {
            Debug.Log($"Successfully added characters: {charactersToAdd}");
        }
        else
        {
            Debug.LogError($"Missing characters: {missingCharacters}");
        }
    }
}