using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Defines cusom editor functionality exclusive to Connectome
/// </summary>
public class ConnectomeEditorTools : EditorWindow {

    private const string ASSET_PATH = "Assets/Prefab/";
    private const string VALIDATOR_PREFAB = "ConnectomeValidator";
    private const string CANVAS_PREFAB = "ConnectomeCanvas";
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
        if (!GameObject.Find(VALIDATOR_PREFAB))
        {
            GameObject connectomeValidator = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(ASSET_PATH + VALIDATOR_PREFAB + ".prefab"));
            connectomeValidator.name = VALIDATOR_PREFAB;
        }
        if (!GameObject.Find(CANVAS_PREFAB))
        {
            GameObject connectomeCanvas = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(ASSET_PATH + CANVAS_PREFAB + ".prefab"));
            connectomeCanvas.name = CANVAS_PREFAB;
        }
    }
}
