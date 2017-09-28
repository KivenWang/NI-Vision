using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NationalInstruments.Vision.Acquisition.Imaqdx;

namespace LabelingVisualIdentification
{
    public class Camera//:ImaqdxSession ; 继承ImaqdxSession,没有安装相机驱动,故临时注释
    {
        private Camera(string cameraName)
            //: base(cameraName)
        {
        }
        private static Camera camera = null;
        public static Camera GetInstance(string cameraName)
        {
            if (camera==null )
            {
                camera = new Camera(cameraName);
            }
            return camera;
        }
    }
}
