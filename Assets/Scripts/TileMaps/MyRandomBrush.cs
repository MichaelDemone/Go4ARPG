using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor {
    [CustomGridBrush(false, false, false, "My Random Brush")]
    [CreateAssetMenu(fileName = "New My Random Brush", menuName = "Brushes/My Random Brush")]
    public class MyRandomBrush : UnityEditor.Tilemaps.GridBrush {

        [Serializable]
        public struct RandomTileSet {
            public TileBase randomTiles;
            public int Weight;
        }

        public RandomTileSet[] randomTileSets;

        public override void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position) {
            if (randomTileSets != null && randomTileSets.Length > 0) {
                if (brushTarget == null) {
                    return;
                }

                Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
                if (tilemap == null) {
                    return;
                }

                BoundsInt bounds = new BoundsInt(position.position - pivot, position.size);
                foreach (Vector3Int pos in bounds.allPositionsWithin) {
                    tilemap.SetTile(pos, GetRandomTile());
                }
            }
            else {
                base.BoxFill(gridLayout, brushTarget, position);
            }
        }

        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position) {
            if(randomTileSets != null && randomTileSets.Length > 0) {
                if(brushTarget == null) {
                    return;
                }

                Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
                if(tilemap == null) {
                    return;
                }

                Vector3Int startLocation = position - pivot;

                tilemap.SetTile(startLocation, GetRandomTile());
            } else {
                base.Paint(grid, brushTarget, position);
            }
        }

        public TileBase GetRandomTile() {
            int totalWeight = randomTileSets.Sum(set => set.Weight);
            int val = (int) (UnityEngine.Random.value * totalWeight) + 1;
            int idx = 0;
            while(val > 0) {
                val -= randomTileSets[idx++].Weight;
            }

            RandomTileSet randomTileSet = randomTileSets[idx - 1];
            return randomTileSet.randomTiles;
        }
    }



    [CustomEditor(typeof(MyRandomBrush))]
    public class RandomBrushEditor : UnityEditor.Tilemaps.GridBrushEditor {
        private MyRandomBrush randomBrush { get { return target as MyRandomBrush; } }
        private GameObject lastBrushTarget;

        public override void PaintPreview(GridLayout grid, GameObject brushTarget, Vector3Int position) {
            if(randomBrush.randomTileSets != null && randomBrush.randomTileSets.Length > 0) {
                base.PaintPreview(grid, null, position);
                if(brushTarget == null) {
                    return;
                }

                Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
                if(tilemap == null) {
                    return;
                }

                Vector3Int min = position - randomBrush.pivot;
                tilemap.SetEditorPreviewTile(min, randomBrush.GetRandomTile());
                lastBrushTarget = brushTarget;
            } else {
                base.PaintPreview(grid, brushTarget, position);
            }
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

            SerializedProperty textureProp = so.FindProperty(nameof(randomBrush.randomTileSets));
            EditorGUILayout.PropertyField(textureProp, true); // True means show children
            so.ApplyModifiedProperties(); // Remember to apply modified properties

            so.Update();
            so.ApplyModifiedProperties(); // Remember to apply modified properties

        }
    }
}