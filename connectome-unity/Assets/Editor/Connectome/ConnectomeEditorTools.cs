using Connectome.Unity.Keyboard;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Defines cusom editor functionality exclusive to Connectome
/// </summary>
public class ConnectomeEditorTools : EditorWindow {

    private const string ASSET_PATH = "Assets/Prefab/";
    private const string RESOURCES_PATH = "Assets/Prefab/Resources";
    private const string VALIDATOR_PREFAB = "ConnectomeScene";
    private const string CANVAS_PREFAB = "ConnectomeCanvas";
    private const string OVERLAY_PREFAB = "ConnectomeOverlay";
    private const string KEYBOARD_MANAGER_PREFAB = "KeyboardManager";
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
        if (!GameObject.Find(OVERLAY_PREFAB))
        {
            GameObject connectomeOverlay = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(ASSET_PATH + OVERLAY_PREFAB + ".prefab"));
            connectomeOverlay.name = OVERLAY_PREFAB;
        }
    }

    [MenuItem("Connectome/GenerateKeyboards")]
    static void GenerateKeyboards()
    {
        GameObject obj = GameObject.Find(KEYBOARD_MANAGER_PREFAB);
        if (obj)
        {
            KeyboardTemplate[] keyboards = obj.GetComponentsInChildren<KeyboardTemplate>(true);
            foreach(KeyboardTemplate k in keyboards)
            {
                DestroyImmediate(k.gameObject);
            }
            //Object[] newkeyboards = AssetDatabase.LoadAllAssetsAtPath(RESOURCES_PATH);
            keyboards = Resources.LoadAll<KeyboardTemplate>("Keyboards");
            foreach(KeyboardTemplate k in keyboards)
            {
                KeyboardTemplate kb = Instantiate(k,obj.transform);
                kb.gameObject.SetActive(false);
                kb.gameObject.name = k.gameObject.name;
            }
        }
    }
}
