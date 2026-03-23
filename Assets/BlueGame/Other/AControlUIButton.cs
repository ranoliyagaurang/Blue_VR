using UnityEngine;
using UnityEngine.EventSystems;

namespace Shemarooverse.MobileControls
{
    public abstract class AControlUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public abstract void OnPointerUp();
        public abstract void OnPointerDown();

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUp();
        }
    }
}
