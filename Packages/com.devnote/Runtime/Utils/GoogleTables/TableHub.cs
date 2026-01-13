using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevNote
{
    [CreateAssetMenu(menuName = "DevNote/TableHub", fileName = "TableHub")]
    public class TableHub : LoadableFromTable
    {
        [SerializeField] private List<LoadableFromTable> _loadables;


        public override void LoadData(Dictionary<TableKey, Table> tables)
        {
            foreach (var loadable in _loadables)
            {
#if UNITY_EDITOR
                EditorUtility.SetDirty(loadable);
#endif

                loadable.LoadData(tables);
            }
                
        }
    }
}


