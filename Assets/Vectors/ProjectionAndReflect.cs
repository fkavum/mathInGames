using UnityEngine;

namespace Vectors
{
    public class ProjectionAndReflect : MonoBehaviour
    {
        [SerializeField] private Arrow projectV1;
        [SerializeField] private Arrow projectV2Normal;
        [SerializeField] private Arrow projectV3;
     
        
        [SerializeField] private Arrow reflectV1InDirect;
        [SerializeField] private Arrow reflectV2InNormal;
        [SerializeField] private Arrow reflectV3;


        private void Update()
        {
            projectV3.SetAsVector(Vector3.Project(projectV1.GetVector(), projectV2Normal.GetVector()));
            reflectV3.SetAsVector(Vector3.Reflect(reflectV1InDirect.GetVector(),reflectV2InNormal.GetVector()));
        }
    }
}