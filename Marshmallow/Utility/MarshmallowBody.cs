using Marshmallow.External;
using UnityEngine;

namespace Marshmallow.Utility
{
    public class MarshmallowBody
    {
        public MarshmallowBody(IPlanetConfig config)
        {
            Config = config;
        }

        public IPlanetConfig Config;

        public GameObject Object;
    }
}
