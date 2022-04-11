using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCLLib.Utils
{
    public class Filters
    { 
        /// <summary>
        /// 
        /// </summary>
        private string ModeloAux = GLSettings.locateTMP + GLSettings.ModeloAux;        

        /// <summary>
        /// 
        /// </summary>
        private string ModeloAuxOut_ = "auxModeloResult.obj";
       
        /// <summary>
        /// 
        /// </summary>
        private EdgeRefineFlags useFlags;

        /// <summary>
        /// FilterOptimization
        /// </summary>
        private int triangleCount = 500000;

        /// <summary>
        /// 
        /// </summary>
        private double edgeLength = 5.0;

        /// <summary>
        /// FilterSmoothing
        /// private double smoothSpeedT = 0.7f;//0.5f; 
        /// FilterReduceConstraintsFixedverts
        /// </summary>
        private int reduceToTriangleCount = 50000;//100000;

        /// <summary>
        /// 
        /// </summary>
        public void FilterSmoothing()
        {
            DMesh3 mesh = IO.GeneralIO.LoadtMesh(ModeloAux);

            Remesher r = new Remesher(mesh);
            r.EnableFlips = r.EnableSplits = r.EnableCollapses = false;
            r.EnableSmoothing = true;
            r.SmoothSpeedT = GLSettings.filterSmoothing_smoothSpeedT_;//smoothSpeedT;  //peso da suavizacao
       
            /*
             * F001: SmoothTypes.Cotan
             * F002: SmoothTypes.Uniform
             * F003: SmoothTypes.MeanValue
             */

            switch (GLSettings.filterSmoothing_type)
            {
                case "Cotan":
                    r.SmoothType = Remesher.SmoothTypes.Cotan;
                    break;
                case "Uniform":
                    r.SmoothType = Remesher.SmoothTypes.Uniform;
                    break;
                case "MeanValue":
                    r.SmoothType = Remesher.SmoothTypes.MeanValue;
                    break;
            }           

            for (int k = 0; k < 100; ++k)
            {
                r.BasicRemeshPass();
                //mesh.CheckValidity();
            }
            IO.GeneralIO.SaveMesh(mesh, GLSettings.locateTMP + ModeloAuxOut_);
        }

        /// <summary>
        /// suaviza e já atualiza arquivo. 
        /// </summary>
        /// <param name="path"></param>
        public void FilterSmoothing(string path)
        {
            DMesh3 mesh = IO.GeneralIO.LoadtMesh(path);

            Remesher r = new Remesher(mesh);
            r.EnableFlips = r.EnableSplits = r.EnableCollapses = false;
            r.EnableSmoothing = true;
            r.SmoothSpeedT = GLSettings.filterSmoothing_smoothSpeedT_;//smoothSpeedT;  //peso da suavizacao

            /*
             * F001: SmoothTypes.Cotan
             * F002: SmoothTypes.Uniform
             * F003: SmoothTypes.MeanValue
             */

            switch (GLSettings.filterSmoothing_type)
            {
                case "Cotan":
                    r.SmoothType = Remesher.SmoothTypes.Cotan;
                    break;
                case "Uniform":
                    r.SmoothType = Remesher.SmoothTypes.Uniform;
                    break;
                case "MeanValue":
                    r.SmoothType = Remesher.SmoothTypes.MeanValue;
                    break;
            }

            for (int k = 0; k < 100; ++k)
            {
                r.BasicRemeshPass();
                //mesh.CheckValidity();
            }
            IO.GeneralIO.SaveMesh(mesh, path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void FilterReduceConstraintsFixedverts(string type)
        {
            DMesh3 mesh = IO.GeneralIO.LoadtMesh(ModeloAux); 
            MeshUtil.ScaleMesh(mesh, Frame3f.Identity, new Vector3f(1, 1, 1));
            mesh.CheckValidity();
            AxisAlignedBox3d bounds = mesh.CachedBounds;

            // construct mesh projection target
            DMesh3 meshCopy = new DMesh3(mesh);
            meshCopy.CheckValidity();
            DMeshAABBTree3 tree = new DMeshAABBTree3(meshCopy);
            tree.Build();            

            MeshProjectionTarget target = new MeshProjectionTarget()
            {
                Mesh = meshCopy,
                Spatial = tree
            };

            // construct constraint set
            MeshConstraints cons = new MeshConstraints();

            // PreserveTopology;           
            /* O002: EdgeRefineFlags.FullyConstrained
             * O003: EdgeRefineFlags.NoCollapse
             * O004: EdgeRefineFlags.NoConstraint
             * O005: EdgeRefineFlags.NoFlip
             * O006: EdgeRefineFlags.NoSplit
             * O007: EdgeRefineFlags.PreserveTopology
             */ 
            switch (type)
            {
                case "FullyConstrained":
                    useFlags = EdgeRefineFlags.FullyConstrained;
                    break;
                case "NoCollapse":
                    useFlags = EdgeRefineFlags.NoCollapse;
                    break;
                case "NoConstraint":
                    useFlags = EdgeRefineFlags.NoConstraint;
                    break;
                case "NoFlip":
                    useFlags = EdgeRefineFlags.NoFlip;
                    break;
                case "NoSplit":
                    useFlags = EdgeRefineFlags.NoSplit;
                    break;
                case "PreserveTopology":
                    useFlags = EdgeRefineFlags.PreserveTopology;
                    break;
            }

            foreach (int eid in mesh.EdgeIndices())
            {
                double fAngle = MeshUtil.OpeningAngleD(mesh, eid);
                if (fAngle > 30.0f)
                {
                    cons.SetOrUpdateEdgeConstraint(eid, new EdgeConstraint(useFlags) { TrackingSetID = 1 });
                    Index2i ev = mesh.GetEdgeV(eid);
                    int nSetID0 = (mesh.GetVertex(ev[0]).y > bounds.Center.y) ? 1 : 2;
                    int nSetID1 = (mesh.GetVertex(ev[1]).y > bounds.Center.y) ? 1 : 2;
                    cons.SetOrUpdateVertexConstraint(ev[0], new VertexConstraint(true, nSetID0));
                    cons.SetOrUpdateVertexConstraint(ev[1], new VertexConstraint(true, nSetID1));
                }
            }

            Reducer r = new Reducer(mesh);
            r.SetExternalConstraints(cons);
            r.SetProjectionTarget(target);
            r.ReduceToTriangleCount(reduceToTriangleCount); //numero de triangulos 
            mesh.CheckValidity();
            IO.GeneralIO.SaveMesh(mesh, GLSettings.locateTMP + GLSettings.ModeloAuxOut_);
        }

        /// <summary>
        /// O001: FilterOptimization
        /// </summary>
        public void FilterOptimization()
        {
            DMesh3 mesh = IO.GeneralIO.LoadtMesh(ModeloAux);
            //mesh.CheckValidity();
            Reducer r = new Reducer(mesh);
            DMeshAABBTree3 tree = new DMeshAABBTree3(new DMesh3(mesh));
            tree.Build();
            r.ReduceToTriangleCount(triangleCount); ///quanto de reducao deve ter 
			r.ReduceToEdgeLength(edgeLength); //tamanho dos triangulos 
            double mine, maxe, avge;
            MeshQueries.EdgeLengthStats(mesh, out mine, out maxe, out avge);

            IO.GeneralIO.SaveMesh(mesh, GLSettings.locateTMP + GLSettings.ModeloAuxOut_);
        }

        /// <summary>
        /// 
        /// </summary>
        public void scala()
        { 
            //DMesh3 mesh = LoadtMesh("bunny_solid.obj");
            //MeshUtil.ScaleMesh(mesh, Frame3f.Identity, new Vector3f(2, 2, 2));
            //OpenGLControl.RefreshShowModels();
        }

        /*
        private static DMesh3 LoadtMesh(string sPath)
        {
            StandardMeshReader reader = new StandardMeshReader();
            reader.MeshBuilder = new DMesh3Builder();
            reader.Read(sPath, new ReadOptions());
            return (reader.MeshBuilder as DMesh3Builder).Meshes[0];
        }

        private void SaveMesh(IMesh mesh, string sfilename, bool write_groups = true, bool write_vtxcolors = false, bool write_vtxuv = false)
        {
            OBJWriter writer = new OBJWriter();
            var s = new System.IO.StreamWriter(ModeloAuxOut + sfilename, false);
            List<WriteMesh> meshes = new List<WriteMesh>() { new WriteMesh(mesh) };
            writer.Write(s, meshes, new WriteOptions() { bWriteGroups = write_groups, bPerVertexColors = write_vtxcolors, bPerVertexUVs = write_vtxuv });
            s.Close();
        }     
        */
        private void teste()
        {
           // Model3D.CultureInfo;
        }
    }
}
