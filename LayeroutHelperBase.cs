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
        public int instanceID;
        public Transform transform;
        public Vector3 targetPos;
    }
    public List<LayerItem> childList = new List<LayerItem>();

    public Padding padding = new Padding();

    [HideInInspector]
    public Transform root;

    public bool positionTween = false;
    public float smooting = 8;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        root = transform.GetComponent<Transform>();
    }

    private Vector3 _pos;
    private float _moveX;
    private float _moveZ;
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
                _pos = item.transform.localPosition;
                _moveX = Mathf.Lerp(_pos.x, (float)item.targetPos.x, Time.deltaTime * smooting);
                _moveZ = Mathf.Lerp(_pos.z, (float)item.targetPos.z, Time.deltaTime * smooting);
                _pos = new Vector3(_moveX, _pos.y, _moveZ);
                item.transform.localPosition = _pos;
            }
        }
    }

    void UpdateGrid() {
        if (root == null)
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

            var _childList = new List<LayerItem>(childList);
            foreach (var item in childList)
            {
                if (!_list.Contains(item.instanceID))
                    _childList.Remove(item);
            }
            childList = _childList;
        }
        else if(transform.childCount > childList.Count)
        {
            foreach (Transform child in transform)
            {
                if (!CheckIsChildInList(child))
                {
                    LayerItem item = new LayerItem();
                    item.instanceID = child.GetInstanceID();
                    item.transform = child;
                    childList.Add(item);
                }

            }
        }
    }

    bool CheckIsChildInList(Transform child)
    {
        foreach (var item in childList)
        {
            if (item.transform == child)
                return true;
        }
        return false;
    }

    public virtual void SetGrid(){}

    public void SetPadding(Padding _padding)
    {
        padding = _padding;
        SetGrid();
    }

    public void OnEnable()
    {
        root = transform.GetComponent<Transform>();
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
