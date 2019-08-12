using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerScript : MonoBehaviour
{
    public void LoadGameScene()
	{
        InputManager_Riki.Instance.ButtonLeftPressedEvent -= Instance_ButtonLeftPressedEvent;
        InputManager_Riki.Instance.ButtonRightPressedEvent -= Instance_ButtonRightPressedEvent;
        InputManager_Riki.Instance.ButtonAPressedEvent -= Instance_ButtonAPressedEvent;
        InputManager_Riki.Instance.ButtonLPressedEvent -= Instance_ButtonLPressedEvent;
        InputManager_Riki.Instance.ButtonRPressedEvent -= Instance_ButtonRPressedEvent;
        SceneManager.LoadScene(1);
	}

    private void Start()
    {
        InputManager_Riki.Instance.ButtonLeftPressedEvent += Instance_ButtonLeftPressedEvent; 
        InputManager_Riki.Instance.ButtonRightPressedEvent += Instance_ButtonRightPressedEvent; 
        InputManager_Riki.Instance.ButtonAPressedEvent += Instance_ButtonAPressedEvent;
        InputManager_Riki.Instance.ButtonLPressedEvent += Instance_ButtonLPressedEvent;
        InputManager_Riki.Instance.ButtonRPressedEvent += Instance_ButtonRPressedEvent; 
    }

    private void Instance_ButtonRPressedEvent()
    {
    }

    private void Instance_ButtonLPressedEvent()
    {
    }

    private void Instance_ButtonRightPressedEvent()
    {
    }

    private void Instance_ButtonLeftPressedEvent()
    {
    }

    private void Instance_ButtonAPressedEvent()
    {
        LoadGameScene();
    }
}
