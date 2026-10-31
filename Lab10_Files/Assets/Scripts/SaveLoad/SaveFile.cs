using System;
using System.Collections.Generic;

/*
 * The layout of the JSON save file: one entry per saveable object, holding its SaveID and its saved data
*/

[Serializable]
public class SaveFile
{
    public List<Entry> objects = new List<Entry>();

    // One saved object
    [Serializable]
    public class Entry
    {
        public string saveID;
        public string data;
    }
}