using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimatedText : MonoBehaviour
{
    
    public float letterPaused = 0.01f;
    
    public string message;
    
    public Text textComp;

    public float timeToCleanMessage = 0.0f;

    
    void Start()
    {
        Time.timeScale = 1;
        textComp = GetComponent<Text>();
        
        message = textComp.text;
        
        textComp.text = "";

        StartCoroutine(TypeText());
        StartCoroutine(CleanMessage());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            textComp.text += letter;
            yield return 0;
            yield return new WaitForSeconds(letterPaused);
        }
    }

    IEnumerator CleanMessage()
    {
        yield return new WaitForSeconds(timeToCleanMessage);
        textComp.text = "";
    }
}