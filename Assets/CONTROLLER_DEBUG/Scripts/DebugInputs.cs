using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DebugInputs : MonoBehaviour
{
    public DebugInputSO debugInput;
    public GameObject debugPanel;
    // Start is called before the first frame update
    void Awake()
    {
        debugPanel.SetActive(false);

        for(int i = 0; i < debugInput.map.actions.Count; i++)
        {
            debugInput.map.actions[i].started += InputReceived;
            debugInput.map.actions[i].performed += InputReceived;
            debugInput.map.actions[i].canceled += InputReceived;
            if(debugInput.map.actions[i].name == "Toggle Debug")
            {
                debugInput.map.actions[i].Enable();
            }
        }

    }

    public void InputReceived(InputAction.CallbackContext context)
    {
        if(context.action.name == "Toggle Debug" && context.started){
            SwitchDebugVisibility();
        }
        else
        {
            for(int i = 0; i < debugPanel.transform.childCount; i++)
            {
                GameObject child = debugPanel.transform.GetChild(i).gameObject;
                if(child.name == context.action.name)
                {
                    if(context.started)
                    {
                    child.GetComponent<Image>().color = Color.red;
                    }
                    else if(context.canceled)
                    {
                        child.GetComponent<Image>().color = Color.white;
                    }

                }
                else if(child.name == context.action.name + "XText")
                {
                    child.GetComponent<TextMeshProUGUI>().text = $"{context.ReadValue<Vector2>().x}";
                }
                else if(child.name == context.action.name + "YText")
                {
                    child.GetComponent<TextMeshProUGUI>().text = $"{context.ReadValue<Vector2>().y}";
                }
                else if(child.name == context.action.name + "Text")
                {
                    child.GetComponent<TextMeshProUGUI>().text = $"{context.ReadValue<float>()}";
                }
            }
        }
    }

    private void SwitchDebugVisibility()
    {
        bool currentValue = !debugPanel.activeSelf;
        if(currentValue)
        {
            for(int i = 0; i < debugInput.map.actions.Count; i++)
        {
            if(debugInput.map.actions[i].name != "Toggle Debug")
            {
                debugInput.map.actions[i].Enable();
            }
        }

        }
        else
        {
            for(int i = 0; i < debugInput.map.actions.Count; i++)
        {
            if(debugInput.map.actions[i].name != "Toggle Debug")
            {
                debugInput.map.actions[i].Disable();
            }
        }

        }
        debugPanel.SetActive(currentValue);
    }


}
