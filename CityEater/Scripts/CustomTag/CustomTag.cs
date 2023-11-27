using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duelit.Hole
{
    public enum Tag { VEHICLE, DANGER, BOMB }
    public class CustomTag : MonoBehaviour
    {
        public List<Tag> tags = new List<Tag>();
        public bool HasTag(string v)
        {
            foreach (Tag tg in tags)
            {
                if (tg.ToString().ToLower() == v.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
