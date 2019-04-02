using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient
{
    class Program
    {
        static void Main(string[] args)
        {


            //    //Console.ReadLine();
            //    // create FtpWebRequest
            //    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://10.7.180.101:21/temp.txt");
            //    //request.Method = WebRequestMethods.Ftp.DownloadFile;
            //Console.WriteLine(request.Method = WebRequestMethods.Ftp.ListDirectory);
            //    request.Credentials = new NetworkCredential("test_user", "1234567890");
            //    //request.EnableSsl = true; // если используется ssl

            //    // get answer as FtpWebResponse
            //    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            //    // get stream and save as file 
            //    Stream responseStream = response.GetResponseStream();
            //    FileStream fs = new FileStream("new_temp.txt", FileMode.Create);
            //    byte[] buffer = new byte[64];
            //    int size = 0;

            //    while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        fs.Write(buffer, 0, size);
            //    }
            //    fs.Close();
            //    response.Close();

            //Console.WriteLine(ListFiles());
            var list = ListFiles();
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
                Console.WriteLine("Saving complete");
                Console.Read();
            
        }
        public static List<string> ListFiles()
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://10.7.180.120:21/");
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                request.Credentials = new NetworkCredential("test_user", "1234567890");
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();

                reader.Close();
                response.Close();

                return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

