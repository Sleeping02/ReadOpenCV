using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.IO;

class Program
{
    static void Main()
    {
        string videoPath = "C:\\Users\\SleepingF\\Desktop\\test1.mp4";
        string outputDirectory = "C:\\Users\\SleepingF\\Desktop\\faceDifferences";

        // Cargar el clasificador de Haar para la detección facial
        string faceCascadePath = "C:\\Users\\SleepingF\\Desktop\\FaceAnalysis\\FaceAnalysis\\haarcascade_frontalface_default.xml";
        CascadeClassifier faceCascade = new CascadeClassifier(faceCascadePath);

        VideoCapture videoCapture = new VideoCapture(videoPath);

        double nuevoValorDeBrillo = 0.9;
        videoCapture.Set(CapProp.Brightness, nuevoValorDeBrillo);


        if (!videoCapture.IsOpened)
        {
            Console.WriteLine("Error al abrir el video.");
            return;
        }

        System.IO.Directory.CreateDirectory(outputDirectory);

        Mat previousFrame = new Mat();
        if (!videoCapture.Read(previousFrame) || previousFrame.IsEmpty)
        {
            Console.WriteLine("No se pudo leer el primer frame del video.");
            return;
        }

        int frameCount = 1;

        while (true)
        {
            Mat currentFrame = new Mat();
            if (!videoCapture.Read(currentFrame) || currentFrame.IsEmpty)
            {
                Console.WriteLine("Fin del video alcanzado.");
                break;
            }

            // Detectar rostros en el frame actual
            var faces = DetectFaces(currentFrame, faceCascade);

            // Comparación de diferencias faciales con el frame anterior
            if (frameCount > 1 && faces.Length > 0)
            {
                Mat diffFrame = new Mat();
                CvInvoke.AbsDiff(previousFrame, currentFrame, diffFrame);

                // Guardar la imagen con las diferencias faciales
                string outputPath = Path.Combine(outputDirectory, $"diff_face_{frameCount}.png");
                CvInvoke.Imwrite(outputPath, diffFrame);

                Console.WriteLine($"Diferencias faciales en el frame {frameCount} guardadas en {outputPath}");
            }

            frameCount++;

            // Actualizar frame anterior
            previousFrame.Dispose();
            previousFrame = currentFrame.Clone();
        }

        Console.WriteLine("Proceso completado. Presiona cualquier tecla para salir.");
        Console.ReadKey();
    }

    static Rectangle[] DetectFaces(Mat image, CascadeClassifier faceCascade)
    {
        // Detección de rostros
        Rectangle[] faces = faceCascade.DetectMultiScale(
            image,
            scaleFactor: 1.1,
            minNeighbors: 5,
            minSize: new Size(30, 30)); 

        return faces;
    }
}
