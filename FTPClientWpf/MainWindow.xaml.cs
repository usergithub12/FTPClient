using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FTPClientWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var list = ListFiles();

            //Console.WriteLine(item);







            Lb_Directory.ItemsSource = list;

        

        }



        public static List<string> ListFiles()
        {

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://10.7.180.101:21/");
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            request.Credentials = new NetworkCredential("test_user", "1234567890");
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            List<string> files = new List<string>();
            while (!reader.EndOfStream)
            {
                string n = reader.ReadLine();
                files.Add(n);
            }
            reader.Close();
            response.Close();



            return files;


        }

        private void Btn_Get_Click(object sender, RoutedEventArgs e)
        {

            // create FtpWebRequest
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://10.7.180.111:21/regex.pdf");
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential("test_user", "1234567890");
            //request.EnableSsl = true; // если используется ssl

            // get answer as FtpWebResponse
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            // get stream and save as file 
            Stream responseStream = response.GetResponseStream();
            FileStream fs = new FileStream("ph.pdf", FileMode.Create);
            byte[] buffer = new byte[64];
            int size = 0;

            while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fs.Write(buffer, 0, size);
            }
            fs.Close();
            response.Close();

            MessageBox.Show("Saving Complete!");
        }

        private void Btn_Upload_Click(object sender, RoutedEventArgs e)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://10.7.180.111:21/regex.pdf");
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential("test_user", "1234567890");
            //request.EnableSsl = true; // если используется ssl

            // get answer as FtpWebResponse
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            // get stream and save as file 
            Stream responseStream = response.GetResponseStream();
            FileStream fs = new FileStream("ph.pdf", FileMode.Create);
            byte[] buffer = new byte[64];
            int size = 0;

            while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fs.Write(buffer, 0, size);
            }
            fs.Close();
            response.Close();

            MessageBox.Show("Upload Complete!");
        }

        private void Lb_Directory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Lb_Directory.SelectedItem.ToString() != null)
            {
                try
                {
                    string s = Lb_Directory.SelectedItem.ToString();
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://10.7.180.101:21/" + $"{s}");

                    request.Method = WebRequestMethods.Ftp.ListDirectory;
                    request.Credentials = new NetworkCredential("test_user", "1234567890");
                    FtpWebResponse response1 = (FtpWebResponse)request.GetResponse();
                    Stream responseStream1 = response1.GetResponseStream();
                    StreamReader reader1 = new StreamReader(responseStream1);

                    List<string> files1 = new List<string>();
                    //reader1 = new StreamReader(responseStream1);


                    while (!reader1.EndOfStream)
                    {
                        string n = reader1.ReadLine();
                        files1.Add(n);
                    }



                    reader1.Close();
                    response1.Close();

                    Lb_Directory.ItemsSource = files1;
                }
                catch (Exception)
                {

                }
            }
           // return files1;
        }
    }
}
