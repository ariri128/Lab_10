using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/*
 * Writes and reads the save files: object locations as JSON and the score as binary
 * Files are stored in Application.persistentDataPath
*/

public static class SavingService
{
    // Builds the full path for a save file inside the persistent data folder
    public static string PathForFilename(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    // Finds every ISaveable in the scene, asks each one for its data and writes all of it to a JSON file
    public static void SaveGame(string fileName)
    {
        SaveFile saveFile = new SaveFile();

        foreach (ISaveable saveable in FindAllSaveables())
        {
            saveFile.objects.Add(new SaveFile.Entry { saveID = saveable.SaveID, data = saveable.SavedData });
        }

        if (saveFile.objects.Count == 0)
        {
            Debug.LogWarning("The scene did not include any saveable objects.");
        }

        string path = PathForFilename(fileName);
        using (StreamWriter stream = File.CreateText(path))
        {
            stream.WriteLine(JsonUtility.ToJson(saveFile, true));
        }

        Debug.LogFormat("Wrote saved game to {0}", path);
    }

    // Reads the JSON file and gives each saved entry back to the scene object with the matching SaveID
    public static bool LoadGame(string fileName)
    {
        string path = PathForFilename(fileName);
        if (!File.Exists(path))
        {
            Debug.LogWarningFormat("No file exists at {0}", path);
            return false;
        }

        SaveFile saveFile;
        try
        {
            saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(path));
        }
        catch (Exception exception)
        {
            Debug.LogErrorFormat("Could not read the save file at {0}: {1}", path, exception.Message);
            return false;
        }

        if (saveFile == null || saveFile.objects == null)
        {
            Debug.LogErrorFormat("Data at {0} is not a valid save file.", path);
            return false;
        }

        Dictionary<string, ISaveable> loadables = new Dictionary<string, ISaveable>();
        foreach (ISaveable saveable in FindAllSaveables())
        {
            if (!loadables.ContainsKey(saveable.SaveID))
            {
                loadables.Add(saveable.SaveID, saveable);
            }
        }

        foreach (SaveFile.Entry entry in saveFile.objects)
        {
            if (entry != null && entry.saveID != null && loadables.TryGetValue(entry.saveID, out ISaveable loadable))
            {
                loadable.LoadFromData(entry.data);
            }
        }

        Debug.LogFormat("Loaded saved game from {0}", path);
        return true;
    }

    // Writes the score to a binary file
    public static void SaveScore(string fileName, int score)
    {
        string path = PathForFilename(fileName);
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            writer.Write(score);
        }

        Debug.LogFormat("Wrote score to {0}", path);
    }

    // Reads the score back from the binary file
	// Returns false if there is no file or it can't be read
    public static bool TryLoadScore(string fileName, out int score)
    {
        score = 0;
        string path = PathForFilename(fileName);
        if (!File.Exists(path))
        {
            Debug.LogWarningFormat("No file exists at {0}", path);
            return false;
        }

        try
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
            {
                score = reader.ReadInt32();
            }
        }
        catch (Exception exception)
        {
            Debug.LogErrorFormat("Could not read the score file at {0}: {1}", path, exception.Message);
            return false;
        }

        return true;
    }

    // Returns every active MonoBehaviour in the scene that implements ISaveable and has a SaveID
    private static IEnumerable<ISaveable> FindAllSaveables()
    {
        return UnityEngine.Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<ISaveable>()
            .Where(saveable => !string.IsNullOrEmpty(saveable.SaveID));
    }
}