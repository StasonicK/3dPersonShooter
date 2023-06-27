using Cinemachine;
using UnityEngine;

namespace CodeBase.Hero
{
    public class Aiming : MonoBehaviour
    {
        [SerializeField] private AxisState _xAxis;
        [SerializeField] private AxisState _yAxis;
        [SerializeField] private Transform _cameraFollowPosition;

        private void Update()
        {
            _xAxis.Update(Time.deltaTime);
            _yAxis.Update(Time.deltaTime);
        }

        private void LateUpdate()
        {
            _cameraFollowPosition.localEulerAngles = new Vector3(_yAxis.Value, _cameraFollowPosition.localEulerAngles.y,
                _cameraFollowPosition.localEulerAngles.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, _xAxis.Value, transform.eulerAngles.z);
        }
    }
}