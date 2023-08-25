using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using Andhana;


namespace inovaGL
{
    class AdnApplicationContext : ApplicationContext
    {
            private FScLogin ofmLogin;
            private AdnScPengguna pengguna;

            private AdnApplicationContext()
            {
                //formCount = 0;

                Application.ApplicationExit += new EventHandler(this.OnApplicationExit);

                try
                {
                    AppFungsi.SetKoneksiApp();
                    // Create a file that the application will store user specific data in.
                    //userData = new FileStream(Application.UserAppDataPath + "\\appdata.txt", FileMode.OpenOrCreate);

                }
                catch (IOException e)
                {
                    MessageBox.Show("An error occurred while attempting to show the application." +
                                    "The error is:" + e.ToString());
                    ExitThread();
                }


                ofmLogin = new FScLogin(AppVar.AppName, AppVar.AppConn);
                ofmLogin.Closed += new EventHandler(OnFormClosed);
                ofmLogin.Closing += new CancelEventHandler(OnFormClosing);
                
                ofmLogin.Show();

            }

            private void OnApplicationExit(object sender, EventArgs e)
            {
                try
                {
                }
                catch { }
            }
            private void OnFormClosing(object sender, CancelEventArgs e)
            {
                if (sender is FScLogin)
                    this.pengguna = ((FScLogin) sender).GetPengguna();
                if (sender is F_APP)
                    this.pengguna = null;

            }
            private void OnFormClosed(object sender, EventArgs e)
            {

                if (this.pengguna== null)
                {
                    this.ExitThread();
                }
                else
                {
                    AppFungsi.SetPengguna(this.pengguna);
                    F_APP oApp = new F_APP();
                    oApp.Closed += new EventHandler(OnFormClosed);
                    oApp.Closing += new CancelEventHandler(OnFormClosing);
                    oApp.Show();
                }


            }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AdnApplicationContext contextApp = new AdnApplicationContext();
            Application.Run(contextApp);
        }
    }
}
