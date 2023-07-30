using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CursorText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image cursor;

    public void SetText(string s)
    {
        if (s != "")
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
        text.text = "";
    }
}