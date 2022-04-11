using g3;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using PCL_Models.Class;

namespace PCLLib.IO
{
    public class STLinIO
    {
        /**
         */ 
        public string export(string out_path, string path, string type)
        {
            DMesh3 mesh = StandardMeshReader.ReadMesh(path);
            //DMesh3 mesh = IO.GeneralIO.LoadtMesh(path);         

            StandardMeshWriter writer = new StandardMeshWriter();
            var list = new List<WriteMesh>() { new WriteMesh(mesh) };

            switch(type)
            {
                case "obj":
                    if (writer.Write(out_path, list, WriteOptions.Defaults).code != IOCode.Ok)
                        return "test_write_formats: obj failed";
                    break;
                case "stl":
                    if (writer.Write(out_path, list, WriteOptions.Defaults).code != IOCode.Ok)
                        return "test_write_formats: stl failed";                
                    break;
                case "off":
                    if (writer.Write(out_path, list, WriteOptions.Defaults).code != IOCode.Ok)
                        return "test_write_formats: off failed";
                    break;
                case "g3mesh": 
                    if (writer.Write(out_path, list, WriteOptions.Defaults).code != IOCode.Ok)
                        return "test_write_formats: g3mesh failed";
                    break;
            }
            MemoryStream fileStream = new MemoryStream();
            MemoryStream mtlStream = new MemoryStream();
            writer.OpenStreamF = (filename) => 
            {
                return filename.EndsWith(".mtl") ? mtlStream : fileStream;
            };
            writer.CloseStreamF = (stream) => { };

            WriteOptions opt = WriteOptions.Defaults;

            opt.bWriteMaterials = true;
            opt.MaterialFilePath = out_path + ".mtl";

            if (writer.Write(out_path + ".obj", list, opt).code != IOCode.Ok)
                System.Console.WriteLine("test_write_formats: write to memory stream failed");

            //string s = Encoding.ASCII.GetString(fileStream.ToArray());
            if (fileStream.Length == 0)
                System.Console.WriteLine("test_write_formats: write to memory stream produced zero-length stream");

            return "ok";
        }

        public string importSTL(string in_path)
        {

            //  StandardMeshReader read = new StandardMeshReader();
            //  IOReadResult result = new IOReadResult();
            //IMeshBuilder perspectivaConsolidacao;

            //DMesh3 mesh = StandardMeshReader.ReadFile(in_path, ReadOptions.Defaults);

          //  BinaryReader leitura;
          //  leitura.

         //   STLReader stlRead = new STLReader();
          //  stlRead.Read();
          //  var list = new List<WriteMesh>() { new WriteMesh(mesh) };
          // result = read.Read(in_path, ReadOptions.Defaults);
          //  if (writer.Write("\\", list, WriteOptions.Defaults).code != IOCode.Ok)
          //       return "test_write_formats: obj failed";


            return "ok";
        }       
    }
}
