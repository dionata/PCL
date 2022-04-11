using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCL_Models.Class;

namespace PCL_Models
{

    public class Comunication
    {
        public List<PacientModel> pacients = new List<PacientModel>();
        public List<ProjectsModel> projects = new List<ProjectsModel>();
        public List<CADModel> cads = new List<CADModel>();
        public List<CAMModel> cams = new List<CAMModel>();
        public List<MeshModel> meshes = new List<MeshModel>();

        /// <summary>
        /// Aplica método GET  (API Restfull)
        /// </summary>
        public void RestGET()
        {
            pacients.Clear();
            projects.Clear();
            cads.Clear();
            cams.Clear();
            meshes.Clear();

            /*
            * conexão com cliente 
            */
            var client = new RestClient("http://10.167.1.56:8000/");

            var requestPacients = new RestRequest("api/pacients", Method.GET);        
            IRestResponse responsePacients = client.Execute(requestPacients);
            var contentPacients = responsePacients.Content; // raw content as string
            pacients = JsonConvert.DeserializeObject<List<PacientModel>>(contentPacients);

            var requestProjects = new RestRequest("api/projects", Method.GET);           
            IRestResponse responseProjects = client.Execute(requestProjects);
            var contentProjects = responseProjects.Content; // raw content as string
            projects = JsonConvert.DeserializeObject<List<ProjectsModel>>(contentProjects);

            var requestCads = new RestRequest("api/cads", Method.GET);         
            IRestResponse responseCads = client.Execute(requestCads);
            var contentCads = responseCads.Content; // raw content as string
            cads = JsonConvert.DeserializeObject<List<CADModel>>(contentCads);

            var requestCams = new RestRequest("api/cams", Method.GET);        
            IRestResponse responseCams = client.Execute(requestCams);
            var contentCams = responseCams.Content; // raw content as string
            cams = JsonConvert.DeserializeObject<List<CAMModel>>(contentCams);

            var requestMeshes = new RestRequest("api/meshes", Method.GET);       
            IRestResponse responseMeshes = client.Execute(requestMeshes);
            var contentMeshes = responseMeshes.Content; // raw content as string
            meshes = JsonConvert.DeserializeObject<List<MeshModel>>(contentMeshes);
        }

        /// <summary>
        /// Aplica método post (API Restfull)
        /// </summary>
        /// <param name="insertPacient"></param>
        /// <param name="insertProject"></param>
        /// <param name="insertCam"></param>
        /// <param name="insertMesh"></param>
        /// <param name="insertCad"></param>
        public void RestPOST(PacientModel insertPacient, ProjectsModel insertProject, CAMModel insertCam, MeshModel insertMesh, CADModel insertCad)
        {
            /*
          * conexão com cliente 
          */
            var client = new RestClient("http://10.167.1.56:8000/");

            if(insertPacient != null)
            { 
                var pacient = new RestRequest("api/pacients", Method.POST);
                pacient.AddJsonBody(insertPacient);
                client.Execute(pacient);
            }

            if(insertProject != null)
            {
                var project = new RestRequest("api/projects", Method.POST);
                project.AddJsonBody(insertProject);
                client.Execute(project);
            }
           
            if(insertCam != null )
            {
                var cam = new RestRequest("api/cams", Method.POST);
                cam.AddJsonBody(insertCam);
                client.Execute(cam);
            }
           
            if(insertMesh != null)
            {
                var mesh = new RestRequest("api/meshes", Method.POST);
                mesh.AddJsonBody(insertMesh);
                client.Execute(mesh);
            }
           
            if(insertCad != null)
            {
                var cad = new RestRequest("api/cads", Method.POST);
                cad.AddJsonBody(insertCad);
                client.Execute(cad);
            }                                 
        }
    }
}
