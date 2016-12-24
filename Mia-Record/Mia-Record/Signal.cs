using NAudio.Wave;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Mia_Record
{
    class Signal
    {
        public string outputFilename1 = "Rec1.wav";

        public void Process(int N, Commands obj)
        {
            double corel = 0;
            double[] corel_array = new double[N];
            double[] data1 = prepare(outputFilename1);
            data1 = Normalize(data1);

            for (int i = 0; i < data1.Length; i++)
            {
                if (data1[i] > 0.2)
                {
                    offset(ref data1, i);
                    break;
                }
            }

            for (int l = 0; l < N; l++)
            {
                corel = 0;
                double[] data2 = prepare(obj.CommandList[l].Audio_URL);
                data2 = Normalize(data2);

                for (int i = 0; i < data2.Length; i++)
                {
                    if (data2[i] > 0.2)
                    {
                        offset(ref data2, i);
                        break;
                    }
                }

                if (data1.Length < data2.Length)
                {
                    Array.Resize(ref data2, data1.Length);
                    corel = corr(data1, data2);
                }
                else if (data1.Length > data2.Length)
                {
                    Array.Resize(ref data1, data2.Length);
                    corel = corr(data1, data2);
                }
                else
                {
                    corel = corr(data1, data2);
                }

                corel_array[l] = corel;
            }

            double swapon = 0;
            int maxInd = 0;
            for (int k = 0; k < corel_array.Length; k++)
            {
                if (corel_array[k] > swapon)
                {
                    swapon = corel_array[k];
                    maxInd = k;
                }
            }

            if (swapon > 0.6)
            {
                System.Diagnostics.Process.Start(obj.CommandList[maxInd].Program_URL);
            }
            else MessageBox.Show("Please, record your commad again!");
        }
        
        // Обработка речи - вычисляем, есть ли сама речь на звуковом отрезке
        private bool ProcessData(WaveInEventArgs e)
        {
            double threshold = 0.05;
            bool result = false;
            bool Tr = false;
            double Sum2 = 0;
            int Count = e.BytesRecorded / 2;

            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                double Tmp = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]);
                Tmp /= 32768.0;
                Sum2 += Tmp * Tmp;

                if (Tmp > threshold)
                {
                    Tr = true;
                }
            }

            Sum2 /= Count;
            if (Tr || Sum2 > threshold)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }


        public double corr(double[] data1, double[] data2)
        {
            try
            {
                double sumX = 0, sumY = 0, sumX2 = 0, sumY2 = 0, sumXY = 0;
                int N = data1.Length;
                for (int i = 0; i < N; i++)
                {
                    sumXY = sumXY + (data1[i] * data2[i]);
                }

                for (int i = 0; i < N; i++)
                {
                    sumX = sumX + data1[i];
                }

                for (int i = 0; i < N; i++)
                {
                    sumY = sumY + data2[i];
                }

                for (int i = 0; i < N; i++)
                {
                    sumX2 = sumX2 + Math.Pow(data1[i], 2);
                }

                for (int i = 0; i < N; i++)
                {
                    sumY2 = sumY2 + Math.Pow(data2[i], 2);
                }

                return (sumXY - (sumX * sumY) / N) / (Math.Sqrt(sumX2 - (Math.Pow(sumX, 2) / N)) * Math.Sqrt(sumY2 - (Math.Pow(sumY, 2) / N)));
            }
            catch
            {
                return 0;
            }
        }

        public double[] Normalize(double[] array)
        {
            for (int index = 0; index < array.Length; index++)
            {
                array[index] = (array[index] - array.Min()) / (array.Max() - array.Min());
            }

            return array;
        }

        public Double[] prepare(String wavePath)
        {
            double[] data;
            double sum = 0;
            int precision = 100;
            byte[] wave;
            FileStream WaveFile = File.OpenRead(wavePath);
            wave = new byte[WaveFile.Length];
            data = new double[(wave.Length - 44) / 4];
            WaveFile.Read(wave, 0, Convert.ToInt32(WaveFile.Length));
            double[] envelope = new double[data.Length / 100];

            for (int i = 44, j = 0; i < data.Length; i++)
            {
                data[i] = Math.Abs((BitConverter.ToInt32(wave, (1 + i) * 4)) / 65536.0);

                if (i != precision)
                {
                    sum = sum + data[i];
                }
                else
                {
                    envelope[j] = sum / 100;
                    j++;
                    sum = 0;
                    precision = precision + 100;
                }
            }

            WaveFile.Close();
            WaveFile.Dispose();

            return envelope;
        }

        private void offset(ref double[] data, int ind)
        {
            int offset = ind;
            for (int i = 0; i < data.Length - offset; i++, ind++)
            {
                data[i] = data[ind];
            }
            Array.Resize(ref data, (data.Length - offset));
        }
    }
}
