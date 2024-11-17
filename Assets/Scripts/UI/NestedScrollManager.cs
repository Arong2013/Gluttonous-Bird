using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NestedScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;
    public Transform content;

    [SerializeField] int ViewSlotCount;
    [SerializeField] private float[] pos;
    [SerializeField] private float distance, curPos, targetPos;
    [SerializeField] private bool isDrag;
    [SerializeField] private int targetIndex;

    private int SIZE;

    void Start()
    {
        SIZE = content.childCount;
        pos = new float[SIZE];
        distance = 1f / (SIZE - ViewSlotCount);

        for (int i = 0; i < SIZE; i++)
        {
            if (i - ViewSlotCount >= 0)
            {
                pos[i] = 1f;
                continue;
            }
            pos[i] = distance * i;
        }

        curPos = scrollbar.value;
        targetPos = curPos;
        targetIndex = 0;
    }

    float FindClosestPos()
    {
        float closestDistance = Mathf.Infinity;
        float closestPos = 0;
        for (int i = 0; i < SIZE; i++)
        {
            float diff = Mathf.Abs(scrollbar.value - pos[i]);
            if (diff < closestDistance)
            {
                closestDistance = diff;
                targetIndex = i;
                closestPos = pos[i];
            }
        }
        return closestPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        curPos = FindClosestPos();
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
        if(scrollbar.value >= pos[SIZE-4])
        HandleInfiniteScroll();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        targetPos = FindClosestPos();
        if (curPos == targetPos)
        {
            if (eventData.delta.x > 30 && targetIndex > 0) // Fast left drag
            {
                targetIndex--;
                targetPos = pos[targetIndex];
            }
            else if (eventData.delta.x < -30 && targetIndex < SIZE - 1) // Fast right drag
            {
                targetIndex++;
                targetPos = pos[targetIndex];
            }
        }
        
    }

    void Update()
    {
        if (!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, Time.deltaTime * 10f);

            if (Mathf.Abs(scrollbar.value - targetPos) < 0.001f)
            {
                scrollbar.value = targetPos;
            }

            int centralIndex = Mathf.Clamp(targetIndex + 2, 0, SIZE - 1);

            for (int i = 0; i < pos.Length; i++)
            {
                if (i == centralIndex)
                {
                    content.GetChild(i).localScale = Vector2.Lerp(content.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                }
                else
                {
                    content.GetChild(i).localScale = Vector2.Lerp(content.GetChild(i).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                }
            }
        }
    }

    private void HandleInfiniteScroll()
    {
        for (int i = 0; i < 4; i++)
        {
            Transform lastSlot = content.GetChild(0);
            lastSlot.SetSiblingIndex(SIZE - 1); // 맨 앞으로 이동
        }
        scrollbar.value = pos[SIZE - 8];
    }

    private void UpdatePositionArray()
    {
        for (int i = 0; i < SIZE; i++)
        {
            pos[i] = distance * i;
        }
    }
}
