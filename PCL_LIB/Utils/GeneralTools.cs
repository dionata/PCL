/*
 * 
 */
 
using g3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCLLib.IO;

namespace PCLLib.Utils
{
    public class GeneralTools
    {
        #region parametros
        public string ModeloAux = GLSettings.locateTMP + GLSettings.ModeloAux;
        private string ModeloAux2 = GLSettings.locateTMP + GLSettings.ModeloAux2;
        public string ModeloAuxTringuleGerado = GLSettings.locateTMP + "auxTringuleGerado.obj";

        public string Bloco1Out_Tringulos = GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos;
        public string Bloco2Out_Tringulos = GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos;
        public string Bloco3Out_Tringulos = GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos;
        public string Bloco4Out_Tringulos = GLSettings.locateTMP + GLSettings.Bloco4Out_Tringulos;
        public string Bloco5Out_Tringulos = GLSettings.locateTMP + GLSettings.Bloco5Out_Tringulos;

        public string Bloco1Out_Tringulos_Desbaste = GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos_Desbaste;
        public string Bloco2Out_Tringulos_Desbaste = GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos_Desbaste;
        public string Bloco3Out_Tringulos_Desbaste = GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos_Desbaste;
        public string Bloco4Out_Tringulos_Desbaste = GLSettings.locateTMP + GLSettings.Bloco4Out_Tringulos_Desbaste;
        public string Bloco5Out_Tringulos_Desbaste = GLSettings.locateTMP + GLSettings.Bloco5Out_Tringulos_Desbaste;
        public string Desbaste_Complemento_1 = GLSettings.locateTMP + GLSettings.Desbaste_Complemento1;
        public string Desbaste_Complemento_2 = GLSettings.locateTMP + GLSettings.Desbaste_Complemento2;
        public string Desbaste_Complemento_3 = GLSettings.locateTMP + GLSettings.Desbaste_Complemento3;
        public string Desbaste_Complemento_4 = GLSettings.locateTMP + GLSettings.Desbaste_Complemento4;
        public string Desbaste_Complemento_5 = GLSettings.locateTMP + GLSettings.Desbaste_Complemento5;

        public string Bloco1Out_Tringulos_Tampa = GLSettings.locateTMP + GLSettings.Bloco1Out_Tringulos_Tampa;
        public string Bloco2Out_Tringulos_Tampa = GLSettings.locateTMP + GLSettings.Bloco2Out_Tringulos_Tampa;
        public string Bloco3Out_Tringulos_Tampa = GLSettings.locateTMP + GLSettings.Bloco3Out_Tringulos_Tampa;
        public string Bloco4Out_Tringulos_Tampa = GLSettings.locateTMP + GLSettings.Bloco4Out_Tringulos_Tampa;
        public string Bloco5Out_Tringulos_Tampa = GLSettings.locateTMP + GLSettings.Bloco5Out_Tringulos_Tampa;

        private int corter_select = -1;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        /// <summary>
        /// Método que realiza o corte da malha seguindo a marcação pré-definida
        /// </summary>
        public void cutmeshX()
        {
            DMesh3 mesh = IO.GeneralIO.LoadtMesh(ModeloAux);
            int contBloco = 0;                       

            double mine, maxe, avge;         

            Vector3d origin;//Vector3d origin = bounds.Center;
            Vector3d axis = Vector3d.AxisX;// AxisY;

            /* Cópia do arquivo original para ser 
             * utilizado para recorte do assento
             */ 
            DMesh3 blocoAssento = new DMesh3();
            blocoAssento.Copy(mesh);
            MeshQueries.EdgeLengthStats(blocoAssento, out mine, out maxe, out avge);
            AxisAlignedBox3d boundsAssento = blocoAssento.CachedBounds;

            do {
                if(contBloco == 0)
                {                   
                    DMesh3 bloco01 = new DMesh3();
                    bloco01.Copy(mesh);
                    MeshQueries.EdgeLengthStats(bloco01, out mine, out maxe, out avge);
                    AxisAlignedBox3d bounds = bloco01.CachedBounds;

                    //Cortes de perspectivaConsolidacao projeto Tecpost  X
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco1XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco1YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco1ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(1.00000000, 0.00000000, 0.00000000);

                    MeshPlaneCut cut = new MeshPlaneCut(bloco01, origin, axis);
                    cut.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(-1.00000000, 0.00000000, 0.00000000);

                    MeshPlaneCut cut2 = new MeshPlaneCut(bloco01, origin, axis);
                    cut2.Cut();

                    /****************************/
                    //Cortes de perspectivaConsolidacao projeto Tecpost Y
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco1XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco1YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco1ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                    MeshPlaneCut cut3 = new MeshPlaneCut(bloco01, origin, axis);
                    cut3.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, -1.00000000, 0.00000000);

                    MeshPlaneCut cut4 = new MeshPlaneCut(bloco01, origin, axis);
                    cut4.Cut();

                    /****************************/
                    //Separação referente ao Assento
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                    MeshPlaneCut cutAssento = new MeshPlaneCut(blocoAssento, origin, axis);
                    cutAssento.Cut();

                    /****************************/
                    //Cortes de perspectivaConsolidacao projeto Tecpost Z
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco1XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco1YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco1ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 0.00000000, 1.00000000);

                    MeshPlaneCut cut5 = new MeshPlaneCut(bloco01, origin, axis);
                    cut5.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 0.00000000, -1.00000000);

                    MeshPlaneCut cut6 = new MeshPlaneCut(bloco01, origin, axis);
                    cut6.Cut();

                    if(GLSettings.separarEncostoAssento)
                    {
                        IO.GeneralIO.SaveMesh(bloco01, GLSettings.locateTMP + GLSettings.Encosto);
                        File.Copy(GLSettings.locateTMP + GLSettings.Encosto, GLSettings.locateMALHA + GLSettings.Encosto);
                        IO.GeneralIO.SaveMesh(blocoAssento, GLSettings.locateTMP + GLSettings.Assento);
                        File.Copy(GLSettings.locateTMP + GLSettings.Assento, GLSettings.locateMALHA + GLSettings.Assento);
                    }
                    else
                    {
                        IO.GeneralIO.SaveMesh(bloco01, GLSettings.locateTMP + GLSettings.Bloco1Out_);
                        File.Copy(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateMALHA + GLSettings.Bloco1Out_);
                    }                   
                }
                else if(contBloco == 1)
                {
                    DMesh3 bloco02 = new DMesh3();
                    bloco02.Copy(mesh);
                    MeshQueries.EdgeLengthStats(bloco02, out mine, out maxe, out avge);
                    AxisAlignedBox3d bounds = bloco02.CachedBounds;

                    //Cortes de perspectivaConsolidacao projeto Tecpost  X

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco2XEncostoFinish + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco2YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco2ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(1.00000000, 0.00000000, 0.00000000);

                    MeshPlaneCut cut = new MeshPlaneCut(bloco02, origin, axis);
                    cut.Cut();

                    origin = new Vector3d((Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco2XEncostoInit + Model3D.vetTransl_port.X), Model3D.refTranslMarcacaoCorte_port.Y 
                    + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(-1.00000000, 0.00000000, 0.00000000);

                    MeshPlaneCut cut2 = new MeshPlaneCut(bloco02, origin, axis);
                    cut2.Cut();

                    /****************************/
                    //Cortes de perspectivaConsolidacao projeto Tecpost Y
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco2XEncosto + Model3D.vetTransl_port.X, -Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco2YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco2ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                    MeshPlaneCut cut3 = new MeshPlaneCut(bloco02, origin, axis);
                    cut3.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, -Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, -1.00000000, 0.00000000);

                    MeshPlaneCut cut4 = new MeshPlaneCut(bloco02, origin, axis);
                    cut4.Cut();

                    /****************************/
                    //Separação referente ao Assento
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                    MeshPlaneCut cutAssento = new MeshPlaneCut(blocoAssento, origin, axis);
                    cutAssento.Cut();

                    /****************************/
                    //Cortes de perspectivaConsolidacao projeto Tecpost Z
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco2XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco2YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco2ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 0.00000000, 1.00000000);

                    MeshPlaneCut cut5 = new MeshPlaneCut(bloco02, origin, axis);
                    cut5.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 0.00000000, -1.00000000);

                    MeshPlaneCut cut6 = new MeshPlaneCut(bloco02, origin, axis);
                    cut6.Cut();

                    MeshTransforms.Translate(bloco02, -GLSettings.Bloco2XEncostoInit, 0, 0);

                    IO.GeneralIO.SaveMesh(bloco02, GLSettings.locateTMP + GLSettings.Bloco2Out_);
                    File.Copy(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateMALHA + GLSettings.Bloco2Out_);
                }
                else if(contBloco == 2)
                {                    
                    DMesh3 bloco03 = new DMesh3();
                    bloco03.Copy(mesh);
                    MeshQueries.EdgeLengthStats(bloco03, out mine, out maxe, out avge);
                    AxisAlignedBox3d bounds = bloco03.CachedBounds;

                    //Cortes de perspectivaConsolidacao projeto Tecpost  X
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco3XEncostoFinish + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco3YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco3ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(1.00000000, 0.00000000, 0.00000000);

                    MeshPlaneCut cut = new MeshPlaneCut(bloco03, origin, axis);
                    cut.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco3XEncostoInit + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(-1.00000000, 0.00000000, 0.00000000);

                    MeshPlaneCut cut2 = new MeshPlaneCut(bloco03, origin, axis);
                    cut2.Cut();

                    /****************************/
                    //Cortes de perspectivaConsolidacao projeto Tecpost Y
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco3XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco3YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco3ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                    MeshPlaneCut cut3 = new MeshPlaneCut(bloco03, origin, axis);
                    cut3.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, -1.00000000, 0.00000000);

                    MeshPlaneCut cut4 = new MeshPlaneCut(bloco03, origin, axis);
                    cut4.Cut();

                    /****************************/
                    //Separação referente ao Assento
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                    MeshPlaneCut cutAssento = new MeshPlaneCut(blocoAssento, origin, axis);
                    cutAssento.Cut();

                    /****************************/
                    //Cortes de perspectivaConsolidacao projeto Tecpost Z
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco3XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco3YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco3ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 0.00000000, 1.00000000);

                    MeshPlaneCut cut5 = new MeshPlaneCut(bloco03, origin, axis);
                    cut5.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 0.00000000, -1.00000000);

                    MeshPlaneCut cut6 = new MeshPlaneCut(bloco03, origin, axis);
                    cut6.Cut();

                    MeshTransforms.Translate(bloco03, -GLSettings.Bloco3XEncostoInit, 0, 0);

                    IO.GeneralIO.SaveMesh(bloco03, GLSettings.locateTMP + GLSettings.Bloco3Out_);
                    File.Copy(GLSettings.locateTMP + GLSettings.Bloco3Out_, GLSettings.locateMALHA + GLSettings.Bloco3Out_);
                }
                else if (contBloco == 3)
                {
                    DMesh3 bloco04 = new DMesh3();
                    bloco04.Copy(mesh);
                    MeshQueries.EdgeLengthStats(bloco04, out mine, out maxe, out avge);
                    AxisAlignedBox3d bounds = bloco04.CachedBounds;

                    //Cortes de perspectivaConsolidacao projeto Tecpost  X
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco4XEncostoFinish + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco4YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco4ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(1.00000000, 0.00000000, 0.00000000);

                    MeshPlaneCut cut = new MeshPlaneCut(bloco04, origin, axis);
                    cut.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco4XEncostoInit + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(-1.00000000, 0.00000000, 0.00000000);

                    MeshPlaneCut cut2 = new MeshPlaneCut(bloco04, origin, axis);
                    cut2.Cut();

                    /****************************/
                    //Cortes de perspectivaConsolidacao projeto Tecpost Y
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco4XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco4YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco4ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                    MeshPlaneCut cut3 = new MeshPlaneCut(bloco04, origin, axis);
                    cut3.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, -1.00000000, 0.00000000);

                    MeshPlaneCut cut4 = new MeshPlaneCut(bloco04, origin, axis);
                    cut4.Cut();

                    /****************************/
                    //Separação referente ao Assento
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                    MeshPlaneCut cutAssento = new MeshPlaneCut(blocoAssento, origin, axis);
                    cutAssento.Cut();

                    /****************************/
                    //Cortes de perspectivaConsolidacao projeto Tecpost Z
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco5XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco3YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco3ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 0.00000000, 1.00000000);

                    MeshPlaneCut cut5 = new MeshPlaneCut(bloco04, origin, axis);
                    cut5.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 0.00000000, -1.00000000);

                    MeshPlaneCut cut6 = new MeshPlaneCut(bloco04, origin, axis);
                    cut6.Cut();

                    MeshTransforms.Translate(bloco04, -GLSettings.Bloco4XEncostoInit, 0, 0);

                    IO.GeneralIO.SaveMesh(bloco04, GLSettings.locateTMP + GLSettings.Bloco4Out_);
                    File.Copy(GLSettings.locateTMP + GLSettings.Bloco4Out_, GLSettings.locateMALHA + GLSettings.Bloco4Out_);
                }
                else if (contBloco == 4)
                {
                    DMesh3 bloco05 = new DMesh3();
                    //bloco05.Copy(mesh);
                    MeshQueries.EdgeLengthStats(bloco05, out mine, out maxe, out avge);
                    AxisAlignedBox3d bounds = bloco05.CachedBounds;

                    //Cortes de perspectivaConsolidacao projeto Tecpost  X
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco5XEncostoFinish + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco5YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco5ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(1.00000000, 0.00000000, 0.00000000);

                    MeshPlaneCut cut = new MeshPlaneCut(bloco05, origin, axis);
                    cut.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco5XEncostoInit + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                    + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(-1.00000000, 0.00000000, 0.00000000);

                    MeshPlaneCut cut2 = new MeshPlaneCut(bloco05, origin, axis);
                    cut2.Cut();

                    /****************************/
                    //Cortes de perspectivaConsolidacao projeto Tecpost Y
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco5XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco5YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco5ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                    MeshPlaneCut cut3 = new MeshPlaneCut(bloco05, origin, axis);
                    cut3.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, -1.00000000, 0.00000000);

                    MeshPlaneCut cut4 = new MeshPlaneCut(bloco05, origin, axis);
                    cut4.Cut();

                    /****************************/
                    //Separação referente ao Assento
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                    MeshPlaneCut cutAssento = new MeshPlaneCut(blocoAssento, origin, axis);
                    cutAssento.Cut();

                    /****************************/
                    //Cortes de perspectivaConsolidacao projeto Tecpost Z
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco5XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y 
                    + GLSettings.Bloco5YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco5ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 0.00000000, 1.00000000);

                    MeshPlaneCut cut5 = new MeshPlaneCut(bloco05, origin, axis);
                    cut5.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y, 
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 0.00000000, -1.00000000);

                    MeshPlaneCut cut6 = new MeshPlaneCut(bloco05, origin, axis);
                    cut6.Cut();

                    MeshTransforms.Translate(bloco05, -GLSettings.Bloco5XEncostoInit, 0, 0);
                    IO.GeneralIO.SaveMesh(bloco05, GLSettings.locateTMP + GLSettings.Bloco5Out_);
                    File.Copy(GLSettings.locateTMP + GLSettings.Bloco5Out_, GLSettings.locateMALHA + GLSettings.Bloco5Out_);                    
                }

                contBloco++;
            } while (GLSettings.numberDivblocoExecutado > contBloco);                     
        }

        /// <summary>
        /// Método que realiza o corte da malha seguindo a marcação pré-definida 
        /// </summary>
        public void cutmeshZ(string path, bool desbaste, bool base_, int initContBloco)
        {
            DMesh3 mesh;

            if (path == "")
            {
                mesh = IO.GeneralIO.LoadtMesh(ModeloAux);
            }
           else
            {
                mesh = IO.GeneralIO.LoadtMesh(path);
            }
            int contBloco = initContBloco;

            double mine, maxe, avge;

            Vector3d origin;//Vector3d origin = bounds.Center;
            Vector3d axis = Vector3d.AxisZ;// AxisX;// AxisY;

            /* Cópia do arquivo original para ser 
             * utilizado para recorte do assento
             */
            DMesh3 blocoAssento = new DMesh3();
            blocoAssento.Copy(mesh);
            MeshQueries.EdgeLengthStats(blocoAssento, out mine, out maxe, out avge);
            AxisAlignedBox3d boundsAssento = blocoAssento.CachedBounds;

            do
            {
                if (contBloco == 0)
                {
                    DMesh3 bloco01 = new DMesh3();
                    bloco01.Copy(mesh);
                    MeshQueries.EdgeLengthStats(bloco01, out mine, out maxe, out avge);
                    AxisAlignedBox3d bounds = bloco01.CachedBounds;

                    //blank enable
                    if (GLSettings.MainBlock)
                    {
                        
                        //Cortes de perspectivaConsolidacao projeto Tecpost  X
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco1XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                        + GLSettings.Bloco1YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco1ZEncosto + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(1.00000000, 0.00000000, 0.00000000);

                        MeshPlaneCut cut = new MeshPlaneCut(bloco01, origin, axis);
                        cut.Cut();
                        
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                        Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(-1.00000000, 0.00000000, 0.00000000);

                        MeshPlaneCut cut2 = new MeshPlaneCut(bloco01, origin, axis);
                        cut2.Cut();
                  
                        //Cortes de perspectivaConsolidacao projeto Tecpost Y
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco1XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                        + GLSettings.Bloco1YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco1ZEncosto + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                        MeshPlaneCut cut3 = new MeshPlaneCut(bloco01, origin, axis);
                        cut3.Cut();
                        
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                        Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, -1.00000000, 0.00000000);

                        MeshPlaneCut cut4 = new MeshPlaneCut(bloco01, origin, axis);
                        cut4.Cut();
                      
                        //Separação referente ao Assento
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                        Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                        MeshPlaneCut cutAssento = new MeshPlaneCut(blocoAssento, origin, axis);
                        cutAssento.Cut();                        

                        //Cortes de perspectivaConsolidacao projeto Tecpost Z
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco1XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                        + GLSettings.Bloco1YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco1ZEncosto + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, 0.00000000, 1.00000000);

                        MeshPlaneCut cut5 = new MeshPlaneCut(bloco01, origin, axis);
                        cut5.Cut();
                       
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                        Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, 0.00000000, -1.00000000);

                        MeshPlaneCut cut6 = new MeshPlaneCut(bloco01, origin, axis);
                        cut6.Cut();                        
                    }
                    else
                    {
                        //Cortes de perspectivaConsolidacao projeto Tecpost Z
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco1XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                        + GLSettings.Bloco1YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco1ZEncosto + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, 0.00000000, 1.00000000);

                        MeshPlaneCut cut5 = new MeshPlaneCut(bloco01, origin, axis);
                        cut5.Cut();
                    }

                    if (GLSettings.separarEncostoAssento)
                    {
                        IO.GeneralIO.SaveMesh(bloco01, GLSettings.locateTMP + GLSettings.Encosto);
                        File.Copy(GLSettings.locateTMP + GLSettings.Encosto, GLSettings.locateMALHA + GLSettings.Encosto);
                        IO.GeneralIO.SaveMesh(blocoAssento, GLSettings.locateTMP + GLSettings.Assento);
                        File.Copy(GLSettings.locateTMP + GLSettings.Assento, GLSettings.locateMALHA + GLSettings.Assento);
                    }
                    else
                    {
                        if(desbaste)
                        {
                            IO.GeneralIO.SaveMesh(bloco01, GLSettings.locateTMP + GLSettings.Bloco1InvOut_);
                            File.Copy(GLSettings.locateTMP + GLSettings.Bloco1InvOut_, GLSettings.locateMALHA + GLSettings.Bloco1InvOut_);
                        }
                        else
                        {
                            IO.GeneralIO.SaveMesh(bloco01, GLSettings.locateTMP + GLSettings.Bloco1Out_);
                            File.Copy(GLSettings.locateTMP + GLSettings.Bloco1Out_, GLSettings.locateMALHA + GLSettings.Bloco1Out_);
                        }                       
                    }
                }
                else if (contBloco == 1)
                {
                    DMesh3 bloco02 = new DMesh3();
                    bloco02.Copy(mesh);
                    MeshQueries.EdgeLengthStats(bloco02, out mine, out maxe, out avge);
                    AxisAlignedBox3d bounds = bloco02.CachedBounds;

                    //Cortes de perspectivaConsolidacao projeto Tecpost  X

                    //blank enable
                    if (GLSettings.MainBlock)
                    {
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco2XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                        + GLSettings.Bloco2YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco2ZEncosto + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(1.00000000, 0.00000000, 0.00000000);

                        MeshPlaneCut cut = new MeshPlaneCut(bloco02, origin, axis);
                        cut.Cut();
                        
                        origin = new Vector3d((Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X), Model3D.refTranslMarcacaoCorte_port.Y
                        + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(-1.00000000, 0.00000000, 0.00000000);

                        MeshPlaneCut cut2 = new MeshPlaneCut(bloco02, origin, axis);
                        cut2.Cut();
                        
                        //Cortes de perspectivaConsolidacao projeto Tecpost Y
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco2XEncosto + Model3D.vetTransl_port.X, -Model3D.refTranslMarcacaoCorte_port.Y
                        + GLSettings.Bloco2YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco2ZEncosto + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                        MeshPlaneCut cut3 = new MeshPlaneCut(bloco02, origin, axis);
                        cut3.Cut();
                      
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, -Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                        Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, -1.00000000, 0.00000000);

                        MeshPlaneCut cut4 = new MeshPlaneCut(bloco02, origin, axis);
                        cut4.Cut();
                       
                        //Separação referente ao Assento
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                        Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                        MeshPlaneCut cutAssento = new MeshPlaneCut(blocoAssento, origin, axis);
                        cutAssento.Cut();

                        //Cortes de perspectivaConsolidacao projeto Tecpost Z
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco2XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                        + GLSettings.Bloco2YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco2ZEncostoFinish + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, 0.00000000, 1.00000000);

                        MeshPlaneCut cut5 = new MeshPlaneCut(bloco02, origin, axis);
                        cut5.Cut();
                       
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                        Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco2ZEncostoInit + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, 0.00000000, -1.00000000);

                        MeshPlaneCut cut6 = new MeshPlaneCut(bloco02, origin, axis);
                        cut6.Cut();
                    }
                    else
                    {
                        if (base_)
                        {    
                            origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                            Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco2ZEncostoInit + Model3D.vetTransl_port.Z);
                            axis = new Vector3d(0.00000000, 0.00000000, -1.00000000);

                            MeshPlaneCut cut6 = new MeshPlaneCut(bloco02, origin, axis);
                            cut6.Cut();

                            IO.GeneralIO.SaveMesh(bloco02, GLSettings.locateTMP + GLSettings.Bloco2InvTMP_);
                            File.Copy(GLSettings.locateTMP + GLSettings.Bloco2InvTMP_, GLSettings.locateMALHA + GLSettings.Bloco2InvTMP_);

                            break;
                        }
                        else
                        {
                            if(!desbaste)
                            {
                                origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                                 Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco2ZEncostoInit + Model3D.vetTransl_port.Z);
                                axis = new Vector3d(0.00000000, 0.00000000, -1.00000000);

                                MeshPlaneCut cut2 = new MeshPlaneCut(bloco02, origin, axis);
                                cut2.Cut();
                            }

                            origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco2XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                            + GLSettings.Bloco2YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco2ZEncosto + Model3D.vetTransl_port.Z);
                            axis = new Vector3d(1.00000000, 0.00000000, 0.00000000);

                            MeshPlaneCut cut = new MeshPlaneCut(bloco02, origin, axis);
                            cut.Cut();                  
                        }
                     
                    }
                    //MeshTransforms.Translate(bloco02, -GLSettings.Bloco2XEncostoInit, 0, 0);

                    if (desbaste)
                    {
                        IO.GeneralIO.SaveMesh(bloco02, GLSettings.locateTMP + GLSettings.Bloco2InvOut_);
                        File.Copy(GLSettings.locateTMP + GLSettings.Bloco2InvOut_, GLSettings.locateMALHA + GLSettings.Bloco2InvOut_);
                    }
                    else
                    {
                        IO.GeneralIO.SaveMesh(bloco02, GLSettings.locateTMP + GLSettings.Bloco2Out_);
                        File.Copy(GLSettings.locateTMP + GLSettings.Bloco2Out_, GLSettings.locateMALHA + GLSettings.Bloco2Out_);
                    }
                }
                else if (contBloco == 2)
                {
                    DMesh3 bloco03 = new DMesh3();
                    bloco03.Copy(mesh);
                    MeshQueries.EdgeLengthStats(bloco03, out mine, out maxe, out avge);
                    AxisAlignedBox3d bounds = bloco03.CachedBounds;

                    if (GLSettings.MainBlock)
                    {                        
                        //Cortes de perspectivaConsolidacao projeto Tecpost  X
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco2XEncosto + GLSettings.Bloco2XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                        + GLSettings.Bloco3YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco3ZEncosto + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(1.00000000, 0.00000000, 0.00000000);

                        MeshPlaneCut cut = new MeshPlaneCut(bloco03, origin, axis);
                        cut.Cut();
                        
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco2XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                        + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(-1.00000000, 0.00000000, 0.00000000);

                        MeshPlaneCut cut2 = new MeshPlaneCut(bloco03, origin, axis);
                        cut2.Cut();
                        
                        //Cortes de perspectivaConsolidacao projeto Tecpost Y
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco3XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                        + GLSettings.Bloco3YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco3ZEncosto + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                        MeshPlaneCut cut3 = new MeshPlaneCut(bloco03, origin, axis);
                        cut3.Cut();
                       
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                        Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, -1.00000000, 0.00000000);

                        MeshPlaneCut cut4 = new MeshPlaneCut(bloco03, origin, axis);
                        cut4.Cut();
                        
                        //Separação referente ao Assento
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                        Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                        MeshPlaneCut cutAssento = new MeshPlaneCut(blocoAssento, origin, axis);
                        cutAssento.Cut();
                        
                        //Cortes de perspectivaConsolidacao projeto Tecpost Z
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco3XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                        + GLSettings.Bloco3YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco2ZEncostoFinish + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, 0.00000000, 1.00000000);

                        MeshPlaneCut cut5 = new MeshPlaneCut(bloco03, origin, axis);
                        cut5.Cut();
                        
                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                        Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco2ZEncostoInit + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(0.00000000, 0.00000000, -1.00000000);

                        MeshPlaneCut cut6 = new MeshPlaneCut(bloco03, origin, axis);
                        cut6.Cut();

                    }
                    else
                    {
                        if (!desbaste)
                        {
                            origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                            Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco2ZEncostoInit + Model3D.vetTransl_port.Z);
                            axis = new Vector3d(0.00000000, 0.00000000, -1.00000000);

                            MeshPlaneCut cut3 = new MeshPlaneCut(bloco03, origin, axis);
                            cut3.Cut();
                        }

                        origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + (GLSettings.Bloco1XEncosto - GLSettings.Bloco3XEncosto) + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                       + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                        axis = new Vector3d(-1.00000000, 0.00000000, 0.00000000);

                        MeshPlaneCut cut2 = new MeshPlaneCut(bloco03, origin, axis);
                        cut2.Cut();
                    }

                    MeshTransforms.Translate(bloco03, -GLSettings.Bloco2XEncosto, 0, 0);

                    if (desbaste)
                    {
                        IO.GeneralIO.SaveMesh(bloco03, GLSettings.locateTMP + GLSettings.Bloco3InvOut_);
                        File.Copy(GLSettings.locateTMP + GLSettings.Bloco3InvOut_, GLSettings.locateMALHA + GLSettings.Bloco3InvOut_);
                    }
                    else
                    {
                        IO.GeneralIO.SaveMesh(bloco03, GLSettings.locateTMP + GLSettings.Bloco3Out_);
                        File.Copy(GLSettings.locateTMP + GLSettings.Bloco3Out_, GLSettings.locateMALHA + GLSettings.Bloco3Out_);
                    }
                }

                //TODO ver este último bloco para blank
                else if (contBloco == 3)
                {
                    DMesh3 bloco04 = new DMesh3();
                    bloco04.Copy(mesh);
                    MeshQueries.EdgeLengthStats(bloco04, out mine, out maxe, out avge);
                    AxisAlignedBox3d bounds = bloco04.CachedBounds;

                    //Cortes de perspectivaConsolidacao projeto Tecpost  X
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco4XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                    + GLSettings.Bloco4YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco4ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(1.00000000, 0.00000000, 0.00000000);

                    MeshPlaneCut cut = new MeshPlaneCut(bloco04, origin, axis);
                    cut.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                    + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(-1.00000000, 0.00000000, 0.00000000);

                    MeshPlaneCut cut2 = new MeshPlaneCut(bloco04, origin, axis);
                    cut2.Cut();

                    /****************************/
                    //Cortes de perspectivaConsolidacao projeto Tecpost Y
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco4XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                    + GLSettings.Bloco4YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco4ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                    MeshPlaneCut cut3 = new MeshPlaneCut(bloco04, origin, axis);
                    cut3.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, -1.00000000, 0.00000000);

                    MeshPlaneCut cut4 = new MeshPlaneCut(bloco04, origin, axis);
                    cut4.Cut();

                    /****************************/
                    //Separação referente ao Assento
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                    MeshPlaneCut cutAssento = new MeshPlaneCut(blocoAssento, origin, axis);
                    cutAssento.Cut();

                    /****************************/
                    //Cortes de perspectivaConsolidacao projeto Tecpost Z
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco5XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                    + GLSettings.Bloco3YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco3ZEncostoFinish + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 0.00000000, 1.00000000);

                    MeshPlaneCut cut5 = new MeshPlaneCut(bloco04, origin, axis);
                    cut5.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                    Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco3ZEncostoInit + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 0.00000000, -1.00000000);

                    MeshPlaneCut cut6 = new MeshPlaneCut(bloco04, origin, axis);
                    cut6.Cut();

                    //MeshTransforms.Translate(bloco04, -GLSettings.Bloco4XEncostoInit, 0, 0);

                    IO.GeneralIO.SaveMesh(bloco04, GLSettings.locateTMP + GLSettings.Bloco4Out_);
                    File.Copy(GLSettings.locateTMP + GLSettings.Bloco4Out_, GLSettings.locateMALHA + GLSettings.Bloco4Out_);
                }
                else if (contBloco == 4)
                {
                    DMesh3 bloco05 = new DMesh3();
                    bloco05.Copy(mesh);
                    MeshQueries.EdgeLengthStats(bloco05, out mine, out maxe, out avge);
                    AxisAlignedBox3d bounds = bloco05.CachedBounds;

                    //Cortes de perspectivaConsolidacao projeto Tecpost  X
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco4XEncosto + GLSettings.Bloco4XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                    + GLSettings.Bloco5YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco5ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(1.00000000, 0.00000000, 0.00000000);

                    MeshPlaneCut cut = new MeshPlaneCut(bloco05, origin, axis);
                    cut.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco4XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                    + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(-1.00000000, 0.00000000, 0.00000000);

                    MeshPlaneCut cut2 = new MeshPlaneCut(bloco05, origin, axis);
                    cut2.Cut();

                    /****************************/
                    //Cortes de perspectivaConsolidacao projeto Tecpost Y
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco5XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                    + GLSettings.Bloco5YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco5ZEncosto + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                    MeshPlaneCut cut3 = new MeshPlaneCut(bloco05, origin, axis);
                    cut3.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, -1.00000000, 0.00000000);

                    MeshPlaneCut cut4 = new MeshPlaneCut(bloco05, origin, axis);
                    cut4.Cut();

                    /****************************/
                    //Separação referente ao Assento
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                    Model3D.refTranslMarcacaoCorte_port.Z + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 1.00000000, 0.00000000);

                    MeshPlaneCut cutAssento = new MeshPlaneCut(blocoAssento, origin, axis);
                    cutAssento.Cut();

                    /****************************/
                    //Cortes de perspectivaConsolidacao projeto Tecpost Z
                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + GLSettings.Bloco5XEncosto + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y
                    + GLSettings.Bloco5YEncosto + Model3D.vetTransl_port.Y, Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco3ZEncostoFinish + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 0.00000000, 1.00000000);

                    MeshPlaneCut cut5 = new MeshPlaneCut(bloco05, origin, axis);
                    cut5.Cut();

                    origin = new Vector3d(Model3D.refTranslMarcacaoCorte_port.X + Model3D.vetTransl_port.X, Model3D.refTranslMarcacaoCorte_port.Y + Model3D.vetTransl_port.Y,
                    Model3D.refTranslMarcacaoCorte_port.Z + GLSettings.Bloco3ZEncostoInit + Model3D.vetTransl_port.Z);
                    axis = new Vector3d(0.00000000, 0.00000000, -1.00000000);

                    MeshPlaneCut cut6 = new MeshPlaneCut(bloco05, origin, axis);
                    cut6.Cut();

                    MeshTransforms.Translate(bloco05, -GLSettings.Bloco4XEncosto, 0, 0);

                    IO.GeneralIO.SaveMesh(bloco05, GLSettings.locateTMP + GLSettings.Bloco5Out_);
                    File.Copy(GLSettings.locateTMP + GLSettings.Bloco5Out_, GLSettings.locateMALHA + GLSettings.Bloco5Out_);
                }               

                contBloco++;
            } while (GLSettings.numberDivblocoExecutado > contBloco);
        }


        /// <summary>
        /// 
        /// </summary>
        public void ReverseTriOrientation()
        {
            DMesh3 mesh = IO.GeneralIO.LoadtMesh(ModeloAux);
            mesh.ReverseOrientation();
            IO.GeneralIO.SaveMesh(mesh, GLSettings.locateTMP + GLSettings.ModeloAuxOut_);
        }

        /// <summary>
        /// 
        /// </summary>
        public void GenerationTriangle_corte_X()
        {           
            int contBLoco = 0;

            GLSettings.zComplemento.Clear();

            if (GLSettings.numberDivblocoExecutado == 0)
            {                
                DMesh3 mesh1 = new DMesh3();
                mesh1 = IO.GeneralIO.LoadtMesh(GLSettings.locateTMP + GLSettings.ModeloAux);
                GenerationTriangle_(mesh1, Bloco1Out_Tringulos, GLSettings.Bloco1XEncosto, GLSettings.Bloco1YEncosto, GLSettings.Bloco1ZEncosto, 1, 0);
                GenerationTriangle_desbaste(mesh1, Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1XEncosto, GLSettings.Bloco1YEncosto, GLSettings.Bloco1ZEncosto, 0);                
                //GenerationTriangle_desgaste_complemento(mesh1, Desbaste_Complemento_1, 0, GLSettings.Bloco1XEncosto, GLSettings.Bloco1YEncosto, GLSettings.Bloco1ZEncosto);
            }
            else
            { 
                do
                {
                    if (contBLoco == 0)
                    {
                       DMesh3 mesh1 = new DMesh3();                      

                       mesh1 = IO.GeneralIO.LoadtMesh(GLSettings.locateTMP + GLSettings.Bloco1Out_);
                       GenerationTriangle_(mesh1, Bloco1Out_Tringulos, GLSettings.Bloco1XEncosto, GLSettings.Bloco1YEncosto, GLSettings.Bloco1ZEncosto, 1,0);                   
                       GenerationTriangle_desbaste(mesh1, Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1XEncosto, GLSettings.Bloco1YEncosto, GLSettings.Bloco1ZEncosto, 0);                   
                       //GenerationTriangle_desgaste_complemento(mesh1, Desbaste_Complemento_1, 0, GLSettings.Bloco1XEncosto, GLSettings.Bloco1YEncosto, GLSettings.Bloco1ZEncosto);
                    }
                    else if (contBLoco == 1)
                    {
                        DMesh3 mesh2 = new DMesh3();                        
                    
                        mesh2 = IO.GeneralIO.LoadtMesh(GLSettings.locateTMP + GLSettings.Bloco2Out_);
                        GenerationTriangle_(mesh2, Bloco2Out_Tringulos, GLSettings.Bloco2XEncosto, GLSettings.Bloco2YEncosto, GLSettings.Bloco2ZEncosto,  2, GLSettings.Bloco1ZEncosto);
                        GenerationTriangle_desbaste(mesh2, Bloco2Out_Tringulos_Desbaste, GLSettings.Bloco2XEncosto, GLSettings.Bloco2YEncosto, GLSettings.Bloco2ZEncosto, GLSettings.Bloco1ZEncosto);                    
                        //GenerationTriangle_desgaste_complemento(mesh2, Desbaste_Complemento_2, GLSettings.Bloco1XEncosto, GLSettings.Bloco2XEncosto, GLSettings.Bloco2YEncosto, GLSettings.Bloco2ZEncosto);
                    }
                    else if (contBLoco == 2)
                    {
                        DMesh3 mesh3 = new DMesh3();                    
                        
                        mesh3 = IO.GeneralIO.LoadtMesh(GLSettings.locateTMP + GLSettings.Bloco3Out_);                 
                        GenerationTriangle_(mesh3, Bloco3Out_Tringulos, GLSettings.Bloco3XEncosto, GLSettings.Bloco3YEncosto, GLSettings.Bloco3ZEncosto, 3, GLSettings.Bloco1ZEncosto);
                        GenerationTriangle_desbaste(mesh3, Bloco3Out_Tringulos_Desbaste, GLSettings.Bloco3XEncosto, GLSettings.Bloco3YEncosto, GLSettings.Bloco3ZEncosto, GLSettings.Bloco1ZEncosto);                     
                        //GenerationTriangle_desgaste_complemento(mesh3, Desbaste_Complemento_3, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto,  GLSettings.Bloco3XEncosto, GLSettings.Bloco3YEncosto, GLSettings.Bloco3ZEncosto);
                    }
                    else if (contBLoco == 3)
                    {
                        DMesh3 mesh4 = new DMesh3();
                        
                        mesh4 = IO.GeneralIO.LoadtMesh(GLSettings.locateTMP + GLSettings.Bloco4Out_);                       
                        GenerationTriangle_(mesh4, Bloco4Out_Tringulos, GLSettings.Bloco4XEncosto, GLSettings.Bloco4YEncosto, GLSettings.Bloco4ZEncosto, 4, GLSettings.Bloco2ZEncosto);
                        GenerationTriangle_desbaste(mesh4, Bloco4Out_Tringulos_Desbaste, GLSettings.Bloco4XEncosto, GLSettings.Bloco4YEncosto, GLSettings.Bloco4ZEncosto, GLSettings.Bloco2ZEncosto);
                        //GenerationTriangle_desgaste_complemento(mesh4, Desbaste_Complemento_4, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto, GLSettings.Bloco4XEncosto, GLSettings.Bloco4YEncosto, GLSettings.Bloco4ZEncosto);
                    }
                    else if (contBLoco == 4)
                    {
                        DMesh3 mesh5 = new DMesh3();
                        
                        mesh5 = IO.GeneralIO.LoadtMesh(GLSettings.locateTMP + GLSettings.Bloco5Out_);          
                        GenerationTriangle_(mesh5, Bloco5Out_Tringulos, GLSettings.Bloco5XEncosto, GLSettings.Bloco5YEncosto, GLSettings.Bloco5ZEncosto, 5, GLSettings.Bloco3ZEncosto);
                        GenerationTriangle_desbaste(mesh5, Bloco5Out_Tringulos_Desbaste, GLSettings.Bloco5XEncosto, GLSettings.Bloco5YEncosto, GLSettings.Bloco5ZEncosto, GLSettings.Bloco3ZEncosto);
                        //GenerationTriangle_desgaste_complemento(mesh5, Desbaste_Complemento_5, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto + GLSettings.Bloco4XEncosto, GLSettings.Bloco5XEncosto, GLSettings.Bloco5YEncosto, GLSettings.Bloco5ZEncosto);
                    }
                    contBLoco++;

                } while (GLSettings.numberDivblocoExecutado > contBLoco);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void GenerationTriangle_corte_Z()
        {           
            int contBLoco = 0;

            GLSettings.zComplemento.Clear();

            if (GLSettings.numberDivblocoExecutado == 0)
            {                
                DMesh3 mesh1 = new DMesh3();
                mesh1 = IO.GeneralIO.LoadtMesh(GLSettings.locateTMP + GLSettings.ModeloAux);
                GenerationTriangle_(mesh1, Bloco1Out_Tringulos, GLSettings.Bloco1XEncosto, GLSettings.Bloco1YEncosto, GLSettings.Bloco1ZEncosto, 11, 0);
                GenerationTriangle_desbaste(mesh1, Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1XEncosto, GLSettings.Bloco1YEncosto, GLSettings.Bloco1ZEncosto, 0);                
                //GenerationTriangle_desgaste_complemento(mesh1, Desbaste_Complemento_1, 0, GLSettings.Bloco1XEncosto, GLSettings.Bloco1YEncosto, GLSettings.Bloco1ZEncosto);
            }
            else
            { 
                do
                {
                    if (contBLoco == 0)
                    {
                       DMesh3 mesh1 = new DMesh3();                      

                       mesh1 = IO.GeneralIO.LoadtMesh(GLSettings.locateTMP + GLSettings.Bloco1Out_);
                       GenerationTriangle_(mesh1, Bloco1Out_Tringulos, GLSettings.Bloco1XEncosto, GLSettings.Bloco1YEncosto, GLSettings.Bloco1ZEncosto, 11, 0);                   
                       GenerationTriangle_desbaste(mesh1, Bloco1Out_Tringulos_Desbaste, GLSettings.Bloco1XEncosto, GLSettings.Bloco1YEncosto, GLSettings.Bloco1ZEncosto, 0);                   
                       //GenerationTriangle_desgaste_complemento(mesh1, Desbaste_Complemento_1, 0, GLSettings.Bloco1XEncosto, GLSettings.Bloco1YEncosto, GLSettings.Bloco1ZEncosto);
                    }
                    else if (contBLoco == 1)
                    {
                        DMesh3 mesh2 = new DMesh3();                        
                    
                        mesh2 = IO.GeneralIO.LoadtMesh(GLSettings.locateTMP + GLSettings.Bloco2Out_);                     
                        MeshTransforms.Rotate(mesh2, mesh2.CachedBounds.Center, Quaternionf.AxisAngleD(Vector3f.AxisY, -90.0f));
                        
                        GeneralIO.SaveMesh(mesh2, GLSettings.locateTMP + GLSettings.Bloco2Out_, true, true, true);

                        GenerationTriangle_(mesh2, Bloco2Out_Tringulos, GLSettings.Bloco2XEncosto, GLSettings.Bloco2YEncosto, GLSettings.Bloco2ZEncosto, 12, 0);
                        GenerationTriangle_desbaste(mesh2, Bloco2Out_Tringulos_Desbaste, GLSettings.Bloco2XEncosto, GLSettings.Bloco2YEncosto, GLSettings.Bloco2ZEncosto, 0);                    
                        //GenerationTriangle_desgaste_complemento(mesh2, Desbaste_Complemento_2, GLSettings.Bloco1XEncosto, GLSettings.Bloco2XEncosto, GLSettings.Bloco2YEncosto, GLSettings.Bloco2ZEncosto);
                    }
                    else if (contBLoco == 2)
                    {
                        DMesh3 mesh3 = new DMesh3();                    
                        
                        mesh3 = IO.GeneralIO.LoadtMesh(GLSettings.locateTMP + GLSettings.Bloco3Out_);
                        MeshTransforms.Rotate(mesh3, mesh3.CachedBounds.Center, Quaternionf.AxisAngleD(Vector3f.AxisY.Normalized, 90.0f));
                        GeneralIO.SaveMesh(mesh3, GLSettings.locateTMP + GLSettings.Bloco3Out_, true, true, true);
                        GenerationTriangle_(mesh3, Bloco3Out_Tringulos, GLSettings.Bloco3XEncosto, GLSettings.Bloco3YEncosto, GLSettings.Bloco3ZEncosto, 13, 0);
                        GenerationTriangle_desbaste(mesh3, Bloco3Out_Tringulos_Desbaste, GLSettings.Bloco3XEncosto, GLSettings.Bloco3YEncosto, GLSettings.Bloco3ZEncosto, 0);                     
                        //GenerationTriangle_desgaste_complemento(mesh3, Desbaste_Complemento_3, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto,  GLSettings.Bloco3XEncosto, GLSettings.Bloco3YEncosto, GLSettings.Bloco3ZEncosto);
                    }
                    else if (contBLoco == 3)
                    {
                        DMesh3 mesh4 = new DMesh3();
                        
                        mesh4 = IO.GeneralIO.LoadtMesh(GLSettings.locateTMP + GLSettings.Bloco4Out_);                       
                        GenerationTriangle_(mesh4, Bloco4Out_Tringulos, GLSettings.Bloco4XEncosto, GLSettings.Bloco4YEncosto, GLSettings.Bloco4ZEncosto, 14, 0);
                        GenerationTriangle_desbaste(mesh4, Bloco4Out_Tringulos_Desbaste, GLSettings.Bloco4XEncosto, GLSettings.Bloco4YEncosto, GLSettings.Bloco4ZEncosto, 0);
                        //GenerationTriangle_desgaste_complemento(mesh4, Desbaste_Complemento_4, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto, GLSettings.Bloco4XEncosto, GLSettings.Bloco4YEncosto, GLSettings.Bloco4ZEncosto);
                    }
                    else if (contBLoco == 4)
                    {
                        DMesh3 mesh5 = new DMesh3();
                        
                        mesh5 = IO.GeneralIO.LoadtMesh(GLSettings.locateTMP + GLSettings.Bloco5Out_);          
                        GenerationTriangle_(mesh5, Bloco5Out_Tringulos, GLSettings.Bloco5XEncosto, GLSettings.Bloco5YEncosto, GLSettings.Bloco5ZEncosto, 15, 0);
                        GenerationTriangle_desbaste(mesh5, Bloco5Out_Tringulos_Desbaste, GLSettings.Bloco5XEncosto, GLSettings.Bloco5YEncosto, GLSettings.Bloco5ZEncosto, 0);
                        //GenerationTriangle_desgaste_complemento(mesh5, Desbaste_Complemento_5, GLSettings.Bloco1XEncosto + GLSettings.Bloco2XEncosto + GLSettings.Bloco3XEncosto + GLSettings.Bloco4XEncosto, GLSettings.Bloco5XEncosto, GLSettings.Bloco5YEncosto, GLSettings.Bloco5ZEncosto);
                    }
                    contBLoco++;

                } while (GLSettings.numberDivblocoExecutado > contBLoco);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void createDirectory()
        {
            /*
            if (!Directory.Exists(GLSettings.locateTMP))
            {
                Directory.CreateDirectory(GLSettings.locateTMP);
            }
            else
            {
                Directory.Delete(GLSettings.locateTMP, true);
                Directory.CreateDirectory(GLSettings.locateTMP);
            }
            */
        }

        /// <summary>
        /// 
        /// </summary>
        public void createDirectoryInit()
        {
            if (!Directory.Exists(GLSettings.locateTMP))
            {
                Directory.CreateDirectory(GLSettings.locateTMP);
            }

            if (!Directory.Exists(GLSettings.locateCAD))
            {
                Directory.CreateDirectory(GLSettings.locateCAD);
            }

            if (!Directory.Exists(GLSettings.locateCAM))
            {
                Directory.CreateDirectory(GLSettings.locateCAM);
            }

            if (!Directory.Exists(GLSettings.locateSTL))
            {
                Directory.CreateDirectory(GLSettings.locateSTL);
            }

            if (!Directory.Exists(GLSettings.locateMALHA))
            {
                Directory.CreateDirectory(GLSettings.locateMALHA);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="path"></param>
        /// <param name="x_"></param>
        /// <param name="y_"></param>
        /// <param name="z_"></param>
        /// <param name="numberBlock"></param>
        public void GenerationTriangle_(DMesh3 mesh, string path, double x_, double y_, double z_, int numberBlock, double z_ref)
        {            
            Vector3d vertexCurrent = new Vector3d();
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            string[] nameBlock = path.Split('\\');

            double offsetAlturaZ = 10000;

            //FileStream fs_tampa = new FileStream(tampa, FileMode.Create);
            //StreamWriter sw_tampa = new StreamWriter(fs_tampa);

            if (numberBlock > 11)
            {
                foreach (var valor_ in mesh.Vertices())
                {
                    if (valor_.z < offsetAlturaZ)
                    {
                        offsetAlturaZ = valor_.z;
                    }
                }
            }

            MeshBoundaryLoops loops = new MeshBoundaryLoops(mesh);
            Debug.Assert(loops.Loops.Count > 0);

            double countPoints = 0;
            Vector3d centroideX0Y0 = new Vector3d();
            double coeficinteAngular;

            double xmax, xmin;
            double ymax, ymin;
            double zmax, zmin;
            xmax = x_ + Model3D.refTranslMarcacaoCorte_port.X;
            ymax = y_ + Model3D.refTranslMarcacaoCorte_port.Y;
            zmax = z_ + Model3D.refTranslMarcacaoCorte_port.Z;
            xmin = Model3D.refTranslMarcacaoCorte_port.X;
            ymin = Model3D.refTranslMarcacaoCorte_port.Y;
            zmin = Model3D.refTranslMarcacaoCorte_port.Z;
            double zMinBloco = 100000;       

            sw.Flush();

            int valor = 0;

            /* Pega a maior contorno para fazer o bloco
             * os pequenos contornos seriam buracos na malha 
             * estes buracos seram corrigidos posteriomente
             */

            foreach (EdgeLoop loop in loops.Loops)
            {
                if (loop.EdgeCount > valor)
                {
                    valor = loop.EdgeCount;
                }
            }

            /*
            * Pega o menor z do bloco
            */
            foreach (EdgeLoop loop in loops.Loops)
            {
                for (int v2 = 0; v2 < loop.VertexCount; v2++)
                {
                    vertexCurrent = loop.GetVertex(v2);
                    if (vertexCurrent.z < zMinBloco)
                    {
                        zMinBloco = vertexCurrent.z;
                    }
                }
            }

            foreach (EdgeLoop loop in loops.Loops)
            {
                if (loop.EdgeCount < valor) continue;

                #region método novo
                AxisAlignedBox3d e = loop.GetBounds();
                centroideX0Y0.x = e.Center.x;
                centroideX0Y0.y = e.Center.y;
                centroideX0Y0.z = e.Center.z;

                //m = y2 - y1 / x2 - x1
                //ymax - y0 = m (xmax - x0)

                for (int v2 = 0; v2 < loop.VertexCount; v2++)
                {
                    vertexCurrent = loop.GetVertex(v2);
                    coeficinteAngular = ((vertexCurrent.y - centroideX0Y0.y) / (vertexCurrent.x - centroideX0Y0.x));

                    if (vertexCurrent.x > centroideX0Y0.x)  //xmax
                    {
                        double py = centroideX0Y0.y + coeficinteAngular * (xmax - centroideX0Y0.x);

                        if (py > ymax) //ymax
                        {
                            if (corter_select == -1 || corter_select == 2)
                            {
                                double px = centroideX0Y0.x + (ymax - centroideX0Y0.y) / coeficinteAngular;

                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos.obj") //auxModelo.obj
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + zMinBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + zMinBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + px + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + px + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + px + " " + ymax + " " + zmin);
                                    sw.WriteLine("v " + px + " " + ymax + " " + zmin);
                                }
                                corter_select = 2;
                            }
                            else
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);  
                                if(nameBlock[3] != "Bloco1Out_Tringulos.obj") //auxModelo.obj
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + zMinBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + zMinBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + xmax + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + xmax + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + xmax + " " + ymax + " " + zmin);//+ z_ref);
                                    sw.WriteLine("v " + xmax + " " + ymax + " " + zmin);//+ z_ref);
                                }
                                corter_select = 2;
                            }
                            countPoints += 3;
                        }
                        else if (py < ymin) //ymin
                        {
                            if (corter_select == -1 || corter_select == 3)
                            {
                                double px = centroideX0Y0.x + (ymin - centroideX0Y0.y) / coeficinteAngular;

                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + zMinBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + zMinBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + px + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + px + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + px + " " + ymin + " " + zmin);//+ z_ref);
                                    sw.WriteLine("v " + px + " " + ymin + " " + zmin);//+ z_ref);
                                }
                                corter_select = 3;
                            }
                            else
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos.obj") //auxModelo.obj
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + zMinBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + zMinBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + xmax + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + xmax + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + xmax + " " + ymin + " " + zmin);//+ z_ref);
                                    sw.WriteLine("v " + xmax + " " + ymin + " " + zmin);//+ z_ref);
                                }
                                corter_select = 3;
                            }

                            countPoints += 3;
                        }
                        else
                        {
                            if (corter_select == -1 || corter_select == 0)
                            {
                                
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos.obj") //auxModelo.obj
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + py + " " + zMinBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + py + " " + zMinBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + xmax + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + xmax + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + xmax + " " + py + " " + zmin);//+ z_ref);
                                    sw.WriteLine("v " + xmax + " " + py + " " + zmin);//+ z_ref);
                                }
                                corter_select = 0;
                            }
                            else
                            {
                                if (corter_select == 2)
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    if (nameBlock[3] != "Bloco1Out_Tringulos.obj")
                                    {
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + zMinBloco);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + zMinBloco);
                                    }
                                    else
                                    {
                                        sw.WriteLine("vn " + xmax + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + xmax + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + xmax + " " + ymax + " " + zmin);//+ z_ref);
                                        sw.WriteLine("v " + xmax + " " + ymax + " " + zmin);//+ z_ref);
                                    }
                                    corter_select = 0;
                                }
                                else
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    if (nameBlock[3] != "Bloco1Out_Tringulos.obj")
                                    {
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + zMinBloco);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + zMinBloco);
                                    }
                                    else
                                    {
                                        sw.WriteLine("vn " + xmax + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + xmax + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + xmax + " " + ymin + " " + zmin);//+ z_ref);
                                        sw.WriteLine("v " + xmax + " " + ymin + " " + zmin);//+ z_ref);
                                    }
                                    corter_select = 0;
                                }

                            }
                            countPoints += 3;
                        }
                    }
                    else //xmin
                    {
                        //double py = centroideX0Y0.y + coeficinteAngular * (vertexCurrent.x - centroideX0Y0.x);
                        double py = centroideX0Y0.y + coeficinteAngular * (xmin - centroideX0Y0.x);

                        if (py > ymax) //ymax
                        {
                            double px = centroideX0Y0.x + (ymax - centroideX0Y0.y) / coeficinteAngular;

                            if (corter_select == -1 || corter_select == 2)
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + zMinBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + zMinBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + px + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + px + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + px + " " + ymax + " " + zmin);//+ z_ref);
                                    sw.WriteLine("v " + px + " " + ymax + " " + zmin);//+ z_ref);
                                }
                                corter_select = 2;
                            }

                            else
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + zMinBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + zMinBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + xmin + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + xmin + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + xmin + " " + ymax + " " + zmin);//+ z_ref);
                                    sw.WriteLine("v " + xmin + " " + ymax + " " + zmin);//+ z_ref);
                                }
                                corter_select = 2;
                            }

                            countPoints += 3;
                        }
                        else if (py < ymin) //ymin
                        {
                            double px = centroideX0Y0.x + (ymin - centroideX0Y0.y) / coeficinteAngular;

                            if (corter_select == -1 || corter_select == 3)
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + zMinBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + zMinBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + px + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + px + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + px + " " + ymin + " " + zmin);//+ z_ref);
                                    sw.WriteLine("v " + px + " " + ymin + " " + zmin);//+ z_ref);
                                }
                                corter_select = 3;
                            }
                            else
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + zMinBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + zMinBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + xmin + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + xmin + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + xmin + " " + ymin + " " + zmin);//+ z_ref);
                                    sw.WriteLine("v " + xmin + " " + ymin + " " + zmin);//+ z_ref);
                                }
                                corter_select = 3;
                            }
                            countPoints += 3;
                        }
                        else
                        {
                            if (corter_select == -1 || corter_select == 0)
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + py + " " + zMinBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + py + " " + zMinBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + xmin + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + xmin + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + xmin + " " + py + " " + zmin);//+ z_ref);
                                    sw.WriteLine("v " + xmin + " " + py + " " + zmin);//+ z_ref);
                                }
                                corter_select = 0;
                            }
                            else
                            {
                                if (corter_select == 2)
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    if (nameBlock[3] != "Bloco1Out_Tringulos.obj")
                                    {
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + zMinBloco);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + zMinBloco);
                                    }
                                    else
                                    {
                                        sw.WriteLine("vn " + xmin + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + xmin + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + xmin + " " + ymax + " " + zmin);//+ z_ref);
                                        sw.WriteLine("v " + xmin + " " + ymax + " " + zmin);//+ z_ref);
                                    }
                                    corter_select = 0;
                                }
                                else
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    if (nameBlock[3] != "Bloco1Out_Tringulos.obj")
                                    {
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + zMinBloco);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + zMinBloco);
                                    }
                                    else
                                    {
                                        sw.WriteLine("vn " + xmin + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + xmin + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + xmin + " " + ymin + " " + zmin);//+ z_ref);
                                        sw.WriteLine("v " + xmin + " " + ymin + " " + zmin);//+ z_ref);
                                    }
                                    corter_select = 0;
                                }

                            }
                            countPoints += 3;
                        }
                    }
                }

                //debug
                //sw.Dispose();
                //fs.Dispose();
                //sw.Close();
                //fs.Close();

                #endregion método novo

                // ligação dos triangulos 02 ok 
                int pontoFinal = 0;
                int tampaPontos = 0;

                for (int v2 = tampaPontos; v2 < (countPoints - 3); v2 += 3)
                {

                    sw.WriteLine("f " + (v2 + 5).ToString() + "//" + (v2 + 5).ToString() + " " + (v2 + 2).ToString() + "//" + (v2 + 2).ToString() + " " + (v2 + 1).ToString() + "//" + (v2 + 1).ToString());
                    sw.WriteLine("f " + (v2 + 4).ToString() + "//" + (v2 + 4).ToString() + " " + (v2 + 5).ToString() + "//" + (v2 + 5).ToString() + " " + (v2 + 1).ToString() + "//" + (v2 + 1).ToString());
                    sw.WriteLine("f " + (v2 + 5).ToString() + "//" + (v2 + 5).ToString() + " " + (v2 + 3).ToString() + "//" + (v2 + 3).ToString() + " " + (v2 + 2).ToString() + "//" + (v2 + 2).ToString());
                    sw.WriteLine("f " + (v2 + 5).ToString() + "//" + (v2 + 5).ToString() + " " + (v2 + 6).ToString() + "//" + (v2 + 6).ToString() + " " + (v2 + 3).ToString() + "//" + (v2 + 3).ToString());

                    pontoFinal = v2;
                }

                //pontoFinal = pontoFinal;

                sw.WriteLine("f " + (tampaPontos + 1).ToString() + "//" + (tampaPontos + 1).ToString() + " " + (tampaPontos + 2).ToString() + "//" + (tampaPontos + 2).ToString() + " " + (pontoFinal + 5).ToString() + "//" + (pontoFinal + 5).ToString());
                sw.WriteLine("f " + (tampaPontos + 1).ToString() + "//" + (tampaPontos + 1).ToString() + " " + (pontoFinal + 5).ToString() + "//" + (pontoFinal + 5).ToString() + " " + (pontoFinal + 4).ToString() + "//" + (pontoFinal + 4).ToString());
                sw.WriteLine("f " + (tampaPontos + 2).ToString() + "//" + (tampaPontos + 2).ToString() + " " + (tampaPontos + 3).ToString() + "//" + (tampaPontos + 3).ToString() + " " + (pontoFinal + 5).ToString() + "//" + (pontoFinal + 5).ToString());
                sw.WriteLine("f " + (pontoFinal + 5).ToString() + "//" + (pontoFinal + 5).ToString() + " " + (tampaPontos + 3).ToString() + "//" + (tampaPontos + 3).ToString() + " " + (pontoFinal + 6).ToString() + "//" + (pontoFinal + 6).ToString());
            }

            sw.Dispose();
            fs.Dispose();
            sw.Close();
            fs.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        private double calcularZmaxModelo(DMesh3 mesh)
        {
            MeshBoundaryLoops loops = new MeshBoundaryLoops(mesh);
            Debug.Assert(loops.Loops.Count > 0);
            Vector3d vertexCurrent = new Vector3d();
            double zMaxModelo = 0;

            foreach (EdgeLoop loop in loops.Loops)
            {
                //Ordenador função 
                for (int v2 = 0; v2 < loop.VertexCount; v2++)
                {
                    vertexCurrent = loop.GetVertex(v2);
                    if (vertexCurrent.z > zMaxModelo)
                    {
                        zMaxModelo = vertexCurrent.z;
                    }
                }
            }

            return zMaxModelo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="path"></param>
        /// <param name="x_init_"></param>
        /// <param name="x_"></param>
        /// <param name="y_"></param>
        /// <param name="z_"></param>
        public void GenerationTriangle_desgaste_complemento(DMesh3 mesh, string path, double x_init_,  double x_, double y_, double z_)
        {                                          
            double[] cornerIndex = new double[5];         
                       
            List<Vector3d> ordenar = new List<Vector3d>();
            List<Vector3d> ordenar_Aux = new List<Vector3d>();
            List<Vector3d> corner = new List<Vector3d>();

            double zMaxModelo  = calcularZmaxModelo(mesh);

            double x, xInit;
            double y, yInit;
            double z, zInit;
           
            x = x_init_ + x_ + Model3D.refTranslMarcacaoCorte_port.X;
            y = y_ + Model3D.refTranslMarcacaoCorte_port.Y;
            z = z_ + Model3D.refTranslMarcacaoCorte_port.Z;

            xInit = x_init_ + Model3D.refTranslMarcacaoCorte_port.X;
            yInit = Model3D.refTranslMarcacaoCorte_port.Y;
            zInit = Model3D.refTranslMarcacaoCorte_port.Z;
          
            File.Delete(path);

            GLSettings.zComplemento.Add(z - zMaxModelo);

            if ((GLSettings.zComplemento[GLSettings.zComplemento.Count -1]) > 0)
            { 
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine("vn " + (0).ToString() + " " + (0).ToString() + " " + (0).ToString());
                sw.WriteLine("v " + xInit.ToString() + " " + yInit.ToString() + " " + z.ToString());
                sw.WriteLine("vn " + (0).ToString() + " " + (0).ToString() + " " + (0).ToString());
                sw.WriteLine("v " + x.ToString() + " " + yInit.ToString() + " " + z.ToString());
                sw.WriteLine("vn " + (0).ToString() + " " + (0).ToString() + " " + (0).ToString());
                sw.WriteLine("v " + x.ToString() + " " + y.ToString() + " " + z.ToString());
                sw.WriteLine("vn " + (0).ToString() + " " + (0).ToString() + " " + (0).ToString());
                sw.WriteLine("v " + xInit.ToString() + " " + y.ToString() + " " + z.ToString());

                sw.WriteLine("vn " + (0).ToString() + " " + (0).ToString() + " " + (0).ToString());
                sw.WriteLine("v " + xInit.ToString() + " " + yInit.ToString() + " " + zMaxModelo.ToString());
                sw.WriteLine("vn " + (0).ToString() + " " + (0).ToString() + " " + (0).ToString());
                sw.WriteLine("v " + x.ToString() + " " + yInit.ToString() + " " + zMaxModelo.ToString());
                sw.WriteLine("vn " + (0).ToString() + " " + (0).ToString() + " " + (0).ToString());
                sw.WriteLine("v " + x.ToString() + " " + y.ToString() + " " + zMaxModelo.ToString());
                sw.WriteLine("vn " + (0).ToString() + " " + (0).ToString() + " " + (0).ToString());
                sw.WriteLine("v " + xInit.ToString() + " " + y.ToString() + " " + zMaxModelo.ToString());

                sw.WriteLine("f " + (1).ToString() + "//" + (1).ToString() + " " + (2).ToString() + "//" + (2).ToString() + " " + (4).ToString() + "//" + (4).ToString());
                sw.WriteLine("f " + (4).ToString() + "//" + (4).ToString() + " " + (2).ToString() + "//" + (2).ToString() + " " + (3).ToString() + "//" + (3).ToString());
                sw.WriteLine("f " + (5).ToString() + "//" + (5).ToString() + " " + (7).ToString() + "//" + (7).ToString() + " " + (6).ToString() + "//" + (6).ToString());
                sw.WriteLine("f " + (7).ToString() + "//" + (7).ToString() + " " + (5).ToString() + "//" + (5).ToString() + " " + (8).ToString() + "//" + (8).ToString());
                sw.WriteLine("f " + (8).ToString() + "//" + (8).ToString() + " " + (1).ToString() + "//" + (1).ToString() + " " + (4).ToString() + "//" + (4).ToString());
                sw.WriteLine("f " + (8).ToString() + "//" + (8).ToString() + " " + (5).ToString() + "//" + (5).ToString() + " " + (1).ToString() + "//" + (1).ToString());
                sw.WriteLine("f " + (1).ToString() + "//" + (1).ToString() + " " + (5).ToString() + "//" + (5).ToString() + " " + (2).ToString() + "//" + (2).ToString());
                sw.WriteLine("f " + (2).ToString() + "//" + (2).ToString() + " " + (5).ToString() + "//" + (5).ToString() + " " + (6).ToString() + "//" + (6).ToString());
                sw.WriteLine("f " + (2).ToString() + "//" + (2).ToString() + " " + (6).ToString() + "//" + (6).ToString() + " " + (3).ToString() + "//" + (3).ToString());
                sw.WriteLine("f " + (3).ToString() + "//" + (3).ToString() + " " + (6).ToString() + "//" + (6).ToString() + " " + (7).ToString() + "//" + (7).ToString());
                sw.WriteLine("f " + (7).ToString() + "//" + (7).ToString() + " " + (8).ToString() + "//" + (8).ToString() + " " + (3).ToString() + "//" + (3).ToString());
                sw.WriteLine("f " + (3).ToString() + "//" + (3).ToString() + " " + (8).ToString() + "//" + (8).ToString() + " " + (4).ToString() + "//" + (4).ToString());

                sw.Dispose();
                fs.Dispose();
                sw.Close();
                fs.Close();
            }                           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="path"></param>
        /// <param name="x_"></param>
        /// <param name="y_"></param>
        /// <param name="z_"></param>
        private void GenerationTriangle_desbaste(DMesh3 mesh, string path, double x_, double y_, double z_, double z_ref)
        {
            Vector3d vertexCurrent = new Vector3d();
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            string[] nameBlock = path.Split('\\');   

            //FileStream fs_tampa = new FileStream(tampa, FileMode.Create);
            //StreamWriter sw_tampa = new StreamWriter(fs_tampa);

            MeshBoundaryLoops loops = new MeshBoundaryLoops(mesh);
            Debug.Assert(loops.Loops.Count > 0);
            double[] cornerIndex = new double[5];

            double countPoints = 0;

            List<Vector3d> ordenar = new List<Vector3d>();
            List<Vector3d> ordenar_Aux = new List<Vector3d>();
            List<Vector3d> corner = new List<Vector3d>();
            Vector3d centroideX0Y0 = new Vector3d();
            double coeficinteAngular;
            double zMaxBloco = 0;            

            double x, xInit;
            double y, yInit;
            double z, zInit;

            double xmax, xmin;
            double ymax, ymin;
            double zmax, zmin;

            x = x_ + Model3D.refTranslMarcacaoCorte_port.X;
            y = y_ + Model3D.refTranslMarcacaoCorte_port.Y;
            z = z_ + Model3D.refTranslMarcacaoCorte_port.Z;

            xInit = Model3D.refTranslMarcacaoCorte_port.X;
            yInit = Model3D.refTranslMarcacaoCorte_port.Y;
            zInit = Model3D.refTranslMarcacaoCorte_port.Z;

            xmax = x_ + Model3D.refTranslMarcacaoCorte_port.X;
            ymax = y_ + Model3D.refTranslMarcacaoCorte_port.Y;
            zmax = z_ + Model3D.refTranslMarcacaoCorte_port.Z;

            xmin = Model3D.refTranslMarcacaoCorte_port.X;
            ymin = Model3D.refTranslMarcacaoCorte_port.Y;
            zmin = Model3D.refTranslMarcacaoCorte_port.Z;

            sw.Flush();

            int valor = 0;

            /* Pega a maior contorno para fazer o bloco
             * os pequenos contornos seriam buracos na malha 
             * estes buracos seram corrigidos posteriomente
             */

            foreach (EdgeLoop loop in loops.Loops)
            {
                if (loop.EdgeCount > valor)
                {
                    valor = loop.EdgeCount;
                }
            }

            /*
             * Pega o maior z do bloco
             */
            foreach (EdgeLoop loop in loops.Loops)
            {             
                for (int v2 = 0; v2 < loop.VertexCount; v2++)
                {
                    vertexCurrent = loop.GetVertex(v2);
                    if(vertexCurrent.z > zMaxBloco)
                    {
                        zMaxBloco = vertexCurrent.z;
                    }
                }
            }           

            foreach (EdgeLoop loop in loops.Loops)
            {
                if (loop.EdgeCount < valor) continue;

                #region método novo
                AxisAlignedBox3d e = loop.GetBounds();
                centroideX0Y0.x = e.Center.x;
                centroideX0Y0.y = e.Center.y;
                centroideX0Y0.z = e.Center.z;


                //m = y2 - y1 / x2 - x1
                //ymax - y0 = m (xmax - x0)

                DMesh3 nova = new DMesh3();

                for (int v2 = 0; v2 < loop.VertexCount; v2++)
                {
                    vertexCurrent = loop.GetVertex(v2);
                    coeficinteAngular = ((vertexCurrent.y - centroideX0Y0.y) / (vertexCurrent.x - centroideX0Y0.x));

                    if (vertexCurrent.x > centroideX0Y0.x)  //xmax
                    {
                        double py = centroideX0Y0.y + coeficinteAngular * (xmax - centroideX0Y0.x);

                        if (py > ymax) //ymax
                        {
                            if (corter_select == -1 || corter_select == 2)
                            {
                                double px = centroideX0Y0.x + (ymax - centroideX0Y0.y) / coeficinteAngular;

                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos_desbaste.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + zMaxBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + zMaxBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + px + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + px + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + px + " " + ymax + " " + zmax);
                                    sw.WriteLine("v " + px + " " + ymax + " " + zmax);
                                }
                                corter_select = 2;
                            }
                            else
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos_desbaste.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + zMaxBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + zMaxBloco);
                                }
                                else
                                { 
                                    sw.WriteLine("vn " + xmax + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + xmax + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + xmax + " " + ymax + " " + zmax);
                                    sw.WriteLine("v " + xmax + " " + ymax + " " + zmax);
                                }
                                corter_select = 2;
                            }
                            countPoints += 3;
                        }
                        else if (py < ymin) //ymin
                        {
                            if (corter_select == -1 || corter_select == 3)
                            {
                                double px = centroideX0Y0.x + (ymin - centroideX0Y0.y) / coeficinteAngular;

                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos_desbaste.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + zMaxBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + zMaxBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + px + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + px + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + px + " " + ymin + " " + zmax);
                                    sw.WriteLine("v " + px + " " + ymin + " " + zmax);
                                }
                                corter_select = 3;
                            }
                            else
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos_desbaste.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + zMaxBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + zMaxBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + xmax + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + xmax + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + xmax + " " + ymin + " " + zmax);
                                    sw.WriteLine("v " + xmax + " " + ymin + " " + zmax);
                                }
                                corter_select = 3;
                            }

                            countPoints += 3;
                        }
                        else
                        {
                            if (corter_select == -1 || corter_select == 0)
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos_desbaste.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + py + " " + zMaxBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + py + " " + zMaxBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + xmax + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + xmax + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + xmax + " " + py + " " + zmax);
                                    sw.WriteLine("v " + xmax + " " + py + " " + zmax);
                                }
                                corter_select = 0;
                            }
                            else
                            {
                                if (corter_select == 2)
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    if (nameBlock[3] != "Bloco1Out_Tringulos_desbaste.obj")
                                    {
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + zMaxBloco);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + zMaxBloco);
                                    }
                                    else
                                    {
                                        sw.WriteLine("vn " + xmax + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + xmax + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + xmax + " " + ymax + " " + zmax);
                                        sw.WriteLine("v " + xmax + " " + ymax + " " + zmax);
                                    }
                                    corter_select = 0;
                                }
                                else
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    if (nameBlock[3] != "Bloco1Out_Tringulos_desbaste.obj")
                                    {
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + zMaxBloco);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + zMaxBloco);
                                    }
                                    else
                                    {
                                        sw.WriteLine("vn " + xmax + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + xmax + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + xmax + " " + ymin + " " + zmax);
                                        sw.WriteLine("v " + xmax + " " + ymin + " " + zmax);
                                    }
                                    corter_select = 0;
                                }

                            }
                            countPoints += 3;
                        }
                    }
                    else //xmin
                    {
                        //double py = centroideX0Y0.y + coeficinteAngular * (vertexCurrent.x - centroideX0Y0.x);
                        double py = centroideX0Y0.y + coeficinteAngular * (xmin - centroideX0Y0.x);

                        if (py > ymax) //ymax
                        {
                            double px = centroideX0Y0.x + (ymax - centroideX0Y0.y) / coeficinteAngular;

                            if (corter_select == -1 || corter_select == 2)
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos_desbaste.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + zMaxBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + zMaxBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + px + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + px + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + px + " " + ymax + " " + zmax);
                                    sw.WriteLine("v " + px + " " + ymax + " " + zmax);
                                }
                                corter_select = 2;
                            }

                            else
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos_desbaste.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + zMaxBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + zMaxBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + xmin + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + xmin + " " + ymax + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + xmin + " " + ymax + " " + zmax);
                                    sw.WriteLine("v " + xmin + " " + ymax + " " + zmax);
                                }
                                corter_select = 2;
                            }

                            countPoints += 3;
                        }
                        else if (py < ymin) //ymin
                        {
                            double px = centroideX0Y0.x + (ymin - centroideX0Y0.y) / coeficinteAngular;

                            if (corter_select == -1 || corter_select == 3)
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos_desbaste.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + zMaxBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + zMaxBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + px + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + px + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + px + " " + ymin + " " + zmax);
                                    sw.WriteLine("v " + px + " " + ymin + " " + zmax);
                                }
                                corter_select = 3;
                            }
                            else
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos_desbaste.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + zMaxBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + zMaxBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + xmin + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + xmin + " " + ymin + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + xmin + " " + ymin + " " + zmax);
                                    sw.WriteLine("v " + xmin + " " + ymin + " " + zmax);
                                }
                                corter_select = 3;
                            }
                            countPoints += 3;
                        }
                        else
                        {
                            if (corter_select == -1 || corter_select == 0)
                            {
                                sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                if (nameBlock[3] != "Bloco1Out_Tringulos_desbaste.obj")
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + py + " " + zMaxBloco);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + py + " " + zMaxBloco);
                                }
                                else
                                {
                                    sw.WriteLine("vn " + xmin + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + xmin + " " + py + " " + vertexCurrent.z);
                                    sw.WriteLine("vn " + xmin + " " + py + " " + zmax);
                                    sw.WriteLine("v " + xmin + " " + py + " " + zmax);
                                }
                                corter_select = 0;
                            }
                            else
                            {
                                if (corter_select == 2)
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    if (nameBlock[3] != "Bloco1Out_Tringulos_desbaste.obj")
                                    {
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymax + " " + zMaxBloco);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymax + " " + zMaxBloco);
                                    }
                                    else
                                    {
                                        sw.WriteLine("vn " + xmin + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + xmin + " " + ymax + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + xmin + " " + ymax + " " + zmax);
                                        sw.WriteLine("v " + xmin + " " + ymax + " " + zmax);
                                    }
                                    corter_select = 0;
                                }
                                else
                                {
                                    sw.WriteLine("vn " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    sw.WriteLine("v " + vertexCurrent.x + " " + vertexCurrent.y + " " + vertexCurrent.z);
                                    if (nameBlock[3] != "Bloco1Out_Tringulos_desbaste.obj")
                                    {
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + vertexCurrent.x + " " + ymin + " " + zMaxBloco);
                                        sw.WriteLine("v " + vertexCurrent.x + " " + ymin + " " + zMaxBloco);
                                    }
                                    else
                                    {
                                        sw.WriteLine("vn " + xmin + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("v " + xmin + " " + ymin + " " + vertexCurrent.z);
                                        sw.WriteLine("vn " + xmin + " " + ymin + " " + zmax);
                                        sw.WriteLine("v " + xmin + " " + ymin + " " + zmax);
                                    }
                                    corter_select = 0;
                                }

                            }
                            countPoints += 3;
                        }
                    }
                }

                //debug
                //sw.Dispose();
                //fs.Dispose();
                //sw.Close();
                //fs.Close();

                #endregion método novo

                // ligação dos triangulos 02 ok 
                int pontoFinal = 0;
                int tampaPontos = 0;

                for (int v2 = tampaPontos; v2 < (countPoints - 3); v2 += 3)
                {

                    sw.WriteLine("f " + (v2 + 5).ToString() + "//" + (v2 + 5).ToString() + " " + (v2 + 2).ToString() + "//" + (v2 + 2).ToString() + " " + (v2 + 1).ToString() + "//" + (v2 + 1).ToString());
                    sw.WriteLine("f " + (v2 + 4).ToString() + "//" + (v2 + 4).ToString() + " " + (v2 + 5).ToString() + "//" + (v2 + 5).ToString() + " " + (v2 + 1).ToString() + "//" + (v2 + 1).ToString());
                    sw.WriteLine("f " + (v2 + 5).ToString() + "//" + (v2 + 5).ToString() + " " + (v2 + 3).ToString() + "//" + (v2 + 3).ToString() + " " + (v2 + 2).ToString() + "//" + (v2 + 2).ToString());
                    sw.WriteLine("f " + (v2 + 5).ToString() + "//" + (v2 + 5).ToString() + " " + (v2 + 6).ToString() + "//" + (v2 + 6).ToString() + " " + (v2 + 3).ToString() + "//" + (v2 + 3).ToString());


                    pontoFinal = v2;
                }

                pontoFinal = pontoFinal;

                sw.WriteLine("f " + (tampaPontos + 1).ToString() + "//" + (tampaPontos + 1).ToString() + " " + (tampaPontos + 2).ToString() + "//" + (tampaPontos + 2).ToString() + " " + (pontoFinal + 5).ToString() + "//" + (pontoFinal + 5).ToString());
                sw.WriteLine("f " + (tampaPontos + 1).ToString() + "//" + (tampaPontos + 1).ToString() + " " + (pontoFinal + 5).ToString() + "//" + (pontoFinal + 5).ToString() + " " + (pontoFinal + 4).ToString() + "//" + (pontoFinal + 4).ToString());
                sw.WriteLine("f " + (tampaPontos + 2).ToString() + "//" + (tampaPontos + 2).ToString() + " " + (tampaPontos + 3).ToString() + "//" + (tampaPontos + 3).ToString() + " " + (pontoFinal + 5).ToString() + "//" + (pontoFinal + 5).ToString());
                sw.WriteLine("f " + (pontoFinal + 5).ToString() + "//" + (pontoFinal + 5).ToString() + " " + (tampaPontos + 3).ToString() + "//" + (tampaPontos + 3).ToString() + " " + (pontoFinal + 6).ToString() + "//" + (pontoFinal + 6).ToString());
                //sw.WriteLine("f " + (tampaPontos + 2).ToString() + "//" + (tampaPontos + 2).ToString() + " " + (tampaPontos + 3).ToString() + "//" + (tampaPontos + 3).ToString() + " " + (pontoFinal + 6).ToString() + "//" + (pontoFinal + 6).ToString());
                //sw.WriteLine("f " + (pontoFinal + 6).ToString() + "//" + (pontoFinal + 6).ToString() + " " + (tampaPontos + 3).ToString() + "//" + (tampaPontos + 3).ToString() + " " + (pontoFinal + 7).ToString() + "//" + (pontoFinal + 7).ToString());

            }

            sw.Dispose();
            fs.Dispose();
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// Algoritmo de de Marching Cubes
        /// </summary>
        /// <param name="path"></param>
        public void MarchingCubes(string path)
        {
            DMesh3 mesh = IO.GeneralIO.LoadtMesh(path);
            int numcells = 512;//1024;
            int numcellsBounds = 128;//256; //influencia na qualidade da peça - qualidade máx igualar ao numcells no caso 512

            AxisAlignedBox3d bounds = mesh.CachedBounds;
            
            double cellsize = bounds.MaxDim / numcells;

            MeshSignedDistanceGrid levelSet = new MeshSignedDistanceGrid(mesh, cellsize);
            levelSet.ExactBandWidth = 3;//10;          
            levelSet.UseParallel = true;// true;
            levelSet.ComputeMode = MeshSignedDistanceGrid.ComputeModes.NarrowBandOnly;
            levelSet.Compute();

            var iso = new DenseGridTrilinearImplicit(levelSet.Grid, levelSet.GridOrigin, levelSet.CellSize);

            MarchingCubes c = new MarchingCubes();
            c.Implicit = iso;
            c.Bounds = mesh.CachedBounds;
            c.Bounds.Expand(c.Bounds.MaxDim * 0.1);
            c.CubeSize = c.Bounds.MaxDim / numcellsBounds; 
            c.Generate();
            
            GeneralIO.SaveMesh(c.Mesh, path);
        }

        public void MarchingCubes2(string path, string name_, OpenGLControl OpenGLControl_, Filters filters) 
        {
            Model3D model3D;
            string errorText = string.Empty;       

            model3D = OpenGLControl_.GLrender.LoadModel(path + name_, errorText);
            Models.Model3DAUX.Save_OBJ(model3D, GLSettings.locateTMP + name_, "");

            DMesh3 mesh = IO.GeneralIO.LoadtMesh(GLSettings.locateTMP + name_);
            int numcells = 512;//1024;
            int numcellsBounds = 128;//256; //influencia na qualidade da peça - qualidade máx igualar ao numcells no caso 512

            AxisAlignedBox3d bounds = mesh.CachedBounds;

            double cellsize = bounds.MaxDim / numcells;

            MeshSignedDistanceGrid levelSet = new MeshSignedDistanceGrid(mesh, cellsize);
            levelSet.ExactBandWidth = 3;//10;          
            levelSet.UseParallel = true;// true;
            levelSet.ComputeMode = MeshSignedDistanceGrid.ComputeModes.NarrowBandOnly;
            levelSet.Compute();

            var iso = new DenseGridTrilinearImplicit(levelSet.Grid, levelSet.GridOrigin, levelSet.CellSize);

            MarchingCubes c = new MarchingCubes();
            c.Implicit = iso;
            c.Bounds = mesh.CachedBounds;
            c.Bounds.Expand(c.Bounds.MaxDim* 0.1);
            c.CubeSize = c.Bounds.MaxDim / numcellsBounds; 
            c.Generate();

            name_ = name_.Replace("INV", "");

            GeneralIO.SaveMesh(c.Mesh, GLSettings.locateTMP + name_);

            model3D = OpenGLControl_.GLrender.LoadModel(GLSettings.locateTMP + name_, errorText);
            Models.Model3DAUX.Save_OBJ(model3D, GLSettings.locateTMP + name_, "");

            filters.FilterSmoothing(GLSettings.locateTMP + name_);

            model3D = OpenGLControl_.GLrender.LoadModel(GLSettings.locateTMP + name_, errorText);                   
            Models.Model3DAUX.Save_OBJ(model3D, GLSettings.locateCAD + name_, "");
            string[] name = name_.Split('.');
            STLinIO stl = new STLinIO();            
            stl.export(GLSettings.locateSTL + name[0] + ".stl", GLSettings.locateCAD + name_, "stl");            
        }


    /// <summary>
    /// 
    /// </summary>
    public void MarchingCubes()
        {
            DMesh3 mesh = GeneralIO.LoadtMesh(GLSettings.locateTMP + GLSettings.ModeloAux);  

            int numcells = 512;
            int numcellsBounds = 128;//influencia na qualidade da peça - qualidade máx igualar ao numcells no caso 512

            AxisAlignedBox3d bounds = mesh.CachedBounds;

            double cellsize = bounds.MaxDim / numcells;

            MeshSignedDistanceGrid levelSet = new MeshSignedDistanceGrid(mesh, cellsize);
            levelSet.ExactBandWidth = 3;//10;
            levelSet.UseParallel = true;
            levelSet.ComputeMode = MeshSignedDistanceGrid.ComputeModes.NarrowBandOnly;
            levelSet.Compute();

            var iso = new DenseGridTrilinearImplicit(levelSet.Grid, levelSet.GridOrigin, levelSet.CellSize);

            MarchingCubes c = new MarchingCubes();
            c.Implicit = iso;
            c.Bounds = mesh.CachedBounds;
            c.Bounds.Expand(c.Bounds.MaxDim * 0.1);
            c.CubeSize = c.Bounds.MaxDim / numcellsBounds;
            c.Generate();

            GeneralIO.SaveMesh(c.Mesh, GLSettings.locateTMP + GLSettings.ModeloAuxOut_);
        }
       
        /// <summary>
        /// 
        /// </summary>
        public void RotateAxisAngle()
        {
            DMesh3 mesh = IO.GeneralIO.LoadtMesh(ModeloAux);

            Vector3f vector3F = new Vector3f(0, 0, 0);
            Vector3f vector3F_ = new Vector3f(0, 0, 0);
            vector3F.x = 0;
            vector3F.y = -1;
            vector3F.z = 0;

            MeshTransforms.Rotate(mesh, vector3F_, Quaternionf.AxisAngleD(vector3F, ((float)1 * 90)));
            MeshTransforms.Translate(mesh, 0, 0, 0);

            GeneralIO.SaveMesh(mesh, GLSettings.locateTMP + GLSettings.ModeloAuxOut_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="out_"></param>
        /// <param name="OpenGLControl_"></param>
        public void GirarMenos180GrausEixoY(string path, string out_ , OpenGLControl OpenGLControl_)
        {
            Model3D model3D;
            string errorText = string.Empty;

            model3D = OpenGLControl_.GLrender.LoadModel(path, errorText);
            Models.Model3DAUX.Save_OBJ(model3D, out_, "");

            DMesh3 mesh = GeneralIO.LoadtMesh(out_);

            Vector3f vector3F = new Vector3f(0, 0, 0);
            Vector3f vector3F_ = new Vector3f(0, 0, 0);
            vector3F.x = 0;
            vector3F.y = -1;
            vector3F.z = 0;

            MeshTransforms.Rotate(mesh, vector3F_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 180)));
            MeshTransforms.Translate(mesh, 0, 0, 0);

            GeneralIO.SaveMesh(mesh, out_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="out_"></param>
        /// <param name="OpenGLControl_"></param>
        public void Girar0GrausEixoY(string path, string out_, OpenGLControl OpenGLControl_)
        {
            Model3D model3D;
            string errorText = string.Empty;

            model3D = OpenGLControl_.GLrender.LoadModel(path, errorText);
            Models.Model3DAUX.Save_OBJ(model3D, out_, "");

            DMesh3 mesh = GeneralIO.LoadtMesh(out_);

            Vector3f vector3F = new Vector3f(0, 0, 0);
            Vector3f vector3F_ = new Vector3f(0, 0, 0);
            vector3F.x = 0;
            vector3F.y = -1;
            vector3F.z = 0;

            MeshTransforms.Rotate(mesh, vector3F_, Quaternionf.AxisAngleD(vector3F, 1));
            MeshTransforms.Translate(mesh, 0, 0, 0);

            GeneralIO.SaveMesh(mesh, out_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="out_"></param>
        /// <param name="OpenGLControl_"></param>
        public void GirarMenos90GrausEixoY(string path, string out_, OpenGLControl OpenGLControl_, int numberblock)
        {
            Model3D model3D;
            string errorText = string.Empty;

            model3D = OpenGLControl_.GLrender.LoadModel(path, errorText);

            string[] out_aux = out_.Split('.');

            string outfinal = "." + out_aux[1] + ".obj";

            Models.Model3DAUX.Save_OBJ(model3D, outfinal, "");

            DMesh3 mesh = GeneralIO.LoadtMesh(outfinal);

            Vector3f vector3F = new Vector3f(0, 0, 0);
            Vector3d vector3d_ = new Vector3d(0, 0, 0);
            vector3F.x = 0;
            vector3F.y = -1;
            vector3F.z = 0;

            attractingAttributes(mesh, numberblock);
            MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 90)));
            adjustmentZ(mesh, numberblock);
            MeshTransforms.Translate(mesh, GLSettings.ajusteX_juste_rotacao90[numberblock], 0, GLSettings.ajusteZ_total_juste_rotacao90[numberblock]);

            GeneralIO.SaveMesh(mesh, outfinal);

            ////teste
            model3D = OpenGLControl_.GLrender.LoadModel(outfinal, errorText);         
            Models.Model3DAUX.Save_OBJ(model3D, outfinal, "");

            STLinIO stl = new STLinIO();

            string[] nameQuebra = outfinal.Split('\\');
            string[] name = nameQuebra[3].Split('.');

            stl.export(GLSettings.locateTMP + name[0] + ".stl", outfinal, "stl");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="out_"></param>
        /// <param name="OpenGLControl_"></param>
        public void GirarMenos90GrausEixo_Y_X(string path, string out_, OpenGLControl OpenGLControl_, int numberBlock)
        {
            Model3D model3D;
            string errorText = string.Empty;

            model3D = OpenGLControl_.GLrender.LoadModel(path, errorText);

            string[] out_aux = out_.Split('.');

            string outfinal = "." + out_aux[1] + ".obj";

            Models.Model3DAUX.Save_OBJ(model3D, outfinal, "");

            DMesh3 mesh = GeneralIO.LoadtMesh(outfinal);

            Vector3f vector3F = new Vector3f(0, 0, 0);
            Vector3d vector3d_ = new Vector3d(0, 0, 0);           

            attractingAttributes(mesh, numberBlock);

            switch(numberBlock)
            {
                case 0:
                case 1:
                    vector3F.x = 0;
                    vector3F.y = -1;
                    vector3F.z = 0;
                    MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 90)));
                    vector3F.x = 1;
                    vector3F.y = 0;
                    vector3F.z = 0;
                    MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 90)));
                    vector3F.x = 0;
                    vector3F.y = 0;
                    vector3F.z = -1;
                    MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 180)));
                    adjustmentZ(mesh, numberBlock);
                    MeshTransforms.Translate(mesh, -1 * GLSettings.ajusteX_total_juste_rotacao90[numberBlock], 0, GLSettings.ajusteZ_total_juste_rotacao90[numberBlock]);
  
                    break;
                case 2:
                    vector3F.x = 0;
                    vector3F.y = -1;
                    vector3F.z = 0;
                    MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 90)));
                    vector3F.x = 1;
                    vector3F.y = 0;
                    vector3F.z = 0;
                    MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 90)));
                    vector3F.x = 0;
                    vector3F.y = 0;
                    vector3F.z = 1;
                    MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 180))); 
                    adjustmentZ(mesh, numberBlock);
                    MeshTransforms.Translate(mesh, -1 * GLSettings.ajusteX_total_juste_rotacao90[numberBlock], -1 * GLSettings.ajusteX_desbasteXmin[numberBlock], GLSettings.ajusteZ_total_juste_rotacao90[numberBlock]);                

                    break; 
                case 3:
                    vector3F.x = 0;
                    vector3F.y = 0;//-1; //já foi rotacionado para coreção de bug
                    vector3F.z = 0;
                    MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 90)));
                    vector3F.x = 1;
                    vector3F.y = 0;
                    vector3F.z = 0;
                    MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 90)));
                    vector3F.x = 0;
                    vector3F.y = 0;
                    vector3F.z = -1;
                    MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 90)));
                    adjustmentZ(mesh, numberBlock);
                    MeshTransforms.Translate(mesh, -1 * GLSettings.ajusteX_total_juste_rotacao90[numberBlock], -1 * GLSettings.ajusteX_desbasteXmin[numberBlock], GLSettings.ajusteZ_total_juste_rotacao90[numberBlock]);        

                    break;
                case 4:
                    vector3F.x = 0;
                    vector3F.y = -1;
                    vector3F.z = 0;
                    MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 90)));
                    //vector3F.x = 0;
                    //vector3F.y = 0;
                    //vector3F.z = 0;
                    //MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 90)));
                    //vector3F.x = 0;
                    //vector3F.y = 0;
                    //vector3F.z = -1;
                    //MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 180)));
                    //adjustmentZ(mesh, numberBlock);
                    //MeshTransforms.Translate(mesh, -1 * GLSettings.ajusteX_total_juste_rotacao90[numberBlock], -1 * GLSettings.ajusteX_desbasteXmin[numberBlock], GLSettings.ajusteZ_total_juste_rotacao90[numberBlock]);
                    break;
                case 5:
                    vector3F.x = 0;
                    vector3F.y = -1;
                    vector3F.z = 0;
                    MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 90)));
                    //vector3F.x = 0;
                    //vector3F.y = 0;
                    //vector3F.z = 0;
                    //MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 90)));
                    //vector3F.x = 1;
                    //vector3F.y = 0;
                    //vector3F.z = -1;
                    //MeshTransforms.Rotate(mesh, vector3d_, Quaternionf.AxisAngleD(vector3F, ((float)-1 * 180)));
                    //adjustmentZ(mesh, numberBlock);
                    //MeshTransforms.Translate(mesh, -1 * GLSettings.ajusteX_total_juste_rotacao90[numberBlock], -1 * GLSettings.ajusteX_desbasteXmin[numberBlock], GLSettings.ajusteZ_total_juste_rotacao90[numberBlock]);
                    break;
            }      

            GeneralIO.SaveMesh(mesh, outfinal);

            ////teste
            model3D = OpenGLControl_.GLrender.LoadModel(outfinal, errorText);
            Models.Model3DAUX.Save_OBJ(model3D, outfinal, "");

            STLinIO stl = new STLinIO();

            string[] nameQuebra = outfinal.Split('\\');
            string[] name = nameQuebra[3].Split('.');

            stl.export(GLSettings.locateTMP + name[0] + ".stl", outfinal, "stl");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="out_"></param>
        /// <param name="OpenGLControl_"></param>
        public void AjustePosicionamento(string path, string out_, OpenGLControl OpenGLControl_, int numberBlock)
        {
            Model3D model3D;
            string errorText = string.Empty;

            model3D = OpenGLControl_.GLrender.LoadModel(path, errorText);

            string[] out_aux = out_.Split('.');

            string outfinal = "." + out_aux[1] + ".obj";

            Models.Model3DAUX.Save_OBJ(model3D, outfinal, "");

            DMesh3 mesh = GeneralIO.LoadtMesh(outfinal);

            attractingAttributesDesbaste(mesh, numberBlock);

            switch (numberBlock)
            {
                case 0:
                case 1:
                    adjustmentZDesbaste(mesh, numberBlock);
                    MeshTransforms.Translate(mesh, -1 * GLSettings.ajusteX_total_juste_rotacao90N[numberBlock], 0, 0);
                    break;
                case 2:
                    adjustmentZDesbaste(mesh, numberBlock);
                    MeshTransforms.Translate(mesh, -1 * GLSettings.ajusteX_total_juste_rotacao90N[numberBlock], 0, 0);                  
                    break;
                case 3:
                    adjustmentZDesbaste(mesh, numberBlock);
                    MeshTransforms.Translate(mesh, -1 * GLSettings.ajusteX_total_juste_rotacao90N[numberBlock], 0, 0);
                    break;
                case 4:
                    adjustmentZDesbaste(mesh, numberBlock);
                    MeshTransforms.Translate(mesh, -1 * GLSettings.ajusteX_total_juste_rotacao90N[numberBlock], 0, 0);
                    break;
                case 5:
                    adjustmentZDesbaste(mesh, numberBlock);
                    MeshTransforms.Translate(mesh, -1 * GLSettings.ajusteX_total_juste_rotacao90N[numberBlock], 0, 0);
                    break;
            }

            GeneralIO.SaveMesh(mesh, outfinal);

            ////teste
            model3D = OpenGLControl_.GLrender.LoadModel(outfinal, errorText);
            Models.Model3DAUX.Save_OBJ(model3D, outfinal, "");

            STLinIO stl = new STLinIO();

            string[] nameQuebra = outfinal.Split('\\');
            string[] name = nameQuebra[3].Split('.');

            stl.export(GLSettings.locateSTL + name[0] + ".stl", outfinal, "stl");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        private void attractingAttributes(DMesh3 mesh, int numberBlock)
        {
            double zMin = 100000;
            double zMax = -100000;
            double xMin = 100000;
            double xMax = -100000;
            double yMin = 100000;
            double yMax = -100000;
  
            foreach (var valor_ in mesh.Vertices())
            {
                if (valor_.z < zMin)
                {
                    zMin = valor_.z;
                }

                if (valor_.z > zMax)
                {
                    zMax = valor_.z;
                }

                if (valor_.x < xMin)
                {
                    xMin = valor_.x;
                }

                if (valor_.x > xMax)
                {
                    xMax = valor_.x;
                }

                if (valor_.y < yMin)
                {
                    yMin = valor_.y;
                }

                if (valor_.y > yMax)
                {
                    yMax = valor_.y;
                }
            }

            if (zMin < 0 && zMax > 0)
            {
                GLSettings.ajusteZ_total[numberBlock] = Math.Abs(Math.Abs(zMin) + Math.Abs(zMax));
            }
            else if (zMin < 0 && zMax < 0 || zMin >= 0 && zMax > 0)
            {
                GLSettings.ajusteZ_total[numberBlock] = Math.Abs(Math.Abs(zMin) - Math.Abs(zMax));
            }

            if (yMin < 0 && yMax > 0)
            {
                GLSettings.ajusteY_total[numberBlock] = Math.Abs(Math.Abs(yMin) + Math.Abs(yMax));
            }
            else if (yMin < 0 && yMax < 0 || yMin >= 0 && yMax > 0)
            {
                GLSettings.ajusteY_total[numberBlock] = Math.Abs(Math.Abs(yMin) - Math.Abs(yMax));
            }

            if (xMin < 0 && xMax > 0)
            {
                GLSettings.ajusteX_total[numberBlock] = Math.Abs(Math.Abs(xMin) + Math.Abs(xMax));
            }
            else if (xMin < 0 && xMax < 0 || xMin >= 0 && xMax > 0)
            {
                GLSettings.ajusteX_total[numberBlock] = Math.Abs(Math.Abs(xMin) - Math.Abs(xMax));
            }          

            GLSettings.ajusteZ_desbasteZmin[numberBlock] = zMin;
            GLSettings.ajusteZ_desbasteZmax[numberBlock] = zMax;
            GLSettings.ajusteX_desbasteXmin[numberBlock] = xMin;
            GLSettings.ajusteX_desbasteXmax[numberBlock] = xMax;
            GLSettings.ajusteY_desbasteYmin[numberBlock] = yMin;
            GLSettings.ajusteY_desbasteYmax[numberBlock] = yMax;             
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        private void attractingAttributesDesbaste(DMesh3 mesh, int numberBlock)
        {
            double zMin = 100000;
            double zMax = -100000;
            double xMin = 100000;
            double xMax = -100000;
            double yMin = 100000;
            double yMax = -100000;

            foreach (var valor_ in mesh.Vertices())
            {
                if (valor_.z < zMin)
                {
                    zMin = valor_.z;
                }

                if (valor_.z > zMax)
                {
                    zMax = valor_.z;
                }

                if (valor_.x < xMin)
                {
                    xMin = valor_.x;
                }

                if (valor_.x > xMax)
                {
                    xMax = valor_.x;
                }

                if (valor_.y < yMin)
                {
                    yMin = valor_.y;
                }

                if (valor_.y > yMax)
                {
                    yMax = valor_.y;
                }
            }

            if (zMin < 0 && zMax > 0)
            {
                GLSettings.ajusteZ_totalN[numberBlock] = Math.Abs(Math.Abs(zMin) + Math.Abs(zMax));
            }
            else if (zMin < 0 && zMax < 0 || zMin > 0 && zMax > 0)
            {
                GLSettings.ajusteZ_totalN[numberBlock] = Math.Abs(Math.Abs(zMin) - Math.Abs(zMax));
            }

            if (yMin < 0 && yMax > 0)
            {
                GLSettings.ajusteZ_totalN[numberBlock] = Math.Abs(Math.Abs(yMin) + Math.Abs(yMax));
            }
            else if (yMin < 0 && yMax < 0 || yMin > 0 && yMax > 0)
            {
                GLSettings.ajusteZ_totalN[numberBlock] = Math.Abs(Math.Abs(yMin) - Math.Abs(yMax));
            }

            if (xMin < 0 && xMax > 0)
            {
                GLSettings.ajusteZ_totalN[numberBlock] = Math.Abs(Math.Abs(xMin) + Math.Abs(xMax));
            }
            else if (xMin < 0 && xMax < 0 || xMin > 0 && xMax > 0)
            {
                GLSettings.ajusteZ_totalN[numberBlock] = Math.Abs(Math.Abs(xMin) - Math.Abs(xMax));
            }

            GLSettings.ajusteZ_desbasteZminN[numberBlock] = zMin;
            GLSettings.ajusteZ_desbasteZmaxN[numberBlock] = zMax;
            GLSettings.ajusteX_desbasteXminN[numberBlock] = xMin;
            GLSettings.ajusteX_desbasteXmaxN[numberBlock] = xMax;
            GLSettings.ajusteY_desbasteYminN[numberBlock] = yMin;
            GLSettings.ajusteY_desbasteYmaxN[numberBlock] = yMax;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        private void adjustmentZ(DMesh3 mesh, int numberBlock)
        {
            double zMin = 100000;
            double zMax = -100000;
            double xMin = 100000;
            double xMax = -100000;
            double zt = 0;
            double xt = 0;           

            foreach (var valor_ in mesh.Vertices())
            {
                if (valor_.z < zMin)
                {
                    zMin = valor_.z;
                }

                if (valor_.z > zMax)
                {
                    zMax = valor_.z;
                }

                if (valor_.x < xMin)
                {
                    xMin = valor_.x;
                }

                if (valor_.x > xMax)
                {
                    xMax = valor_.x;
                }

                zt = Math.Abs(Math.Abs(zMin) - Math.Abs(zMax));
                xt = Math.Abs(Math.Abs(xMin) - Math.Abs(xMax));                       
            }

            GLSettings.ajusteX_total_juste_rotacao90[numberBlock] = xMin;

            GLSettings.ajusteX_juste_rotacao90[numberBlock] = ((zt / 2 - xt) + xt / 2);

            if(zMax < 0)
            {
                GLSettings.ajusteZ_total_juste_rotacao90[numberBlock] = zt + (Math.Abs(zMax));
            }
            else
            {
                GLSettings.ajusteZ_total_juste_rotacao90[numberBlock] = zt + 50;
            }            
        }



        private double adjustmentZ_2(DMesh3 mesh)
        {
            double zMin = 100000;
            double zMax = -100000;
            double xMin = 100000;
            double xMax = -100000;
            double adjustment = 0;           

            foreach (var valor_ in mesh.Vertices())
            {
                if (valor_.z < zMin)
                {
                    zMin = valor_.z;
                }

                if (valor_.z > zMax)
                {
                    zMax = valor_.z;
                }

                if (valor_.x < xMin)
                {
                    xMin = valor_.x;
                }

                if (valor_.x > xMax)
                {
                    xMax = valor_.x;
                }                
            }

            if(zMin > 0)
            {
                adjustment = 0; 
            }
            else
            {            
                adjustment = Math.Abs(zMin);
            }

            return adjustment;
        }

        private double adjustmentZ_3(DMesh3 mesh, int block)
        {
            double zMin = 100000;
            double zMax = -100000;
            double xMin = 100000;
            double xMax = -100000;
            double yMin = 100000;
            double yMax = -100000;

            double adjustment = 0;

            foreach (var valor_ in mesh.Vertices())
            {
                if (valor_.z < zMin)
                {
                    zMin = valor_.z;
                }

                if (valor_.z > zMax)
                {
                    zMax = valor_.z;
                }

                if (valor_.x < xMin)
                {
                    xMin = valor_.x;
                }

                if (valor_.x > xMax)
                {
                    xMax = valor_.x;
                }

                if (valor_.y < yMin)
                {
                    yMin = valor_.y;
                }

                if (valor_.y > yMax)
                {
                    yMax = valor_.y;
                }
            }

            if(block == 2)
            {
                adjustment = Math.Abs(yMin);
            }
            else if(block == 3)
            {
                adjustment = Math.Abs(yMin);
            } 

            return adjustment;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        private void adjustmentZDesbaste(DMesh3 mesh, int numberBlock)
        {
            double zMin = 100000;
            double zMax = -100000;
            double xMin = 100000;
            double xMax = -100000;
            double zt = 0;
            double xt = 0;

            foreach (var valor_ in mesh.Vertices())
            {
                if (valor_.z < zMin)
                {
                    zMin = valor_.z;
                }

                if (valor_.z > zMax)
                {
                    zMax = valor_.z;
                }

                if (valor_.x < xMin)
                {
                    xMin = valor_.x;
                }

                if (valor_.x > xMax)
                {
                    xMax = valor_.x;
                }

                zt = Math.Abs(Math.Abs(zMin) - Math.Abs(zMax));
                xt = Math.Abs(Math.Abs(xMin) - Math.Abs(xMax));
            }

            GLSettings.ajusteX_total_juste_rotacao90N[numberBlock] = xMin;

            GLSettings.ajusteX_juste_rotacao90N[numberBlock] = ((zt / 2 - xt) + xt / 2);

            if (zMax < 0)
            {
                GLSettings.ajusteZ_total_juste_rotacao90N[numberBlock] = zt + (Math.Abs(zMax));
            }
            else
            {
                GLSettings.ajusteZ_total_juste_rotacao90N[numberBlock] = zt + 50;
            }
        }

        /// <summary>
        ///  Rotate Axis Angle
        /// </summary>
        /// <param name="path"></param>
        /// <param name="out_"></param>
        /// <param name="OpenGLControl_"></param>
        /// <param name="angle">angle</param>        
        /// <param name="axisX"> 1 right / -1 left / 0 without rotation </param>
        /// <param name="axisY"> 1 right / -1 left / 0 without rotation </param>
        /// <param name="axisZ"> 1 right / -1 left / 0 without rotation </param>
        public void RotateAxisAngle(string path, string out_, OpenGLControl OpenGLControl_,int angle, int axisX, int axisY, int axisZ)
        {
            Model3D model3D;
            string errorText = string.Empty;

            model3D = OpenGLControl_.GLrender.LoadModel(path, errorText);
            Models.Model3DAUX.Save_OBJ(model3D, out_, "");

            DMesh3 mesh = GeneralIO.LoadtMesh(out_);    

            Vector3f vector3F = new Vector3f(0, 0, 0);
            Vector3f vector3F_ = new Vector3f(0, 0, 0);
            vector3F.x = axisX;
            vector3F.y = axisY;
            vector3F.z = axisZ;           
            MeshTransforms.Rotate(mesh, vector3F_, Quaternionf.AxisAngleD(vector3F, ((float)1 * angle)));
            MeshTransforms.Translate(mesh, 0, 0, adjustmentZ_2(mesh));

            GeneralIO.SaveMesh(mesh, out_ );
        }

        public void RotateAxisAngleAdjustment(string path, string out_, OpenGLControl OpenGLControl_, int angle, int axisX, int axisY, int axisZ, int block)
        {
            Model3D model3D;
            string errorText = string.Empty;

            model3D = OpenGLControl_.GLrender.LoadModel(path, errorText);
            Models.Model3DAUX.Save_OBJ(model3D, out_, "");

            DMesh3 mesh = GeneralIO.LoadtMesh(out_);

            Vector3f vector3F = new Vector3f(0, 0, 0);
            Vector3f vector3F_ = new Vector3f(0, 0, 0);
            vector3F.x = axisX;
            vector3F.y = axisY;
            vector3F.z = axisZ;
            MeshTransforms.Rotate(mesh, vector3F_, Quaternionf.AxisAngleD(vector3F, ((float)1 * angle)));

            MeshTransforms.Translate(mesh, 0, -1 * adjustmentZ_3(mesh, block), 0);

            GeneralIO.SaveMesh(mesh, out_);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Girar45GrausEixoY()
        {
            DMesh3 mesh = IO.GeneralIO.LoadtMesh(ModeloAux);

            Vector3f vector3F = new Vector3f(0, 0, 0);
            Vector3f vector3F_ = new Vector3f(0, 0, 0);
            vector3F.x = 0;
            vector3F.y = -1;
            vector3F.z = 0;

            MeshTransforms.Rotate(mesh, vector3F_, Quaternionf.AxisAngleD(vector3F, ((float)1 * 45)));
            MeshTransforms.Translate(mesh, 0, 0, 0);

            GeneralIO.SaveMesh(mesh, GLSettings.locateTMP + GLSettings.ModeloAuxOut_);
        }

        /// <summary>
        /// 
        /// </summary>
        public void GirarMenos45GrausEixoY()
        {
            DMesh3 mesh = IO.GeneralIO.LoadtMesh(ModeloAux);

            Vector3f vector3F = new Vector3f(0, 0, 0);
            Vector3f vector3F_ = new Vector3f(0, 0, 0);
            vector3F.x = 0;
            vector3F.y = -1;
            vector3F.z = 0;

            MeshTransforms.Rotate(mesh, vector3F_, Quaternionf.AxisAngleD(vector3F, ((float) -1 * 45)));
            MeshTransforms.Translate(mesh, 0, 0, 0);

            GeneralIO.SaveMesh(mesh, GLSettings.locateTMP + GLSettings.ModeloAuxOut_);
        }

        /// <summary>
        /// Método que alteração a orinetação do modelo 3D
        /// </summary>
        /// <param name="select"></param>
        public void ConvertpToUp(int select)
        {
            DMesh3 mesh = GeneralIO.LoadtMesh(ModeloAux);

            switch(select)
            {
                case 0: MeshTransforms.ConvertZUpToYUp(mesh);
                    break;
                case 1: MeshTransforms.ConvertZUpToYUp(mesh);
                    break;
            }
            
            GeneralIO.SaveMesh(mesh, GLSettings.locateTMP + GLSettings.ModeloAuxOut_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OpenGLControl"></param>
        /// <param name="tipo"></param>
        public void ConfirmChanges(OpenGLControl OpenGLControl, string tipo)
        {
            DMesh3 mesh = GeneralIO.LoadtMesh(ModeloAux);
            //DMesh3 mesh2 = new DMesh3(mesh);            

            Vector3f vector3F = new Vector3f(0,0,0);
            Vector3f vector3F_ = new Vector3f(0,0,0);

            /******************************************/
            vector3F.x = 1;
            vector3F.y = 0;
            vector3F.z = 0;

            if(tipo == "confirmar") Model3D.vetRot_portDesfazer.Add(-1 * Model3D.vetRot_port);
        
            MeshTransforms.Rotate(mesh, vector3F_, Quaternionf.AxisAngleD(vector3F, ((float)Model3D.vetRot_port.X * 57.29578f)));
     
            /******************************************/

            vector3F.x = 0; 
            vector3F.y = 1;
            vector3F.z = 0;
           
            MeshTransforms.Rotate(mesh, vector3F_, Quaternionf.AxisAngleD(vector3F, ((float)Model3D.vetRot_port.Y * 57.29578f)));

            /*****************************************/

            vector3F.x = 0;
            vector3F.y = 0;
            vector3F.z = 1;
     
            MeshTransforms.Rotate(mesh, vector3F_, Quaternionf.AxisAngleD(vector3F, ((float)Model3D.vetRot_port.Z * 57.29578f)));
            //DMesh3 mesh3 = new DMesh3(mesh);
            MeshTransforms.Translate(mesh, Model3D.vetTransl_port.X, Model3D.vetTransl_port.Y, Model3D.vetTransl_port.Z);

            GeneralIO.SaveMesh(mesh, GLSettings.locateTMP + GLSettings.ModeloAuxOut_);        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelo1"></param>
        /// <param name="modelo2"></param>
        /// <param name="nameOut"></param>
        /// <param name="OpenGLControl_"></param>
        public void Union(string modelo1, string modelo2, string nameOut, OpenGLControl OpenGLControl_, Filters filters)
        {
            Model3D model3D;
            string errorText = string.Empty;

            model3D = OpenGLControl_.GLrender.LoadModel(modelo1, errorText);
            Models.Model3DAUX.Save_OBJ(model3D, modelo1, "");
            model3D = OpenGLControl_.GLrender.LoadModel(modelo2, errorText);
            Models.Model3DAUX.Save_OBJ(model3D, modelo2, "");

            DMesh3 mesh1 = GeneralIO.LoadtMesh(modelo1);
            DMesh3 mesh2 = GeneralIO.LoadtMesh(modelo2);

            mesh2.ReverseOrientation(); //Necessário para o método novo 

            MeshEditor.Append(mesh1, mesh2);      
            GeneralIO.SaveMesh(mesh1, GLSettings.locateCAD + nameOut);
            model3D = OpenGLControl_.GLrender.LoadModel(GLSettings.locateCAD + nameOut, errorText);
            Models.Model3DAUX.Save_OBJ(model3D, GLSettings.locateCAD + nameOut, "");         
            MarchingCubes(GLSettings.locateCAD + nameOut);
           
            model3D = OpenGLControl_.GLrender.LoadModel(GLSettings.locateCAD + nameOut, errorText);
            Models.Model3DAUX.Save_OBJ(model3D, GLSettings.locateCAD + nameOut, "");
            
            filters.FilterSmoothing(GLSettings.locateCAD + nameOut);

            model3D = OpenGLControl_.GLrender.LoadModel(GLSettings.locateCAD + nameOut, errorText);
            Models.Model3DAUX.Save_OBJ(model3D, GLSettings.locateCAD + nameOut, "");

            STLinIO stl = new STLinIO();

            string[] name = nameOut.Split('.');
           
            stl.export(GLSettings.locateSTL + name[0] + ".stl", GLSettings.locateCAD + nameOut, "stl");   
        }

     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelo1"></param>
        /// <param name="modelo2"></param>
        /// <param name="nameOut"></param>
        /// <param name="OpenGLControl_"></param>
        public void UnionDesbaste(string modelo1, string modelo2, string nameOut, OpenGLControl OpenGLControl_)
        {
            string errorText = string.Empty;
            Model3D model3D;
            Model3D model3D_;

            model3D = OpenGLControl_.GLrender.LoadModel(modelo2, errorText);
            Models.Model3DAUX.Save_OBJ(model3D, modelo2, "");

            DMesh3 mesh1 = GeneralIO.LoadtMesh(modelo1);
            DMesh3 mesh2_desbaste = GeneralIO.LoadtMesh(modelo2);

            mesh1.ReverseOrientation();

            MeshEditor.Append(mesh1, mesh2_desbaste);        

            GeneralIO.SaveMesh(mesh1, GLSettings.locateCAD + nameOut);
            model3D_ = OpenGLControl_.GLrender.LoadModel(GLSettings.locateCAD + nameOut, errorText);
            Models.Model3DAUX.Save_OBJ(model3D_, GLSettings.locateCAD + nameOut, "");           
            MarchingCubes(GLSettings.locateCAD + nameOut);
           
            model3D_ = OpenGLControl_.GLrender.LoadModel(GLSettings.locateCAD + nameOut, errorText);
            Models.Model3DAUX.Save_OBJ(model3D_, GLSettings.locateCAD + nameOut, "");                   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OpenGLControl"></param>
        /// <param name="indice_"></param>
        /// <param name="selectView_"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="sz"></param>
        public void Scale(OpenGLControl OpenGLControl, int indice_, string selectView_, double sx, double sy, double sz)
        {
            Vector3d scale = new Vector3d(sx, sy, sz);
            Vector3d origen = new Vector3d(1, 1, 1);        
            DMesh3 mesh = GeneralIO.LoadtMesh(ModeloAux);
            MeshTransforms.Scale(mesh, scale, origen);

            OpenGLControl.RefreshShowModels(indice_, selectView_, "*", GLSettings.ModeloAuxOut_);
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OpenGLControl"></param>
        /// <param name="tipo"></param>
        public void atualizarStatusMarcacao(OpenGLControl OpenGLControl, String tipo)
        {                                           
            if(tipo == "atualizar")
            { 
                foreach (var j in GLSettings.vertexMarcacao)
                {
                    OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList[j].Color[0] = 0.4f;
                    OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList[j].Color[1] = 0.8f;
                    OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList[j].Color[2] = 0.8f;
                }
           // OpenGLControl.ChangeDisplayMode(OpenGLControl.GLrender.Models3D[].ModelRenderStyle);
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OpenGLControl"></param>
        /// <param name="tipo"></param>
        public void removerPontos(OpenGLControl OpenGLControl, String tipo)
        {
            DMesh3 mesh = IO.GeneralIO.LoadtMesh(ModeloAux);         

            if (OpenGLControl.modelsListSelect - 1 >= 0)
            {
                if(GLSettings.marcarRegiaoOn)
                { 
                    for (int i = 0; i < OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList.Count; i++)
                    {
                        if (OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList[i].Color[0] == 0.2f)
                        {                         
                           mesh.RemoveVertex(i, true, false);                        
                        }
                    }
                    IO.GeneralIO.SaveMesh(mesh, GLSettings.locateTMP + GLSettings.ModeloAuxOut_);
                }
                else if(GLSettings.marcacaoPolygonOn && !GLSettings.marcacaoPolygonOnRemove)
                {
                    for (int i = 0; i < OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList.Count; i++)
                    {
                        if (!(OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList[i].Color[0] == 0.4f))
                        {                          
                            mesh.RemoveVertex(i, true, false);
                        }
                    }
                    IO.GeneralIO.SaveMesh(mesh, GLSettings.locateTMP + GLSettings.ModeloAuxOut_);

                    GLSettings.vertexMarcacao.Clear();
                }

                else if (GLSettings.marcacaoPolygonOnRemove)
                {
                    for (int i = 0; i < OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList.Count; i++)
                    {
                        if ((OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList[i].Color[0] == 0.4f))
                        {
                            mesh.RemoveVertex(i, true, false);
                        }
                    }
                    IO.GeneralIO.SaveMesh(mesh, GLSettings.locateTMP + GLSettings.ModeloAuxOut_);

                    GLSettings.vertexMarcacao.Clear();
                }

                if (tipo == "removerTriangulos")
                {                 
                    /*
                    for(int vertex = 0; vertex < GLSettings.vertexMarcacao.Count(); vertex ++)
                    {                    
                      
                    for (int j = 0; j < OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].Parts[0].Triangles.Count(); j++)
                        { 
                            if(((OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].Parts[0].Triangles[j].IndVertices[0])  == GLSettings.vertexMarcacao[vertex]) || (OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].Parts[0].Triangles[j].IndVertices[1] == GLSettings.vertexMarcacao[vertex]) || (OpenGLControl.GLrender.Models3D[0].Parts[0].Triangles[j].IndVertices[2] == GLSettings.vertexMarcacao[vertex]))
                            {                                  
                                OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].Parts[0].Triangles.RemoveAt(j);                                  
                            }
                        }                            
                    }      
                    */
                }
                else
                {
                    for (int i = 0; i < OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList.Count; i++)
                    {
                        if (OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList[i].Color[0] == 0.2f)
                        {
                            if (!(OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].Parts[0].Triangles.Count > 1))
                            {
                                OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].VertexList.RemoveAt(i);
                            }
                        }
                        else
                        {                   
                            WarningView sf = new WarningView(1);
                            sf.ShowDialog();
                            break;
                        }
                    }                            
                }             
                OpenGLControl.ChangeDisplayMode(OpenGLControl.GLrender.Models3D[OpenGLControl.modelsListSelect - 1].ModelRenderStyle);              
            }
            else
            {
                WarningView sf = new WarningView(0);
                sf.ShowDialog();
            }
        }       
    }
}
