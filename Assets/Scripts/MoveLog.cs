using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveLog : MonoBehaviour
{
    [SerializeField]
    List<Message> messages = new List<Message>();
    List<GameObject> texts = new List<GameObject>();

    public GameObject chatPanel, textObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendMessageToLog(string text)
    {
        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textOjbect = newText.GetComponent<Text>();

        newMessage.textOjbect.text = newMessage.text;

        texts.Add(newText);

        messages.Add(newMessage);
    }

    public void ClearLog()
    {
        foreach (GameObject obj in texts)
            Destroy(obj);
        messages.Clear();
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textOjbect;
}
