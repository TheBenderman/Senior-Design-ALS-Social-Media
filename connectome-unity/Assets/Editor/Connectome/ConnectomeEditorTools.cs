using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Defines cusom editor functionality exclusive to Connectome
/// </summary>
public class ConnectomeEditorTools : EditorWindow {
    /// <summary>
    /// Create a new Unity Scene and insert all common prefabs.
    /// </summary>
    [MenuItem("Connectome/Create Connectome Scene")]
    static void CreateConnectomeScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.DefaultGameObjects);
        GenerateCommonObjects();
    }

    /// <summary>
    /// Add the common prefabs to the current scene.
    /// </summary>
    [MenuItem("Connectome/Generate Common Objects")]
    static void GenerateCommonObjects()
    {
        if (!GameObject.Find("ConnectomeScene"))
        {
            GameObject connectomeScene = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefab/ConnectomeScene.prefab"));
            connectomeScene.name = "ConnectomeScene";
        }
    }
}
