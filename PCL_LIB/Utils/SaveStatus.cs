using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCLLib.Utils
{   
    public static class SaveStatus
    {
        /// <summary>
        /// 
        /// </summary>
        public static string ModeloAuxAutoSave = GLSettings.locateTMP + GLSettings.ModeloAux;
                 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OpenGLControl"></param>
        /// <param name="tipo"></param>
        public static void ConfirmChanges(OpenGLControl OpenGLControl, string tipo)
        {
            if(Model3D.vetRot_port.X > 0 || Model3D.vetRot_port.Y > 0 || Model3D.vetRot_port.Z > 0 && OpenGLControl.modelsListSelect > -1)
            { 
                Models.Model3DAUX.Save_OBJ(OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1], GLSettings.locateTMP, GLSettings.ModeloAux);
                DMesh3 mesh = IO.GeneralIO.LoadtMesh(ModeloAuxAutoSave);
                DMesh3 mesh2 = new DMesh3(mesh);

                Vector3f vector3F = new Vector3f(0, 0, 0);
                Vector3f vector3F_ = new Vector3f(0, 0, 0);            

                /******************************************/
                vector3F.x = 1;
                vector3F.y = 0;
                vector3F.z = 0;
           
                MeshTransforms.Rotate(mesh2, vector3F_, Quaternionf.AxisAngleD(vector3F, ((float)Model3D.vetRot_port.X * 57.29578f)));

                /******************************************/

                vector3F.x = 0;
                vector3F.y = 1;
                vector3F.z = 0;
 
                MeshTransforms.Rotate(mesh2, vector3F_, Quaternionf.AxisAngleD(vector3F, ((float)Model3D.vetRot_port.Y * 57.29578f)));

                /*****************************************/

                vector3F.x = 0;
                vector3F.y = 0;
                vector3F.z = 1;
      
                MeshTransforms.Rotate(mesh2, vector3F_, Quaternionf.AxisAngleD(vector3F, ((float)Model3D.vetRot_port.Z * 57.29578f)));
                DMesh3 mesh3 = new DMesh3(mesh2);
                MeshTransforms.Translate(mesh3, Model3D.vetTransl_port.X, Model3D.vetTransl_port.Y, Model3D.vetTransl_port.Z);

                IO.GeneralIO.SaveMesh(mesh3, GLSettings.locateTMP + GLSettings.ModeloAuxOut_);

                GeneralTools tools = new GeneralTools();
                tools.atualizarStatusMarcacao(OpenGLControl, "salvar");

                if (tipo == "confirmar")
                {
                    Model3D.vetRot_port.X = -1 * Model3D.vetRot_port.X;
                    Model3D.vetRot_port.Y = -1 * Model3D.vetRot_port.Y;
                    Model3D.vetRot_port.Z = -1 * Model3D.vetRot_port.Z;
                    Model3D.vetRot_portDesfazer.Add(Model3D.vetRot_port);
                }

                OpenGLControl.RefreshShowModels(OpenGLControl.modelsListSelect - 1, "Wireframe", "*", GLSettings.ModeloAuxOut_);
                tools.atualizarStatusMarcacao(OpenGLControl, "atualizar");
            }
        }
    }
}
