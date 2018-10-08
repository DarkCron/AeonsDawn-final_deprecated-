using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace TBAGW
{
    public class UIScreenSlider : BaseUIElement
    {
        public enum UISliderType { Horizontal, Vertical }
        UISliderType sliderType { get; set; } = UISliderType.Vertical;

        public UIScreenSlider() : base() { }

        public override void Reload(UICollection uic, UIElementLayout uiel)
        {
            base.Reload(uic, uiel);
            var temp = uiel as UIScreenSliderSaveLayout;
            sliderType = temp.UIST;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

        }
    }

    [XmlRoot("UI Button Save Layout")]
    public class UIScreenSliderSaveLayout : UIElementLayout
    {
        [XmlElement("Slider type")]
        public UIScreenSlider.UISliderType UIST = UIScreenSlider.UISliderType.Vertical; 

        public UIScreenSliderSaveLayout() { }
    }
}
