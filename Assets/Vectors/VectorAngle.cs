using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Vectors
{
    public class VectorAngle : MonoBehaviour
    {
        [SerializeField] private Arrow angle1;
        [SerializeField] private Arrow angle2;
        [SerializeField] private Arrow axis;
        [SerializeField] private TextMeshPro angleResult;

        private void Update()
        {
            string angle = Vector3.Angle(angle1.GetVector(), angle2.GetVector()).ToString(CultureInfo.InvariantCulture);
            string signedAngle = Vector3.SignedAngle(angle1.GetVector(), angle2.GetVector(), axis.GetVector()).ToString(CultureInfo.InvariantCulture);
            angleResult.text = $"Angle: {angle}\nSignedAngle: {signedAngle}";
        }
    }
}