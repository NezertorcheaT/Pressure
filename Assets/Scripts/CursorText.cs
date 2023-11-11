using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CursorText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image cursor;

    public void SetCursorText(TextMeshProUGUI text)
    {
        this.text = text == null ? this.text : text;
    }

    public void SetText(string s)
    {
        if (s.Replace(" ", string.Empty) != string.Empty)
        {
            text.text = s;
            cursor.enabled = false;
        }
        else
            ClearText();
    }

    public void ClearText()
    {
        cursor.enabled = true;
        if (text.text.Replace(" ", string.Empty) != string.Empty)
            text.text = string.Empty;
    }
}