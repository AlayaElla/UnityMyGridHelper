using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHelperTrapezoid : LayeroutHelperBase
{
    public Vector2 Area = new Vector2(5,5);
    public Vector2 spacing = new Vector2(1,1);
    public float addValue = 1f;
    public override void SetGrid()
    {
        float paddingLeft = padding.Left;
        float areaWidth = Area.x;
        float offsetWidth = 0 + paddingLeft- CalCenterOffset(areaWidth);
        float offsetHeight = 0 - padding.Top;
        float current_width = 0;

        Vector3 _pos = Vector3.zero;
        foreach (var item in childList)
        {
            if (item == null)
                childList.Remove(item);

            //如果超过容器长度则换行
            if (current_width >= areaWidth)
            {
                current_width = 0;
                areaWidth += addValue;
                offsetWidth = 0 - CalCenterOffset(areaWidth);
                offsetHeight -= spacing.y;
            }
            _pos = new Vector3(
              offsetWidth,//x
              0,//y
              offsetHeight //z
              );

            offsetWidth += spacing.x;
            current_width += spacing.x;

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

    float CalCenterOffset(float nowAreaWidth)
    {
        float width = 0;
        for(int i=0;i< childList.Count; i++)
        {
            if (width >= nowAreaWidth)
                return Mathf.Max((width - spacing.x) / 2, 0);
            width += spacing.x;
        }
        return Mathf.Max((width - spacing.x) / 2, 0);
    }

}
