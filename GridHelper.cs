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
    public Vector2 spacing = new Vector2();
    public override void SetGrid()
    {
        float offsetWidth = 0 + padding.Left;
        float offsetHeight = 0 - padding.Top;
        float current_width = 0;
        Vector3 _pos = Vector3.zero;
        RectTransform rectParent;
        foreach (var item in childList)
        {
            if (item.Value == null)
                childList.Remove(item.Key);

            //如果超过容器长度则换行
            current_width = offsetWidth + item.Value.rectTransform.rect.width + padding.Right;
            if (current_width > rootRect.rect.width)
            {
                offsetWidth = 0 + padding.Left;
                offsetHeight -= spacing.y;
            }
            if (childAligment == ChildAligment.Upper_Left)
            {
                _pos = new Vector3(
                    offsetWidth + item.Value.rectTransform.rect.width / 2,//x
                    offsetHeight,//y
                    0
                    );

                offsetWidth += item.Value.rectTransform.rect.width + spacing.x;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                item.Value.targetPos = GetRectTransformLocalPosition(_pos, rootRect);
                item.Value.rectTransform.localPosition = GetRectTransformLocalPosition(_pos, rootRect);
            }
#endif
            if (positionTween)
                item.Value.targetPos = GetRectTransformLocalPosition(_pos, rootRect);
            else
                item.Value.rectTransform.localPosition = GetRectTransformLocalPosition(_pos, rootRect);
        }
    }
}
