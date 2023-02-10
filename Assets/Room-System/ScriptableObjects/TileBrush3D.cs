//Created by: Marshall Krueger
//Last edited by: Marshall Krueger 02/09/23
//Purpose: A 3D Tile for our 3D tile system
using UnityEditor.Tilemaps;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New TileBrush3D", menuName = "3D/Tilemap/TileBrush3D", order = 0)]
[CustomGridBrush(false, true, false, "TileBrush3D")]
public class TileBrush3D : GameObjectBrush 
{

    private BrushCell currentCell;

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

    public void SetCurrentCell(BrushCell newCell)
    {
        currentCell = newCell;
    }

    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        if(gridLayout != null)
        {
            Vector3Int min = position - pivot;
            //BoundsInt bounds = new BoundsInt(min, m_Size);
            Transform existingObject = GetObjectInCell(gridLayout, brushTarget.transform, new Vector3Int(position.x, position.y, 0));

            if(existingObject != null)
            {
                Undo.DestroyObjectImmediate(existingObject.gameObject);
            }
            
        }
    }

    private void PaintCell()
    {

    }

    private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
    {
        int childCount = parent.childCount;
        Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position - Vector3Int.one));
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
        public override void OnInspectorGUI()
        {
            TileBrush3D tileBrush3DInstance = (TileBrush3D)target;
            int nonNullIndex = 0;

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

                    if(widthMod <= 0)
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

            DrawDefaultInspector();
        }
    }
#endif
