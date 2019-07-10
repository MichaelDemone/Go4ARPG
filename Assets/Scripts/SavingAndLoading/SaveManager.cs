using CustomEvents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Saving {
    [CreateAssetMenu(menuName = "Misc/Save Manager")]
    public class SaveManager : ScriptableObject {

        public UnityEvent OnLoadNotSuccessful;
        public UnityEvent OnNewGame;
        public UnityEvent OnSuccessfulLoad;
        public UnityEvent OnSaveFinish;

        private string saveFile;
        private string backUpFile;

        public BoolReference AllowSave;

        public List<ListObject> ObjectsToSave;

        [System.Serializable]
        public class ListObject {
            public ScriptableObject ObjectToSave; // These objects must inherit from ISaveable. There's no way to enforce that though.
            public List<ScriptableObject> OtherData;
        }

        void OnEnable() {
            saveFile = Path.Combine(Application.persistentDataPath, "Saves", "G4AW2_" + name + ".save");
            backUpFile = saveFile + "_Backup";
        }

        [ContextMenu("Save")]
        public void Save() {
            if (!AllowSave) return;

            if(File.Exists(saveFile)) {
                if(File.Exists(backUpFile)) {
                    File.Delete(backUpFile);
                }

                File.Copy(saveFile, backUpFile);
                File.Delete(saveFile);
            }

            string directoryName = Path.GetDirectoryName(saveFile);

            if(!Directory.Exists(directoryName)) {
                Directory.CreateDirectory(directoryName);
            }

            string saveString = GetSaveString();

            File.WriteAllText(saveFile, saveString);
            OnSaveFinish?.Invoke();
        }

        [ContextMenu("Load")]
        public void Load() {
            string saveFilePath;
            if(!File.Exists(saveFile)) {

                Debug.LogWarning("Could not find save file. Attempting to load back up");
                if(!File.Exists(backUpFile)) {
                    Debug.LogWarning("Could not find back up file.");
                    OnNewGame?.Invoke();
                    return;
                } else {
                    saveFilePath = backUpFile;
                }
            } else {
                saveFilePath = saveFile;
            }

            bool success;

            try {
                LoadFromString(File.ReadAllText(saveFilePath));
                success = true;
            } catch(Exception e) {
                ShowErrorPopUp();
                Debug.LogError(e);
                success = false;
            }

            if(!success) {
                OnLoadNotSuccessful?.Invoke();
            } else {
                OnSuccessfulLoad?.Invoke();
            }

        }

        private void ShowErrorPopUp() {
            /*PopUp.SetPopUp("Could not load save data. Try closing the game and reopening. If the problem persists please contact support.", new[] { "Ok", "Other" },
                new Action[] {
                    () => {
                        Application.Quit();
                    },
                    () => {
                        PopUp.SetPopUp("Would you like to clear your save data and start over?",
                            new [] {"Yes", "No"},
                            new Action[] {
                                () => {
                                    ClearSaveData();
                                    AllowSave.Value = false;
                                },
                                () => {
                                    ShowErrorPopUp();
                                }
                            });
                    }
                });*/
        }

        private void LoadFromString(string loadText) {
            string errorMessage = "Could not load the following:\n";
            bool error = false;
            SaveObject saveData = JsonUtility.FromJson<SaveObject>(loadText);
            foreach(KeyValuePairStringString kvp in saveData.VariableDictionary) {
                ListObject soToOverwrite = ObjectsToSave.FirstOrDefault(so => so.ObjectToSave.name.Equals(kvp.Key));

                if(soToOverwrite == null) {
                    if (kvp.Key.Equals("Time")) {
                        LastTimePlayedUTC = DateTime.Parse(kvp.Value, null, DateTimeStyles.RoundtripKind);
                        continue;
                    }
                    // Removed a variable?
                    Debug.LogWarning("Could not find scriptable object matching name: " + kvp.Key);
                    continue;
                }

                try {
                    if (soToOverwrite.OtherData.Count == 0) {
                        ((ISaveable) soToOverwrite.ObjectToSave).SetData(kvp.Value);
                    }
                    else {
                        ((ISaveable) soToOverwrite.ObjectToSave).SetData(kvp.Value, soToOverwrite.OtherData[0]);
                    }
                }
                catch (Exception e) {
                    Debug.LogWarning(e.Message + "\n" + e.StackTrace);
                    errorMessage += $"<b>{soToOverwrite.ObjectToSave.name}\n{e.Message}\n{e.StackTrace}</b>\n";
                    error = true;
                }
            }

            /*
            if (error) {
                PopUp.SetPopUp(errorMessage, new[] {"Ok", "Close Game"},
                    new Action[] {() => { }, () => Application.Quit()});
            }*/
            
        }

        private string GetSaveString() {
            return JsonUtility.ToJson(GetSaveData());
        }

        public static DateTime LastTimePlayedUTC = DateTime.UtcNow;

        private SaveObject GetSaveData() {
            List<KeyValuePairStringString> saveDictForVariables = new List<KeyValuePairStringString>();// = 
                                                                                                       //ObjectsToSave.Select(so => new KeyValuePairStringString(so.ObjectToSave.name, ((ISaveable)so.ObjectToSave).GetSaveString())).ToList();

            foreach(ListObject saveObjs in ObjectsToSave) {
                string key = saveObjs.ObjectToSave.name;
                string value = ((ISaveable) saveObjs.ObjectToSave).GetSaveString();


                saveDictForVariables.Add(new KeyValuePairStringString(key, value));
            }

            saveDictForVariables.Add(new KeyValuePairStringString("Time", DateTime.UtcNow.ToString("o")));

            return new SaveObject {
                VariableDictionary = saveDictForVariables,
            };
        }

        [System.Serializable]
        private struct KeyValuePairStringString {
            public string Key;
            public string Value;
            public KeyValuePairStringString(string key, string value) {
                Key = key;
                Value = value;
            }
        }

        private struct SaveObject {
            public List<KeyValuePairStringString> VariableDictionary;
        }

        [ContextMenu("Clear all save data")]
        void ClearSaveData() {
            File.Delete(saveFile);
            File.Delete(backUpFile);
        }

        public void ClearDataAndPreventFromSaving() {
            ClearSaveData();
            AllowSave.Value = false;
        }

#if UNITY_EDITOR
        [ContextMenu("Print Save String")]
        void PrintSaveString() {
            Debug.Log(GetSaveString());
        }
#endif
    }
}

