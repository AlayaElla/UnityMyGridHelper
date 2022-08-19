using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalHelperUI : LayeroutHelperUIBase
{
    public enum ChildAligment
    {
        UpperLeft,
        UpperCenter
    }
    public ChildAligment childAligment = ChildAligment.UpperCenter;
    public float spacing = 0;

    public override void SetGrid()
    {
        float offsetWidth = 0 + padding.Left;
        float offsetHeight = 0 - padding.Top;
        Vector3 _pos = Vector3.zero;

        foreach (var item in childList)
        {
            if (item == null)
                childList.Remove(item);

            if (childAligment == ChildAligment.UpperLeft)
            {
                _pos = new Vector3(
                    offsetWidth,//x
                    offsetHeight - item.rectTransform.rect.height / 2,//y
                    0
                    );
            }
            else if(childAligment == ChildAligment.UpperCenter)
            {
                _pos = new Vector3(
                    offsetWidth + rootRect.rect.width / 2,//x
                    offsetHeight - item.rectTransform.rect.height / 2,//y
                    0
                    );
            }
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                item.targetPos = GetRectTransformLocalPosition(_pos, rootRect);
                item.rectTransform.localPosition = GetRectTransformLocalPosition(_pos, rootRect);
            }
#endif
            if (positionTween)
                item.targetPos = GetRectTransformLocalPosition(_pos, rootRect);
            else
                item.rectTransform.localPosition = GetRectTransformLocalPosition(_pos, rootRect);
            offsetHeight -= item.rectTransform.rect.height + spacing;
        }
    }

    public void SetSpacing(float _spacing)
    {
        spacing = _spacing;
        SetGrid();
    }
}
