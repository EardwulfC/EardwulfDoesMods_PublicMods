namespace ComfyLib;

using System;

using UnityEngine;
using UnityEngine.EventSystems;

public sealed class PanelDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
  RectTransform _rectTransform;
  Vector2 _lastMousePosition;

  public EventHandler<Vector2> OnEndDragEvent;

  void Awake() {
    _rectTransform = GetComponent<RectTransform>();
  }

  public void OnBeginDrag(PointerEventData eventData) {
    _lastMousePosition = eventData.position;
  }

  public void OnDrag(PointerEventData eventData) {
    Vector2 difference = eventData.position - _lastMousePosition;
    _rectTransform.position += new Vector3(difference.x, difference.y, 0f);
    _lastMousePosition = eventData.position;
  }

  public void OnEndDrag(PointerEventData eventData) {
    OnEndDragEvent?.Invoke(this, _rectTransform.anchoredPosition);
  }
}