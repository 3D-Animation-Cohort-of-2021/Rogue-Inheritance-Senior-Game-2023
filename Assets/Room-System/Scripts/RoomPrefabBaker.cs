using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[RequireComponent(typeof(Grid))]
public class RoomPrefabBaker : MonoBehaviour
{

    private string folderPath = "Assets";

    public bool GeneratePrefab()
    {
        
        
        bool success = false;

        if(!AssetDatabase.IsValidFolder(folderPath))
        {
            Object activeFolder = Selection.activeObject;

            folderPath = "Assets";
        }

        if(transform.childCount > 0)
        {

            for(int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if(child.childCount != 0)
                {

                    PrefabUtility.SaveAsPrefabAssetAndConnect(child.gameObject, folderPath + "/" + child.gameObject.name + ".prefab", InteractionMode.UserAction);
                    success = true;
                }
            }

            for(int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                Destroy(child);
            }

        }

        Instantiate(new GameObject(), transform);

        return success;
    }

    public void SetFolderPath(string newFolderPath)
    {
        folderPath = newFolderPath;
    }

    public string GetFolderPath()
    {
        return folderPath;
    }
    
}




#if UNITY_EDITOR
[CustomEditor(typeof(RoomPrefabBaker))]
public class RoomPrefabBakerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            RoomPrefabBaker roomPrefabBakerInstance = (RoomPrefabBaker)target;

            GUILayout.BeginHorizontal();

            roomPrefabBakerInstance.SetFolderPath(GUILayout.TextField(roomPrefabBakerInstance.GetFolderPath()));

            if(GUILayout.Button("Use Open Folder", GUILayout.Height(20), GUILayout.Width(110)))
            {
                System.Type projectWindowUtilType = typeof(ProjectWindowUtil);
                MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
                object obj = getActiveFolderPath.Invoke(null, new object[0]);
                string pathToCurrentFolder = obj.ToString();

                roomPrefabBakerInstance.SetFolderPath(pathToCurrentFolder);

            }
            GUILayout.EndHorizontal();

            if(GUILayout.Button("Generate Prefab from Tilemaps", GUILayout.Height(50)))
            {
                if(roomPrefabBakerInstance.GeneratePrefab())
                {
                    Debug.Log("Prefab Successfully Generated");
                }
                else
                {
                    Debug.LogError("Prefab could not be generated");
                }
            }
        }
    }
#endif