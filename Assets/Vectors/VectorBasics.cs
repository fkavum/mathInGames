using System.Globalization;
using TMPro;
using UnityEngine;

namespace Vectors
{
    public class VectorBasics : MonoBehaviour
    {
        [SerializeField] private Arrow dotV1;
        [SerializeField] private Arrow dotV2;
        [SerializeField] private TextMeshPro dotText;
        
        
        [SerializeField] private Arrow crossV1;
        [SerializeField] private Arrow crossV2;
        [SerializeField] private Arrow crossV3;
        
        //reflect
        //project

        public void Update()
        {
            dotText.text = Vector3.Dot(dotV1.GetVector(), dotV2.GetVector()).ToString(CultureInfo.InvariantCulture);
            crossV3.SetAsVector(Vector3.Cross(crossV1.GetVector(), crossV2.GetVector()));
        }
        
    }
}