using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.ReadWrite;

namespace TBAGW.Forms.FTP_Utility
{
    public partial class FTPWindow : Form
    {
        public FTPWindow()
        {
            InitializeComponent();
        }

        String uri = "ftp://127.0.0.1/";

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var list = new List<string>();
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential("Admin", "Bubbi100");

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                List<string> directories = new List<string>();

                string line = reader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    directories.Add(line);
                    line = reader.ReadLine();
                }

                listBox1.Items.Clear();
                listBox1.SelectedIndex = -1;
                listBox1.DataSource = directories;

                Console.WriteLine("Directory List Complete, status {0}", response.StatusDescription);

                reader.Close();
                response.Close();

                label3.Text = "As 'admin' --> " + uri;
                // AttemptDownload();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong");
                Console.WriteLine(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                try
                {
                    String uriDir = uri + listBox1.SelectedItem.ToString() + @"/";

                    var list = new List<string>();
                    // Get the object used to communicate with the server.
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uriDir);
                    request.Method = WebRequestMethods.Ftp.ListDirectory;

                    // This example assumes the FTP site uses anonymous logon.
                    request.Credentials = new NetworkCredential("Admin", "Bubbi100");

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);

                    List<string> files = new List<string>();

                    string line = reader.ReadLine();
                    while (!string.IsNullOrEmpty(line))
                    {
                        files.Add(line);
                        line = reader.ReadLine();
                    }

                    Console.WriteLine("Directory List Complete, status: {0}", response.StatusDescription);

                    reader.Close();
                    response.Close();

                    List<String> finalDLlocs = new List<string>();
                    foreach (var item in files)
                    {
                        finalDLlocs.Add(AttemptDownload(uriDir,item));
                    }

                    var map = EditorFileWriter.MapReader(finalDLlocs.Find(loc=>loc.EndsWith(".cgmapc",StringComparison.OrdinalIgnoreCase)));

                    richTextBox1.Text = "Succesfully loaded the map :'"+ finalDLlocs.Find(loc => loc.EndsWith(".cgmapc", StringComparison.OrdinalIgnoreCase))+"'";
                }
                catch (Exception)
                {
                }
            }
        }

        private String AttemptDownload(String uriDir, String fileName)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uriDir+fileName);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("Admin", "Bubbi100");

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();

            String tempLoc = Path.Combine(Path.GetTempPath(), fileName);

            using (var fileStream = File.Create(tempLoc))
            {
                //responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);

            }

           

            //StreamReader reader = new StreamReader(responseStream);
            //Console.WriteLine(reader.ReadToEnd());

            Console.WriteLine("Download Complete, status {0}", response.StatusDescription);

            response.Close();

            return tempLoc;
        }
    }
}
