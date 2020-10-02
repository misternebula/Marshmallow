using System.Collections.Generic;
using UnityEngine;

namespace Marshmallow.Utility
{
    public static class MarshmallowExtensions
    {
        public static MVector3 ToMVector3(this Vector3 vector3)
        {
            return new MVector3(vector3.x, vector3.y, vector3.z);
        }

        public static T GetSetting<T>(this Dictionary<string, object> dict, string settingName)
        {
            return (T)dict[settingName];
        }
    }
}
