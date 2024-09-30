using BioEngineerLab.Core;
using UnityEngine;

namespace BioEngineerLab.Configurations
{
    public class InputConfiguration : Configuration
    {
        public ETargetPlatform Platform = ETargetPlatform.Auto;
        public LocalRig LocalRigTemplate;
        public Vector3 PlayerSpawnPoint;
        public Transform BallPrefab;
    }
}
