using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputRebinding : MonoBehaviour
{
    private InputAction inputAction;
    public InputActionReference actionReference;
    public int actionValueReference;

    /// <summary>
    /// Enable this to show what bindings are listed under each action reference. These will print out in the debugger.
    /// </summary>
    [SerializeField] bool showBindings;

    //Ui Related 
    //[SerializeField] private TextMeshProUGUI actionType; 
    [SerializeField] private TextMeshProUGUI actionBinding;
    [SerializeField] private TextMeshProUGUI rebindText;
    [SerializeField] private GameObject rebindOverlay;


    public InputBinding.DisplayStringOptions displayStringOptions;

    private InteractiveRebindEvent RebindStopEvent;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    private void OnValidate()
    {
        RefreshBindings();
        UpdateUi();
    }

    /// <summary>
    /// Refresh the Bindings, changing both the UI and button functionality. 
    /// </summary>
    private void RefreshBindings()
    {        
        inputAction = actionReference.action;
        var bindingCount = actionReference.action.bindings.Count;
        if (actionValueReference > bindingCount)
        {
            actionValueReference = 1; //If for some reason our value is HIGHER than the amount of bindings, we just rest it to 1 (the first input). 
        }
        var binding = actionReference.action.bindings;
        if(binding[actionValueReference].isPartOfComposite)
        {
            //var bindingToChang213123e = binding[actionValueReference].comp;
        }
        var bindingToChange = binding[actionValueReference];
        Debug.Log("The binding we want to change is: " + inputAction.GetBindingDisplayString(actionValueReference));

        if (showBindings)
        {
            for (var i = 0; i < bindingCount; ++i)
            {
                var displaytext = inputAction.GetBindingDisplayString(i);
                /*if (binding[i].isPartOfComposite)
                {
                    Debug.Log(binding[i]+" was part of composite");
                    return;
                }*/
                Debug.Log(" The binding | " + displaytext + " | has an ID of: " + i + ", use this in the editor to change the Ui interaction!");
            }
        }

    }

    void UpdateUi()
    {
        var changeNameTo = inputAction.GetBindingDisplayString(actionValueReference);
        actionBinding.text = changeNameTo;
    }

    public void StartInteractiveReibnd()
    {        
        if (inputAction.bindings[actionValueReference].isComposite)
        {
            //Debug.Log("OH CRAP ITS A COMPOSITE!");
            var firstPartIndex = actionValueReference + 1;
            if (firstPartIndex < inputAction.bindings.Count && inputAction.bindings[firstPartIndex].isPartOfComposite)
            {
                PerformInteractiveRebind(inputAction, firstPartIndex, allCompositeParts: true);
            }            
        }
        else PerformInteractiveRebind(inputAction, actionValueReference);
    }

    private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
    {
        rebindOperation?.Cancel();

        void CleanUp()
        {
            Debug.Log("We desposed of the action");
            rebindOperation?.Dispose();
            rebindOperation = null;
        }
        
        // Configure the rebind.
        rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                .Start()
                //.WithCancelingThrough("<Keyboard>/tab")
                .OnCancel(
                operation =>
                {
                    RebindStopEvent?.Invoke(this, operation);
                    rebindOverlay?.SetActive(false);
                    //UpdateBindingDisplay();
                    CleanUp();
                })
                .OnComplete(
                operation =>
                {
                    Debug.Log("Changed binding to " + operation.selectedControl);
                    rebindOverlay?.SetActive(false);
                    RebindStopEvent?.Invoke(this, operation);
                    //UpdateBindingDisplay();
                    CleanUp();

                    // If there's more composite parts we should bind, initiate a rebind
                    // for the next part.
                    if (allCompositeParts)
                    {
                        var nextBindingIndex = bindingIndex + 1;
                        if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                            PerformInteractiveRebind(action, nextBindingIndex, true);
                    }
                });

        // If it's a part binding, show the name of the part in the UI.
        var partName = default(string);
        if (action.bindings[bindingIndex].isPartOfComposite)
            partName = $"Binding '{action.bindings[bindingIndex].name}'. ";

        // Bring up rebind overlay, if we have one.
        rebindOverlay.SetActive(true);
        if (rebindText != null)
        {
            var text = !string.IsNullOrEmpty(rebindOperation.expectedControlType)
                ? $"{partName}Waiting for {rebindOperation.expectedControlType} input..."
                : $"{partName}Waiting for input...";
            rebindText.text = text;
        }       
    }

    [Serializable]
    public class InteractiveRebindEvent : UnityEvent<InputRebinding, InputActionRebindingExtensions.RebindingOperation>
    {
    }
}
