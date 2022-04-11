using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using g3;
using OpenTK.Graphics.OpenGL;

namespace PCLLib.CNC
{
    public class Gcode
    {
        private static bool zEncontrado = false;
        private static bool MESH = false;
        private static bool MESH_NONMESH = false;
        private static bool TYPE_SKIN = false;
        private static bool initZ = true;
     
        /*
        private static Stream saida1Vbs = File.Create("slicer\\gcode1.vbs");
        private static StreamWriter escritor1Vbs = new StreamWriter(saida1Vbs);
        private static Stream saida2Vbs = File.Create("slicer\\gcode2.vbs");
        private static StreamWriter escritor2Vbs = new StreamWriter(saida2Vbs);
        private static Stream saida3Vbs = File.Create("slicer\\gcode3.vbs");
        private static StreamWriter escritor3Vbs = new StreamWriter(saida3Vbs);
        */
        private static bool G92 = false;
        private static bool M107 = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="nameModelIn"></param>
        /// <param name="nameGcode"></param>
        /// <param name="smoothing"></param>
        private void MountBat(string name, string path, string nameModelIn, string nameGcode, bool smoothing)
        {
            Stream saida = File.Create("slicer\\" + name);
            StreamWriter escritor = new StreamWriter(saida);

            if (!smoothing)
            {
                /*
                escritor.WriteLine("slicer\\Slic3r-console.exe" + " " + path + nameModelIn +
                " --gcode-flavor "              + GLSettings.flavor +
              //  " --use-firmware-retraction "   + GLSettings.use_firmware_retraction +
                " --retract-before-travel "     + "0.5" +
                " --perimeters "                + GLSettings.perimeters +
                " --print-center "              + GLSettings.print_center + 
                " --layer-height "              + GLSettings.layer_height + 
                " --fill-density "              + GLSettings.fill_density + 
                " --solid-infill-below-area "   + GLSettings.solid_infill_below_area +
                " --solid-infill-every-layers " + GLSettings.solid_infill_every_layers + 
                " --fill-angle "                + GLSettings.fill_angle +
                " --filament-diameter "         + GLSettings.filament_diameter + 
                " --nozzle-diameter "           + GLSettings.nozzle_diameter + 
                " --bottom-solid-layers "       + GLSettings.bottom_solid_layers +
                " --top-solid-layers "          + GLSettings.top_solid_layers +
                " --output "                    + GLSettings.locateTMP + nameGcode);
                escritor.Close();
                */
                escritor.WriteLine("slicer\\cura\\CuraEngine.exe slice -v -j slicer\\cura\\resources\\definitions\\cnc_tecnodrill.def.json -o " + 
                GLSettings.locateTMP + nameGcode + " -l " + path + nameModelIn);
                escritor.Close();
            }
            else
            {
                /*
                escritor.WriteLine("slicer\\Slic3r-console.exe" + " " + path + nameModelIn +
                " --gcode-flavor "              + GLSettings.flavorA +
               // " --use-firmware-retraction "   + GLSettings.use_firmware_retractionA +
                " --perimeters "                + GLSettings.perimetersA +
                " --print-center "              + GLSettings.print_centerA +
                " --layer-height "              + GLSettings.layer_heightA + 
                " --fill-density "              + GLSettings.fill_densityA + 
                " --solid-infill-below-area "   + GLSettings.solid_infill_below_areaA + 
                " --solid-infill-every-layers " + GLSettings.solid_infill_every_layersA + 
                " --fill-angle "                + GLSettings.fill_angleA + 
                " --filament-diameter "         + GLSettings.filament_diameterA + 
                " --nozzle-diameter "           + GLSettings.nozzle_diameterA +
                " --bottom-solid-layers "       + GLSettings.bottom_solid_layersA + 
                " --top-solid-layers "          + GLSettings.top_solid_layersA +          
                " --output "                    + GLSettings.locateTMP + nameGcode);
                escritor.Close();
                */
                escritor.WriteLine("slicer\\cura\\CuraEngine.exe slice -v -j slicer\\cura\\resources\\definitions\\cnc_tecnodrilla.def.json -o " +
                GLSettings.locateTMP + nameGcode + " -l " + path + nameModelIn);
                escritor.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        private void MountVbs(string name)
        {
            Stream saida = File.Create("slicer\\" + name + ".vbs");
            StreamWriter escritor = new StreamWriter(saida);

            escritor.WriteLine("Dim wshShell");
            escritor.WriteLine("Set wshShell = CreateObject(\"WScript.Shell\")");
            escritor.WriteLine("wshShell.Run \"slicer\\" + name + ".bat\", 0, false");
            escritor.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public void gerarGcodeIndividual()
        {
            MountBat("gcode.bat", GLSettings.locateCAD, GLSettings.ModeloAux, GLSettings.Individual_G, false);
            MountVbs("gcode");

            if (File.Exists(GLSettings.locateTMP + GLSettings.Desbaste_Complemento1))
            {
                MountBat("gcoded.bat", GLSettings.locateTMP, GLSettings.Desbaste_Complemento1, GLSettings.Desbaste_ComplementoG, false);
                MountVbs("gcoded");
            }

            ExecutarCMD("./slicer/gcode.vbs");
            if (File.Exists(GLSettings.locateSLICER + "gcoded.vbs"))
            {
                ExecutarCMD("./slicer/gcoded.vbs");
            }

            while (!File.Exists(GLSettings.locateTMP + GLSettings.Individual_G)) { }
            CAMGenerate_(GLSettings.Individual_G, GLSettings.Individual_NC, false);

            /*
            if(File.Exists(GLSettings.locateMALHA + GLSettings.Individual_NC))
            {
                File.Delete(GLSettings.locateMALHA + GLSettings.Individual_NC);
                File.Copy(GLSettings.locateTMP + GLSettings.Individual_NC, GLSettings.locateCAM + GLSettings.Individual_NC);
            }
            else
            { 
                File.Copy(GLSettings.locateTMP + GLSettings.Individual_NC, GLSettings.locateCAM + GLSettings.Individual_NC);
            }
            */
            if (File.Exists(GLSettings.locateSLICER + "gcoded.vbs"))
            {
                if (File.Exists(GLSettings.locateTMP + GLSettings.Desbaste_ComplementoG))
                {
                    CAMGenerate_(GLSettings.Desbaste_ComplementoG, GLSettings.Desbaste_ComplementoNC, false);
                    while (!File.Exists(GLSettings.locateCAM + GLSettings.Desbaste_ComplementoNC)) { }
                    unionGcode(GLSettings.Desbaste_ComplementoNC, GLSettings.Individual_NC);
                    File.Delete(GLSettings.locateCAM + GLSettings.Desbaste_ComplementoNC);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void GcodeGenerate(bool part)
        {      
            switch (GLSettings.numberDivblocoExecutado)
            {
                case 0:
                case 1:
                    if(part)
                    {
                        MountBat("gcode1.bat", GLSettings.locateSTL, GLSettings.Bloco1Out_Tringulos_solido_desbasteSTL, GLSettings.Bloco1_solido_desbaste_G, false);
                        MountVbs("gcode1");
                        MountBat("gcode1d.bat", GLSettings.locateTMP, GLSettings.Desbaste_Complemento1, GLSettings.Bloco1_solido_acabamento_G, true);
                        MountVbs("gcode1d");
                    }
                    else
                    {
                        MountBat("gcode1.bat", GLSettings.locateSTL, GLSettings.Bloco1Out_Tringulos_solido_desbasteSTL, GLSettings.Bloco1_solido_desbaste_G, false);
                        MountVbs("gcode1");
                        MountBat("gcode1d.bat", GLSettings.locateTMP, GLSettings.Desbaste_Complemento1, GLSettings.Bloco1_solido_acabamento_G, true);
                        MountVbs("gcode1d");
                    }         
                    break;
                case 2:
                    if(part)
                    {
                        MountBat("gcode1.bat", GLSettings.locateSTL, GLSettings.Bloco1Out_Tringulos_solido_desbasteSTL, GLSettings.Bloco1_solido_desbaste_G, false);
                        MountVbs("gcode1");
                        MountBat("gcode1d.bat", GLSettings.locateTMP, GLSettings.Desbaste_Complemento1, GLSettings.Bloco1_solido_acabamento_G, true);
                        MountVbs("gcode1d");

                        MountBat("gcode2.bat", GLSettings.locateSTL, GLSettings.Bloco2Out_Tringulos_solido_desbasteSTL, GLSettings.Bloco2_solido_desbaste_G, false);
                        MountVbs("gcode2");
                        MountBat("gcode2d.bat", GLSettings.locateTMP, GLSettings.Desbaste_Complemento2, GLSettings.Bloco2_solido_acabamento_G, true);
                        MountVbs("gcode2d");
                    }
                    else
                    {
                        MountBat("gcode1.bat", GLSettings.locateSTL, GLSettings.Bloco1Out_Tringulos_solido_desbasteSTL, GLSettings.Bloco1_solido_desbaste_G, false);
                        MountVbs("gcode1");
                        MountBat("gcode1d.bat", GLSettings.locateTMP, GLSettings.Desbaste_Complemento1, GLSettings.Bloco1_solido_acabamento_G, true);
                        MountVbs("gcode1d");

                        MountBat("gcode2.bat", GLSettings.locateSTL, GLSettings.Bloco2Out_Tringulos_solido_desbasteSTL, GLSettings.Bloco2_solido_desbaste_G, false);
                        MountVbs("gcode2");
                        MountBat("gcode2d.bat", GLSettings.locateTMP, GLSettings.Desbaste_Complemento2, GLSettings.Bloco2_solido_acabamento_G, true);
                        MountVbs("gcode2d");
                    }
                  
                    break;
                case 3:
                    if(part)
                    {
                        MountBat("gcode1.bat", GLSettings.locateSTL, GLSettings.Bloco1InvOutSTL, GLSettings.Bloco1_solido_desbaste_G, false);
                        MountVbs("gcode1");
                        MountBat("gcode1d.bat", GLSettings.locateTMP, GLSettings.Desbaste_Complemento1, GLSettings.Bloco1_solido_acabamento_G, true);
                        MountVbs("gcode1d");

                        MountBat("gcode2.bat", GLSettings.locateSTL, GLSettings.Bloco2InvOutSTL, GLSettings.Bloco2_solido_desbaste_G, false);
                        MountVbs("gcode2");
                        MountBat("gcode2d.bat", GLSettings.locateTMP, GLSettings.Desbaste_Complemento2, GLSettings.Bloco2_solido_acabamento_G, true);
                        MountVbs("gcode2d");

                        MountBat("gcode3.bat", GLSettings.locateSTL, GLSettings.Bloco3InvOutSTL, GLSettings.Bloco3_solido_desbaste_G, false);
                        MountVbs("gcode3");
                        MountBat("gcode3d.bat", GLSettings.locateTMP, GLSettings.Desbaste_Complemento3, GLSettings.Bloco3_solido_acabamento_G, true);
                        MountVbs("gcode3d");
                    }
                    else
                    {
                        MountBat("gcode1.bat", GLSettings.locateSTL, GLSettings.Bloco1InvOutSTL, GLSettings.Bloco1_solido_desbaste_G, false);
                        MountVbs("gcode1");
                        MountBat("gcode1d.bat", GLSettings.locateTMP, GLSettings.Desbaste_Complemento1, GLSettings.Bloco1_solido_acabamento_G, true);
                        MountVbs("gcode1d");

                        MountBat("gcode2.bat", GLSettings.locateSTL, GLSettings.Bloco2InvOutSTL, GLSettings.Bloco2_solido_desbaste_G, false);
                        MountVbs("gcode2");
                        MountBat("gcode2d.bat", GLSettings.locateTMP, GLSettings.Desbaste_Complemento2, GLSettings.Bloco2_solido_acabamento_G, true);
                        MountVbs("gcode2d");

                        MountBat("gcode3.bat", GLSettings.locateSTL, GLSettings.Bloco3InvOutSTL, GLSettings.Bloco3_solido_desbaste_G, false);
                        MountVbs("gcode3");
                        MountBat("gcode3d.bat", GLSettings.locateTMP, GLSettings.Desbaste_Complemento3, GLSettings.Bloco3_solido_acabamento_G, true);
                        MountVbs("gcode3d");
                    }                 
                    break;
            }
          
            switch (GLSettings.numberDivblocoExecutado)
            {
                case 0:
                case 1:
                    //Thread.Sleep(15000);
                    ExecutarCMD("./slicer/gcode1.vbs");
                    if(File.Exists(GLSettings.locateSLICER + "gcode1d.vbs"))
                    {                        
                        ExecutarCMD("./slicer/gcode1d.vbs");
                    }
                    break;
                case 2:
                    //Thread.Sleep(15000);
                    ExecutarCMD("./slicer/gcode1.vbs");
                    if(File.Exists(GLSettings.locateSLICER + "gcode1d.vbs"))
                    {                        
                        ExecutarCMD("./slicer/gcode1d.vbs");
                    }
                    ExecutarCMD("./slicer/gcode2.vbs");
                    if(File.Exists(GLSettings.locateSLICER + "gcode2d.vbs"))
                    {                    
                        ExecutarCMD("./slicer/gcode2d.vbs");
                    }
                    break;
                case 3:
                    //Thread.Sleep(15000);
                    ExecutarCMD("./slicer/gcode1.vbs");
                    if(File.Exists(GLSettings.locateSLICER + "gcode1d.vbs"))
                    {                        
                        ExecutarCMD("./slicer/gcode1d.vbs");
                    }
                    ExecutarCMD("./slicer/gcode2.vbs");
                    if(File.Exists(GLSettings.locateSLICER + "gcode2d.vbs"))
                    {                     
                        ExecutarCMD("./slicer/gcode2d.vbs");
                    }
                    ExecutarCMD("./slicer/gcode3.vbs");
                    if(File.Exists(GLSettings.locateSLICER + "gcode3d.vbs"))
                    {                     
                        ExecutarCMD("./slicer/gcode3d.vbs");
                    }
                    break;
            }

            Thread.Sleep(30000);

            switch (GLSettings.numberDivblocoExecutado)
            {
                case 0:
                case 1:                    
                    while (!File.Exists(GLSettings.locateTMP + GLSettings.Bloco1_solido_desbaste_G)) { }
                    CAMGenerateV2_(GLSettings.Bloco1_solido_desbaste_G, GLSettings.Bloco1_solido_desbaste_NC, false, 0);
                    CAMGenerateV2_(GLSettings.Bloco1_solido_acabamento_G, GLSettings.Bloco1_solido_acabamento_NC, true, 0);

                    if (GLSettings.modoFuncionamentoCorte == "Corte X")
                    {
                        CamXYToZYV2(GLSettings.Bloco1_solido_acabamento_NC, GLSettings.Bloco1_solido_desbaste_NC, GLSettings.Bloco1_solido_acabamentoXZ_NC, 1);
                    }
                    else
                    {
                        CamXYToZXV2(GLSettings.Bloco1_solido_acabamento_NC, GLSettings.Bloco1_solido_desbaste_NC, GLSettings.Bloco1_solido_acabamentoXZ_NC, 1);
                    }

                    RedirectingNumberingV2(GLSettings.Bloco1_solido_acabamentoXZ_NC, GLSettings.Bloco1_solido_desbaste_NC, GLSettings.Bloco1_solido_AD_NC); 
                    break;
                case 2:                  
                    while (!File.Exists(GLSettings.locateTMP + GLSettings.Bloco1_solido_desbaste_G)) { }
                    CAMGenerateV2_(GLSettings.Bloco1_solido_desbaste_G, GLSettings.Bloco1_solido_desbaste_NC, false, 1);
                    CAMGenerateV2_(GLSettings.Bloco1_solido_acabamento_G, GLSettings.Bloco1_solido_acabamento_NC, true, 1);

                    if (GLSettings.modoFuncionamentoCorte == "Corte X")
                    {
                        CamXYToZYV2(GLSettings.Bloco1_solido_acabamento_NC, GLSettings.Bloco1_solido_desbaste_NC, GLSettings.Bloco1_solido_acabamentoXZ_NC, 1);
                    }
                    else
                    { 
                        CamXYToZXV2(GLSettings.Bloco1_solido_acabamento_NC, GLSettings.Bloco1_solido_desbaste_NC, GLSettings.Bloco1_solido_acabamentoXZ_NC, 1);
                    }

                    RedirectingNumberingV2(GLSettings.Bloco1_solido_acabamentoXZ_NC, GLSettings.Bloco1_solido_desbaste_NC, GLSettings.Bloco1_solido_AD_NC);
                    while (!File.Exists(GLSettings.locateTMP + GLSettings.Bloco2_solido_desbaste_G)) { }
                    CAMGenerateV2_(GLSettings.Bloco2_solido_desbaste_G, GLSettings.Bloco2_solido_desbaste_NC, false, 2);
                    CAMGenerateV2_(GLSettings.Bloco2_solido_acabamento_G, GLSettings.Bloco2_solido_acabamento_NC, true, 2);

                    if (GLSettings.modoFuncionamentoCorte == "Corte X")
                    {
                        CamXYToZYV2(GLSettings.Bloco2_solido_acabamento_NC, GLSettings.Bloco2_solido_desbaste_NC, GLSettings.Bloco2_solido_acabamentoXZ_NC, 2);
                    }
                    else
                    {                    
                        CamXYToZXV2(GLSettings.Bloco2_solido_acabamento_NC, GLSettings.Bloco2_solido_desbaste_NC, GLSettings.Bloco2_solido_acabamentoXZ_NC, 2);
                    }

                    RedirectingNumberingV2(GLSettings.Bloco2_solido_acabamentoXZ_NC, GLSettings.Bloco2_solido_desbaste_NC, GLSettings.Bloco2_solido_AD_NC);
                    break;
                case 3:                   
                    while (!File.Exists(GLSettings.locateTMP + GLSettings.Bloco1_solido_desbaste_G)) { }
                    CAMGenerateV2_(GLSettings.Bloco1_solido_desbaste_G, GLSettings.Bloco1_solido_desbaste_NC, false, 1);
                    CAMGenerateV2_(GLSettings.Bloco1_solido_acabamento_G, GLSettings.Bloco1_solido_acabamento_NC, true, 1);

                    if (GLSettings.modoFuncionamentoCorte == "Corte X")
                    {
                        CamXYToZYV2(GLSettings.Bloco1_solido_acabamento_NC, GLSettings.Bloco1_solido_desbaste_NC, GLSettings.Bloco1_solido_acabamentoXZ_NC, 1);
                    }
                    else
                    {                    
                        CamXYToZXV2(GLSettings.Bloco1_solido_acabamento_NC, GLSettings.Bloco1_solido_desbaste_NC, GLSettings.Bloco1_solido_acabamentoXZ_NC, 1);
                    }

                    RedirectingNumberingV2(GLSettings.Bloco1_solido_acabamentoXZ_NC, GLSettings.Bloco1_solido_desbaste_NC, GLSettings.Bloco1_solido_AD_NC);
                    while (!File.Exists(GLSettings.locateTMP + GLSettings.Bloco2_solido_desbaste_G)) { }
                    CAMGenerateV2_(GLSettings.Bloco2_solido_desbaste_G, GLSettings.Bloco2_solido_desbaste_NC, false, 2);
                    CAMGenerateV2_(GLSettings.Bloco2_solido_acabamento_G, GLSettings.Bloco2_solido_acabamento_NC, true, 2);

                    if (GLSettings.modoFuncionamentoCorte == "Corte X")
                    {
                        CamXYToZYV2(GLSettings.Bloco2_solido_acabamento_NC, GLSettings.Bloco2_solido_desbaste_NC, GLSettings.Bloco2_solido_acabamentoXZ_NC, 2);
                    }
                    else
                    {                    
                        CamXYToZXV2(GLSettings.Bloco2_solido_acabamento_NC, GLSettings.Bloco2_solido_desbaste_NC, GLSettings.Bloco2_solido_acabamentoXZ_NC, 2);
                    }

                    RedirectingNumberingV2(GLSettings.Bloco2_solido_acabamentoXZ_NC, GLSettings.Bloco2_solido_desbaste_NC, GLSettings.Bloco2_solido_AD_NC);
                    while (!File.Exists(GLSettings.locateTMP + GLSettings.Bloco3_solido_desbaste_G)) { }
                    CAMGenerateV2_(GLSettings.Bloco3_solido_desbaste_G, GLSettings.Bloco3_solido_desbaste_NC, false, 3);
                    CAMGenerateV2_(GLSettings.Bloco3_solido_acabamento_G, GLSettings.Bloco3_solido_acabamento_NC, true, 3);

                    if (GLSettings.modoFuncionamentoCorte == "Corte X")
                    {
                        CamXYToZYV2(GLSettings.Bloco3_solido_acabamento_NC, GLSettings.Bloco3_solido_desbaste_NC, GLSettings.Bloco3_solido_acabamentoXZ_NC, 3);
                    }
                    else
                    {                    
                        CamXYToZXV2(GLSettings.Bloco3_solido_acabamento_NC, GLSettings.Bloco3_solido_desbaste_NC, GLSettings.Bloco3_solido_acabamentoXZ_NC, 3);
                    }

                    RedirectingNumberingV2(GLSettings.Bloco3_solido_acabamentoXZ_NC, GLSettings.Bloco3_solido_desbaste_NC, GLSettings.Bloco3_solido_AD_NC);
                    break;
            }
        }

        /// <summary>
        /// Algoritmo de numeração versão 1
        /// </summary>
        /// <param name="acabamento"></param>
        /// <param name="desbaste"></param>
        /// <param name="out_"></param>
        private void RedirectingNumbering(string acabamento, string desbaste, string out_)
        {
            string linha;
            List<String> linhasAcabamento = new List<string>();
            List<String> linhasDesbaste = new List<string>();
            List<String> linhasDesbasteAcabamento = new List<string>();

            File.Copy(GLSettings.locateTMP + acabamento, GLSettings.locateCAM + acabamento);

            Stream acabamentoNC = File.Open(GLSettings.locateCAM + acabamento, FileMode.Open);
            StreamReader acabamentoNCR = new StreamReader(acabamentoNC);

            Stream desbasteNC = File.Open(GLSettings.locateCAM + desbaste, FileMode.Open);
            StreamReader desbasteNCR = new StreamReader(desbasteNC);

            Stream desbasteAcabamentoNC = File.Create(GLSettings.locateCAM + out_);
            StreamWriter desbasteAcabamentoNCW = new StreamWriter(desbasteAcabamentoNC);

            linha = acabamentoNCR.ReadLine();
            
            while (linha != null)
            {
                linhasAcabamento.Add(linha);
                linha = acabamentoNCR.ReadLine();
            }

            linha = desbasteNCR.ReadLine();

            while (linha != null)
            {
                linhasDesbaste.Add(linha);
                linha = desbasteNCR.ReadLine();
            }

            foreach(var linha_ in linhasDesbaste)
            {
                if (linha_ != "M30") linhasDesbasteAcabamento.Add(linha_);
            }

            /*
            // Remanezar esta parte para o acabamento//
            double cont = 0;
            foreach (var linha_ in linhasAcabamento)
            {
                if (linha_.StartsWith("G1 X"))
                {
                    cont++;
                }
            }

            */
            bool startInit = false; 
           // double comp = 0;
            foreach (var linha_ in linhasAcabamento)
            {
                //if (linha_.StartsWith("G1 X"))
                //{
                //    comp++;
                //}

                //if (comp < cont)
               // {
                    //if (linha_.StartsWith("G0 Z0") || startInit)
                    //{                        
                        linhasDesbasteAcabamento.Add(linha_);
                        //startInit = true;                        
                    //}
                //}
            }

            linhasDesbasteAcabamento.Add("M30");

            foreach (var linha_ in linhasDesbasteAcabamento)
            {
                desbasteAcabamentoNCW.WriteLine(linha_);
            }

            acabamentoNCR.Close();
            acabamentoNC.Close();
            desbasteNCR.Close();
            desbasteNC.Close();
            desbasteAcabamentoNCW.Close();
            desbasteAcabamentoNC.Close();
        }

        /// <summary>
        /// Algoritmo de numeração versão 2
        /// </summary>
        /// <param name="acabamento"></param>
        /// <param name="desbaste"></param>
        /// <param name="out_"></param>
        private void RedirectingNumberingV2(string acabamento, string desbaste, string out_)
        {
            string linha, tmp;
            List<String> linhasAcabamento = new List<string>();
            List<String> linhasDesbaste = new List<string>();
            List<String> linhasDesbasteAcabamento = new List<string>();                     

            Stream acabamentoNC = File.Open(GLSettings.locateTMP + "A" + acabamento, FileMode.Open);
            StreamReader acabamentoNCR = new StreamReader(acabamentoNC);

            Stream desbasteNC = File.Open(GLSettings.locateTMP + "D" + desbaste, FileMode.Open);
            StreamReader desbasteNCR = new StreamReader(desbasteNC);

            Stream acabamentoNC_ = File.Create(GLSettings.locateCAM + acabamento);
            StreamWriter acabamentoNCW = new StreamWriter(acabamentoNC_);

            Stream desbasteNC_ = File.Create(GLSettings.locateCAM + desbaste);
            StreamWriter desbasteNCW = new StreamWriter(desbasteNC_);

            Stream desbasteAcabamentoNC = File.Create(GLSettings.locateCAM + out_);
            StreamWriter desbasteAcabamentoNCW = new StreamWriter(desbasteAcabamentoNC);

            #region  Acabamento
            linha = acabamentoNCR.ReadLine();
            double number = 0;
            while (linha != null)
            {
                //linha = linha.Replace("G0", "G1");                  
                linhasAcabamento.Add("N" + number + " " + linha);
                number++;
                linha = acabamentoNCR.ReadLine();
            }
            #endregion Acabamento

            #region Desbaste         
            linha = desbasteNCR.ReadLine();
            number = 0;
            while (linha != null)
            {
                //linha = linha.Replace("G0", "G1");
                linhasDesbaste.Add("N" + number + " " + linha);
                number++;
                linha = desbasteNCR.ReadLine();
            }
            #endregion Desbaste

            #region Desbaste e acabamento
            number = 0;           
            foreach (var linha_ in linhasDesbaste)
            {
                tmp = linha_.Substring(linha_.IndexOf(' '));
                if (!tmp.StartsWith(" M30"))
                {
                    try
                    {
                        tmp = linha_.Substring(linha_.IndexOf(' '));
                        linhasDesbasteAcabamento.Add("N" + number + tmp);
                        number++;
                    }
                    catch
                    {
                        System.Console.WriteLine("Desbaste: " + "N" + number);
                    }
                   
                }               
            }            
            foreach (var linha_ in linhasAcabamento)
            {
                tmp = linha_.Substring(linha_.IndexOf(' '));
                if (!tmp.StartsWith(" M30") && !tmp.StartsWith(" S") && !tmp.StartsWith(" G54.1")) 
                {
                    try
                    {                       
                        linhasDesbasteAcabamento.Add("N" + number + tmp);
                        number++;
                    }
                    catch 
                    {
                        System.Console.WriteLine("Acabamento: " + "N" + number);
                    }                  
                }
            }

            linhasDesbasteAcabamento.Add("N" + number + " " + "M30");

            #endregion Desbaste e acabamento

            #region Write 
            //Adicionando em arquivo//
            foreach (var linha_ in linhasAcabamento)
            {
                acabamentoNCW.WriteLine(linha_);
            }

            foreach (var linha_ in linhasDesbaste)
            {
                desbasteNCW.WriteLine(linha_);
            }

            foreach (var linha_ in linhasDesbasteAcabamento)
            {
                desbasteAcabamentoNCW.WriteLine(linha_);
            }
            #endregion Write 

            desbasteAcabamentoNCW.Close();//     
            acabamentoNCW.Close();//         
            desbasteNCW.Close();//
         }

        /// <summary>
        /// Mudança de cordenada versão 1
        /// </summary>
        /// <param name="path"></param>
        /// <param name="desbasteAux"></param>
        /// <param name="out_"></param>
        private void CamXYToZY(string path, string desbasteAux, string out_)
        {
            /* Salvando as informações do complement do desbaste em uma List
             */
            GLSettings.ajusteX_deslocamento = 0;
            GLSettings.ajusteZ_deslocamento = 0;
            List<String> acabamentoR= new List<string>();
            List<String> acabamentoRAUX = new List<string>();
            List<String> acabamentoRAUX_NOT_Return = new List<string>();
            List<String> acabamentoRAUX_Aux = new List<string>();
            List<String> acabamentoRAUX_Aux_inveter = new List<string>();

            /* O foi criado um arquivo auxiliar com um A na ajuste de algumas rotas, mas devido problemas 
             * de proibição de escrita foi feito este desdrobre, resolver posteriormente.
             */

            Stream acabamentoNC = File.Open(GLSettings.locateTMP + "A" + path, FileMode.Open);
            StreamReader acabamentoNCR = new StreamReader(acabamentoNC);

            Stream desbasteAux_ = File.Open(GLSettings.locateCAM + desbasteAux, FileMode.Open);
            StreamReader desbasteAux_r = new StreamReader(desbasteAux_);

            string Aux = desbasteAux_r.ReadLine();
            while (Aux != null)
            {
                if ((Aux.StartsWith("G1")) || (Aux.StartsWith("G0")))
                {
                    string[] quebraEspaco = new string[6];

                    quebraEspaco = Aux.Split(' ');

                    if (quebraEspaco[1].StartsWith("X"))
                    {                        
                        string ajusteX = quebraEspaco[1].Replace("X", "");
                        PositionZY(ajusteX, false);
                        Aux = desbasteAux_r.ReadLine();
                    }
                    else
                    {
                        Aux = desbasteAux_r.ReadLine();
                    }
                }
                else
                {
                    Aux = desbasteAux_r.ReadLine();
                }
            }            

            string linha = acabamentoNCR.ReadLine();

            while (linha != null)
            {
                if (linha.StartsWith("G1"))
                {
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linha.Split(' ');

                    if (quebraEspaco[1].StartsWith("X"))
                    {
                        string linha_ = linha.Replace("X", "*");
                        string ajusteZ = quebraEspaco[1].Replace("X", "");
                        PositionZY(ajusteZ, true);
                        acabamentoR.Add(linha_);
                        linha = acabamentoNCR.ReadLine();                                              
                    }                                     
                    else
                    {
                        acabamentoR.Add(linha);
                        linha = acabamentoNCR.ReadLine();
                    }
                }               
                else if(linha.StartsWith("G0"))
                {                  
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linha.Split(' ');

                    if (quebraEspaco[1].StartsWith("X"))
                    {
                        linha = "G0" + " " + "Z0";
                        acabamentoR.Add(linha);
                        linha = "G0" + " " + quebraEspaco[2];
                        acabamentoR.Add(linha);

                        linha = acabamentoNCR.ReadLine();
                    }
                    else
                    {                        
                        linha = acabamentoNCR.ReadLine();                        
                    }                   
                }
                else
                {
                    acabamentoR.Add(linha);
                    linha = acabamentoNCR.ReadLine();
                }
            }

            foreach(var linha_ in acabamentoR)
            {
                if ((linha_.StartsWith("G1")))
                {
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linha_.Split(' ');

                    if (quebraEspaco[1].StartsWith("Z-"))
                    {                     
                        string linha__ = linha_.Replace("Z-", "X");
                        acabamentoRAUX.Add(linha__);                     
                    }
                    else if(quebraEspaco[1].StartsWith("Z"))
                    {
                        string linha__ = linha_.Replace("Z", "X");
                        acabamentoRAUX.Add(linha__);
                    }
                    else
                    {
                        acabamentoRAUX.Add(linha_);       
                    }
                }
                else
                {
                    acabamentoRAUX.Add(linha_);                    
                }
            }

            acabamentoR.Clear();

            foreach (var linha_aux in acabamentoRAUX)
            {
                if (linha_aux.StartsWith("G1"))
                {
                    string[] quebraEspaco = new string[6];

                    quebraEspaco = linha_aux.Split(' ');

                    if (quebraEspaco[1].StartsWith("*"))
                    {
                        string linha__ = linha_aux.Replace("*", "Z");
                        acabamentoR.Add(linha__);                   
                    }
                    else
                    {
                        acabamentoR.Add(linha_aux);                      
                    }
                }
                else
                {
                    acabamentoR.Add(linha_aux);               
                }
            }

            acabamentoRAUX.Clear();

            double cont = 0;

            foreach (var linha_aux in acabamentoR)
            {
                string result;
                cont++;
                if (linha_aux.StartsWith("G1"))
                {
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linha_aux.Split(' ');
                      
                    if (quebraEspaco[1].StartsWith("X"))
                    {                        
                        string ajusteX = quebraEspaco[1].Replace("X", "");
                        float valorAtual = float.Parse(ajusteX, System.Globalization.CultureInfo.InvariantCulture);

                        valorAtual += GLSettings.ajusteX_deslocamento - 12;                    

                        if (quebraEspaco.Count() == 3) 
                        {
                            result = "G1" + " " + "X" + valorAtual.ToString().Replace(',','.') + " " + quebraEspaco[2];                         
                        }
                        else
                        {
                            result = "G1" + " " + "X" + valorAtual.ToString().Replace(',', '.');                          
                        }
                        acabamentoRAUX.Add(result);                       
                    }
                    else if (quebraEspaco[1].StartsWith("Z"))
                    {
                        string ajusteX = quebraEspaco[1].Replace("Z", "");
                        float valorAtual_ = float.Parse(ajusteX, System.Globalization.CultureInfo.InvariantCulture);

                        string result_;
                                                                    

                        //if ((((cont > acabamentoR.Count() / 5)) && (cont < (acabamentoR.Count() - 10))) || valorAtual_ < 30 )
                        //{
                            if (quebraEspaco.Count() == 3)
                            {
                              result_ = "G1" + " " + "Z" + (valorAtual_ + (GLSettings.ajusteZ_deslocamento) - ((GLSettings.stepLayersDesbaste) - (GLSettings.stepLayersDesbaste / 2))).ToString().Replace(',', '.') + " " + quebraEspaco[2];
                            }
                            else
                            {
                               result_ = "G1" + " " + "Z" + (valorAtual_ + (GLSettings.ajusteZ_deslocamento) - ((GLSettings.stepLayersDesbaste) - (GLSettings.stepLayersDesbaste / 2))).ToString().Replace(',', '.');
                            }
                       //}
                        //else
                        //{
                        //    result_ = "G0" + " " + "Z0"; //utilizado para decidas pequenas não prejudicarem a usinagem 
                        //}
                        acabamentoRAUX.Add(result_);                        
                    }
                    else
                    {
                        acabamentoRAUX.Add(linha_aux);                     
                    }
                }                
                else
                {
                    acabamentoRAUX.Add(linha_aux);
                }
            }

            #region testes

            /* testes de melhorias 
             * 
            fatiaGcode = 0;
            extrimidadeYMin[fatiaGcode] = 10000;
            extrimidadeYMax[fatiaGcode] = 0;                        
            foreach (var line in acabamentoRAUX)
            {                                
                string[] quebraEspaco = new string[6];
                quebraEspaco = line.Split(' ');
                if (quebraEspaco.Count() > 1)
                {
                    if (quebraEspaco[0].StartsWith("G1"))
                    {
                        if (quebraEspaco[1].StartsWith("Z"))
                        {
                            string ajusteZ = quebraEspaco[1].Replace("Z", "");
                            float valorAtual_Z = float.Parse(ajusteZ, System.Globalization.CultureInfo.InvariantCulture);

                            string ajusteY = quebraEspaco[2].Replace("Y", "");
                            float valorAtual_Y = float.Parse(ajusteY, System.Globalization.CultureInfo.InvariantCulture);

                            if (valorAtual_Y + valorAtual_Z < extrimidadeYMin[fatiaGcode])
                            {
                                extrimidadeYMin[fatiaGcode] = valorAtual_Y + valorAtual_Z;
                                //extrimidadeYMinZ[fatiaGcode] = valorAtual_Z;
                            }

                            if (valorAtual_Y + valorAtual_Z > extrimidadeYMax[fatiaGcode])
                            {
                                extrimidadeYMax[fatiaGcode] = valorAtual_Y + valorAtual_Z;
                                //extrimidadeYMaxZ[fatiaGcode] = valorAtual_Z;
                            }
                        }
                        else if (quebraEspaco[1].StartsWith("X"))
                        {
                            fatiaGcode++;
                            extrimidadeYMin[fatiaGcode] = 1000;
                            extrimidadeYMax[fatiaGcode] = -1000;
                        }
                    }                  
                }
            }

            fatiaGcode = 0;
          
            foreach (var line in acabamentoRAUX)
            {
                string[] quebraEspaco = new string[6];
                quebraEspaco = line.Split(' ');
                if (quebraEspaco.Count() > 1)
                {
                    if (quebraEspaco[0].StartsWith("G1"))
                    {
                        if (quebraEspaco[1].StartsWith("Z") && retorno)
                        {

                            string ajusteX = quebraEspaco[1].Replace("Z", "");
                            float valorAtual_ = float.Parse(ajusteX, System.Globalization.CultureInfo.InvariantCulture);

                            string ajusteY = quebraEspaco[2].Replace("Y", "");
                            float valorAtual_Y = float.Parse(ajusteY, System.Globalization.CultureInfo.InvariantCulture);
                            try
                            {
                                int proximo = acabamentoRAUX.IndexOf(line);
                                string[] quebraEspacoProximo = new string[6];                         

                                quebraEspacoProximo = acabamentoRAUX[proximo + 1].Split(' ');           

                                if (quebraEspacoProximo[1].StartsWith("Z"))
                                {
                                    string ajusteXProximo = quebraEspacoProximo[1].Replace("Z", "");
                                    float valorAtual_Proximo = float.Parse(ajusteXProximo, System.Globalization.CultureInfo.InvariantCulture);                                    

                                    string ajusteYProximo = quebraEspacoProximo[2].Replace("Y", "");
                                    valorAtual_ProximoY = float.Parse(ajusteYProximo, System.Globalization.CultureInfo.InvariantCulture);
                                   
                                    
                                    if ((valorAtual_Y + valorAtual_ == extrimidadeYMax[fatiaGcode]))
                                    {
                                        retorno2 = 0;
                                    }
                                    
                                    
                                    if ((valorAtual_Y + valorAtual_ == extrimidadeYMin[fatiaGcode]))
                                    {
                                        retorno2 = 2;
                                    }
                                    
                                }
                            }
                            catch
                            {

                            }
                            if (retorno2 == 0)
                            {
                                if (initGcode)
                                {
                                    acabamentoRAUX_NOT_Return.Add(line);
                                }                               
                            }

                           // if (retorno2 > 0) retorno2--;
                        }
                        else if (quebraEspaco[1].StartsWith("X"))
                        {
                            retorno = true;
                            initGcode = true;
                            fatiaGcode++;
                            retorno2 = 0;
                            acabamentoRAUX_NOT_Return.Add(line);
                        }
                    }
                    
                    else if(quebraEspaco[0].StartsWith("G0"))
                    {
                        if (initGcode) acabamentoRAUX_NOT_Return.Add(line);
                    }
                    
                    else
                    {
                        if(initGcode) acabamentoRAUX_NOT_Return.Add(line);
                    }
                }                                            
                else
                {
                    if (initGcode) acabamentoRAUX_NOT_Return.Add(line);
                }
            }

            initGcode = false;
            
           
            foreach (var line in acabamentoRAUX_NOT_Return)
            {
                string[] quebraEspaco = new string[6];
                quebraEspaco = line.Split(' ');
                if (quebraEspaco.Count() > 1)
                {
                    if (quebraEspaco[0].StartsWith("G1"))
                    {
                        if (quebraEspaco[1].StartsWith("X"))
                        {
                            if (inverter)
                            {
                                for (int i = acabamentoRAUX_Aux_inveter.Count() - 1; i >= 0; i--)
                                {
                                    acabamentoRAUX_Aux.Add(acabamentoRAUX_Aux_inveter[i]);
                                }
                                acabamentoRAUX_Aux_inveter.Clear();
                                acabamentoRAUX_Aux.Add(line);
                            }
                            else
                            {
                                acabamentoRAUX_Aux.Add(line);
                            }
                            inverter = !inverter;
                        }
                        else if (quebraEspaco[1].StartsWith("Z"))
                        {
                            if (inverter) acabamentoRAUX_Aux_inveter.Add(line);
                            else acabamentoRAUX_Aux.Add(line);
                        }
                    }
                    else if (quebraEspaco[0].StartsWith("G0"))
                    {
                        //acabamentoRAUX_Aux.Add(line);
                    }
                }
                else
                {
                    acabamentoRAUX_Aux.Add(line);
                }
            }

            acabamentoRAUX_NOT_Return.Clear();


            foreach (var line in acabamentoRAUX_Aux)
            {
                string[] quebraEspaco = new string[6];
                quebraEspaco = line.Split(' ');
                if (quebraEspaco.Count() > 1)
                {
                    if (quebraEspaco[0].StartsWith("G1"))
                    {
                        if (quebraEspaco[1].StartsWith("Z") && retorno)
                        {

                            string ajusteX = quebraEspaco[1].Replace("Z", "");
                            float valorAtual_ = float.Parse(ajusteX, System.Globalization.CultureInfo.InvariantCulture);

                            string ajusteY = quebraEspaco[2].Replace("Y", "");
                            float valorAtual_Y = float.Parse(ajusteY, System.Globalization.CultureInfo.InvariantCulture);
                            try
                            {
                                int proximo = acabamentoRAUX_Aux.IndexOf(line);
                                string[] quebraEspacoProximo = new string[6];                         

                                quebraEspacoProximo = acabamentoRAUX_Aux[proximo + 1].Split(' ');              

                                if (quebraEspacoProximo[1].StartsWith("Z"))
                                {
                                    string ajusteXProximo = quebraEspacoProximo[1].Replace("Z", "");
                                    float valorAtual_Proximo = float.Parse(ajusteXProximo, System.Globalization.CultureInfo.InvariantCulture);

                                    string ajusteYProximo;

                                    ajusteYProximo = quebraEspacoProximo[2].Replace("Y", "");
                                    valorAtual_ProximoY = float.Parse(ajusteYProximo, System.Globalization.CultureInfo.InvariantCulture);


                                    if ((valorAtual_Y > (extrimidadeYMax[fatiaGcode] - (10 * extremidadeAjuste))) && (valorAtual_ProximoY < (extrimidadeYMin[fatiaGcode] + (10 * extremidadeAjuste))))
                                    {
                                        addG0Z0DE = true;
                                    }
                                    else if ((valorAtual_Y < (extrimidadeYMin[fatiaGcode] + (10 * extremidadeAjuste))) && (valorAtual_ProximoY > (extrimidadeYMax[fatiaGcode] - (10 * extremidadeAjuste))))
                                    {
                                        addG0Z0ED = true;
                                    }
                                }
                            }
                            catch
                            {}

                            acabamentoRAUX_NOT_Return.Add(line);
                            
                            if (addG0Z0DE)
                            {
                                acabamentoRAUX_NOT_Return.Add("G0 Z0");
                                acabamentoRAUX_NOT_Return.Add("G0 Z0" + " Y" + valorAtual_ProximoY.ToString());
                                addG0Z0DE = false;
                            }
                            else if (addG0Z0ED)
                            {
                                acabamentoRAUX_NOT_Return.Add("G0 Z0");
                                acabamentoRAUX_NOT_Return.Add("G0 Z0" + " Y" + valorAtual_ProximoY.ToString());
                                addG0Z0ED = false;
                            }
                            valorAtual_ProximoY = 0;
                        }
                        else if (quebraEspaco[1].StartsWith("X"))
                        {
                            acabamentoRAUX_NOT_Return.Add(line);
                        }
                    }
                    else
                    {
                        acabamentoRAUX_NOT_Return.Add(line);
                    }
                }
                else
                {
                    acabamentoRAUX_NOT_Return.Add(line);
                }
            }

            

            acabamentoRAUX.Clear();
            acabamentoRAUX = acabamentoRAUX_NOT_Return;
        
            */
            #endregion teste 

            Stream acabamentoNC_W = File.Create(GLSettings.locateTMP + out_);
            StreamWriter acabamentoNCW = new StreamWriter(acabamentoNC_W);

            foreach (var line in acabamentoRAUX)//acabamentoRAUX)
            {                
                acabamentoNCW.WriteLine(line);
            }
                                                
            acabamentoNCR.Close();
            acabamentoNC.Close();
            desbasteAux_r.Close();
            desbasteAux_.Close();            
            acabamentoNCW.Close();
            acabamentoNC_W.Close();            
        }

        /// <summary>
        /// Mudança de cordenada versão 2 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="desbasteAux"></param>
        /// <param name="out_"></param>
        private void CamXYToZYV2(string path, string desbasteAux, string out_, int numberBlock)
        {
            /* Salvando as informações do complement do desbaste em uma List
             */
            GLSettings.ajusteX_deslocamento = 0;
            GLSettings.ajusteZ_deslocamento = 0;
            List<String> acabamentoR = new List<string>();
            List<String> acabamentoRAUX = new List<string>();

            initZ = true;

            /* O foi criado um arquivo auxiliar com um A na ajuste de algumas rotas, mas devido problemas 
             * de proibição de escrita foi feito este desdrobre, resolver posteriormente.
             */
            Stream acabamentoNC = File.Open(GLSettings.locateTMP + "A" + path, FileMode.Open);
            StreamReader acabamentoNCR = new StreamReader(acabamentoNC);

            Stream desbasteAux_ = File.Open(GLSettings.locateTMP + "D" + desbasteAux, FileMode.Open);
            StreamReader desbasteAux_r = new StreamReader(desbasteAux_);

            string Aux = desbasteAux_r.ReadLine();
            while (Aux != null)
            {
                if ((Aux.StartsWith("G1")) || (Aux.StartsWith("G0")))
                {
                    string[] quebraEspaco = new string[6];

                    quebraEspaco = Aux.Split(' ');

                    if (quebraEspaco[1].StartsWith("X"))
                    {
                        string ajusteX = quebraEspaco[1].Replace("X", "");
                        PositionZY(ajusteX, false);
                        Aux = desbasteAux_r.ReadLine();
                    }
                    else
                    {
                        Aux = desbasteAux_r.ReadLine();
                    }
                }
                else
                {
                    Aux = desbasteAux_r.ReadLine();
                }
            }

            string linha = acabamentoNCR.ReadLine();

            while (linha != null)
            {
                if (linha.StartsWith("G1"))
                {
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linha.Split(' ');

                    if (quebraEspaco[1].StartsWith("X"))
                    {
                        string linha_ = linha.Replace("X", "*");
                        string ajusteZ = quebraEspaco[1].Replace("X", "");
                        PositionZY(ajusteZ, true);
                        acabamentoR.Add(linha_);
                        linha = acabamentoNCR.ReadLine();
                    }
                    else
                    {
                        acabamentoR.Add(linha);
                        linha = acabamentoNCR.ReadLine();
                    }
                }
                else if (linha.StartsWith("G0"))
                {
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linha.Split(' ');

                    if (quebraEspaco[1].StartsWith("X"))
                    {
                        if (initZ)
                        {
                            linha = "G0" + " " + "Z5" + " " + GLSettings.feedrateA;
                            initZ = false;
                        }
                        else
                        {
                            linha = "G0" + " " + "Z-5";
                        }
                        //linha = "G1" + " " + "Z0";
                        acabamentoR.Add(linha);
                        linha = "G0" + " " + quebraEspaco[2];
                        //linha = "G1" + " " + quebraEspaco[2];
                        acabamentoR.Add(linha);

                        linha = acabamentoNCR.ReadLine();
                    }
                    else
                    {
                        linha = acabamentoNCR.ReadLine();
                    }
                }
                else
                {
                    acabamentoR.Add(linha);
                    linha = acabamentoNCR.ReadLine();
                }
            }

            foreach (var linha_ in acabamentoR)
            {
                if ((linha_.StartsWith("G1")))
                {
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linha_.Split(' ');

                    if (quebraEspaco[1].StartsWith("Z-"))
                    {
                        string linha__ = linha_.Replace("Z-", "X");
                        acabamentoRAUX.Add(linha__);
                    }
                    else if (quebraEspaco[1].StartsWith("Z"))
                    {
                        string linha__ = linha_.Replace("Z", "X");
                        acabamentoRAUX.Add(linha__);
                    }
                    else
                    {
                        acabamentoRAUX.Add(linha_);
                    }
                }
                else
                {
                    acabamentoRAUX.Add(linha_);
                }
            }

            acabamentoR.Clear();

            foreach (var linha_aux in acabamentoRAUX)
            {
                if (linha_aux.StartsWith("G1"))
                {
                    string[] quebraEspaco = new string[6];

                    quebraEspaco = linha_aux.Split(' ');

                    if (quebraEspaco[1].StartsWith("*"))
                    {
                        string linha__ = linha_aux.Replace("*", "Z");
                        acabamentoR.Add(linha__);
                    }
                    else
                    {
                        acabamentoR.Add(linha_aux);
                    }
                }
                else
                {
                    acabamentoR.Add(linha_aux);
                }
            }

            acabamentoRAUX.Clear();

            double cont = 0;
            GLSettings.ZAux = 0;           

            foreach (var linha_aux in acabamentoR)
            {
                if (linha_aux.StartsWith("G1"))
                {
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linha_aux.Split(' ');

                    if (quebraEspaco[1].StartsWith("Z"))
                    {
                        string ajusteZ = quebraEspaco[1].Replace("Z", "");
                        double valorAtualZ = float.Parse(ajusteZ, System.Globalization.CultureInfo.InvariantCulture);

                        if(valorAtualZ > GLSettings.ZAux)
                        {
                            GLSettings.ZAux = valorAtualZ;
                        }                      
                    }
                }
            }

            foreach (var linha_aux in acabamentoR)
            {
                string result;
                cont++;
                if (linha_aux.StartsWith("G1"))
                {
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linha_aux.Split(' ');

                    if (quebraEspaco[1].StartsWith("X"))
                    {
                        string ajusteX = quebraEspaco[1].Replace("X", "");
                        double valorAtualX = float.Parse(ajusteX, System.Globalization.CultureInfo.InvariantCulture);                      

                        valorAtualX += Math.Round(12 - 5 + GLSettings.ajusteX_desbasteXmin[numberBlock], 3);       

                        if (quebraEspaco.Count() == 3)
                        {
                            result = "G1" + " " + "X" + valorAtualX.ToString().Replace(',', '.') + " " + quebraEspaco[2];
                        }
                        else
                        {
                            result = "G1" + " " + "X" + valorAtualX.ToString().Replace(',', '.');
                        }
                        acabamentoRAUX.Add(result);
                    }
                    else if (quebraEspaco[1].StartsWith("Y"))
                    {
                        string ajusteY = quebraEspaco[1].Replace("Y", "");
                        double valorAtualY = float.Parse(ajusteY, System.Globalization.CultureInfo.InvariantCulture);

                        valorAtualY += Math.Round((12 - 5) + GLSettings.ajusteY_desbasteYmin[numberBlock], 3);

                        if (quebraEspaco.Count() == 3)
                        {
                            result = "G1" + " " + "Y" + valorAtualY.ToString().Replace(',', '.') + " " + quebraEspaco[2];
                        }
                        else
                        {
                            result = "G1" + " " + "Y" + valorAtualY.ToString().Replace(',', '.');
                        }
                        acabamentoRAUX.Add(result);
                    }
                    else if (quebraEspaco[1].StartsWith("Z"))
                    {
                        string ajusteZ = quebraEspaco[1].Replace("Z", "");
                        double valorAtualZ = float.Parse(ajusteZ, System.Globalization.CultureInfo.InvariantCulture);
                        string result_;

                        valorAtualZ = Math.Round(valorAtualZ - GLSettings.ZAux, 3);

                        if (quebraEspaco.Count() == 3)
                        {
                            string ajusteY = quebraEspaco[2].Replace("Y", "");
                            double valorAtualY = float.Parse(ajusteY, System.Globalization.CultureInfo.InvariantCulture);

                            valorAtualY += (12 - 5) + GLSettings.ajusteY_desbasteYmin[numberBlock];

                            valorAtualY = Math.Round(valorAtualY, 3);

                            result_ = "G0" + " " + "Z" + (valorAtualZ).ToString().Replace(',', '.') + " " + "Y" + valorAtualY.ToString().Replace(',', '.');
                        }
                        else
                        {
                            result_ = "G0" + " " + "Z" + (valorAtualZ).ToString().Replace(',', '.');
                        }
                        acabamentoRAUX.Add(result_);
                    }
                    else
                    {
                        acabamentoRAUX.Add(linha_aux);
                    }
                }
                else
                {
                    acabamentoRAUX.Add(linha_aux);
                }
            }

            Stream acabamentoNC_W = File.Create(GLSettings.locateTMP + "A" + out_);
            StreamWriter acabamentoNCW = new StreamWriter(acabamentoNC_W);

            foreach (var line in acabamentoRAUX)
            {
                acabamentoNCW.WriteLine(line);
            }

            acabamentoNCR.Close();
            acabamentoNC.Close();
            desbasteAux_r.Close();
            desbasteAux_.Close();
            acabamentoNCW.Close();
            acabamentoNC_W.Close();
        }

        private void CamXYToZXV2(string path, string desbasteAux, string out_, int numberBlock)
        {
            /* Salvando as informações do complement do desbaste em uma List
             */
            GLSettings.ajusteX_deslocamento = 0;
            GLSettings.ajusteZ_deslocamento = 0;
            List<String> acabamentoR = new List<string>();
            List<String> acabamentoRAUX = new List<string>();
            List<String> acabamentoRAUXInvXY = new List<string>();
            List<Tuple<double, double>> injectionOfPoints = new List<Tuple<double, double>>();
            bool initInjectionOfPoints = false;

            initZ = true;

            /* O foi criado um arquivo auxiliar com um A na ajuste de algumas rotas, mas devido problemas 
             * de proibição de escrita foi feito este desdrobre, resolver posteriormente.
             */
            Stream acabamentoNC = File.Open(GLSettings.locateTMP + "A" + path, FileMode.Open);
            StreamReader acabamentoNCR = new StreamReader(acabamentoNC);

            Stream desbasteAux_ = File.Open(GLSettings.locateTMP + "D" + desbasteAux, FileMode.Open);
            StreamReader desbasteAux_r = new StreamReader(desbasteAux_);

            string Aux = desbasteAux_r.ReadLine();
            while (Aux != null)
            {
                if ((Aux.StartsWith("G1")) || (Aux.StartsWith("G0")))
                {
                    string[] quebraEspaco = new string[6];

                    quebraEspaco = Aux.Split(' ');

                    if (quebraEspaco[1].StartsWith("X"))
                    {
                        string ajusteX = quebraEspaco[1].Replace("X", "");
                        PositionZY(ajusteX, false);
                        Aux = desbasteAux_r.ReadLine();
                    }
                    else
                    {
                        Aux = desbasteAux_r.ReadLine();
                    }
                }
                else
                {
                    Aux = desbasteAux_r.ReadLine();
                }
            }

            string linha = acabamentoNCR.ReadLine();

            while (linha != null)
            {
                if (linha.StartsWith("G1"))
                {
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linha.Split(' ');

                    if (quebraEspaco[1].StartsWith("X"))
                    {
                        string linha_ = linha.Replace("X", "*");
                        string ajusteZ = quebraEspaco[1].Replace("X", "");
                        PositionZY(ajusteZ, true);
                        acabamentoR.Add(linha_);
                        linha = acabamentoNCR.ReadLine();
                    }
                    else
                    {
                        acabamentoR.Add(linha);
                        linha = acabamentoNCR.ReadLine();
                    }
                }
                else if (linha.StartsWith("G0"))
                {
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linha.Split(' ');

                    if (quebraEspaco[1].StartsWith("X"))
                    {
                        if (initZ)
                        {
                            linha = "G0" + " " + "Z10" + " " + GLSettings.feedrateA;
                            initZ = false;
                        }
                        else
                        {
                            linha = "G0" + " " + "Z10";//"Z-5";
                        }
                        //linha = "G1" + " " + "Z0";
                        acabamentoR.Add(linha);
                        linha = "G0" + " " + quebraEspaco[2];
                        //linha = "G1" + " " + quebraEspaco[2];
                        acabamentoR.Add(linha);

                        linha = acabamentoNCR.ReadLine();
                    }
                    else
                    {
                        linha = acabamentoNCR.ReadLine();
                    }
                }
                else
                {
                    acabamentoR.Add(linha);
                    linha = acabamentoNCR.ReadLine();
                }
            }

            foreach (var linha_ in acabamentoR)
            {
                if ((linha_.StartsWith("G1")))
                {
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linha_.Split(' ');

                    if (quebraEspaco[1].StartsWith("Z-"))
                    {
                        string linha__ = linha_.Replace("Z-", "X");
                        acabamentoRAUX.Add(linha__);
                    }
                    else if (quebraEspaco[1].StartsWith("Z"))
                    {
                        string linha__ = linha_.Replace("Z", "X");
                        acabamentoRAUX.Add(linha__);
                    }
                    else
                    {
                        acabamentoRAUX.Add(linha_);
                    }
                }
                else
                {
                    acabamentoRAUX.Add(linha_);
                }
            }

            acabamentoR.Clear();

            foreach (var linha_aux in acabamentoRAUX)
            {
                if (linha_aux.StartsWith("G1"))
                {
                    string[] quebraEspaco = new string[6];

                    quebraEspaco = linha_aux.Split(' ');

                    if (quebraEspaco[1].StartsWith("*"))
                    {
                        string linha__ = linha_aux.Replace("*", "Z");
                        acabamentoR.Add(linha__);
                    }
                    else
                    {
                        acabamentoR.Add(linha_aux);
                    }
                }
                else
                {
                    acabamentoR.Add(linha_aux);
                }
            }

            acabamentoRAUX.Clear();

            double cont = 0;
            GLSettings.ZAux = 0;

            foreach (var linha_aux in acabamentoR)
            {
                if (linha_aux.StartsWith("G1"))
                {
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linha_aux.Split(' ');

                    if (quebraEspaco[1].StartsWith("Z"))
                    {
                        string ajusteZ = quebraEspaco[1].Replace("Z", "");
                        double valorAtualZ = float.Parse(ajusteZ, System.Globalization.CultureInfo.InvariantCulture);

                        if (valorAtualZ > GLSettings.ZAux)
                        {
                            GLSettings.ZAux = valorAtualZ;
                        }
                    }
                }
            }

            foreach (var linha_aux in acabamentoR)
            {
                string result;
                cont++;
                if (linha_aux.StartsWith("G1"))
                {
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linha_aux.Split(' ');

                    if (quebraEspaco[1].StartsWith("X"))
                    {
                        string ajusteX = quebraEspaco[1].Replace("X", "");
                        double valorAtualX = float.Parse(ajusteX, System.Globalization.CultureInfo.InvariantCulture);

                        valorAtualX = Math.Round(12 -5 + valorAtualX, 3);                 

                        if (quebraEspaco.Count() == 3)
                        {
                            result = "G1" + " " + "X" + valorAtualX.ToString().Replace(',', '.') + " " + quebraEspaco[2];
                        }
                        else
                        {
                            result = "G1" + " " + "X" + valorAtualX.ToString().Replace(',', '.');
                        }
                        acabamentoRAUX.Add(result);                        
                    }
                    else if (quebraEspaco[1].StartsWith("Y"))
                    {
                        string ajusteY = quebraEspaco[1].Replace("Y", "");
                        double valorAtualY = float.Parse(ajusteY, System.Globalization.CultureInfo.InvariantCulture);

                        valorAtualY = Math.Round((12 - 5) + valorAtualY, 3);        

                        if (quebraEspaco.Count() == 3)
                        {
                            result = "G1" + " " + "Y" + valorAtualY.ToString().Replace(',', '.') + " " + quebraEspaco[2];
                        }
                        else
                        {
                            result = "G1" + " " + "Y" + valorAtualY.ToString().Replace(',', '.');
                        }
                        acabamentoRAUX.Add(result);                        
                    }
                    else if (quebraEspaco[1].StartsWith("Z"))
                    {
                        string ajusteZ = quebraEspaco[1].Replace("Z", "");
                        double valorAtualZ = float.Parse(ajusteZ, System.Globalization.CultureInfo.InvariantCulture);
                        string result_;

                        valorAtualZ = valorAtualZ - GLSettings.ZAux;
                        valorAtualZ *= -1;
                        valorAtualZ = (Math.Round(valorAtualZ - GLSettings.ajusteZ_total[numberBlock]));                     

                        if (quebraEspaco.Count() == 3)
                        {
                            string ajusteY = quebraEspaco[2].Replace("Y", "");
                            double valorAtualY = float.Parse(ajusteY, System.Globalization.CultureInfo.InvariantCulture);
                       
                            valorAtualY += (12 - 5);                           
                            valorAtualY = Math.Round(valorAtualY, 3);
                            /*
                            if (initInjectionOfPoints)
                            {
                                injectionOfPoints = MathBase.rectumEquation(GLSettings.initPointZ, valorAtualZ, GLSettings.initPointY, valorAtualY, 2);

                                foreach(var insert in injectionOfPoints)
                                {
                                    result_ = "G1" + " " + "Z" + Math.Round(insert.Item1 , 4).ToString().Replace(',', '.') + " " + "Y" + Math.Round(insert.Item2, 4).ToString().Replace(',', '.');
                                    acabamentoRAUX.Add(result_);
                                }

                                injectionOfPoints.Clear();
                            }

                            GLSettings.initPointY = valorAtualY;                            
                            GLSettings.initPointZ = valorAtualZ;                            
                            
                            initInjectionOfPoints = true;
                            */
                            result_ = "G1" + " " + "Z" + valorAtualZ.ToString().Replace(',', '.') + " " + "Y" + valorAtualY.ToString().Replace(',', '.');
                        }
                        else
                        {
                            result_ = "G0" + " " + "Z" + valorAtualZ.ToString().Replace(',', '.');                           
                        }
                        acabamentoRAUX.Add(result_);
                    }
                    else
                    {
                        acabamentoRAUX.Add(linha_aux);
                    }
                }
                else
                {
                    acabamentoRAUX.Add(linha_aux);
                    initInjectionOfPoints = false;
                }
            }

            
            //Inversor
            foreach (var line in acabamentoRAUX)
            {

                acabamentoRAUXInvXY.Add(line.Replace("X", "K"));
            }
            acabamentoRAUX.Clear();

            foreach (var line in acabamentoRAUXInvXY)
            {

                acabamentoRAUX.Add(line.Replace("Y", "X"));
            }
            acabamentoRAUXInvXY.Clear();

            foreach (var line in acabamentoRAUX)
            {
                acabamentoRAUXInvXY.Add(line.Replace("K", "Y"));
            }
            

            Stream acabamentoNC_W = File.Create(GLSettings.locateTMP + "A" + out_);
            StreamWriter acabamentoNCW = new StreamWriter(acabamentoNC_W);

            foreach (var line in acabamentoRAUXInvXY)
            {
                acabamentoNCW.WriteLine(line);
            }
            

            acabamentoNCR.Close();
            acabamentoNC.Close();
            desbasteAux_r.Close();
            desbasteAux_.Close();
            acabamentoNCW.Close();
            acabamentoNC_W.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberX"></param>
        /// <param name="z"></param>
        private void PositionZY(string numberX, bool z)
        {
            float aux = float.Parse(numberX, System.Globalization.CultureInfo.InvariantCulture);

            if(z)
            {
                if(aux < GLSettings.ajusteZ_deslocamento)
                {
                    GLSettings.ajusteZ_deslocamento = aux;
                }
            }
            else 
            {
                if(aux < GLSettings.ajusteX_deslocamento)
                {
                    GLSettings.ajusteX_deslocamento = aux;
                }                
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        private void unionGcode(string path1, string path2)
        {
            /* Salvando as informações do complement do desbaste em uma List
             */
            Stream Desbaste_ComplementoNC = File.Open(GLSettings.locateCAM + path1, FileMode.Open);
            StreamReader Desbaste_ComplementoNC_R = new StreamReader(Desbaste_ComplementoNC);

            string linha = Desbaste_ComplementoNC_R.ReadLine();

            while (linha != null)
            {
                //MessageBox.Show(linha);
                if (linha.StartsWith("M30"))
                {
                    break;
                }

                GLSettings.Desbaste_Complemento2NC_Line.Add(linha);
                linha = Desbaste_ComplementoNC_R.ReadLine();
            }
            Desbaste_ComplementoNC.Close();
            Desbaste_ComplementoNC_R.Close();

            /* Salvando as informações do bloco em uma List
             */
            Stream Bloco_solido_desbaste_NC = File.Open(GLSettings.locateCAM + path2, FileMode.Open);
            StreamReader Bloco_solido_desbaste_NC_R = new StreamReader(Bloco_solido_desbaste_NC);

            linha = Bloco_solido_desbaste_NC_R.ReadLine();

            while (linha != null)
            {
                //MessageBox.Show(linha);
                if (linha.StartsWith("M30"))
                {
                    break;
                }

                GLSettings.Bloco_solido_desbaste_NC_Line.Add(linha);
                linha = Bloco_solido_desbaste_NC_R.ReadLine();
            }
            Bloco_solido_desbaste_NC.Close();
            Bloco_solido_desbaste_NC_R.Close();

            /* Unindo das duas lista em uma tercera
            */
            Stream Bloco_solido_desbaste_NC_ = File.Create(GLSettings.locateCAM + path2);
            StreamWriter Bloco_solido_desbaste_NC_W = new StreamWriter(Bloco_solido_desbaste_NC_);

            foreach(var line in GLSettings.Desbaste_Complemento2NC_Line)
            {
                GLSettings.aux.Add(line);
            }

            foreach (var line in GLSettings.Bloco_solido_desbaste_NC_Line)
            {
                GLSettings.aux.Add(line);
            }

            int cont = 5;
            foreach(var line in GLSettings.aux)
            {
                Bloco_solido_desbaste_NC_W.WriteLine("N" + cont.ToString() + line);
                cont++;
            }

            Bloco_solido_desbaste_NC_W.WriteLine("M30");

            Bloco_solido_desbaste_NC_W.Close();
            GLSettings.aux.Clear();
            GLSettings.Bloco_solido_desbaste_NC_Line.Clear();
            GLSettings.Desbaste_Complemento2NC_Line.Clear();
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comando"></param>
        /// <returns></returns>
        private string ExecutarCMD(string comando)
        {
            Process scriptProc = new Process();
            scriptProc.StartInfo.FileName = @"wscript.exe";
            scriptProc.StartInfo.Arguments = comando;
            scriptProc.Start();
            scriptProc.WaitForExit();
            scriptProc.Close();

            return "";
        }

        /// <summary>
        /// Tratamento de código gcode, algoritmo default
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="acabamento"></param>
        private void CAMGenerate_(string path, string fileName, bool acabamento)
        {
            zEncontrado = false;            
            Stream entrada = File.Open(GLSettings.locateTMP + path, FileMode.Open);
            StreamReader leitor = new StreamReader(entrada);
            StreamWriter escritor;
            Stream saida;
        
            saida = File.Create(GLSettings.locateTMP + fileName);
            escritor = new StreamWriter(saida);            

            string linha = leitor.ReadLine();
            double initZ = 0;
            if (acabamento)
            {
                initZ = GLSettings.startLayersA;
            }
            else
            {
                initZ = GLSettings.startLayers;
            }
            double camada = 0;

            if (acabamento)
            {
                camada = GLSettings.stepLayersAcabamento;
            }
            else
            {
                camada = GLSettings.stepLayersDesbaste;
            }

            camada = camada + initZ;

            GLSettings.gcodeZs_aux.Clear();
            while (linha != null)
            {
                if (linha.StartsWith("G1 Z"))
                {
                    initZ--;
                    camada--;

                    GLSettings.gcodeZs_aux.Add(linha);
                    
                    if (initZ > 0 || camada < 0)
                    {
                        GLSettings.gcodeZs.Add(linha);
                        if (acabamento)
                        {
                            camada = GLSettings.stepLayersAcabamento;
                            if (GLSettings.gcodeZs_aux.Count() > 10) GLSettings.gcodeZs_aux.Clear();
                        }
                        else
                        {
                            camada = GLSettings.stepLayersDesbaste;
                            if(GLSettings.gcodeZs_aux.Count() > 10) GLSettings.gcodeZs_aux.Clear();
                        }
                    }
                }
                GLSettings.gcode.Add(linha);
                linha = leitor.ReadLine();
            }

            for(int i = GLSettings.gcodeZs_aux.Count() - 1; i > GLSettings.gcodeZs_aux.Count() - 2; i--)
            { 
                GLSettings.gcodeZs.Add(GLSettings.gcodeZs_aux[1]);
            }

            leitor.Close();
            leitor.Dispose();
            entrada.Dispose();
            entrada.Close();

            GLSettings.gcodeZsMenosComplemento.Add("G1 Z0");

            for (int i = 1; i < GLSettings.gcodeZs.Count; i++)
            {
                string[] quebraZ = new string[2];
                string[] quebraEspaco = new string[2];
                quebraZ = GLSettings.gcodeZs[i].Split('Z');
                quebraEspaco = quebraZ[1].Split(' ');
                string zTratado = quebraEspaco[0].Replace('.', ',');
                double z = Double.Parse(zTratado);
                
                GLSettings.gcodeZsMenosComplemento.Add(GLSettings.gcodeZs[i]);
            }

            for (int i = 1; i <= GLSettings.gcodeZsMenosComplemento.Count; i++)
            {
                foreach (var gcodeLinha in GLSettings.gcode)
                {
                    if (gcodeLinha == GLSettings.gcodeZsMenosComplemento[GLSettings.gcodeZsMenosComplemento.Count - i] || zEncontrado)
                    {
                        if ((gcodeLinha.StartsWith("G1 Z") && zEncontrado) || gcodeLinha.StartsWith("M104") || gcodeLinha.StartsWith("M109") || gcodeLinha.StartsWith("G21") || gcodeLinha.StartsWith("G90"))
                        {
                            break;
                        }
                        if (!zEncontrado)
                        {
                            /*
                                * Inicia código Gcode
                                */
                            if ((i - 1) == 0)
                            {
                                escritor.WriteLine(GLSettings.source);
                                escritor.WriteLine(GLSettings.spindle + " " + GLSettings.turnDirection);
                                escritor.WriteLine("G0 Z-" + 0.ToString() + " " + GLSettings.feedrate);
                            }
                            else
                            {                                
                                string[] quebraZ = new string[2];
                                string[] quebraEspaco = new string[2];
                                quebraZ = GLSettings.gcodeZsMenosComplemento[i - 1].Split('Z');
                                quebraEspaco = quebraZ[1].Split(' ');
                                escritor.WriteLine("G1 Z-" + quebraEspaco[0]);                                
                            }
                        }
                        else
                        {
                            if (gcodeLinha.StartsWith("G1"))
                            {
                                string[] quebraEspaco = new string[6];

                                quebraEspaco = gcodeLinha.Split(' ');

                                foreach (var linha_ in quebraEspaco)
                                {
                                    if (linha_.StartsWith("F"))
                                    {
                                        continue;
                                    }
                                }

                                if (quebraEspaco[1].StartsWith("X") || quebraEspaco[1].StartsWith("Y"))
                                {
                                    if (quebraEspaco[3].StartsWith("F7800.000"))
                                    {
                                        if (G92 || M107)
                                        {
                                            escritor.WriteLine("G0 " + "Z-" + 0.ToString());
                                            escritor.WriteLine("G0" + " " + quebraEspaco[1] + " " + quebraEspaco[2]);

                                            string[] quebraZ = new string[2];
                                            string[] quebraEspaco2 = new string[2];
                                            quebraZ = GLSettings.gcodeZsMenosComplemento[i - 1].Split('Z');
                                            quebraEspaco = quebraZ[1].Split(' ');
                                            escritor.WriteLine("G0 Z-" + quebraEspaco[0]);                                         
                                            G92 = false;
                                            M107 = false;
                                        }
                                        else
                                        {
                                            escritor.WriteLine(quebraEspaco[0] + " " + quebraEspaco[1] + " " + quebraEspaco[2]); //teste
                                        }
                                    }
                                    else
                                    {
                                        escritor.WriteLine(quebraEspaco[0] + " " + quebraEspaco[1] + " " + quebraEspaco[2]);
                                    }
                                }
                            }

                            if (gcodeLinha.StartsWith("G92"))
                            {
                                G92 = true;
                            }
                            if (gcodeLinha.StartsWith("M107"))
                            {
                                M107 = true;
                            }
                        }
                        zEncontrado = true;
                    }
                }
                zEncontrado = false;
            }

            escritor.WriteLine("M30");

            escritor.Close();
            saida.Close();

            GLSettings.gcode.Clear();
            GLSettings.gcodeZsMenosComplemento.Clear();
            GLSettings.gcodeZs.Clear();
        
            /*
             * Ajuste necessário para corrigir problema de movimentação de uma extremidade para outra sem subir o eixo z
             */                       
            Stream Desbaste_ajuste = File.Open(GLSettings.locateTMP + fileName, FileMode.Open);
            StreamReader Desbaste_ajuste_R = new StreamReader(Desbaste_ajuste);

            string linhaAjuste = Desbaste_ajuste_R.ReadLine();
            List<String> desbasteAjuste = new List<string>();

            while (linhaAjuste != null)
            {
                if ((linhaAjuste.StartsWith("G1")))
                {
                    string[] quebraEspaco = new string[6];
                    quebraEspaco = linhaAjuste.Split(' ');

                    if (quebraEspaco[1].StartsWith("Z"))
                    {
                        string proximaLinha = Desbaste_ajuste_R.ReadLine();

                        if (!(proximaLinha.StartsWith("G0")))
                        {

                            string[] proximaLinhaQuebra = new string[6];
                            proximaLinhaQuebra = proximaLinha.Split(' ');
                            string linhaAdicional = "G0 Z0";
                            desbasteAjuste.Add(linhaAdicional);
                            if (proximaLinhaQuebra.Count() > 2)
                            {
                                linhaAdicional = "G0" + " " + proximaLinhaQuebra[1] + " " + proximaLinhaQuebra[2];
                            }
                            else
                            {
                                linhaAdicional = "G1" + " " + proximaLinhaQuebra[1];
                            }
                            desbasteAjuste.Add(linhaAdicional);
                            desbasteAjuste.Add(linhaAjuste);
                            desbasteAjuste.Add(proximaLinha);

                            linhaAjuste = Desbaste_ajuste_R.ReadLine();
                        }
                        else if(quebraEspaco.Count() == 2)
                        {
                            string temp = linhaAjuste;
                            //desbasteAjuste.Add(linhaAjuste);
                            desbasteAjuste.Add(proximaLinha);
                            linhaAjuste = Desbaste_ajuste_R.ReadLine();
                            desbasteAjuste.Add(linhaAjuste);//dio
                            linhaAjuste = Desbaste_ajuste_R.ReadLine();
                            desbasteAjuste.Add(linhaAjuste);//dio
                            desbasteAjuste.Add(temp);//dio
                            linhaAjuste = Desbaste_ajuste_R.ReadLine();
                        }
                        else
                        {
                            desbasteAjuste.Add(linhaAjuste);
                            desbasteAjuste.Add(proximaLinha);
                            linhaAjuste = Desbaste_ajuste_R.ReadLine();
                        }
                    }
                    else
                    {
                        desbasteAjuste.Add(linhaAjuste);
                        linhaAjuste = Desbaste_ajuste_R.ReadLine();
                    }
                }
                else
                {
                    desbasteAjuste.Add(linhaAjuste);
                    linhaAjuste = Desbaste_ajuste_R.ReadLine();
                }
            }

            StreamWriter escritor_;
            Stream saida_;

            if (!acabamento)
            {                
                saida_ = File.Create(GLSettings.locateCAM + fileName);
                escritor_ = new StreamWriter(saida_);
            }
            else
            {               
                saida_ = File.Create(GLSettings.locateTMP + "A" + fileName);
                escritor_ = new StreamWriter(saida_);
            }

            foreach (var line in desbasteAjuste)
            {
                escritor_.WriteLine(line);
            }
            escritor_.Close();
            saida_.Close();
                      
        }

        /// <summary>
        /// Tratamento de código gcode, algoritmo alternativo 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="acabamento"></param>
        private void CAMGenerateV2_(string path, string fileName, bool acabamento, int numberBLock)
        {
            zEncontrado = false;
            GLSettings.z_atual_g0 = 5;

            Stream entrada = File.Open(GLSettings.locateTMP + path, FileMode.Open);
            StreamReader leitor = new StreamReader(entrada);
            StreamWriter escritor;
            Stream saida;
            bool notResgister = false;
            bool upZ = false;
            bool correctionRepeatFirstLayer = false;
            bool correctionRepeatFirstLayerEnable = false;

            saida = File.Create(GLSettings.locateTMP + "_" + fileName);
            escritor = new StreamWriter(saida);

            string linha = leitor.ReadLine();          

            GLSettings.gcodeZs_aux.Clear();
            while (linha != null)
            { 
                if(!linha.StartsWith(";")) //comentários do gcode
                {                
                    if (linha.LastIndexOf('Z') > 0)
                    {
                        if(!linha.StartsWith("G1")) GLSettings.gcodeZs_aux.Add(linha);
                    }
                }
                GLSettings.gcode.Add(linha); 
                linha = leitor.ReadLine();
            }

            leitor.Close();
            leitor.Dispose();
            entrada.Dispose();
            entrada.Close();

            initZ = true;         

            if (!acabamento)
            {
                GLSettings.gcodeZs_aux.RemoveAt(0);
            }
            
            for (int i = 0; i < GLSettings.gcodeZs_aux.Count; i++)
            {
                foreach (var gcodeLinha in GLSettings.gcode)
                {
                    if (gcodeLinha == GLSettings.gcodeZs_aux[GLSettings.gcodeZs_aux.Count - (i + 1)] || zEncontrado)
                    {
                        if(!acabamento)
                        { 
                            if (gcodeLinha.StartsWith(";MESH:NONMESH") && zEncontrado)
                            {
                                notResgister = false;
                                break;
                            }
                        }
                        else
                        {
                            if (gcodeLinha.StartsWith(";MESH:NONMESH") && zEncontrado)
                            {
                                notResgister = false;
                                break;
                            }
                        }
                        if (!zEncontrado)
                        {
                            /*
                                * Inicia código Gcode
                                */
                            if (i == 0)
                            {
                                string[] quebraLinha = new string[6];
                                quebraLinha = gcodeLinha.Split(' ');                               

                                escritor.WriteLine(GLSettings.source);
                                escritor.WriteLine(GLSettings.spindle + " " + GLSettings.turnDirection);        

                                GLSettings.z_atual = GLSettings.z_atual_g0;

                                //O Primeiro Z não deve ser negativo para não iniciar cortando o material 
                                escritor.WriteLine(quebraLinha[0] + " Z10" + " " + GLSettings.feedrate);                           
                            }
                            else
                            {
                                string[] quebraLinha = new string[6];
                                quebraLinha = gcodeLinha.Split(' ');

                                string[] quebraLinhaZ = new string[6];
                                quebraLinhaZ = GLSettings.gcodeZs_aux[i].Split(' ');

                                string[] quebraLinhaZ_ = new string[6];
                                quebraLinhaZ_ = quebraLinhaZ[4].Split('Z');
       
                                string zTratado = quebraLinhaZ_[1].Replace('.', ',');

                                GLSettings.z_atual = (int)(Double.Parse(zTratado));

                                //bug corrigir posteriormente 
                                {
                                    escritor.Write(" ");
                                    upZ = true;
                                    if (!acabamento && !correctionRepeatFirstLayer && !correctionRepeatFirstLayerEnable)
                                    {
                                        correctionRepeatFirstLayer = true;
                                        correctionRepeatFirstLayerEnable = true;
                                    }
                                    else
                                    {
                                        correctionRepeatFirstLayer = false;
                                    }
                                }

                                if (!acabamento)
                                {
                                    GLSettings.z_atual = GLSettings.z_atual - 5;                                  
                                }
                                else
                                {
                                    GLSettings.z_atual = GLSettings.z_atual + GLSettings.z_atual_g0;
                                }

                                if (quebraLinha[0] == "G0" && !acabamento)
                                {
                                    escritor.WriteLine(quebraLinha[0] + " Z0");
                                }

                                if(acabamento)
                                {
                                    escritor.WriteLine(quebraLinha[0] + " Z0");
                                }

                                escritor.WriteLine(quebraLinha[0] + " " + quebraLinha[2] + " " + quebraLinha[3]);                                                             
                            }
                        }
                        else
                        {                  
                            /* remover camada mais externa */
                            if(!acabamento)
                            {
                                if (gcodeLinha.StartsWith(";TYPE:WALL-INNER"))
                                {
                                    notResgister = true;                                    
                                }
                                if (gcodeLinha.StartsWith(";TYPE:WALL-INNER"))
                                {
                                    notResgister = true; //false                                   
                                }
                                if (gcodeLinha.StartsWith(";TYPE:WALL-OUTER"))
                                {
                                    notResgister = true; //false                                   
                                }                                

                                else if (gcodeLinha.StartsWith(";TYPE:SKIN"))
                                {
                                    notResgister = false;                                   
                                }
                            }
                           else
                            {
                               if (gcodeLinha.StartsWith(";TYPE:SKIRT"))
                               {
                                   notResgister = true;                                
                               }
                               if (gcodeLinha.StartsWith(";TYPE:WALL-OUTER"))
                               {
                                   notResgister = false;                                 
                               }
                               if (gcodeLinha.StartsWith(";TYPE:SKIN"))
                               {
                                   notResgister = true;                                 
                               }
                            }
                           
                            if (!notResgister && !correctionRepeatFirstLayer)
                            {

                                /*
                                 * Este é utilizado para a última camada a ser tratada que no gcode original 
                                 * é a primeira camada, nesta camada inicial algumas rotas G0 não são necessárias
                                 * para resolver isto é esperado a tag ;MESH para dar início ao tratamento
                                 * das rotas relevantes;
                                 */
                                if (GLSettings.gcodeZs_aux.Count - (i + 1) == 0 && !MESH)
                                {
                                    string[] init_ = new string[6];
                                    init_ = gcodeLinha.Split(':');

                                    if (init_[0] == ";MESH") MESH = true;
                                }
                                else
                                {
                                    if (gcodeLinha.StartsWith("G1"))
                                    {
                                        string[] quebraLinhaG1 = new string[6];
                                        quebraLinhaG1 = gcodeLinha.Split(' ');

                                        if (quebraLinhaG1.Count() > 4)
                                        {
                                            if (quebraLinhaG1[1].StartsWith("X") || quebraLinhaG1[1].StartsWith("Y") || quebraLinhaG1[2].StartsWith("X") || quebraLinhaG1[2].StartsWith("Y"))
                                            {
                                                if (!acabamento)
                                                {
                                                    if (!upZ)
                                                    {
                                                        escritor.WriteLine(quebraLinhaG1[0] + " Z-" + GLSettings.z_atual.ToString());
                                                    }
                                                    else
                                                    {
                                                        escritor.WriteLine(quebraLinhaG1[0] + " Z5");
                                                        upZ = false;
                                                    }
                                                }
                                                else
                                                {
                                                    escritor.WriteLine(quebraLinhaG1[0] + " Z-" + GLSettings.z_atual.ToString());
                                                }

                                                escritor.WriteLine(quebraLinhaG1[0] + " " + quebraLinhaG1[2] + " " + quebraLinhaG1[3]);

                                                string x = quebraLinhaG1[2].Replace("X", "").Replace(".", ",");
                                                string y = quebraLinhaG1[3].Replace("Y", "").Replace(".", ",");

                                                //GLSettings.upZXtoX = Math.Abs(Math.Abs(GLSettings.upZX) - Math.Abs(Convert.ToDouble(x)));
                                                //GLSettings.upZYtoY = Math.Abs(Math.Abs(GLSettings.upZY) - Math.Abs(Convert.ToDouble(y)));

                                                GLSettings.upZX = Convert.ToDouble(x);
                                                GLSettings.upZY = Convert.ToDouble(y);
                                            }
                                        }
                                        else
                                        {
                                            if (quebraLinhaG1[1].StartsWith("X") || quebraLinhaG1[1].StartsWith("Y") || quebraLinhaG1[2].StartsWith("X") || quebraLinhaG1[2].StartsWith("Y"))
                                            {
                                                if (!acabamento)
                                                {
                                                    if (!upZ)
                                                    {
                                                        escritor.WriteLine(quebraLinhaG1[0] + " Z-" + GLSettings.z_atual.ToString());
                                                    }
                                                    else
                                                    {
                                                        escritor.WriteLine(quebraLinhaG1[0] + " Z5");
                                                        upZ = false;
                                                    }
                                                }
                                                else
                                                {
                                                    escritor.WriteLine(quebraLinhaG1[0] + " Z-" + GLSettings.z_atual.ToString());
                                                }

                                                escritor.WriteLine(quebraLinhaG1[0] + " " + quebraLinhaG1[1] + " " + quebraLinhaG1[2]);

                                                string x = quebraLinhaG1[1].Replace("X", "").Replace(".", ",");
                                                string y = quebraLinhaG1[2].Replace("Y", "").Replace(".", ",");

                                                //GLSettings.upZXtoX = Math.Abs(Math.Abs(GLSettings.upZX) - Math.Abs(Convert.ToDouble(x)));
                                                //GLSettings.upZYtoY = Math.Abs(Math.Abs(GLSettings.upZY) - Math.Abs(Convert.ToDouble(y)));

                                                GLSettings.upZX = Convert.ToDouble(x);
                                                GLSettings.upZY = Convert.ToDouble(y);
                                            }                            
                                        }

                                    }
                                    else if (gcodeLinha.StartsWith("G0"))
                                    {
                                        string[] quebraLinhaG0 = new string[6];
                                        quebraLinhaG0 = gcodeLinha.Split(' ');

                                        if (initZ)
                                        {
                                            if (quebraLinhaG0.Count() > 3)
                                            {
                                                if (!acabamento)
                                                {
                                                    string x = quebraLinhaG0[2].Replace("X", "").Replace(".", ",");
                                                    string y = quebraLinhaG0[3].Replace("Y", "").Replace(".", ",");
                                                    GLSettings.upZX_ = Convert.ToDouble(x);
                                                    GLSettings.upZY_ = Convert.ToDouble(y);
                                                    double resultX = Math.Abs(Math.Abs(GLSettings.upZX) - Math.Abs(GLSettings.upZX_));
                                                    double resultY = Math.Abs(Math.Abs(GLSettings.upZY) - Math.Abs(GLSettings.upZY_));
                                                    if (resultX > 20 || resultY > 20) escritor.WriteLine(quebraLinhaG0[0] + " Z" + GLSettings.z_atual_g0);
                                                }

                                                escritor.WriteLine(quebraLinhaG0[0] + " Z-" + GLSettings.z_atual.ToString());                                                
                                                escritor.WriteLine(quebraLinhaG0[0] + " " + quebraLinhaG0[2] + " " + quebraLinhaG0[3]);                                               
                                            }
                                            else
                                            {
                                                if (!acabamento)
                                                {
                                                    string x = quebraLinhaG0[1].Replace("X", "").Replace(".", ",");
                                                    string y = quebraLinhaG0[2].Replace("Y", "").Replace(".", ",");
                                                    GLSettings.upZX_ = Convert.ToDouble(x);
                                                    GLSettings.upZY_ = Convert.ToDouble(y);
                                                    double resultX = Math.Abs(Math.Abs(GLSettings.upZX) - Math.Abs(GLSettings.upZX_));
                                                    double resultY = Math.Abs(Math.Abs(GLSettings.upZY) - Math.Abs(GLSettings.upZY_));
                                                    if (resultX > 20 || resultY > 20) escritor.WriteLine(quebraLinhaG0[0] + " Z" + GLSettings.z_atual_g0);
                                                }
                                                escritor.WriteLine(quebraLinhaG0[0] + " Z-" + GLSettings.z_atual.ToString());
                                                escritor.WriteLine(quebraLinhaG0[0] + " " + quebraLinhaG0[1] + " " + quebraLinhaG0[2]);
                                            }                                         
                                        }
                                        else
                                        {
                                            if (quebraLinhaG0.Count() > 3)
                                            {
                                                if (!acabamento)
                                                {
                                                    string x = quebraLinhaG0[2].Replace("X", "").Replace(".", ",");
                                                    string y = quebraLinhaG0[3].Replace("Y", "").Replace(".", ",");
                                                    GLSettings.upZX_ = Convert.ToDouble(x);
                                                    GLSettings.upZY_ = Convert.ToDouble(y);
                                                    double resultX = Math.Abs(Math.Abs(GLSettings.upZX) - Math.Abs(GLSettings.upZX_));
                                                    double resultY = Math.Abs(Math.Abs(GLSettings.upZY) - Math.Abs(GLSettings.upZY_));
                                                    if (resultX > 15 || resultY > 15) escritor.WriteLine(quebraLinhaG0[0] + "Z10");//" Z-" + GLSettings.z_atual_g0);
                                                }
                                                escritor.WriteLine(quebraLinhaG0[0] + " Z-" + GLSettings.z_atual.ToString());
                                                escritor.WriteLine(quebraLinhaG0[0] + " " + quebraLinhaG0[2] + " " + quebraLinhaG0[3]);                                            
                                            }
                                            else
                                            {
                                                if (!acabamento)
                                                {
                                                    string x = quebraLinhaG0[2].Replace("X", "").Replace(".", ",");
                                                    string y = quebraLinhaG0[3].Replace("Y", "").Replace(".", ",");
                                                    GLSettings.upZX_ = Convert.ToDouble(x);
                                                    GLSettings.upZY_ = Convert.ToDouble(y);
                                                    double resultX = Math.Abs(Math.Abs(GLSettings.upZX) - Math.Abs(GLSettings.upZX_));
                                                    double resultY = Math.Abs(Math.Abs(GLSettings.upZY) - Math.Abs(GLSettings.upZY_));
                                                    if (resultX > 15 || resultY > 15) escritor.WriteLine(quebraLinhaG0[0] + "Z10");//" Z-" + GLSettings.z_atual_g0);
                                                }
                                                escritor.WriteLine(quebraLinhaG0[0] + " Z-" + GLSettings.z_atual.ToString());
                                                escritor.WriteLine(quebraLinhaG0[0] + " " + quebraLinhaG0[1] + " " + quebraLinhaG0[2]);
                                            }
                                        }                                    
                                    }
                                }
                            }
                        }
                        zEncontrado = true;
                    }
                }
                zEncontrado = false;
            }
            escritor.WriteLine("G1 Z10");
            escritor.WriteLine("M30");        
            escritor.Close();
            saida.Close();

            GLSettings.gcode.Clear();
            GLSettings.gcodeZsMenosComplemento.Clear();
            GLSettings.gcodeZs.Clear();

            /*
             * Reduzindo número de G0s
             */
            Stream ajusteGeral;
            StreamReader ajusteGeral_R;

            ajusteGeral = File.Open(GLSettings.locateTMP + "_" + fileName, FileMode.Open);
            ajusteGeral_R = new StreamReader(ajusteGeral);

            string linhaAjuste = ajusteGeral_R.ReadLine();
            string G0 = "";
            List<String> ajusteGerals = new List<string>();

            bool nextG0 = false;
            string[] quebraLinhaG0_ = new string[6];
            string[] quebraLinhaG1_ = new string[6];
            while (linhaAjuste != null)
               {
                   if ((linhaAjuste.StartsWith("G0")))
                   {
                        quebraLinhaG0_ = linhaAjuste.Split(' ');

                        if (!nextG0)
                        {
                            if (quebraLinhaG0_.Count() == 2)
                            {
                                linhaAjuste = quebraLinhaG0_[0] + " " + quebraLinhaG0_[1];
                                nextG0 = true;
                                ajusteGerals.Add(linhaAjuste);
                                linhaAjuste = ajusteGeral_R.ReadLine();
                            }
                            else if (quebraLinhaG0_.Count() == 3)
                            {
                                linhaAjuste = quebraLinhaG0_[0] + " " + quebraLinhaG0_[1] + " " + quebraLinhaG0_[2];
                                ajusteGerals.Add(linhaAjuste);
                                linhaAjuste = ajusteGeral_R.ReadLine();
                            }
                        }
                        else
                        {
                            G0 = linhaAjuste;
                            linhaAjuste = ajusteGeral_R.ReadLine();
                        }
                    }
                   else if(linhaAjuste.StartsWith("G1"))
                   {
                        if(nextG0)
                        {                            
                            quebraLinhaG0_ = G0.Split(' ');
                            quebraLinhaG1_ = linhaAjuste.Split(' ');

                            linhaAjuste = quebraLinhaG0_[0] + " " + quebraLinhaG0_[1] + " " + quebraLinhaG0_[2];
                            ajusteGerals.Add(linhaAjuste);
                            linhaAjuste = quebraLinhaG1_[0] + " " + quebraLinhaG1_[1];                            
                            ajusteGerals.Add(linhaAjuste);

                            nextG0 = false;
                            linhaAjuste = ajusteGeral_R.ReadLine();
                        }
                        else
                        {
                            ajusteGerals.Add(linhaAjuste);
                            linhaAjuste = ajusteGeral_R.ReadLine();
                        }
                   }
                   else
                   {
                       ajusteGerals.Add(linhaAjuste);
                       linhaAjuste = ajusteGeral_R.ReadLine();
                   }
               }

            StreamWriter escritor_;
            Stream saida_;

            if (!acabamento)
            {
                saida_ = File.Create(GLSettings.locateTMP + "D" + fileName);
                escritor_ = new StreamWriter(saida_);
            }
            else
            {
                saida_ = File.Create(GLSettings.locateTMP + "A" + fileName);
                escritor_ = new StreamWriter(saida_);
            }

            foreach (var line in ajusteGerals)
            {
                escritor_.WriteLine(line);
            }
               
            escritor_.Close();
            saida_.Close();
            ajusteGeral.Close();
            ajusteGeral_R.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CAMGenerate()
        {
            double zComplemeto = 0;
            zEncontrado = false;
            OpenFileDialog gcode = new OpenFileDialog();
            gcode.Filter = "*Carregar arquivo|*.gcode";
            gcode.ShowDialog();

            Stream entrada = File.Open(gcode.FileName, FileMode.Open);
            Stream saida = File.Create(GLSettings.locateCAM + GLSettings.Individual_NC);

            StreamReader leitor = new StreamReader(entrada);
            StreamWriter escritor = new StreamWriter(saida);

            string linha = leitor.ReadLine();

            double initZ = GLSettings.startLayers;
            double camada = GLSettings.stepLayersDesbaste;

            camada = camada + initZ;

            while (linha != null)
            {
                if (linha.StartsWith("G1 Z"))
                {
                    initZ--;
                    camada--;
                    if (initZ > 0 || camada <= 0)
                    {
                        GLSettings.gcodeZs.Add(linha);
                        camada = GLSettings.stepLayersDesbaste;
                    }
                }
                GLSettings.gcode.Add(linha);
                linha = leitor.ReadLine();
            }

            leitor.Close();
            leitor.Dispose();
            entrada.Dispose();
            entrada.Close();

            for (int i = 1; i < GLSettings.gcodeZs.Count; i++)
            {
                string[] quebraZ = new string[2];
                string[] quebraEspaco = new string[2];
                quebraZ = GLSettings.gcodeZs[i].Split('Z');
                quebraEspaco = quebraZ[1].Split(' ');
                string zTratado = quebraEspaco[0].Replace('.', ',');
                double z = Double.Parse(zTratado);

                if (z >= zComplemeto) GLSettings.gcodeZsMenosComplemento.Add(GLSettings.gcodeZs[i]);
            }

            for (int i = 1; i < GLSettings.gcodeZsMenosComplemento.Count; i++)
            {
                foreach (var gcodeLinha in GLSettings.gcode)
                {
                    if (gcodeLinha == GLSettings.gcodeZsMenosComplemento[GLSettings.gcodeZsMenosComplemento.Count - (i + 1)] || zEncontrado)
                    {
                        if ((gcodeLinha.StartsWith("G1 Z") && zEncontrado) || gcodeLinha.StartsWith("M104") || gcodeLinha.StartsWith("M109") || gcodeLinha.StartsWith("G21") || gcodeLinha.StartsWith("G90"))
                        {
                            break;
                        }
                        if (!zEncontrado)
                        {
                            /*
                                * Inicia código Gcode
                                */
                            if ((i - 1) == 0)
                            {
                                escritor.WriteLine(GLSettings.source);
                                escritor.WriteLine(GLSettings.spindle + " " + GLSettings.turnDirection);
                                escritor.WriteLine("G1 Z-" + zComplemeto.ToString() + " " + GLSettings.feedrate);
                            }
                            else
                            {
                                string[] quebraZ = new string[2];
                                string[] quebraEspaco = new string[2];
                                quebraZ = GLSettings.gcodeZsMenosComplemento[i - 2].Split('Z');
                                quebraEspaco = quebraZ[1].Split(' ');
                                escritor.WriteLine("G1 Z-" + quebraEspaco[0]);
                            }
                        }
                        else
                        {
                            if (gcodeLinha.StartsWith("G1"))
                            {
                                string[] quebraEspaco = new string[6];

                                quebraEspaco = gcodeLinha.Split(' ');

                                foreach (var linha_ in quebraEspaco)
                                {
                                    if (linha_.StartsWith("F"))
                                    {
                                        continue;
                                    }
                                }

                                if (quebraEspaco[1].StartsWith("X") || quebraEspaco[1].StartsWith("Y"))
                                {
                                    if (quebraEspaco[3].StartsWith("F7800.000"))
                                    {
                                        if (G92)
                                        {
                                            escritor.WriteLine("G0 " + "Z-" + zComplemeto.ToString());
                                            escritor.WriteLine("G0" + " " + quebraEspaco[1] + " " + quebraEspaco[2]);

                                            string[] quebraZ = new string[2];
                                            string[] quebraEspaco2 = new string[2];
                                            quebraZ = GLSettings.gcodeZsMenosComplemento[i].Split('Z');
                                            quebraEspaco = quebraZ[1].Split(' ');
                                            escritor.WriteLine("G0 Z-" + quebraEspaco[0]);
                                            G92 = false;
                                        }
                                    }
                                    else
                                    {
                                        escritor.WriteLine(quebraEspaco[0] + " " + quebraEspaco[1] + " " + quebraEspaco[2]);
                                    }
                                }
                            }

                            if (gcodeLinha.StartsWith("G92"))
                            {
                                G92 = true;
                            }
                        }
                        zEncontrado = true;
                    }
                }
                zEncontrado = false;
            }

            escritor.WriteLine("M30");

            escritor.Close();
            saida.Close();

            GLSettings.gcode.Clear();
            GLSettings.gcodeZsMenosComplemento.Clear();
            GLSettings.gcodeZs.Clear();
        }
    }
}
