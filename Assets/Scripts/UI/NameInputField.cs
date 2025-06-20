using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class NameInputField : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI savedName;

    void Awake()
    {
        GetComponent<TMP_InputField>().onValueChanged.AddListener(
            (string text) => {
                string _text = System.Text.RegularExpressions.Regex.Replace(text, @"[^\p{L}\p{Nd}]", "");
                if (_text.Length > 8) _text = _text[..8];
                savedName.text = _text.Length > 0 ? _text : "NO NAME";
            }
        );
    }
}
