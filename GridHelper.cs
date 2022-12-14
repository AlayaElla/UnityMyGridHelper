using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHelper : LayeroutHelperBase
{
    public enum ChildAligment
    {
        Upper_Left
    }
    public ChildAligment childAligment = ChildAligment.Upper_Left;
    public Vector2 Area = new Vector2(100,100);
    public Vector2 spacing = new Vector2(10,10);
    public override void SetGrid()
    {
        float offsetWidth = 0 + padding.Left;
        float offsetHeight = 0 - padding.Top;
        float current_width = 0;
        Vector3 _pos = Vector3.zero;
        foreach (var item in childList)
        {
            if (item == null)
                childList.Remove(item);

            //如果超过容器长度则换行
            current_width = offsetWidth + padding.Right;
            if (current_width > Area.x)
            {
                offsetWidth = 0 + padding.Left;
                offsetHeight -= spacing.y;
            }
            if (childAligment == ChildAligment.Upper_Left)
            {
                _pos = new Vector3(
                    offsetWidth,//x
                    0,//y
                    offsetHeight //z
                    );

                offsetWidth += spacing.x;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                item.targetPos = _pos;
                item.transform.localPosition = _pos;
            }
#endif
            if (positionTween)
                item.targetPos = _pos;
            else
                item.transform.localPosition = _pos;
        }
    }

    public void SetSpacing(Vector2 _spacing)
    {
        spacing = _spacing;
        SetGrid();
    }


}
