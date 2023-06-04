using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

[ExecuteInEditMode]
public class LayeroutHelperUIBase : MonoBehaviour
{
    [System.Serializable]
    public class Padding {
        public float Left = 0;
        public float Right = 0;
        public float Top = 0;
        public float Bottom = 0;
    }

    public class LayerItem {
        public int instanceID;
        public RectTransform rectTransform;
        public Vector3 targetPos;
    }
    public List<LayerItem> childList = new List<LayerItem>();

    public Padding padding = new Padding();

    [HideInInspector]
    public RectTransform rootRect;

    public bool positionTween = false;
    public float smooting = 8;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rootRect = transform.GetComponent<RectTransform>();
    }

    private Vector3 _pos;
    private float _moveX;
    private float _moveY;
    // Update is called once per frame
    protected virtual void Update()
    {
#if UNITY_EDITOR
        if (transform.hasChanged && !Application.isPlaying)
            UpdateGrid();
#endif
        if (Application.isPlaying && positionTween)
        {
            foreach (var item in childList)
            {
                _pos = item.rectTransform.localPosition;
                _moveX = Mathf.Lerp(_pos.x, (float)item.targetPos.x, Time.deltaTime * smooting);
                _moveY = Mathf.Lerp(_pos.y, (float)item.targetPos.y, Time.deltaTime * smooting);
                _pos = new Vector3(_moveX, _moveY, _pos.z);
                item.rectTransform.localPosition = _pos;
            }
        }
    }

    public void UpdateGrid() {
        if (rootRect == null)
            return;

        UpdateChildList();
        SetGrid();
    }

    void UpdateChildList()
    {
        if (transform.childCount < childList.Count)
        {
            List<int> _list = new List<int>();
            foreach (Transform child in transform)
            {
                _list.Add(child.GetInstanceID());
            }

            childList.RemoveAll(item => !_list.Contains(item.instanceID));
        }
        else if (transform.childCount > childList.Count)
        {
            childList.Clear();
            foreach (RectTransform child in transform)
            {
                LayerItem item = new LayerItem();
                item.instanceID = child.GetInstanceID();
                item.rectTransform = child;
                childList.Add(item);
            }
        }

        childList = childList.OrderBy(item => GetSiblingIndex(item.rectTransform)).ToList();
    }

    int GetSiblingIndex(RectTransform rectTransform)
    {
        return rectTransform.GetSiblingIndex();
    }

    public virtual void SetGrid(){}

    public void SetPadding(Padding _padding)
    {
        padding = _padding;
        SetGrid();
    }

    public Vector3 GetRectTransformLocalPosition(Vector3 pos,RectTransform rect)
    {
        return new Vector3(pos.x - rect.rect.width / 2, pos.y + rect.rect.height / 2, pos.z);
    }

    public Vector3 GetTargetPosByInstanceID(int instanceID)
    {
        LayerItem lastItem = null;
        foreach (var item in childList)
        {
            lastItem = item;
            if (item.instanceID == instanceID)
                return item.targetPos;
        }
        if (lastItem != null)
        {
            return lastItem.targetPos;
        }
        return Vector3.zero;
    }

    public void OnEnable()
    {
        rootRect = transform.GetComponent<RectTransform>();
    }

    public void OnTransformChildrenChanged()
    {
        UpdateGrid();
    }

    public void OnValidate()
    {
        UpdateGrid();
    }
}
