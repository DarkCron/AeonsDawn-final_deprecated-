using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TBAGW
{
    [XmlRoot("Class Point")]
    public class ClassPoints
    {
        [XmlElement("Class ID")]
        public int classID = -1;
        [XmlElement("Points")]
        public int points = 0;
        [XmlElement("Points spend of this class")]
        public int spendPoints = 0;

        internal BaseClass parentClass;

        public ClassPoints()
        {

        }

        public ClassPoints(BaseClass pc)
        {
            parentClass = pc;
            classID = pc.classIdentifier;
        }

        public ClassPoints(BaseClass pc, int amount)
        {
            parentClass = pc;
            classID = pc.classIdentifier;
            this.points = amount;
        }

        internal static ClassPoints Copy(BaseClass bc)
        {
            ClassPoints temp = new ClassPoints(bc);
            temp.points = bc.classPoints.points;
            temp.spendPoints = bc.classPoints.spendPoints;

            return temp;
        }

        internal static void PasteBackFromSave(ClassPoints cp, BaseClass bc)
        {
            bc.classPoints = new ClassPoints(bc);
            bc.classPoints.points = cp.points;
            bc.classPoints.spendPoints = cp.spendPoints;
        }

        internal static ClassPoints toClassPoints(LUA.LuaClassPoints lcp)
        {
            ClassPoints cp = new ClassPoints();
            cp.classID = lcp.ID;
            cp.points = lcp.amount;
            return cp;
        }

        public void Reload(GameContentDataBase gcdb)
        {
            parentClass = gcdb.gameClasses.Find(c=>c.classIdentifier == classID);
        }

        public override string ToString()
        {
            if (parentClass == null) { Reload(GameProcessor.gcDB); }
            return parentClass.ToString()+", Points: "+points;
        }
    }
}
