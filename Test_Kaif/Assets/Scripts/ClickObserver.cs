using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class ClickObserver : MonoBehaviour, IPointerClickHandler
    {
        private Action<Vector2> _onClick;
        private bool _isActive = true;

        public void SubscribeForClick(Action<Vector2> act)
        {
            _onClick += act;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isActive)
            {
                _onClick?.Invoke(eventData.position);
            }
        }

        public void SwitchCLickObserver(bool isClickAble)
        {
            _isActive = isClickAble;
        }
    }
}