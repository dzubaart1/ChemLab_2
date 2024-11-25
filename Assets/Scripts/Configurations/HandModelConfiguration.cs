using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configurations
{
    public class HandModelConfiguration : Configuration
    {
        public enum HandModelType : byte
        {
            Common,
            Gloves
        }

        [Serializable]
        public struct HandModelSettings
        {
            public HandModelType HandModelType;
            public Material HandModelMaterial;
        }

        public List<HandModelSettings> HandModelSettingsList;
    }
}