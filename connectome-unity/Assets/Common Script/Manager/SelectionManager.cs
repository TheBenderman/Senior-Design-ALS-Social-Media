using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Template;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


/// <summary>
/// Allows highlighting a selection of game objects and surfing* between them.
/// </summary>
public class SelectionManager : MonoBehaviour
{
    #region Static Attributes
    /// <summary>
    /// Static reference
    /// </summary>
    public static SelectionManager Instance;
    #endregion
    #region Inspector Attrinutes
    public bool AllowSelection;
    /// <summary>
    /// Hold the initial selection when this scene is started.
    /// </summary>
    public SelectableObject[] BaseSelection;
    /// <summary>
    /// Contains all of the selections the user has gone through in this scene.
    /// </summary>
    public Stack<SelectableObject[]> SelectionStack;
    /// <summary>
    /// The time, in seconds, to wait before the selection changes.
    /// </summary>
    [Range(0.0f, 10.0f)]
    public int WaitInterval = 2;

    /// <summary>
    /// The default color we want buttons to be(if flash is turned off)
    /// </summary>
    public Color DefaultSelectColor;
    /// <summary>
    /// The color when the object is NOT selected
    /// </summary>
    public Color DefaultUnselectColor;
    /// <summary>
    /// Any events that fire off when the selection changes.
    /// </summary>
    public UnityEvent OnSelectionChange;
    #endregion
    #region Public Attributes

    /// <summary>
    /// The list of processors attached to the selection manager.
    /// </summary>
    public SelectableObject CurrentSelection { get { return SelectionStack.Peek()[SelectedIndex]; } }

    #endregion
    #region Private Attributes
    /// <summary>
    /// Hold currently selected element. 
    /// </summary>
    private int SelectedIndex = 0;
    /// <summary>
    /// The current interval
    /// </summary>
    private float CurrentWait = 0;
    #endregion
    #region Public Methods
    /// <summary>
    /// Makes the selection manager start cycling through the selection list
    /// </summary>
    public void Activate()
    {
        AllowSelection = true;
    }
    /// <summary>
    /// Stops the selection manager from cycling through the selection list
    /// </summary>
    public void Deactivate()
    {
        AllowSelection = false;
    }
    /// <summary>
    /// Hilights to next selection 
    /// </summary>
    public void Next()
    {
        SelectedIndex = (SelectedIndex + 1) % SelectionStack.Peek().Length;
        ChangeSelection(SelectedIndex);
    }

    /// <summary>
    /// Hilights to previous selection 
    /// </summary>
    public void Previous()
    {
        SelectedIndex = (SelectedIndex - 1 + SelectionStack.Peek().Length) % SelectionStack.Peek().Length;
        ChangeSelection(SelectedIndex);
    }
    /// <summary>
    /// Clicks the currently selected button
    /// </summary>
    public void TriggerClick()
    {
        if (AllowSelection)
            CurrentSelection.TriggerClick(this);
    }

    /// <summary>
    /// Add the list of selectable objects to the current stack.
    /// This counts as selecting, so reset the interval and reset the current selection.
    /// </summary>
    /// <param name="Selections"></param>
    public void PushSelections(SelectableObject[] Selections)
    {
        SelectionStack.Push(Selections);
        ChangeSelection(0);//After adding a new list of selections, start counting from the first index
        ResetInterval();
    }
    /// <summary>
    /// Remove the current selection list from the stack and go to the previous list.
    /// This counts as selecting, so reset the interval and reset the current selection.
    /// </summary>
    public void PopSelections()
    {
        SelectionStack.Pop();
        ChangeSelection(0);//After removing the selections, start counting from the first index
        ResetInterval();
    }
    #endregion
    #region Private Methods
    /// <summary>
    /// Hilights game object at a given index
    /// </summary>
    /// TODO validate index. 
    /// <param name="index"></param>
    private void ChangeSelection(int index)
    {
        Select(index);
        SelectedIndex = index; 
        OnSelectionChange.Invoke();
        //Debug.Log("The selected element = " + SelectionList[index].name);
    }
   
    private void UpdateSelectionWait()
    {
        CurrentWait += Time.deltaTime;
        if (CurrentWait >= WaitInterval)
        {
            Next();
            ResetInterval();
        }
    }

    /// <summary>
    /// Selects element from the top-most selection list in the stack, at index 
    /// </summary>
    /// <param name="index"></param>
    private void Select(int index)
    {
        if (AllowSelection)
        {
            int CurrentSelectionLength = SelectionStack.Peek().Length;
            SelectionStack.Peek()[index].Select(SelectionStack.Peek()[(CurrentSelectionLength + index - 1) % CurrentSelectionLength]);
        }
    }

   
    /// <summary>
    /// Sets the current wait time back to 0.
    /// Can be used to refresh the waiting time when the user
    /// tries to select an option.
    /// </summary>
    public void ResetInterval()
    {
        Select(SelectedIndex);
        CurrentWait = 0;
    }

    #endregion
    #region Unity Methods
    /// <summary>
    /// Unity's built-in Update method.
    /// Keep track of the application's time, and
    /// change the selection after the interval time passes.
    /// </summary>
    private void Update()
    {
        if (AllowSelection)
        {
            UpdateSelectionWait();
        }
    }
    /// <summary>
    /// Built-in Awake Method.
    /// Used to attach the singleton instance.
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// Built-in Start method.
    /// Used to initialize stack and push the base selection to it.
    /// </summary>
    private void Start()
    {
        SelectionStack = new Stack<SelectableObject[]>();
        PushSelections(BaseSelection);
    }
    #endregion
    #region Validation
    /// <summary>
    /// Validate object's requirments 
    /// </summary>
    private void OnValidate()
    {
        ValidateSelectionList();
    }

    /// <summary>
    /// Validates having a selection list 
    /// </summary>
    private void ValidateSelectionList()
    {
        if (BaseSelection == null)
        {
            Debug.LogError("SelectionList cannot be null");
        }

        if (BaseSelection.Length == 0)
        {
            Debug.LogError("SelectionList cannot be empty");
        }

        for (int i = 0; i < BaseSelection.Length; i++)
        {
            if (BaseSelection[i] == null)
            {
                Debug.LogError("BaseSelection element cannot be null. Check index " + i);
            }
        }
    }
    #endregion
}


