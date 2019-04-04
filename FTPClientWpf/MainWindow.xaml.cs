using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            abspath = "";
        }

        public List<string> FilesToUpload = new List<string>();
        public string abspath { get; set; }

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
            FileStruct fileStruct = new FileStruct();
            Respoonse("ftp://10.7.180.101:21/",fileStruct);
            Save("D://Save/", "ftp://10.7.180.101:21/",fileStruct);
           // Save();
        }

        private void Btn_Upload_Click(object sender, RoutedEventArgs e)
        {

            UploadFtpFile("qwe","temp.txt");
        }
        

        private void Lb_Directory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Lb_Directory.SelectedIndex != -1)
            {

                string s = Lb_Directory.SelectedItem.ToString();
                if (s.Contains(abspath) || s.Contains(abspath.Remove(0, 1)))
                {


                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://10.7.180.101:21/" + $"{s}");

                    request.Method = WebRequestMethods.Ftp.ListDirectory;
                    request.Credentials = new NetworkCredential("test_user", "1234567890");

                    using (FtpWebResponse response1 = (FtpWebResponse)request.GetResponse())
                    {
                        using (Stream responseStream1 = response1.GetResponseStream())
                        {
                            using (StreamReader reader1 = new StreamReader(responseStream1))
                            {
                                //   Debug.WriteLine(reader1.ReadToEnd());


                                var temp = reader1.ReadToEnd().Split('\n', '\r').Where(a => a.Length > 0).ToList();
                                Lb_Directory.ItemsSource = temp;
                                abspath += "/" + temp.First().Substring(0, temp.First().LastIndexOf('/'));
                                FilesToUpload.AddRange(temp);
                            }
                        }
                    }
                }

                else
                {
                    s = abspath.Substring(0, abspath.LastIndexOf('/')) + "/" + s;
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://10.7.180.101:21/" + $"{s}");

                    request.Method = WebRequestMethods.Ftp.ListDirectory;
                    request.Credentials = new NetworkCredential("test_user", "1234567890");

                    using (FtpWebResponse response1 = (FtpWebResponse)request.GetResponse())
                    {
                        using (Stream responseStream1 = response1.GetResponseStream())
                        {
                            using (StreamReader reader1 = new StreamReader(responseStream1))
                            {
                                //   Debug.WriteLine(reader1.ReadToEnd());


                                var temp = reader1.ReadToEnd().Split('\n', '\r').Where(a => a.Length > 0).ToList();
                                Lb_Directory.ItemsSource = temp;
                                abspath += "/" + temp.First().Substring(0, temp.First().LastIndexOf('/'));
                                //add files
                                FilesToUpload.AddRange(temp);
                            }
                        }
                    }
                }
            }
            // return files1;
        }

        private void Btn_connect_Click(object sender, RoutedEventArgs e)
        {
            var list = ListFiles();
            Lb_Directory.ItemsSource = list;
        }

        private void Btn_back_Click(object sender, RoutedEventArgs e)
        {
            //string s = Lb_Directory.SelectedItem.ToString();
            abspath = abspath.Substring(0, abspath.LastIndexOf('/'));
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://10.7.180.101:21/" + $"{abspath}");

            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential("test_user", "1234567890");

            using (FtpWebResponse response1 = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream1 = response1.GetResponseStream())
                {
                    using (StreamReader reader1 = new StreamReader(responseStream1))
                    {
                        //   Debug.WriteLine(reader1.ReadToEnd());


                        var temp = reader1.ReadToEnd().Split('\n', '\r').Where(a => a.Length > 0).ToList();
                        Lb_Directory.ItemsSource = temp;
                        //        string abspath = temp.First().Substring(0, temp.First().LastIndexOf('/'));

                    }
                }
            }
        }



        static public void Respoonse(string Addres, FileStruct FileStr)
        {

            string temp;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Addres);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential("test_user", "1234567890");
            Console.WriteLine(request.GetResponse());

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(responseStream);

            string line;

            while ((line = reader.ReadLine()) != null)

            {

                temp = line.Substring(49);

                if (line[0] == 'd')

                {

                    FileStr.Direct.Add(new Direct

                    {

                        Directory = temp,

                        NodeNext = new FileStruct()

                    });

                }

                else

                {

                    FileStr.Files.Add(temp);

                }

            }

            reader.Close();

            response.Close();

            string AddresTemp;

            for (int i = 0; i < FileStr.Direct.Count; i++)

            {

                AddresTemp = "";

                AddresTemp = Addres + "/" + FileStr.Direct[i].Directory;

                Respoonse(AddresTemp, FileStr.Direct[i].NodeNext);
            }
        }





        static public void Save(string Addres, string AddresSave, FileStruct fileStr)

        {

            for (int i = 0; i < fileStr.Direct.Count; i++)

            {

                Directory.CreateDirectory(Addres + "\\" + fileStr.Direct[i].Directory);

                Save(Addres + "\\" + fileStr.Direct[i].Directory, AddresSave + "/" + fileStr.Direct[i].Directory, fileStr.Direct[i].NodeNext);

            }

            for (int i = 0; i < fileStr.Files.Count; i++)

            {

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(AddresSave + "/" + fileStr.Files[i]);

                request.Method = WebRequestMethods.Ftp.DownloadFile;

                request.Credentials = new NetworkCredential("test_user", "1234567890");
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                FileStream fs = new FileStream(Addres + "/" + fileStr.Files[i], FileMode.Create);

                byte[] buffer = new byte[64];

                int size = 0;
                while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)

                {

                    fs.Write(buffer, 0, size);

                }

                fs.Close();

                response.Close();

            }

        }


        public void UploadFtpFile(string folderName, string fileName)
        {

            FtpWebRequest request;

           
            string absoluteFileName = System.IO.Path.GetFileName(fileName);

            request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", "10.7.180.101:21", folderName, absoluteFileName))) as FtpWebRequest;
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UseBinary = true;
            request.UsePassive = true;
            request.KeepAlive = true;
            request.Credentials = new NetworkCredential("test_user", "1234567890");
            request.ConnectionGroupName = "group";

            using (FileStream fs = File.OpenRead(fileName))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Flush();
                requestStream.Close();
            }
        }

    }
}

