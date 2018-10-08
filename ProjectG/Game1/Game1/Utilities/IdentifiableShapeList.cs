using TBAGW.Utilities.Input;
using TBAGW.Utilities.OnScreen.Particles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TBAGW.Utilities
{
    [XmlRoot("IDList")]
    [XmlInclude(typeof(Shape))]
    [XmlInclude(typeof(ScreenButton))]
    public class IdentifiableShapeList
    {
        [XmlElement("List Name")]
        public String name;

        [XmlArray("Object List")]
        [XmlArrayItem("Object")]
        public List<Object> objectList = new List<object>();

        public IdentifiableShapeList()
        {

        }

        public IdentifiableShapeList(String name, Object list)
        {
            objectList.Clear();
            this.name = name;

            IEnumerable enumerable = list as IEnumerable;
            if (enumerable != null)
            {
                foreach (Object element in enumerable)
                {
                    objectList.Add(element);
                }
            }
        }

        public bool IdentifyByName(String inputName)
        {
            if (inputName.Equals(name))
            {
                return true;
            }

            return false;
        }



    }
}
