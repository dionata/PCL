using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCLLib.Utils
{   
    public static class screenCoordinate
    {
        //private static Point screenCoordinateMin = new Point(660 * 3, 753 * 3);
        private static Point screenCoordinateMin;        
        private static Point worldCoordinate = new Point();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="escala"></param>
        /// <param name="mouse"></param>
        /// <returns></returns>
        public static Point ToWorld(float escala, Point mouse)
        {            
            switch (GLSettings.selecionarTipoProjecao)
            {
                case (int)GLSettings.tipoProjecao.frontal:
                    switch (escala)
                    {
                        case 0.5f:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) + 195)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 274)));
                            worldCoordinate.X = (mouse.X / 2) - screenCoordinateMin.X / 2;
                            worldCoordinate.Y = -1 * ((mouse.Y / 2) - screenCoordinateMin.Y / 2);
                            break;
                        case 1:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX/2 - (((int)GLSettings.BaseX / 2) + 4)), (int)(GLSettings.panelOpenKinectY/2 - (((int)GLSettings.BaseZ / 2) - 176)));
                            worldCoordinate.X = (mouse.X * 1) - screenCoordinateMin.X * 1;
                            worldCoordinate.Y = -1 * ((mouse.Y * 1) - screenCoordinateMin.Y * 1);
                            break;
                        case 2:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 90)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 125)));
                            worldCoordinate.X = (mouse.X * 2) - screenCoordinateMin.X * 2;
                            worldCoordinate.Y = -1 * ((mouse.Y * 2) - screenCoordinateMin.Y * 2);
                            break;
                        case 4:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 138)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 101)));
                            worldCoordinate.X = (mouse.X * 4) - screenCoordinateMin.X * 4;
                            worldCoordinate.Y = -1 * ((mouse.Y * 4) - screenCoordinateMin.Y * 4);
                            break;       
                    }
                    break;
                case (int)GLSettings.tipoProjecao.fundo:
                    switch (escala)
                    {
                        case 0.5f:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) + 195)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 274)));
                            worldCoordinate.X = (mouse.X / 2) - screenCoordinateMin.X / 2;
                            worldCoordinate.Y = -1 * ((mouse.Y / 2) - screenCoordinateMin.Y / 2);
                            break;
                        case 1:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) + 4)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 176)));
                            worldCoordinate.X = (mouse.X * 1) - screenCoordinateMin.X * 1;
                            worldCoordinate.Y = -1 * ((mouse.Y * 1) - screenCoordinateMin.Y * 1);
                            break;
                        case 2:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 90)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 125)));
                            worldCoordinate.X = (mouse.X * 2) - screenCoordinateMin.X * 2;
                            worldCoordinate.Y = -1 * ((mouse.Y * 2) - screenCoordinateMin.Y * 2);
                            break;
                        case 4:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 138)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 101)));
                            worldCoordinate.X = (mouse.X * 4) - screenCoordinateMin.X * 4;
                            worldCoordinate.Y = -1 * ((mouse.Y * 4) - screenCoordinateMin.Y * 4);
                            break;
                    }
                    break;
                case (int)GLSettings.tipoProjecao.ZX:
                    switch (escala)
                    {
                        case 0.5f:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 82)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 514)));
                            worldCoordinate.X = (mouse.X / 2) - screenCoordinateMin.X / 2;
                            worldCoordinate.Y = -1 * ((mouse.Y / 2) - screenCoordinateMin.Y / 2);
                            break;
                        case 1:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 112)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 402)));
                            worldCoordinate.X = (mouse.X * 1) - screenCoordinateMin.X * 1;
                            worldCoordinate.Y = -1 * ((mouse.Y * 1) - screenCoordinateMin.Y * 1);
                            break;
                        case 2:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 148)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 238)));
                            worldCoordinate.X = (mouse.X * 2) - screenCoordinateMin.X * 2;
                            worldCoordinate.Y = -1 * ((mouse.Y * 2) - screenCoordinateMin.Y * 2);
                            break;
                        case 4:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 167)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 158)));
                            worldCoordinate.X = (mouse.X * 4) - screenCoordinateMin.X * 4;
                            worldCoordinate.Y = -1 * ((mouse.Y * 4) - screenCoordinateMin.Y * 4);
                            break;
                    }
                    break;
                case (int)GLSettings.tipoProjecao.esquerda:
                    switch (escala)
                    {
                        case 0.5f:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) + 265)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 276)));
                            worldCoordinate.X = (mouse.X / 2) - screenCoordinateMin.X / 2;
                            worldCoordinate.Y = -1 * ((mouse.Y / 2) - screenCoordinateMin.Y / 2);
                            break;
                        case 1:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) + 40)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 175)));
                            worldCoordinate.X = (mouse.X * 1) - screenCoordinateMin.X * 1;
                            worldCoordinate.Y = -1 * ((mouse.Y * 1) - screenCoordinateMin.Y * 1);
                            break;
                        case 2:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 72)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 126)));
                            worldCoordinate.X = (mouse.X * 2) - screenCoordinateMin.X * 2;
                            worldCoordinate.Y = -1 * ((mouse.Y * 2) - screenCoordinateMin.Y * 2);
                            break;
                        case 4:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 129)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 100)));
                            worldCoordinate.X = (mouse.X * 4) - screenCoordinateMin.X * 4;
                            worldCoordinate.Y = -1 * ((mouse.Y * 4) - screenCoordinateMin.Y * 4);
                            break;
                    }
                    break;
                case (int)GLSettings.tipoProjecao.direita:
                    switch (escala)
                    {
                        case 0.5f:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) + 265)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 276)));
                            worldCoordinate.X = (mouse.X / 2) - screenCoordinateMin.X / 2;
                            worldCoordinate.Y = -1 * ((mouse.Y / 2) - screenCoordinateMin.Y / 2);
                            break;
                        case 1:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) + 40)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 175)));
                            worldCoordinate.X = (mouse.X * 1) - screenCoordinateMin.X * 1;
                            worldCoordinate.Y = -1 * ((mouse.Y * 1) - screenCoordinateMin.Y * 1);
                            break;
                        case 2:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 72)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 126)));
                            worldCoordinate.X = (mouse.X * 2) - screenCoordinateMin.X * 2;
                            worldCoordinate.Y = -1 * ((mouse.Y * 2) - screenCoordinateMin.Y * 2);
                            break;
                        case 4:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 129)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 100)));
                            worldCoordinate.X = (mouse.X * 4) - screenCoordinateMin.X * 4;
                            worldCoordinate.Y = -1 * ((mouse.Y * 4) - screenCoordinateMin.Y * 4);
                            break;
                    }
                    break;
                case (int)GLSettings.tipoProjecao.superior:
                    switch (escala)
                    {
                        case 0.5f:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) + 210)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 675)));
                            worldCoordinate.X = (mouse.X / 2) - screenCoordinateMin.X / 2;
                            worldCoordinate.Y = -1 * ((mouse.Y / 2) - screenCoordinateMin.Y / 2);
                            break;
                        case 1:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) + 5)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 375)));
                            worldCoordinate.X = (mouse.X * 1) - screenCoordinateMin.X * 1;
                            worldCoordinate.Y = -1 * ((mouse.Y * 1) - screenCoordinateMin.Y * 1);
                            break;
                        case 2:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 91)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 226)));
                            worldCoordinate.X = (mouse.X * 2) - screenCoordinateMin.X * 2;
                            worldCoordinate.Y = -1 * ((mouse.Y * 2) - screenCoordinateMin.Y * 2);
                            break;
                        case 4:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 138)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 151)));
                            worldCoordinate.X = (mouse.X * 4) - screenCoordinateMin.X * 4;
                            worldCoordinate.Y = -1 * ((mouse.Y * 4) - screenCoordinateMin.Y * 4);
                            break;
                    }
                    break;
                case (int)GLSettings.tipoProjecao.inferior:
                    switch (escala)
                    {
                        case 0.5f:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) + 100)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 504)));
                            worldCoordinate.X = (mouse.X / 2) - screenCoordinateMin.X / 2;
                            worldCoordinate.Y = -1 * ((mouse.Y / 2) - screenCoordinateMin.Y / 2);
                            break;
                        case 1:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) + 5)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 375)));
                            worldCoordinate.X = (mouse.X * 1) - screenCoordinateMin.X * 1;
                            worldCoordinate.Y = -1 * ((mouse.Y * 1) - screenCoordinateMin.Y * 1);
                            break;
                        case 2:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 91)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 226)));
                            worldCoordinate.X = (mouse.X * 2) - screenCoordinateMin.X * 2;
                            worldCoordinate.Y = -1 * ((mouse.Y * 2) - screenCoordinateMin.Y * 2);
                            break;
                        case 4:
                            screenCoordinateMin = new Point(((int)GLSettings.panelOpenKinectX / 2 - (((int)GLSettings.BaseX / 2) - 138)), (int)(GLSettings.panelOpenKinectY / 2 - (((int)GLSettings.BaseZ / 2) - 151)));
                            worldCoordinate.X = (mouse.X * 4) - screenCoordinateMin.X * 4;
                            worldCoordinate.Y = -1 * ((mouse.Y * 4) - screenCoordinateMin.Y * 4);
                            break;
                    }
                    break;
            }
            return worldCoordinate;
        }

    }
}
