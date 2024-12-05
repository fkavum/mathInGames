using System.Text;
using TMPro;
using UnityEngine;

namespace Vectors
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private Vector3 defaultDir = Vector3.up;
        [SerializeField] private TextMeshPro infoText;

        private Transform _cameraTransform;
        private readonly StringBuilder _str = new StringBuilder();

        private Transform _tm;
        private void Awake()
        {
            _tm = transform;
            _cameraTransform = Camera.main?.transform;
        }

        private void Update()
        {
            LookTowardsCamera(infoText.transform);
            _str.Clear();
            _str.Append($"{QuaternionExtensions.DirectionFromRotation(_tm.rotation, defaultDir)}");
            _str.AppendLine($"|{_tm.localScale.x:0.0}");
            // _str.AppendLine($"Rotation: {_tm.rotation.eulerAngles}");
            infoText.text = _str.ToString();
        }

        public void SetAsVector(Vector3 vector)
        {
            SetDirection(vector);
            SetMagnitude(vector.magnitude);
        }

        public void SetMagnitude(float magnitude)
        {
            _tm.localScale = Vector3.one * magnitude;
        }

        public void SetDirection(Vector3 vector)
        {
            _tm.rotation = QuaternionExtensions.RotationFromDirection(vector.normalized, defaultDir);
        }

        public Vector3 GetVector()
        {
            return QuaternionExtensions.DirectionFromRotation(_tm.rotation, defaultDir) * _tm.localScale.x;
        }

        private void LookTowardsCamera(Transform objToLook)
        {
            // Get the direction vector from the object to the camera
            Vector3 directionToCamera = objToLook.position - _cameraTransform.position;
            // Ensure the object looks at the camera without tilting upwards/downwards
            // directionToCamera.y = 0; // Optional: Lock the Y axis if you want only horizontal rotation
            // Calculate the rotation to look at the camera
            Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);
            // Apply the rotation to the object
            objToLook.rotation = lookRotation;
        }
    }
}