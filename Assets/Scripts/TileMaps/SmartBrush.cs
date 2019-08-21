using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor {
    [CustomGridBrush(false, false, false, "Smart Brush")]
    [CreateAssetMenu(fileName = "New Smart Brush", menuName = "Brushes/Smart Brush")]
    public class SmartBrush : GridBrush {

        [Serializable]
        public struct PathTileSet {
            public TileBase TopLeftTurn;
            public TileBase Top;
            public TileBase TopRightTurn;
            public TileBase Right;
            public TileBase BottomRightTurn;
            public TileBase Bottom;
            public TileBase BottomLeftTurn;
            public TileBase Left;
            public TileBase OutsideTurnTopLeft;
            public TileBase OutsideTurnTopRight;
            public TileBase OutsideTurnBottmLeft;
            public TileBase OutsideTurnBottmRight;
            public TileBase Filled;
            public TileBase Nothing;
            public TileBase TopLeftAndBottomRightCorner;
            public TileBase TopRightAndBottomLeftCorner;
        }


        public PathTileSet pathTileSet;
        private bool[,] corners = new bool[6, 6];
        
        public override void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position) {
            if(pathTileSet.TopLeftTurn != null) {
                if(brushTarget == null) {
                    return;
                }

                Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
                if(tilemap == null) {
                    return;
                }

                BoundsInt bounds = new BoundsInt(position.position - pivot, position.size);
                foreach(Vector3Int pos in bounds.allPositionsWithin) {
                    //tilemap.SetTile(pos, GetRandomTile());
                }
            } else {
                base.BoxFill(gridLayout, brushTarget, position);
            }
        }

        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position) {
            if(pathTileSet.TopLeftTurn != null) {
                if(brushTarget == null) {
                    return;
                }

                Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
                if(tilemap == null) {
                    return;
                }

                Vector3Int startLocation = position - pivot;
                SetTiles(tilemap, position);
                //tilemap.SetTile(startLocation, GetRandomTile());
            } else {
                base.Paint(grid, brushTarget, position);
            }
        }

        public void SetTiles(Tilemap map, Vector3Int position) {

            SetDicts();

            // c c c c c c c    (corners)
            //  t t t t t t     (tiles, responsible for top left corner, except end pieces which are responsible for top right corners)  
            // c c c c c c c    (corners)   
            //  t t t t t t     (tiles, same as above)
            // c c c c c c c    
            //  t t t t t t     (responsible for corners below and above)
            // c c c c c c c

            int topLeft =       0b0001;
            int topRight =      0b0010;
            int bottomLeft =    0b0100;
            int bottomRight =   0b1000;


            Dictionary<int, (int, int)> cornerToTile = new Dictionary<int, (int, int)> {
                {0, (0, bottomLeft)},     {1, (1, bottomLeft)},     {2, (2, bottomLeft)},     {3, (3, bottomLeft)},     {4, (4, bottomLeft) },     {5, (4, bottomRight) },
                {6, (5, bottomLeft)},     {7, (6, bottomLeft)},     {8, (7, bottomLeft)},     {9, (8, bottomLeft)},     {10, (9, bottomLeft)},    {11, (9, bottomRight) },
                {12, (10, bottomLeft)},   {13, (11, bottomLeft)},   {14, (12, bottomLeft)},   {15, (13, bottomLeft)},   {16, (14, bottomLeft)},   { 17, (14, bottomRight)},
                {18, (15, bottomLeft)},   {19, (16, bottomLeft)},   {20, (17, bottomLeft)},   {21, (18, bottomLeft)},   {22, (19, bottomLeft)},   {23, (19, bottomRight)},
                {24, (20, bottomLeft)},   {25, (21, bottomLeft)},   {26, (22, bottomLeft)},   {27, (23, bottomLeft)},   {28, (24, bottomLeft)},   {29, (24, bottomRight)},
                {30, (20, topLeft)},   {31, (21, topLeft)},   {32, (22, topLeft)},   {33, (23, topLeft)},   {34, (24, topLeft)},   {35, (24, topRight)},
            };

            // Set up the corners data structure.
            bool[] corners = new bool[36];
            Vector3Int bottomLeftPos = position + new Vector3Int(-2, -2, 0);

            for(int row = 0; row < 6; row++) {
                for (int col = 0; col < 6; col++) {

                    int corner = row * 6 + col;
                    (int tile, int cornerPosition) = cornerToTile[corner];

                    int tileCol = tile % 5;
                    int tileRow = tile / 5;
                    
                    TileBase actualTile = map.GetTile(bottomLeftPos + new Vector3Int(tileCol, tileRow, 0));

                    if(tilesToCorners.ContainsKey(actualTile)) {
                        int cornerBools = tilesToCorners[actualTile];
                        corners[corner] = (cornerBools & cornerPosition) != 0;
                    } 
                }
            }

            corners[2*6 + 2] = true;
            corners[2*6 + 3] = true;
            corners[3*6 + 2] = true;
            corners[3*6 + 3] = true;


            Dictionary<(int, int), int> tileToCorner = new Dictionary<(int, int), int>();

            for(int row = 0; row < 5; row++) {
                for(int col = 0; col < 5; col++) {
                    tileToCorner.Add((row * 5 + col, bottomLeft), row * 6 + col);
                    tileToCorner.Add((row * 5 + col, bottomRight), row * 6 + col + 1);
                    tileToCorner.Add((row * 5 + col, topLeft), row * 6 + col + 6);
                    tileToCorner.Add((row * 5 + col, topRight), row * 6 + col + 7);
                }
            }

            // Set tiles based on corners
            for(int row = 0; row < 5; row++) {
                for(int col = 0; col < 5; col++) {
                    int v = 0;
                    if(corners[tileToCorner[(row * 5 + col, topLeft)]]) 
                        v |= topLeft;
                    if(corners[tileToCorner[(row * 5 + col, topRight)]])
                        v |= topRight;
                    if(corners[tileToCorner[(row * 5 + col, bottomLeft)]])
                        v |= bottomLeft;
                    if(corners[tileToCorner[(row * 5 + col, bottomRight)]])
                        v |= bottomRight;

                    TileBase tileThatShouldGoHere = cornersToTile[v];
                    map.SetTile(bottomLeftPos + new Vector3Int(col, row, 0), tileThatShouldGoHere);
                }
            }

            return;
        }


        Dictionary<int, TileBase> cornersToTile = null;
        Dictionary<TileBase, int> tilesToCorners = null;

        void SetDicts() {
            if(cornersToTile != null && tilesToCorners != null)
                return;

            cornersToTile = new Dictionary<int, TileBase> {
                { 0b0000, pathTileSet.Nothing},
                { 0b0001, pathTileSet.BottomRightTurn},
                { 0b0010, pathTileSet.BottomLeftTurn},
                { 0b0011, pathTileSet.Bottom},
                { 0b0100, pathTileSet.TopRightTurn},
                { 0b0101, pathTileSet.Right},
                { 0b0111, pathTileSet.OutsideTurnTopLeft},
                { 0b1000, pathTileSet.TopLeftTurn},
                { 0b1010, pathTileSet.Left},
                { 0b1011, pathTileSet.OutsideTurnTopRight},
                { 0b1100, pathTileSet.Top},
                { 0b1101, pathTileSet.OutsideTurnBottmLeft},
                { 0b1110, pathTileSet.OutsideTurnBottmRight},
                { 0b1111, pathTileSet.Filled},
                { 0b0110, pathTileSet.TopRightAndBottomLeftCorner}, // Diagonal
                { 0b1001, pathTileSet.TopLeftAndBottomRightCorner}, // Diagonal
            };

            tilesToCorners = new Dictionary<TileBase, int>();
            foreach(var kvp in cornersToTile) {
                tilesToCorners[kvp.Value] = kvp.Key;
            }
        }
    }



    [CustomEditor(typeof(SmartBrush))]
    public class SmartBrushEditor : GridBrushEditor {
        private SmartBrush randomBrush { get { return target as SmartBrush; } }
        private GameObject lastBrushTarget;

        public override void PaintPreview(GridLayout grid, GameObject brushTarget, Vector3Int position) {
            //if(randomBrush.pathTileSet != null && randomBrush.pathTileSet.Length > 0) {
                base.PaintPreview(grid, null, position);
                if(brushTarget == null) {
                    return;
                }

                Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
                if(tilemap == null) {
                    return;
                }

                Vector3Int min = position - randomBrush.pivot;
                //tilemap.SetEditorPreviewTile(min, randomBrush.GetRandomTile());
                lastBrushTarget = brushTarget;
           // } else {
                base.PaintPreview(grid, brushTarget, position);
           // }
        }

        public override void ClearPreview() {
            if(lastBrushTarget != null) {
                Tilemap tilemap = lastBrushTarget.GetComponent<Tilemap>();
                if(tilemap == null) {
                    return;
                }

                tilemap.ClearAllEditorPreviewTiles();

                lastBrushTarget = null;
            } else {
                base.ClearPreview();
            }
        }

        public override void OnPaintInspectorGUI() {
            ScriptableObject target = randomBrush;
            SerializedObject so = new SerializedObject(target);

            SerializedProperty textureProp = so.FindProperty(nameof(randomBrush.pathTileSet));
            EditorGUILayout.PropertyField(textureProp, true); // True means show children
            so.ApplyModifiedProperties(); // Remember to apply modified properties

            so.Update();
            so.ApplyModifiedProperties(); // Remember to apply modified properties

        }
    }
}