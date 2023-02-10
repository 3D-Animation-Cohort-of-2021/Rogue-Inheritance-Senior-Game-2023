//Created by: Marshall Krueger
//Last edited by: Marshall Krueger 02/09/23
//Purpose: A 3D Tile for our 3D tile system
using UnityEditor.Tilemaps;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New TileBrush3D", menuName = "3D/Tilemap/TileBrush3D", order = 0)]
[CustomGridBrush(false, true, false, "TileBrush3D")]
public class TileBrush3D : GameObjectBrush 
{

    private BrushCell currentCell;
    [HideInInspector]
    public bool replaceTiles = false;



    public void SetCurrentCell(BrushCell newCell)
    {
        currentCell = newCell;
    }

    public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        if(brushTarget.layer == 31)
        {
            return;
        }

        Transform erased = GetObjectInCell(gridLayout, brushTarget.transform, new Vector3Int(position.x, position.y, 0));
        if(erased != null)
        {
            Undo.DestroyObjectImmediate(erased.gameObject);
        }
    }

    

    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            Vector3Int min = position - pivot;
            BoundsInt bounds = new BoundsInt(min, size);

            BoxFill(gridLayout, brushTarget, bounds);
        }

        private void PaintCell(GridLayout grid, Vector3Int position, Transform parent, BrushCell cell)
        {
            if (cell.gameObject == null)
                return;

            Transform existingGO = GetObjectInCell(grid, parent, position);

            if(replaceTiles && existingGO != null && existingGO.gameObject.name != cell.gameObject.name)
            {
                Erase(grid, parent.gameObject, position);
            }

            if (existingGO == null)
            {
                SetSceneCell(grid, parent, position, cell.gameObject, cell.offset, cell.scale, cell.orientation, m_Anchor);
            }
        }

        public override void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
        {
            GetGrid(ref gridLayout, ref brushTarget);
            
            foreach (Vector3Int location in position.allPositionsWithin)
            {
                Vector3Int local = location - position.min;
                BrushCell cell = currentCell;
                PaintCell(gridLayout, location, brushTarget != null ? brushTarget.transform : null, cell);
            }
        }

        private static void SetSceneCell(GridLayout grid, Transform parent, Vector3Int position, GameObject go, Vector3 offset, Vector3 scale, Quaternion orientation, Vector3 anchor)
        {
            if (go == null)
                return;

            GameObject instance;
            if (PrefabUtility.IsPartOfPrefabAsset(go))
            {
                instance = (GameObject) PrefabUtility.InstantiatePrefab(go, parent != null ? parent.root.gameObject.scene : SceneManager.GetActiveScene());
                instance.transform.parent = parent;
            }
            else
            {
                instance = Instantiate(go, parent);
                instance.name = go.name;
                instance.SetActive(true);
                foreach (var renderer in instance.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = true;
                }
            }

            Undo.RegisterCreatedObjectUndo(instance, "Paint GameObject");

            var cellSize = grid.cellSize;
            var cellStride = cellSize + grid.cellGap;
            cellStride.x = Mathf.Approximately(0f, cellStride.x) ? 1f : cellStride.x;
            cellStride.y = Mathf.Approximately(0f, cellStride.y) ? 1f : cellStride.y;
            cellStride.z = Mathf.Approximately(0f, cellStride.z) ? 1f : cellStride.z;
            var anchorRatio = new Vector3(
                anchor.x * cellSize.x / cellStride.x,
                anchor.y * cellSize.y / cellStride.y,
                anchor.z * cellSize.z / cellStride.z
            );
            instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(position + anchorRatio));
            instance.transform.localRotation = orientation;
            instance.transform.localScale = scale;
            instance.transform.Translate(offset);
        }

        private void GetGrid(ref GridLayout gridLayout, ref GameObject brushTarget)
        {
            if (brushTarget == hiddenGrid)
                brushTarget = null;
            if (brushTarget != null)
            {
                var targetGridLayout = brushTarget.GetComponent<GridLayout>();
                if (targetGridLayout != null)
                    gridLayout = targetGridLayout;
            }
        }

    private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
    {
        int childCount = parent.childCount;
        Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
        Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
        Bounds bounds = new Bounds((max + min) * 0.5f, max - min);

        for(int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if(bounds.Contains(child.position))
            {
                return child;
            }
        }

        return null;
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(TileBrush3D))]
public class AnimatedTileEditor : Editor
    {
        Vector2 scrollPosition = Vector2.zero;

        public override void OnInspectorGUI()
        {
            TileBrush3D tileBrush3DInstance = (TileBrush3D)target;
            int nonNullIndex = 0;

            GUILayout.Space(20);
            tileBrush3DInstance.replaceTiles = GUILayout.Toggle(tileBrush3DInstance.replaceTiles,"Replace tiles with current brush.");
            GUILayout.Space(20);

            GUIStyle listStyle = new GUIStyle();

            

            listStyle.normal.background = Texture2D.blackTexture;

            Color[] pixels = listStyle.normal.background.GetPixels();

            for(int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new Color(0.25f, 0.25f, 0.25f, 1f);
            }

            listStyle.normal.background.SetPixels(pixels);
            listStyle.normal.background.Apply();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, listStyle, GUILayout.Width(EditorGUIUtility.currentViewWidth - 10), GUILayout.Height(300));
            for(int i = 0; i < tileBrush3DInstance.cells.Length; i++)
            {
                int widthMod = 1;
                if(tileBrush3DInstance.cells[i].gameObject != null)
                {

                    Texture2D assetPreview = AssetPreview.GetAssetPreview(tileBrush3DInstance.cells[i].gameObject);
                    GUIContent content = new GUIContent((assetPreview), tileBrush3DInstance.cells[i].gameObject.name);
                    

                    if((int)assetPreview.width > 0)
                    {
                        widthMod = (int)EditorGUIUtility.currentViewWidth / (int)assetPreview.width;
                    }

                    if(widthMod < 1)
                    {
                        widthMod = 1;
                    }

                    if(nonNullIndex % widthMod == 0)
                    {
                        GUILayout.BeginHorizontal();
                    }

                    if(GUILayout.Button(content, GUILayout.Width(assetPreview.width)))
                    {
                        tileBrush3DInstance.SetCurrentCell(tileBrush3DInstance.cells[i]);
                    }

                    if(nonNullIndex % widthMod == widthMod - 1 || i == tileBrush3DInstance.cells.Length - 1 )
                    {
                        GUILayout.EndHorizontal();
                    }

                    nonNullIndex++;
                }
            }
            GUILayout.EndScrollView();

            DrawDefaultInspector();
        }
    }
#endif
