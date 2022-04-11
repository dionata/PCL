using g3;
using KinectUtils;
using OpenCLTemplate.CLGLInterop;
using System.Numerics;
//using CSG;

namespace PCLLib.Utils
{
    public class GenerateSolid
    {
        public void SetGenerateSolid(Model3D first, string second)
        {
            /*
            Model3D test = new Model3D();
            
            // Try to grab any meshes this is attached to - if not, allow setting!
            MeshFilter mf = perspectivaConsolidacao.GetComponent<MeshFilter>();
            if (mf == null)
            {
                Debug.LogError("No mesh filter to get vertices from.");
                return;
            }

            perspectivaConsolidacao = new Solid(mf.sharedMesh.vertices, mf.sharedMesh.GetIndices);
            // Make sure the transform has been pushed into the solid.
            perspectivaConsolidacao. ApplyMatrix(perspectivaConsolidacao.transform.localToWorldMatrix);
            */

            // Mesh teste1 = Mesh.LoadFromPLYFile(first);
            // Mesh teste2 = Mesh.LoadFromPLYFile(second);
            //Model3D perspectivaConsolidacao;


            //CarveMesh teste11 = new CarveMesh();
            //CarveMesh teste12 = new CarveMesh();

            //  CarveMesh result = CarveSharp.CarveSharp.PerformCSG(, CarveSharp.CarveSharp.CSGOperations.AMinusB);
            //Mesh testeFinal = new Mesh(result.Vertices.GetValue[], result.FaceIndices, null);
            //testeFinal. SaveMesh(@"D:\perspectivaConsolidacao.PLY");

            // Solid hole2 = Cylinder(HoleDiameter / 2, BracketHeight * 4, center: true).Translate(-TotalWidth / 2 + HoleInset, BodyDescent, HoleSpacing / 2);

            //  teste2.

            //GLRender.

            //   Solid3d . CreateBox


            // Solid perspectivaConsolidacao = new Solid();

            //perspectivaConsolidacao = mesh.            

            // Csg.Matrix4x4 teste3 = new Csg.Matrix4x4();

            // teste3.LeftMultiply1x3Vector(first.Parts[0].Triangles.);
            /*
            CSG.Shapes

            var shape1 = new Cube(position: new Vector3(0, 0, 0), size: new Vector3(0.50f, 0.50f, 0.50f));
            var shape2 = new Cube(position: new Vector3(1, 1, 0), size: new Vector3(0.50f, 0.50f, 0.50f));
            var result = shape1.Intersect(shape2);

            CSG.Vertex teste3 = new CSG.Vertex();
            teste3.

            var shape3 = new CSG.Vertex(first.VertexList); //cVertexList(first.VertexList[0]);

            foreach (var vr in first.VertexList)
            {
                
            }
            */
            
           
            
          //  CSG.Model ttt = new Model(first.Parts[0]);
          //  CSG.Model ttt2 = new Model(first.Parts[0]);
         //   ttt.Subtract(ttt2);
        }
    }
}
