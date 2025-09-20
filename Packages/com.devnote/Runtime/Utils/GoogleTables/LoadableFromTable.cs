using System.Collections.Generic;
using UnityEngine;


namespace DevNote
{
    public abstract class LoadableFromTable : ScriptableObject
    {
        public abstract void LoadData(Dictionary<string, Table> tables);

    }
}


