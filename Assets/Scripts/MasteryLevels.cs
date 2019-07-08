using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Mastery ")]
public class MasteryLevels : ScriptableObject, ISaveable {

	private static Dictionary<int, int> IDToNumTaps = new Dictionary<int, int>();

    public static int GetTaps(int id) {
        if (!IDToNumTaps.ContainsKey(id)) {
            IDToNumTaps.Add(id, 0);
        }
        return IDToNumTaps[id];
    }

    public static void Tap(int id) {
        if(!IDToNumTaps.ContainsKey(id)) {
            IDToNumTaps.Add(id, 0);
        }
        IDToNumTaps[id]++;
    }

    [System.Serializable]
    private class IDToTaps {
        public int ID;
        public int Taps;
    }

    [System.Serializable]
    private class SaveObject {
        public List<IDToTaps> IDToTaps;
    }

    public void Register() {
        //Singleton = this;
    }

    public string GetSaveString() {
        SaveObject so = new SaveObject();
        so.IDToTaps = new List<IDToTaps>();
        foreach (var kvp in IDToNumTaps) {
            so.IDToTaps.Add(new IDToTaps(){ID = kvp.Key, Taps = kvp.Value});
        }

        return JsonUtility.ToJson(so);
    }

    public static bool Loaded = false;

    public void SetData(string saveString, params object[] otherData) {
        SaveObject so = JsonUtility.FromJson<SaveObject>(saveString);

        if(so.IDToTaps == null)
            return;

        IDToNumTaps.Clear();
        foreach (var idToTaps in so.IDToTaps) {
            IDToNumTaps.Add(idToTaps.ID, idToTaps.Taps);
        }

        Loaded = true;
    }

    public void OnNewGame() {
        Loaded = true;
    }
}
