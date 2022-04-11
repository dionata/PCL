using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCLLib
{
    public static class GLSettings
    {
        public static bool teste = false;
        public static double EX = 0;
        public static double EY = 0;
        public static double panelOpenKinectX = 0;
        public static double panelOpenKinectY = 0;
        public static double TY = 0;
        public static bool ShowAxis;
        public static bool ShowNormals;
        public static float PointSize_ = 2;
        public static float PointSizeAxis = 3;
        public static float ajusteX_deslocamento = 0;
        public static float ajusteZ_deslocamento = 0;
        public static double ZAux = 0;
        public static List<double> ajusteZ_total = new List<double>() { 0,0,0,0,0,0,0,0,0,0};
        public static List<double> ajusteX_total = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteY_total = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteX_juste_rotacao90 = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteZ_total_juste_rotacao90 = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteX_total_juste_rotacao90 = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; 
        public static List<double> ajusteZ_desbasteZmin = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteZ_desbasteZmax = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteX_desbasteXmin = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteX_desbasteXmax = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteY_desbasteYmin = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteY_desbasteYmax = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public static List<double> ajusteZ_totalN = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteX_totalN = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteY_totalN = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteX_juste_rotacao90N = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteZ_total_juste_rotacao90N = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteX_total_juste_rotacao90N = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteZ_desbasteZminN = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteZ_desbasteZmaxN = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteX_desbasteXminN = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteX_desbasteXmaxN = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteY_desbasteYminN = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static List<double> ajusteY_desbasteYmaxN = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public static string ViewMode;

        public static double upZX = 0;
        public static double upZY = 0;
        public static double upZX_ = 0;
        public static double upZY_ = 0;
        public static double upZXtoX = 0;
        public static double upZYtoY = 0;
        public static double initPointZ = 0;
        public static double initPointY = 0;

        public static double scaleX = 1;
        public static double scaleY = 1;
        public static double scaleZ = 1;

        public static bool separarEncostoAssento = false;
        public static bool MarcarEncosto = true;
        public static bool MarcarAssento = false;
        public static string DiferenciarAssentoArquivo = "";

        /* Suavização */
        public static string filterSmoothing_type = "Uniform";
        public static double filterSmoothing_smoothSpeedT_ = 0.01;

        /* Blank - bloco principal ativado */
        public static bool MainBlock = true;

        /* Bloco 1 */

        public const double BaseX = 370;
        public const double BaseY = 450;
        public const double BaseZ = 150;

        public static double Bloco1XEncosto = BaseX;
        public static double Bloco1YEncosto = BaseY;
        public static double Bloco1ZEncosto = BaseZ;

        public static double Bloco1XAssento = BaseX;
        public static double Bloco1YAssento = BaseY;
        public static double Bloco1ZAssento = BaseZ;

        /* Bloco 2 */
        public static double Bloco2XEncostoInit = Bloco1XEncosto;
        public static double Bloco2XEncosto = 200;        
        public static double Bloco2YEncosto = 460;
        public static double Bloco2ZEncosto = 175;
        public static double Bloco2ZEncostoInit = Bloco1ZEncosto;
        public static double Bloco2XEncostoFinish = Bloco2XEncostoInit + Bloco2XEncosto;
        public static double Bloco2ZEncostoFinish = Bloco2ZEncostoInit + Bloco2ZEncosto;

        public static double Bloco2XAssentoInit = Bloco1XAssento;
        public static double Bloco2XAssento = 200;
        public static double Bloco2YAssento = 460;
        public static double Bloco2ZAssento = 175;
        public static double Bloco2AssentoFinish = Bloco2XAssentoInit + Bloco2XAssento;

        /* Bloco 3 */
        public static double Bloco3XEncostoInit = Bloco2XEncostoFinish;
        public static double Bloco3XEncosto = 400;        
        public static double Bloco3YEncosto = 460;
        public static double Bloco3ZEncosto = 175;
        public static double Bloco3ZEncostoInit = Bloco2ZEncostoFinish;
        public static double Bloco3XEncostoFinish = Bloco3XEncostoInit + Bloco3XEncosto;
        public static double Bloco3ZEncostoFinish = Bloco3ZEncostoInit + Bloco3ZEncosto;

        public static double Bloco3XAssentoInit = Bloco2AssentoFinish;
        public static double Bloco3XAssento = 100;
        public static double Bloco3YAssento = 460;
        public static double Bloco3ZAssento = 175;
        public static double Bloco3XAssentoFinish = Bloco3XAssentoInit + Bloco3XAssento;

        /* Bloco 4 */
        public static double Bloco4XEncostoInit = Bloco3XEncostoFinish;
        public static double Bloco4XEncosto = 150;        
        public static double Bloco4YEncosto = 460;
        public static double Bloco4ZEncosto = 175;
        public static double Bloco4ZEncostoInit = Bloco3ZEncostoFinish;
        public static double Bloco4XEncostoFinish = Bloco4XEncostoInit + Bloco4XEncosto;
        public static double Bloco4ZEncostoFinish = Bloco4ZEncostoInit + Bloco4ZEncosto;

        public static double Bloco4XAssentoInit = Bloco3XAssentoFinish;
        public static double Bloco4XAssento = 100;
        public static double Bloco4YAssento = 460;
        public static double Bloco4ZAssento = 175;
        public static double Bloco4XAssentoFinish = Bloco4XAssentoInit + Bloco4XAssento;

        /* Bloco 5 */
        public static double Bloco5XEncostoInit = Bloco4XEncostoFinish;
        public static double Bloco5XEncosto = 200;        
        public static double Bloco5YEncosto = 460;
        public static double Bloco5ZEncosto = 175;
        public static double Bloco5ZEncostoInit = Bloco4ZEncostoFinish;
        public static double Bloco5XEncostoFinish = Bloco5XEncostoInit + Bloco5XEncosto;
        public static double Bloco5ZEncostoFinish = Bloco5ZEncostoInit + Bloco5ZEncosto;

        public static double Bloco5XAssentoInit = Bloco4XAssentoFinish;
        public static double Bloco5XAssento = 100;
        public static double Bloco5YAssento = 460;
        public static double Bloco5ZAssento = 175;
        public static double Bloco5XAssentoFinish = Bloco5XAssentoInit + Bloco5XAssento;

        public static List<double> zComplemento = new List<double>();

        public static double apagadorX = 50;
        public static double apagadorY = 50;
        public static double apagadorZ = 50;

        public static int numberDivblocoExecutado = 1;
        public static int numberDivblocoPlanejado = 1;
        public static string modoFuncionamentoCorte = "Corte Z";

        public static string Encosto = "Encosto.obj";
        public static string Bloco1Out_ = "Bloco1Out_.obj";
        public static string Bloco2Out_ = "Bloco2Out_.obj";
        public static string Bloco3Out_ = "Bloco3Out_.obj";

        public static string Bloco2InvTMP_ = "Bloco2InvTMP_.obj";

        public static string Bloco1InvOut_ = "Bloco1InvOut_.obj";
        public static string Bloco2InvOut_ = "Bloco2InvOut_.obj";
        public static string Bloco3InvOut_ = "Bloco3InvOut_.obj";

        public static string Bloco1InvOutSTL = "Bloco1InvOut_.stl";
        public static string Bloco2InvOutSTL = "Bloco2InvOut_.stl";
        public static string Bloco3InvOutSTL = "Bloco3InvOut_.stl";

        public static string Bloco4Out_ = "Bloco4Out_.obj";
        public static string Bloco5Out_ = "Bloco5Out_.obj";
        public static string BlocoTeste1 = "BlocoTeste1.obj";
        public static string BlocoTeste2 = "BlocoTeste2.obj";
        public static string Assento = "Assento.obj";

        public static string EncostoOut_Tringulos = "EncostoOut_Tringulos.obj";
        public static string Bloco1Out_Tringulos = "Bloco1Out_Tringulos.obj";
        public static string Bloco2Out_Tringulos = "Bloco2Out_Tringulos.obj";
        public static string Bloco3Out_Tringulos = "Bloco3Out_Tringulos.obj";
        public static string Bloco4Out_Tringulos = "Bloco4Out_Tringulos.obj";
        public static string Bloco5Out_Tringulos = "Bloco5Out_Tringulos.obj";

        public static string EncostoOut_Tringulos_Desbaste = "EncostoOut_Tringulos_desbaste.obj";
        public static string Bloco1Out_Tringulos_Desbaste = "Bloco1Out_Tringulos_desbaste.obj";
        public static string Bloco2Out_Tringulos_Desbaste = "Bloco2Out_Tringulos_desbaste.obj";
        public static string Bloco3Out_Tringulos_Desbaste = "Bloco3Out_Tringulos_desbaste.obj";
        public static string Bloco4Out_Tringulos_Desbaste = "Bloco4Out_Tringulos_desbaste.obj";
        public static string Bloco5Out_Tringulos_Desbaste = "Bloco5Out_Tringulos_desbaste.obj";

        public static string Desbaste_ComplementoEncosto = "Desbaste_ComplementoEncosto.stl";
        public static string Desbaste_Complemento1 = "Desbaste_Complemento1.stl";
        public static string Desbaste_Complemento2 = "Desbaste_Complemento2.stl";
        public static string Desbaste_Complemento3 = "Desbaste_Complemento3.stl";
        public static string Desbaste_Complemento4 = "Desbaste_Complemento4.stl";
        public static string Desbaste_Complemento5 = "Desbaste_Complemento5.stl";
        public static string Desbaste_ComplementoEncostoG = "Desbaste_ComplementoEncosto.gcode";
        public static string Desbaste_ComplementoG = "Desbaste_Complemento.gcode";
        public static string Desbaste_Complemento1G = "Desbaste_Complemento1.gcode";
        public static string Desbaste_Complemento2G = "Desbaste_Complemento2.gcode";
        public static string Desbaste_Complemento3G = "Desbaste_Complemento3.gcode";
        public static string Desbaste_Complemento4G = "Desbaste_Complemento4.gcode";
        public static string Desbaste_Complemento5G = "Desbaste_Complemento5.gcode";
        public static string Desbaste_ComplementoEncostoNC = "Desbaste_ComplementoEncosto.nc";
        public static string Desbaste_ComplementoNC = "Desbaste_Complemento.nc";
        public static string Desbaste_Complemento1NC = "Desbaste_Complemento1.nc";
        public static string Desbaste_Complemento2NC = "Desbaste_Complemento2.nc";
        public static string Desbaste_Complemento3NC = "Desbaste_Complemento3.nc";
        public static string Desbaste_Complemento4NC = "Desbaste_Complemento4.nc";
        public static string Desbaste_Complemento5NC = "Desbaste_Complemento5.nc";

        public static string EncostoOut_Tringulos_Tampa = "EncostoOut_Tringulos_Tampa.obj";
        public static string Bloco1Out_Tringulos_Tampa = "Bloco1Out_Tringulos_Tampa.obj";
        public static string Bloco2Out_Tringulos_Tampa = "Bloco2Out_Tringulos_Tampa.obj";
        public static string Bloco3Out_Tringulos_Tampa = "Bloco3Out_Tringulos_Tampa.obj";
        public static string Bloco4Out_Tringulos_Tampa = "Bloco4Out_Tringulos_Tampa.obj";
        public static string Bloco5Out_Tringulos_Tampa = "Bloco5Out_Tringulos_Tampa.obj";

        public static string EncostoOut_Tringulos_solido = "EncostoOut_Tringulos_solido.obj";
        public static string Bloco1Out_Tringulos_solido = "Bloco1Out_Tringulos_solido.obj";
        public static string Bloco2Out_Tringulos_solido = "Bloco2Out_Tringulos_solido.obj";
        public static string Bloco3Out_Tringulos_solido = "Bloco3Out_Tringulos_solido.obj";
        public static string Bloco4Out_Tringulos_solido = "Bloco4Out_Tringulos_solido.obj";
        public static string Bloco5Out_Tringulos_solido = "Bloco5Out_Tringulos_solido.obj";

        public static string EncostoOut_Tringulos_solido_desbaste = "Bloco1Out_Tringulos_solido_desbaste.obj";
        public static string Bloco1Out_Tringulos_solido_desbaste = "Bloco1Out_Tringulos_solido_desbaste.obj";
        public static string Bloco2Out_Tringulos_solido_desbaste = "Bloco2Out_Tringulos_solido_desbaste.obj";
        public static string Bloco3Out_Tringulos_solido_desbaste = "Bloco3Out_Tringulos_solido_desbaste.obj";
        public static string Bloco4Out_Tringulos_solido_desbaste = "Bloco4Out_Tringulos_solido_desbaste.obj";
        public static string Bloco5Out_Tringulos_solido_desbaste = "Bloco5Out_Tringulos_solido_desbaste.obj";

        public static string EncostoOut_Tringulos_solido_desbasteSTL = "Bloco1Out_Tringulos_solido_desbaste.stl";
        public static string Bloco1Out_Tringulos_solido_desbasteSTL = "Bloco1Out_Tringulos_solido_desbaste.stl";
        public static string Bloco2Out_Tringulos_solido_desbasteSTL = "Bloco2Out_Tringulos_solido_desbaste.stl";
        public static string Bloco3Out_Tringulos_solido_desbasteSTL = "Bloco3Out_Tringulos_solido_desbaste.stl";
        public static string Bloco4Out_Tringulos_solido_desbasteSTL = "Bloco4Out_Tringulos_solido_desbaste.stl";
        public static string Bloco5Out_Tringulos_solido_desbasteSTL = "Bloco5Out_Tringulos_solido_desbaste.stl";

        public static string Encosto_solido_desbaste_G = "Encosto_solido_desbaste.gcode";
        public static string Bloco1_solido_desbaste_G = "Bloco1_solido_desbaste.gcode";
        public static string Bloco2_solido_desbaste_G = "Bloco2_solido_desbaste.gcode";
        public static string Bloco3_solido_desbaste_G = "Bloco3_solido_desbaste.gcode";
        public static string Bloco4_solido_desbaste_G = "Bloco4_solido_desbaste.gcode";
        public static string Bloco5_solido_desbaste_G = "Bloco5_solido_desbaste.gcode";

        public static string Encosto_solido_acabamento_G = "Encosto_solido_acabamento.gcode";
        public static string Bloco1_solido_acabamento_G = "Bloco1_solido_acabamento.gcode";
        public static string Bloco2_solido_acabamento_G = "Bloco2_solido_acabamento.gcode";
        public static string Bloco3_solido_acabamento_G = "Bloco3_solido_acabamento.gcode";
        public static string Bloco4_solido_acabamento_G = "Bloco4_solido_acabamento.gcode";
        public static string Bloco5_solido_acabamento_G = "Bloco5_solido_acabamento.gcode";

        public static string Encosto_solido_desbaste_NC = "Encosto_solido_desbaste.nc";
        public static string Bloco1_solido_desbaste_NC = "Bloco1_solido_desbaste.nc";
        public static string Bloco2_solido_desbaste_NC = "Bloco2_solido_desbaste.nc";
        public static string Bloco3_solido_desbaste_NC = "Bloco3_solido_desbaste.nc";
        public static string Bloco4_solido_desbaste_NC = "Bloco4_solido_desbaste.nc";
        public static string Bloco5_solido_desbaste_NC = "Bloco5_solido_desbaste.nc";

        public static string Encosto_solido_acabamento_NC = "Encosto_solido_acabamento.nc";
        public static string Bloco1_solido_acabamento_NC = "Bloco1_solido_acabamento.nc";
        public static string Bloco2_solido_acabamento_NC = "Bloco2_solido_acabamento.nc";
        public static string Bloco3_solido_acabamento_NC = "Bloco3_solido_acabamento.nc";
        public static string Bloco4_solido_acabamento_NC = "Bloco4_solido_acabamento.nc";
        public static string Bloco5_solido_acabamento_NC = "Bloco5_solido_acabamento.nc";

        public static string Encosto_solido_AD_NC = "Encosto_solido_AD_NC.nc";
        public static string Bloco1_solido_AD_NC = "Bloco1_solido_AD_NC.nc";
        public static string Bloco2_solido_AD_NC = "Bloco2_solido_AD_NC.nc";
        public static string Bloco3_solido_AD_NC = "Bloco3_solido_AD_NC.nc";
        public static string Bloco4_solido_AD_NC = "Bloco4_solido_AD_NC.nc";
        public static string Bloco5_solido_AD_NC = "Bloco5_solido_AD_NC.nc";

        public static string Encosto_solido_acabamentoXZ_NC = "Encosto_solido_acabamentoXZ.nc";
        public static string Bloco1_solido_acabamentoXZ_NC = "Bloco1_solido_acabamentoXZ.nc";
        public static string Bloco2_solido_acabamentoXZ_NC = "Bloco2_solido_acabamentoXZ.nc";
        public static string Bloco3_solido_acabamentoXZ_NC = "Bloco3_solido_acabamentoXZ.nc";
        public static string Bloco4_solido_acabamentoXZ_NC = "Bloco4_solido_acabamentoXZ.nc";
        public static string Bloco5_solido_acabamentoXZ_NC = "Bloco5_solido_acabamentoXZ.nc";
        public static string Individual_G = "Individual.gcode";
        public static string Individual_NC = "Individual.nc";

        public static string locateCAD = @".\projeto\cad\"; //sólido
        public static string locateMALHA = @".\projeto\malha\"; //modelo       
        public static string locateSTL = @".\projeto\stl\";
        public static string locateCAM = @".\projeto\cam\";
        public static string locateTMP = @".\projeto\tmp\";

        public static string locateRAIZ = @".\projeto\";
        public static string locateSLICER = @".\slicer\";
        public static string locateCURRENT = "";
        public static int projectCURRENT = 0;

        public static int[] color = { 55, 71, 79 };

        public enum structProject
        {
            cad = 0,
            malha,
            stl,
            cam
        }
        public static List<string> newMproject = new List<string>();
        public static int numberProject = 0;

        public static double zMaxModelo = 0;
        public static string ModeloSelect;             
        public static bool gerarAssento = false;

        public static bool gerarGcode = false;
        public static List<String> gcodeZs = new List<string>();
        public static List<String> gcodeZs_aux = new List<string>();
        public static List<String> gcodeZs_linha = new List<string>();
        public static List<String> Desbaste_Complemento2NC_Line = new List<string>(); 
        public static List<String> Bloco_solido_desbaste_NC_Line = new List<string>();
        public static List<String> aux = new List<string>();
        public static int z_atual;
        public static int z_atual_g0;

        public static List<String> gcodeZsMenosComplemento = new List<string>();
        public static List<String> gcode = new List<string>();

        /*
         * Valores default para contrução do CAM
         * 
         */
        public static string filament_diameter_aux = "12";
        public static string spindle_aux = "18000";
        public static string feedrate_aux = "8000";

        //Desbaste
        //slicer\Slic3r-console.exe Bloco1Out_Tringulos_solido_desbaste.obj --perimeters 1 --print-center 100,230 --layer-height 0.8 --fill-density 99 --filament-diameter 6 --nozzle-diameter 6 --output teste.gcode
        public static string flavor = "reprap";
        //public static string use_firmware_retraction = "yes";
        public static string perimeters = "3"; //comando name: --perimeters
        public static string print_center = "0,0"; //comando name: --print-center
        public static string layer_height = "1.0"; //"1.0"; //comando name: --layer-height
        public static string fill_density = "99.9"; //comando name: --fill-density
        public static string filament_diameter = "9.5";//"3.4";//"1.7"; //comando name: --filament-diameter
        public static string nozzle_diameter = "9.5";//"3.4";//"3.4";//"1.7"; //comando name: --nozzle-diameter
        public static string feedrate = "F4000"; //comando name: --nozzle-diameter
        public static string turnDirection = "M3";
        public static string spindle = "S30000";
        public static string source = "G54.1";
        public static string solid_infill_below_area = "1";
        public static string solid_infill_every_layers = "1";
        public static string top_solid_layers = "1";
        public static string bottom_solid_layers = "1";
        public static string fill_angle = "0";//"45";
        public static double startLayers = 2;
        public static double stepLayersDesbaste = 5;//acima de 1 mm layer_height deve ser configurado esta variáveis que irá "pular" as camadas

        //Acabamento
        public static string flavorA = "no-extrusion";
        //public static string use_firmware_retractionA = "yes";
        public static string perimetersA = "3"; //comando name: --perimeters
        public static string print_centerA = "0,0"; //comando name: --print-center
        public static string layer_heightA = "1.0"; //"1.0"; //comando name: --layer-height
        public static string fill_densityA = "0"; //comando name: --fill-density
        public static string filament_diameterA = "9.5";//"1";//"3.4";//"1.7"; //comando name: --filament-diameter
        public static string nozzle_diameterA = "9.5";//"3.4";//"1.7"; //comando name: --nozzle-diameter
        public static string feedrateA = "F4000"; //comando name: --nozzle-diameter
        public static string turnDirectionA = "M3";
        public static string spindleA = "S30000";
        public static string sourceA = "G54.1";
        public static string solid_infill_below_areaA = "0";
        public static string solid_infill_every_layersA = "0";
        public static string top_solid_layersA = "0";
        public static string bottom_solid_layersA = "0";
        public static string fill_angleA = "0"; 
        public static double startLayersA = 1;
        public static double stepLayersAcabamento = 5;//acima de 1 mm layer_height deve ser configurado esta variáveis que irá "pular" as camadas

        /*
        * Fim dos valores        
        */

        public static bool uniao = false;
        public static bool projectionFree = false;
        public static bool projectionFreeAjuste = false;

        public static int reposicaoX = 0;
        public static int reposicaoY = 0;

        public static string ModeloAuxOut_ = "auxModeloResult.obj";
        public static string ModeloAux = "auxModelo.obj";
        public static string ModeloAux2 = "auxModelo2.obj";
        public static string ModeloAux3 = "auxModelo3.obj";
        public static string MarcadorNome = "Marcação Corte";
        public static string nomeDorso = "";

        public static Matrix4d projectionMatrix4_;
        public static Matrix4d modelViewMatrix4_;
        public static Matrix4 projectionMatrix_ = new Matrix4();
        public static Matrix4 modelViewMatrix_;

        public enum tipoProjecao
        {
            frontal = 0,
            esquerda,
            direita,
            superior,
            inferior,
            fundo,
            ZX
        }
        public static int selecionarTipoProjecao = (int)tipoProjecao.frontal;
        public static int selecionarTipoProjecaoAux = 0;

        public static bool marcarRegiaoOn = false;
        public static bool marcacaoPolygonOn = false;
        public static bool marcacaoPolygonOnRemove = false;
        public static bool removerPontosOn = false;
        public static bool removerTriangulosOn = false;
        public static bool projecaoPerpectiva = false;
        public static bool desmarcar = false;
        public static float projecaoOrtogonalEscala = 1;
        public static List<int> vertexMarcacao = new List<int>();
        public static int vertexMarcacaoThread;
        public static int initPolygon = 0;

        public static void InitFromSettings()
        {
            ShowAxis = Properties.Settings.Default.ShowAxis;
            PointSize_ = Properties.Settings.Default.PointSize;
            PointSizeAxis = Properties.Settings.Default.PointSizeAxis;
            ViewMode = Properties.Settings.Default.ViewMode;
            //scaleX = Properties.Settings.Default.scaleX;
            //scaleY = Properties.Settings.Default.scaleY;
            //scaleZ = Properties.Settings.Default.scaleZ;
                      
        }
        public static void SaveSettings()
        {
            Properties.Settings.Default.ShowAxis = ShowAxis;
            Properties.Settings.Default.PointSize = PointSize_;
            Properties.Settings.Default.PointSizeAxis = PointSizeAxis;
            Properties.Settings.Default.ViewMode = ViewMode;
            /*
            Properties.Settings.Default.scaleX = scaleX;
            Properties.Settings.Default.scaleY = scaleY;
            Properties.Settings.Default.scaleZ = scaleZ;

            Properties.Settings.Default.numberDivblocoExecutado = numberDivblocoExecutado;
            Properties.Settings.Default.Bloco1XEncosto = Bloco1XEncosto;
            Properties.Settings.Default.Bloco1YEncosto = Bloco1YEncosto;
            Properties.Settings.Default.Bloco1ZEncosto = Bloco1ZEncosto;
            Properties.Settings.Default.Bloco2XEncosto = Bloco2XEncosto;
            Properties.Settings.Default.Bloco2YEncosto = Bloco2YEncosto;
            Properties.Settings.Default.Bloco2ZEncosto = Bloco2ZEncosto;
            Properties.Settings.Default.Bloco3XEncosto = Bloco3XEncosto;
            Properties.Settings.Default.Bloco3YEncosto = Bloco3YEncosto;
            Properties.Settings.Default.Bloco3ZEncosto = Bloco3ZEncosto;

            Properties.Settings.Default.apagadorX = apagadorX;
            Properties.Settings.Default.apagadorY = apagadorY;
            Properties.Settings.Default.apagadorZ = apagadorZ;
            */
            Properties.Settings.Default.Save();
        }
        public static void SetDefaultSettings()
        {
            SaveSettings();
            InitFromSettings();

        }

        public static void atualizarProjecao(OpenGLControl OpenGLControl)
        {
            switch (GLSettings.selecionarTipoProjecao)
            {
                case (int)GLSettings.tipoProjecao.frontal:
                    OpenGLControl.GLrender.projecaoFrontal(false);
                    break;
                case (int)GLSettings.tipoProjecao.fundo:
                    OpenGLControl.GLrender.projecaoFundo(false);
                    break;
                case (int)GLSettings.tipoProjecao.ZX:
                    OpenGLControl.GLrender.projecaoZX(false);
                    break;
                case (int)GLSettings.tipoProjecao.esquerda:
                    OpenGLControl.GLrender.projecaoEsquerda(false);
                    break;
                case (int)GLSettings.tipoProjecao.direita:
                    OpenGLControl.GLrender.projecaoDireita(false);
                    break;
                case (int)GLSettings.tipoProjecao.superior:
                    OpenGLControl.GLrender.projecaoSuperior(false);
                    break;
                case (int)GLSettings.tipoProjecao.inferior:
                    OpenGLControl.GLrender.projecaoInverior(false);
                    break;
            }
        }
    }
}
