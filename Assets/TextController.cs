using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.UIElements;

public class TextController : MonoBehaviour
{
    public int charactersPerSecond;


    private int currentCharIndex = 0;
    private string targetText = "";
    private float timer;
    private TextMeshProUGUI text;

    public void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }


    public void ShowDialog(string textToView)
    {
        gameObject.transform.localScale = Vector3.one;
        targetText = textToView;
        currentCharIndex = 0;
        text.text = "";
    }

    public void HideDialog(){
        gameObject.transform.localScale = Vector3.zero;
        targetText = "";
        currentCharIndex = 0;
        text.text = "";
    }

    public void Update()
    {
        if (currentCharIndex < targetText.Length)
        {
            timer += Time.deltaTime;
            if (timer >= 1f / charactersPerSecond)
            {
                text.text += targetText[currentCharIndex];
                currentCharIndex++;
                timer = 0;
            }
        }
    }
}
