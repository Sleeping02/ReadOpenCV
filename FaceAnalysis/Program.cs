using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;


class Program
{
    static void Main()
    {

        string videoPath = "C:\\Users\\SleepingF\\Desktop\\test10.mp4";
        string outputDirectory = "C:\\Users\\SleepingF\\Desktop\\saveFrames";

      



        VideoCapture videoCapture = new VideoCapture(videoPath);

        if (!videoCapture.IsOpened)
        {
            Console.WriteLine("Error");
            return;
        }

        System.IO.Directory.CreateDirectory(outputDirectory);

       



        int frameCount = (int)videoCapture.Get(CapProp.FrameCount);
        for(int i = 0; i< frameCount; i++)
        {
            Mat frame= new Mat();
            videoCapture.Read(frame);

            string outputPath = System.IO.Path.Combine(outputDirectory, $"frame_{i}.jpg");
            CvInvoke.Imwrite(outputPath, frame);

            
        }
        Console.WriteLine($"Cantidad total de frames en el video: {frameCount}");
        Console.WriteLine("Frames guardados con éxito, Angelo.");
    }
}


