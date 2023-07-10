using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CursorText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image cursor;

    public void SetText(string text)
    {
        if (text != "")
        {
            this.text.text = text;
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
