using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private void Awake()
    {
        // 싱글톤
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    public DialogueSequence lines;
    public DialogueSequence billboardLines;
    private GameObject dialogueObj;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if(dialogueObj == null)
            {
                dialogueObj = UIManager.instance.ShowPopupUI("UI_Dialogue", lines).gameObject;
            }

        }
    }

    public void ShowBillBoardDialogue(string name, Transform root)
    {
        UIManager.instance.ShowPopupUI(name, billboardLines, root);
    }
}
