using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHelperUI : LayeroutHelperUIBase
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
        foreach (var item in childList)
        {
            if (item == null)
                childList.Remove(item);

            //�������������������
            current_width = offsetWidth + item.rectTransform.rect.width + padding.Right;
            if (current_width > rootRect.rect.width)
            {
                offsetWidth = 0 + padding.Left;
                offsetHeight -= spacing.y;
            }
            if (childAligment == ChildAligment.Upper_Left)
            {
                _pos = new Vector3(
                    offsetWidth + item.rectTransform.rect.width / 2,//x
                    offsetHeight,//y
                    0
                    );

                offsetWidth += item.rectTransform.rect.width + spacing.x;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                item.targetPos = GetRectTransformLocalPosition(_pos, rootRect);
                item.rectTransform.localPosition = GetRectTransformLocalPosition(_pos, rootRect);
            }
#endif
            if (positionTween)
                item.targetPos = GetRectTransformLocalPosition(_pos, rootRect);
            else
                item.rectTransform.localPosition = GetRectTransformLocalPosition(_pos, rootRect);
        }
    }

    public void SetSpacing(Vector2 _spacing)
    {
        spacing = _spacing;
        SetGrid();
    }


}
