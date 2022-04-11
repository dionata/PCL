using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCLLib.IO
{
    public class GeneralIO
    {
        public static DMesh3 LoadtMesh(string sPath)
        {
            StandardMeshReader reader = new StandardMeshReader();
            reader.MeshBuilder = new DMesh3Builder();
            reader.Read(sPath, new ReadOptions());
            return (reader.MeshBuilder as DMesh3Builder).Meshes[0];
        }

        public static void SaveMesh(IMesh mesh, string sfilename, bool write_groups = true, bool write_vtxcolors = false, bool write_vtxuv = false)
        {
            OBJWriter writer = new OBJWriter();
            var s = new System.IO.StreamWriter(sfilename, false);  
            List<WriteMesh> meshes = new List<WriteMesh>() { new WriteMesh(mesh) };
            writer.Write(s, meshes, new WriteOptions() { bWriteGroups = write_groups, bPerVertexColors = write_vtxcolors, bPerVertexUVs = write_vtxuv });
            s.Close();
        }
    }
}
