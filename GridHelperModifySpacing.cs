using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(GridHelper))]
public class GridHelperModifySpacing : MonoBehaviour
{
    private GridHelper layeroutHelper;
    int childCount = 0;

    [System.Serializable]
    public class Modify
    {
        public int count;
        public Vector2 spaceing;
    }
    public Modify[] ModifyList;

    public void OnTransformChildrenChanged(){
        if (layeroutHelper == null)
            layeroutHelper = GetComponent<GridHelper>();

        foreach(Modify modify in ModifyList)
        {
            if (modify.count <= transform.childCount)
                layeroutHelper.spacing = modify.spaceing;
        }
    }
}
