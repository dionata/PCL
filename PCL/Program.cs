

using System;
using System.Threading;
using System.Windows.Forms;
using PCL_Models;
using PCL_Models.Class;
using PCLLib;

namespace PCLLib
{
  internal static class Program
  {
    [STAThread]
    public static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

            /*
            //Instantiate a new instance of SplashScreen
            SplashScreenView splashInit_ = new SplashScreenView(100);
            //Display the splash screen with a max value for the progress bar
            splashInit_.ShowSplashScreen(4);

            //Wait for the form to be loaded.
            while (!splashInit_.Ready())
            {
                //Loop to wait for the splash screen to be ready before trying to
                // update it. We will get a null reference exception if the splash screen isn't ready yet.
                if (finalizarSplashScreen == true) break;
            }

            //Update the progress bar
            splashInit_.UpdateProgress("Loading Step 1");
            //Perform action
            Thread.Sleep(500); //Lets pretend this is doing something constructive
                                //Update the progress bar
             splashInit_.UpdateProgress("Loading Step 2");
            //Perform action
            Thread.Sleep(500); //Lets pretend this is doing something constructive
                                //Update the progress bar
            splashInit_.UpdateProgress("Loading Step 3");
            //Perform action
            Thread.Sleep(500); //Lets pretend this is doing something constructive
                                //Update the progress bar
            splashInit_.UpdateProgress("Loading Step 4");
            //Perform action
            Thread.Sleep(500); //Lets pretend this is doing something constructive
                                //Close the Splash Screen
            splashInit_.CloseForm();  
            finalizarSplashScreen = true;
            */
            /*teste
            EstadoModel states = new EstadoModel();
            states.states = "São Paulo";
            states.id = 10;


            DataBaseModule db = new DataBaseModule();

            db.updateEstadoModel(states);
            */
            try
        {
            Application.Run((Form)new MainView());
        }
        catch
        {
          //  Application.Run((Form)new MainView());
        }       
    }
  }
}
