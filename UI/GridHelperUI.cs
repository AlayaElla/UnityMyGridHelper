using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHelperUI : LayeroutHelperUIBase
{
    public enum ChildAligment
    {
        Upper_Left,
        Upper_Center
    }
    public ChildAligment childAligment = ChildAligment.Upper_Left;
    public Vector2 spacing = new Vector2();
    public override void SetGrid()
    {
        float current_width = 0;
        int currentCount = 0;
        float row_width = CalRowWidth(currentCount);
        Vector3 _pos = Vector3.zero;

        float offsetWidth = 0 + padding.Left;
        float offsetHeight = 0 - padding.Top;

        foreach (var item in childList)
        {
            if (item == null)
                childList.Remove(item);

            //如果超过容器长度则换行
            current_width = offsetWidth + item.rectTransform.rect.width + padding.Right;
            if (currentCount == 0 || current_width > rootRect.rect.width)
            {
                offsetHeight -= currentCount == 0 ? 0 : spacing.y;
                if (childAligment == ChildAligment.Upper_Left)
                {
                    offsetWidth = 0 + padding.Left;
                }
                else if (childAligment == ChildAligment.Upper_Center)
                {
                    row_width = CalRowWidth(currentCount);
                    offsetWidth = 0 + padding.Left + rootRect.rect.width / 2 - Mathf.Max(row_width, item.rectTransform.rect.width) / 2;
                }
            }
            _pos = new Vector3(
                offsetWidth + item.rectTransform.rect.width / 2,//x
                offsetHeight,//y
                0);
            offsetWidth += item.rectTransform.rect.width + spacing.x;
            currentCount++;

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

    float CalRowWidth(int startIndex)
    {
        float width = 0;
        for (int i = startIndex; i < childList.Count; i++)
        {
            if (width + spacing.x + childList[i].rectTransform.rect.width >= rootRect.rect.width)
                return width - spacing.x;
            width += spacing.x + childList[i].rectTransform.rect.width;
        }
        return width - spacing.x;
    }

}
