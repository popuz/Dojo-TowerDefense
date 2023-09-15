using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MissingScriptsRemover
{
    [MenuItem("Tools/Remove Missing Scripts in Scenes and Prefabs")]
    public static void RemoveMissingScripts()
    {
        // Process scenes
        string[] sceneGUIDs = AssetDatabase.FindAssets("t:Scene");
        foreach (string sceneGUID in sceneGUIDs)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(sceneGUID);
            Scene scene = EditorSceneManager.OpenScene(scenePath);

            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObject in rootObjects)
            {
                CleanMissingScriptsRecursively(rootObject);
            }

            EditorSceneManager.SaveScene(scene);
        }

        // Process prefabs
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
        foreach (string prefabGUID in prefabGUIDs)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            CleanMissingScriptsRecursively(prefab);

            EditorUtility.SetDirty(prefab);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void CleanMissingScriptsRecursively(GameObject obj)
    {
        if (obj == null) return;

        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            CleanMissingScriptsRecursively(obj.transform.GetChild(i).gameObject);
        }
    }
}
