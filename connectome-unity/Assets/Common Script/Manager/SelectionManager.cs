using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Template;
using Connectome.Unity.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Connectome.Unity.UI;

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

    /// <summary>
    /// Allows selection manager to select. 
    /// </summary>
    public bool AllowSelection;

    /// <summary>
    /// Contains all of the selections the user has gone through in this scene.
    /// </summary>
    public Stack<ISelectionMenu> SelectionStack;

    public SelectionHighlighter Highlighter;

    public SelectionMenu MainMenu;

    /// <summary>
    /// The time, in seconds, to wait before the selection changes.
    /// This variable is in UserSettings now
    /// </summary>
    [Range(0.0f, 10.0f)]
    public float WaitInterval;
    #endregion
    #region Private Attributes

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
        Highlighter.EnableHighlight(); 
        SelectionStack.Peek().SelectNext(Highlighter); 
    }

    /// <summary>
    /// Hilights to previous selection 
    /// </summary>
    public void Previous()
    {
       // SelectedIndex = (SelectedIndex - 1 + SelectionStack.Peek().Length) % SelectionStack.Peek().Length;
       //ChangeSelection(SelectedIndex);
    }
    /// <summary>
    /// Clicks the currently selected elements. A submenu is pushed if the invoked elements contains one. 
    /// </summary>
    public void TriggerClick()
    {
        if (!AllowSelection)
            return;

        Highlighter.DisableHighlight(); 
        ISelectionMenu subMenu =  SelectionStack.Peek().InvokeSelected();
        if(subMenu != null)
        {
            Push(subMenu);
        }
    }

    /// <summary>
    /// Add selection menu to the current stack.
    /// </summary>
    /// <param name="Selections"></param>
    public void Push(ISelectionMenu Selections)
    {
        SelectionStack.Peek().Paused(); 
        SelectionStack.Push(Selections);
        Selections.Pushed();
    }

    /// <summary>
    /// Pushes a selection menu GameObject to selection stack
    /// </summary>
    /// <param name="menu"></param>
    public void PushSelectionMenu(SelectionMenu menu)
    {
        Push(menu); 
    }

    /// <summary>
    /// Remove the current selection list from the stack and go to the previous list.
    /// This counts as selecting, so reset the interval and reset the current selection.
    /// </summary>
    public void Pop()
    {
        if (SelectionStack.Count > 1)
        {
            ISelectionMenu menu = SelectionStack.Pop();
            menu.Popped();
            SelectionStack.Peek().Resumed(); 
            ResetInterval();
        }
        else
        {
            Debug.LogWarning("Poping main selection menu attempted");
        }
        
    }

    /// <summary>
    /// Sets the current wait time back to 0.
    /// Can be used to refresh the waiting time when the user
    /// tries to select an option.
    /// </summary>
    public void ResetInterval()
    {
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
        if (!AllowSelection)
        {
            return;
        }

        CurrentWait += Time.deltaTime;

        if (CurrentWait >= WaitInterval)
        {
            Next();
            ResetInterval();
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
    /// Used to initialize stack and push the base selection to it.
    /// Called by ConnectomeScene
    /// </summary>
    public void Start()
    {
        SelectionStack = new Stack<ISelectionMenu>();

        Push(MainMenu);
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
        /*if (BaseSelection == null)
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
        }*/


        if (MainMenu == null)
        {
           
        }

    }
    #endregion
}


