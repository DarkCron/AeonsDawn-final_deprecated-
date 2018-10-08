using TBAGW.Utilities.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TBAGW.Utilities.ReadWrite
{
    [XmlRoot("Test")]
    public class TestSerialize
    {
        [XmlElement("Position")]
        public Vector2 pos = new Vector2(5,89);

        [XmlArray("All the testObjects")]
        [XmlArrayItem("Test Object")]
        public List<TestSerialize> tests = new List<TestSerialize>();

        [XmlIgnore]
        public Texture2D tex;
        public TestSerialize()
        {
        }

        public TestSerialize(Vector2 pos)
        {
            this.pos = pos;
        }

        public void testMeth()
        {
            Console.Out.WriteLine(pos);
        }


    }
}
