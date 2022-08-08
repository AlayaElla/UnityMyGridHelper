using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class LayeroutHelperBase : MonoBehaviour
{
    [System.Serializable]
    public class Padding {
        public float Left = 0;
        public float Right = 0;
        public float Top = 0;
        public float Bottom = 0;
    }

    public class LayerItem {
        public RectTransform rectTransform;
        public Vector3 targetPos;
    }
    public Dictionary<int, LayerItem> childList = new Dictionary<int, LayerItem>();

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
                _pos = item.Value.rectTransform.localPosition;
                _moveX = Mathf.Lerp(_pos.x, (float)item.Value.targetPos.x, Time.deltaTime * smooting);
                _moveY = Mathf.Lerp(_pos.y, (float)item.Value.targetPos.y, Time.deltaTime * smooting);
                _pos = new Vector3(_moveX, _moveY, _pos.z);
                item.Value.rectTransform.localPosition = _pos;
            }
        }
    }

    void UpdateGrid() {
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

            var _childList = new Dictionary<int, LayerItem>(childList);
            foreach (var item in childList)
            {
                if (!_list.Contains(item.Key))
                    _childList.Remove(item.Key);
            }
            childList = _childList;
        }
        else if(transform.childCount > childList.Count)
        {
            foreach (RectTransform child in transform)
            {
                if (!childList.ContainsKey(child.GetInstanceID()))
                {
                    LayerItem item = new LayerItem();
                    item.rectTransform = child;
                    childList.Add(child.GetInstanceID(), item);
                }

            }
        }
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
