/*
 * Any MonoBehaviour that can save and restore its own state implements this interface
*/

public interface ISaveable
{
    string SaveID { get; }
    string SavedData { get; }
    void LoadFromData(string data);
}