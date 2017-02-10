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
    public static SelectionManager Instance;
    #region Inspector Attrinutes
    public bool AllowSelection;
    /// <summary>
    /// Hold selectable game objects 
    /// </summary>
    public SelectableObject[] SelectionList;
    /// <summary>
    /// The time, in seconds, to wait before the selection changes.
    /// </summary>
    [Range(0.0f, 10.0f)]
    public int WaitInterval = 2;
    

    public UnityEvent OnSelectionChange;
    #endregion
    #region Public Attributes

    /// <summary>
    /// The list of processors attached to the selection manager.
    /// </summary>
    public SelectableObject CurrentSelection { get { return SelectionList[SelectedIndex]; } }

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
        SelectedIndex = (SelectedIndex + 1) % SelectionList.Length;
        ChangeSelection(SelectedIndex);
    }

    /// <summary>
    /// Hilights to previous selection 
    /// </summary>
    public void Previous()
    {
        SelectedIndex = (SelectedIndex - 1 + SelectionList.Length) % SelectionList.Length;
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

    public void PushSelections(SelectableObject[] Selections)
    {
        SelectionList = Selections;
        ChangeSelection(0);
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
    /// Selects element at index 
    /// </summary>
    /// <param name="index"></param>
    private void Select(int index)
    {
        if(AllowSelection)
            SelectionList[index].Select(SelectionList[(SelectionList.Length + index - 1) % SelectionList.Length]); 
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
            //Select(SelectedIndex);//To prevent clicks from taking focus away from the selection. Is this performance heavy?
            UpdateSelectionWait();
        }
    }

    /// <summary>
    /// Unity's built-in OnEnable method.
    /// Start the coroutine to check the attached processors
    /// </summary>
    private void OnEnable()
    {
        if (AllowSelection)
        {
            ChangeSelection(SelectedIndex);
        }
    }

    private void Awake()
    {
        Instance = this;
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
        if (SelectionList == null)
        {
            Debug.LogError("SelectionList cannot be null");
        }

        if (SelectionList.Length == 0)
        {
            Debug.LogError("SelectionList cannot be empty");
        }

        for (int i = 0; i < SelectionList.Length; i++)
        {
            if (SelectionList[i] == null)
            {
                Debug.LogError("SelectionList element cannot be null. Check index " + i);
            }
        }
    }
    #endregion
}


