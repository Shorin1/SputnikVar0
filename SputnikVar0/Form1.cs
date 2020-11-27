using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SputnikVar0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            double v = 0;
            double t = 0;
            double Massa = 350;
            double Stsputnika = 0.8;
            double X1 = 0, X1_diskr = 0, X2 = 0, X2_diskr = 0, X3 = 0;
            double w = 0;
            double t_diskr = Convert.ToDouble(textBox1.Text);
            double dt = Convert.ToDouble(textBox2.Text);
            double a0 = Convert.ToDouble(textBox4.Text);
            double a1 = Convert.ToDouble(textBox3.Text);
            double a3 = Convert.ToDouble(textBox5.Text);
            double a4 = Convert.ToDouble(textBox6.Text);
            double Wmax = Convert.ToDouble(textBox7.Text);
            double a = Convert.ToDouble(textBox8.Text);
            double wshtrihmax = Convert.ToDouble(textBox9.Text);
            double wshtriht = Convert.ToDouble(textBox10.Text);
            double Mvuscor = Convert.ToDouble(textBox11.Text);
            double Is = Convert.ToDouble(textBox12.Text);
            double Purd = Convert.ToDouble(textBox13.Text);
            double Lm = Convert.ToDouble(textBox14.Text);
            double Im = Convert.ToDouble(textBox15.Text);
            double Mv = Convert.ToDouble(textBox16.Text);
            double Murd = Purd * Lm;
            double X1_prev = 0, X2_prev = 0, X3_prev = 0;
            double F = 0;
            int f = 0;
            double Mupr;
            double Mt = wshtriht*Im;

            StreamWriter UglVMax = new StreamWriter("UglMax.txt");
            //SaveFileDialog UglVMax = new SaveFileDialog("UglVMax.txt");
            StreamWriter UgolSputnika = new StreamWriter("UgolSputnika.txt");
            StreamWriter UgolSputnikaDiskr = new StreamWriter("UgolSputnikaDiskr.txt");
            StreamWriter UglVSputnika = new StreamWriter("UglVSputnika.txt");
            StreamWriter UglVSputnikaDiskr = new StreamWriter("UglVSputnikaDiskr.txt");
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series[0].Name = "Частота оборотов маховика";
            if (chart2.Series.Count == 1)
            {
                chart2.Series[0].Name = "Угол отклонения спутника";
                chart2.Series.Add("Дискретный угол отклонения спутника");
                chart2.Series[1].Color = Color.Red;
                chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart2.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            }
            if (chart3.Series.Count == 1)
            {
                chart3.Series[0].Name = "Угловая скорость спутника";
                chart3.Series[0].Color = Color.Red;
                chart3.Series.Add("Дискретная угловая скоротсь спутника");
                chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart3.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            }

            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart3.Series[1].Points.Clear();

            while (Math.Abs(w) < Wmax)
            {
                if (Math.Round(t, 3) % t_diskr == 0)
                {
                    X1_diskr = X1_prev + dt * X2_prev;
                    X2_diskr = X2_prev + dt * ((Mv / Is) - (Im / Is) * X3_prev);
                }
                X1 = X1_prev + dt * X2_prev;
                X2 = X2_prev + dt * ((Mv / Is) - (Im / Is) * X3_prev);
                X3 = a0 * X1_diskr + a1 * X2_diskr;
             
                w = w + dt * X3;
                //t += dt;
                chart1.Series[0].Points.AddXY(t, w);
                chart2.Series[0].Points.AddXY(t, X1);
                chart2.Series[1].Points.AddXY(t, X1_diskr);
                chart3.Series[0].Points.AddXY(t, X2);
                chart3.Series[1].Points.AddXY(t, X2_diskr);
                UglVMax.Write(t+" ");
                UglVMax.WriteLine(w);
                UgolSputnika.Write(t + " ");
                UgolSputnika.WriteLine(X1);
                UgolSputnikaDiskr.Write(t + " ");
                UgolSputnikaDiskr.WriteLine(X1_diskr);
                UglVSputnika.Write(t + " ");
                UglVSputnika.WriteLine(X2);
                UglVSputnikaDiskr.Write(t + " ");
                UglVSputnikaDiskr.WriteLine(X2_diskr);
                //UglVMax.Close();
                //UgolSputnika.Close();
                //UgolSputnikaDiskr.Close();
                //UglVSputnika.Close();
                //UglVSputnikaDiskr.Close();
                t += dt;
                X1_prev = X1;
                X2_prev = X2;
                X3_prev = X3;

            }

            while (w > 0)
            {
                F = a3 * X1_diskr + a4 * X2_diskr;
                if (F > a) f = 1;
                else if ((F >= -a) && (F <= a)) { f = 0; }
                else if (F < -a) { f = -1; }
                Mupr = Murd * f;

                if (Math.Round(t, 3) % t_diskr == 0)
                {
                    X1_diskr = X1_prev + dt * X2_prev;
                    X2_diskr = X2_prev + dt * ((Mt / Is) + (Mv / Is) - Mupr/Is);
                }
                X1 = X1_prev + dt * X2_prev;
                X2 = X2_prev + dt * ((Mt / Is) + (Mv / Is) - Mupr/Is);

                w = w - dt * wshtriht;
                chart1.Series[0].Points.AddXY(t, w);
                chart2.Series[0].Points.AddXY(t, X1);
                chart2.Series[1].Points.AddXY(t, X1_diskr);
                chart3.Series[0].Points.AddXY(t, X2);
                chart3.Series[1].Points.AddXY(t, X2_diskr);
                /*UglVMax.Write(t + " ");
                UglVMax.WriteLine(w);
                UgolSputnika.Write(t + " ");
                UgolSputnika.WriteLine(X1);
                UgolSputnikaDiskr.Write(t + " ");
                UgolSputnikaDiskr.WriteLine(X1_diskr);
                UglVSputnika.Write(t + " ");
                UglVSputnika.WriteLine(X2);
                UglVSputnikaDiskr.Write(t + " ");
                UglVSputnikaDiskr.WriteLine(X2_diskr);
                UglVMax.Close();
                UgolSputnika.Close();
                UgolSputnikaDiskr.Close();
                UglVSputnika.Close();
                UglVSputnikaDiskr.Close();*/
                t += dt;
                X1_prev = X1;
                X2_prev = X2;
                X3_prev = X3;
            }
            while (w < 0)
            {
                F = a3 * X1_diskr + a4 * X2_diskr;
                if (F > a) f = 1;
                else if ((F >= -a) && (F <= a)) { f = 0; }
                else if (F < -a) { f = -1; }
                Mupr = Murd * f;

                if (Math.Round(t, 3) % t_diskr == 0)
                {
                    X1_diskr = X1_prev + dt * X2_prev;
                    X2_diskr = X2_prev + dt * ((Mt / Is) + (Mv / Is) - Mupr/Is);
                }
                X1 = X1_prev + dt * X2_prev;
                X2 = X2_prev + dt * ((Mt / Is) + (Mv / Is) - Mupr/Is);

                w = w + dt * wshtriht;
                chart1.Series[0].Points.AddXY(t, w);
                chart2.Series[0].Points.AddXY(t, X1);
                chart2.Series[1].Points.AddXY(t, X1_diskr);
                chart3.Series[0].Points.AddXY(t, X2);
                chart3.Series[1].Points.AddXY(t, X2_diskr);
                /*UglVMax.Write(t + " ");
                UglVMax.WriteLine(w);
                UgolSputnika.Write(t + " ");
                UgolSputnika.WriteLine(X1);
                UgolSputnikaDiskr.Write(t + " ");
                UgolSputnikaDiskr.WriteLine(X1_diskr);
                UglVSputnika.Write(t + " ");
                UglVSputnika.WriteLine(X2);
                UglVSputnikaDiskr.Write(t + " ");
                UglVSputnikaDiskr.WriteLine(X2_diskr);
                UglVMax.Close();
                UgolSputnika.Close();
                UgolSputnikaDiskr.Close();
                UglVSputnika.Close();
                UglVSputnikaDiskr.Close();*/
                t += dt;
                X1_prev = X1;
                X2_prev = X2;
                X3_prev = X3;
                
            }
            UglVMax.Close();
            UgolSputnika.Close();
            UgolSputnikaDiskr.Close();
            UglVSputnika.Close();
            UglVSputnikaDiskr.Close();
            GlobalVars.maxT = t;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double v = 0;
            double t = 0;
            double Massa = 350;
            double Stsputnika = 0.8;
            double X1 = 0, X1_diskr = 0, X2 = 0, X2_diskr = 0, X3 = 0;
            double w = 0;
            double t_diskr = Convert.ToDouble(textBox1.Text);
            double dt = Convert.ToDouble(textBox2.Text);
            double a0 = Convert.ToDouble(textBox4.Text);
            double a1 = Convert.ToDouble(textBox3.Text);
            double a3 = Convert.ToDouble(textBox5.Text);
            double a4 = Convert.ToDouble(textBox6.Text);
            double Wmax = Convert.ToDouble(textBox7.Text);
            double a = Convert.ToDouble(textBox8.Text);
            double wshtrihmax = Convert.ToDouble(textBox9.Text);
            double wshtriht = Convert.ToDouble(textBox10.Text);
            double Mvuscor = Convert.ToDouble(textBox11.Text);
            double Is = Convert.ToDouble(textBox12.Text);
            double Purd = Convert.ToDouble(textBox13.Text);
            double Lm = Convert.ToDouble(textBox14.Text);
            double Im = Convert.ToDouble(textBox15.Text);
            double Mv = Convert.ToDouble(textBox16.Text);
            double Murd = Purd * Lm;
            double X1_prev = 0, X2_prev = 0, X3_prev = 0;
            double F = 0;
            int f = 0;
            double Mupr;
            double Mt = wshtriht * Im;
            Random rand =  new Random();
            double otkazDUS = 0 + rand.NextDouble()*GlobalVars.maxT;
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series[0].Name = "Частота оборотов маховика";
            if (chart2.Series.Count == 1)
            {
                chart2.Series[0].Name = "Угол отклонения спутника";
                chart2.Series.Add("Дискретный угол отклонения спутника");
                chart2.Series[1].Color = Color.Red;
                chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart2.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart2.Series.Add("Точка отказа ДУС");
                chart2.Series[2].Color = Color.Green;
                chart2.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                chart2.Series[2].Points.AddXY(otkazDUS, 0);
                //chart2.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            }
            if (chart3.Series.Count == 1)
            {
                chart3.Series[0].Name = "Угловая скорость спутника";
                chart3.Series[0].Color = Color.Red;
                chart3.Series.Add("Дискретная угловая скоротсь спутника");
                chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart3.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            }

            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();
            //chart2.Series[2].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart3.Series[1].Points.Clear();

            while (Math.Abs(w) < Wmax)
            {
                if (t >= otkazDUS)
                {
                    if (Math.Round(t, 3) % t_diskr == 0)
                    {
                        X1_diskr = X1_prev + dt * X2_prev;
                        X2_diskr = (X1_diskr - X1_prev) / dt + dt * ((Mv / Is) - (Im / Is) * X3_prev);
                    }
                    X1 = X1_prev + dt * X2_prev;
                    X2 = (X1 - X1_prev) / dt + dt * ((Mv / Is) - (Im / Is) * X3_prev);
                    X3 = a0 * X1_diskr + a1 * X2_diskr;
                }
                else
                {
                    if (Math.Round(t, 3) % t_diskr == 0)
                    {
                        X1_diskr = X1_prev + dt * X2_prev;
                        X2_diskr = X2_prev + dt * ((Mv / Is) - (Im / Is) * X3_prev);
                    }
                    X1 = X1_prev + dt * X2_prev;
                    X2 = X2_prev + dt * ((Mv / Is) - (Im / Is) * X3_prev);
                    X3 = a0 * X1_diskr + a1 * X2_diskr;
                }

                w = w + dt * X3;
                t += dt;
                chart1.Series[0].Points.AddXY(t, w);
                chart2.Series[0].Points.AddXY(t, X1);
                chart2.Series[1].Points.AddXY(t, X1_diskr);
                chart3.Series[0].Points.AddXY(t, X2);
                chart3.Series[1].Points.AddXY(t, X2_diskr);
                X1_prev = X1;
                X2_prev = X2;
                X3_prev = X3;

            }

            while (w > 0)
            {
                F = a3 * X1_diskr + a4 * X2_diskr;
                if (F > a) f = 1;
                else if ((F >= -a) && (F <= a)) { f = 0; }
                else if (F < -a) { f = -1; }
                Mupr = Murd * f;
                if (t >= otkazDUS)
                {
                    if (Math.Round(t, 3) % t_diskr == 0)
                    {
                        X1_diskr = X1_prev + dt * X2_prev;
                        X2_diskr = (X1_diskr - X1_prev) / dt + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                    }
                    X1 = X1_prev + dt * X2_prev;
                    X2 = (X1 - X1_prev) / dt + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                }
                else
                {
                    if (Math.Round(t, 3) % t_diskr == 0)
                    {
                        X1_diskr = X1_prev + dt * X2_prev;
                        X2_diskr = X2_prev + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                    }
                    X1 = X1_prev + dt * X2_prev;
                    X2 = X2_prev + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                }
                w = w - dt * wshtriht;
                chart1.Series[0].Points.AddXY(t, w);
                chart2.Series[0].Points.AddXY(t, X1);
                chart2.Series[1].Points.AddXY(t, X1_diskr);
                chart2.Series[1].Points.AddXY(t, X1_diskr);
                chart3.Series[0].Points.AddXY(t, X2);
                chart3.Series[1].Points.AddXY(t, X2_diskr);
                t += dt;
                X1_prev = X1;
                X2_prev = X2;
                X3_prev = X3;
            }
            while (w < 0)
            {
                F = a3 * X1_diskr + a4 * X2_diskr;
                if (F > a) f = 1;
                else if ((F >= -a) && (F <= a)) { f = 0; }
                else if (F < -a) { f = -1; }
                Mupr = Murd * f;

                if (t >= otkazDUS)
                {
                    if (Math.Round(t, 3) % t_diskr == 0)
                    {
                        X1_diskr = X1_prev + dt * X2_prev;
                        X2_diskr = (X1_diskr - X1_prev) / dt + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                    }
                    X1 = X1_prev + dt * X2_prev;
                    X2 = (X1 - X1_prev) / dt + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                }
                else
                {
                    if (Math.Round(t, 3) % t_diskr == 0)
                    {
                        X1_diskr = X1_prev + dt * X2_prev;
                        X2_diskr = X2_prev + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                    }
                    X1 = X1_prev + dt * X2_prev;
                    X2 = X2_prev + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                }
                w = w + dt * wshtriht;
                chart1.Series[0].Points.AddXY(t, w);
                chart2.Series[0].Points.AddXY(t, X1);
                chart2.Series[1].Points.AddXY(t, X1_diskr);
                chart2.Series[1].Points.AddXY(t, X1_diskr);
                chart3.Series[0].Points.AddXY(t, X2);
                chart3.Series[1].Points.AddXY(t, X2_diskr);
                t += dt;
                X1_prev = X1;
                X2_prev = X2;
                X3_prev = X3;
                
            }
            
        }
    

        private void button3_Click(object sender, EventArgs e)
        {
            double v = 0;
            double t = 0;
            double Massa = 350;
            double Stsputnika = 0.8;
            double X1 = 0, X1_diskr = 0, X2 = 0, X2_diskr = 0, X3 = 0;
            double w = 0;
            double t_diskr = Convert.ToDouble(textBox1.Text);
            double dt = Convert.ToDouble(textBox2.Text);
            double a0 = Convert.ToDouble(textBox4.Text);
            double a1 = Convert.ToDouble(textBox3.Text);
            double a3 = Convert.ToDouble(textBox5.Text);
            double a4 = Convert.ToDouble(textBox6.Text);
            double Wmax = Convert.ToDouble(textBox7.Text);
            double a = Convert.ToDouble(textBox8.Text);
            double wshtrihmax = Convert.ToDouble(textBox9.Text);
            double wshtriht = Convert.ToDouble(textBox10.Text);
            double Mvuscor = Convert.ToDouble(textBox11.Text);
            double Is = Convert.ToDouble(textBox12.Text);
            double Purd = Convert.ToDouble(textBox13.Text);
            double Lm = Convert.ToDouble(textBox14.Text);
            double Im = Convert.ToDouble(textBox15.Text);
            double Mv = Convert.ToDouble(textBox16.Text);
            double Murd = Purd * Lm;
            double X1_prev = 0, X2_prev = 0, X3_prev = 0;
            double F = 0;
            int f = 0;
            double Mupr;
            double Mt = wshtriht * Im;
            Random rand = new Random();
            double otkazDUS = 0 + rand.NextDouble() * GlobalVars.maxT;
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series[0].Name = "Частота оборотов маховика";
            if (chart2.Series.Count == 1)
            {
                chart2.Series[0].Name = "Угол отклонения спутника";
                chart2.Series.Add("Дискретный угол отклонения спутника");
                chart2.Series[1].Color = Color.Red;
                chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart2.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart2.Series.Add("Точка отказа ДУС");
                chart2.Series[2].Color = Color.Green;
                chart2.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                chart2.Series[2].Points.AddXY(otkazDUS, 0);
                //chart2.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            }
            if (chart3.Series.Count == 1)
            {
                chart3.Series[0].Name = "Угловая скорость спутника";
                chart3.Series[0].Color = Color.Red;
                chart3.Series.Add("Дискретная угловая скоротсь спутника");
                chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart3.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            }

            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();
            //chart2.Series[2].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart3.Series[1].Points.Clear();

            while (Math.Abs(w) < Wmax)
            {
                if (t >= otkazDUS)
                {
                    if (Math.Round(t, 3) % t_diskr == 0)
                    {
                        X1_diskr = X1_prev + dt * X2_prev;
                        X2_diskr = (X1_diskr - X1_prev) / dt + dt * ((Mv / Is) - (Im / Is) * X3_prev);
                    }
                    X1 = X1_prev + dt * X2_prev;
                    X2 = (X1 - X1_prev) / dt + dt * ((Mv / Is) - (Im / Is) * X3_prev);
                    X3 = a0 * X1_diskr + a1 * X2_diskr;
                }
                else
                {
                    if (Math.Round(t, 3) % t_diskr == 0)
                    {
                        X1_diskr = X1_prev + dt * X2_prev;
                        X2_diskr = X2_prev + dt * ((Mv / Is) - (Im / Is) * X3_prev);
                    }
                    X1 = X1_prev + dt * X2_prev;
                    X2 = X2_prev + dt * ((Mv / Is) - (Im / Is) * X3_prev);
                    X3 = a0 * X1_diskr + a1 * X2_diskr;
                }

                w = w + dt * X3;
                t += dt;
                chart1.Series[0].Points.AddXY(t, w);
                chart2.Series[0].Points.AddXY(t, X1);
                chart2.Series[1].Points.AddXY(t, X1_diskr);
                chart3.Series[0].Points.AddXY(t, X2);
                chart3.Series[1].Points.AddXY(t, X2_diskr);
                X1_prev = X1;
                X2_prev = X2;
                X3_prev = X3;

            }

            while (w > 0)
            {
                F = a3 * X1_diskr + a4 * X2_diskr;
                if (F > a) f = 1;
                else if ((F >= -a) && (F <= a)) { f = 0; }
                else if (F < -a) { f = -1; }
                Mupr = Murd * f;
                if (t >= otkazDUS)
                {
                    if (Math.Round(t, 3) % t_diskr == 0)
                    {
                        X1_diskr = X1_prev + dt * X2_prev;
                        X2_diskr = (X1_diskr - X1_prev) / dt + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                    }
                    X1 = X1_prev + dt * X2_prev;
                    X2 = (X1 - X1_prev) / dt + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                }
                else
                {
                    if (Math.Round(t, 3) % t_diskr == 0)
                    {
                        X1_diskr = X1_prev + dt * X2_prev;
                        X2_diskr = X2_prev + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                    }
                    X1 = X1_prev + dt * X2_prev;
                    X2 = X2_prev + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                }
                w = w - dt * wshtriht;
                chart1.Series[0].Points.AddXY(t, w);
                chart2.Series[0].Points.AddXY(t, X1);
                chart2.Series[1].Points.AddXY(t, X1_diskr);
                chart2.Series[1].Points.AddXY(t, X1_diskr);
                chart3.Series[0].Points.AddXY(t, X2);
                chart3.Series[1].Points.AddXY(t, X2_diskr);
                t += dt;
                X1_prev = X1;
                X2_prev = X2;
                X3_prev = X3;
            }
            while (w < 0)
            {
                F = a3 * X1_diskr + a4 * X2_diskr;
                if (F > a) f = 1;
                else if ((F >= -a) && (F <= a)) { f = 0; }
                else if (F < -a) { f = -1; }
                Mupr = Murd * f;

                if (t >= otkazDUS)
                {
                    if (Math.Round(t, 3) % t_diskr == 0)
                    {
                        X1_diskr = X1_prev + dt * X2_prev;
                        X2_diskr = (X1_diskr - X1_prev) / dt + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                    }
                    X1 = X1_prev + dt * X2_prev;
                    X2 = (X1 - X1_prev) / dt + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                }
                else
                {
                    if (Math.Round(t, 3) % t_diskr == 0)
                    {
                        X1_diskr = X1_prev + dt * X2_prev;
                        X2_diskr = X2_prev + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                    }
                    X1 = X1_prev + dt * X2_prev;
                    X2 = X2_prev + dt * ((Mt / Is) + (Mv / Is) - Mupr / Is);
                }
                w = w + dt * wshtriht;
                chart1.Series[0].Points.AddXY(t, w);
                chart2.Series[0].Points.AddXY(t, X1);
                chart2.Series[1].Points.AddXY(t, X1_diskr);
                chart2.Series[1].Points.AddXY(t, X1_diskr);
                chart3.Series[0].Points.AddXY(t, X2);
                chart3.Series[1].Points.AddXY(t, X2_diskr);
                t += dt;
                X1_prev = X1;
                X2_prev = X2;
                X3_prev = X3;
            
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {
            //chart3.ChartAreas[0].InnerPlotPosition.Height = chart3.ChartAreas[0].InnerPlotPosition.Height * ((float)1.1);
            //chart3.Invalidate();
            //chart3.ChartAreas[0].AxisX.ScaleView.Scroll(chart3.ChartAreas[0].AxisX.Maximum);
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }
    }
}
static class GlobalVars
{
    public static double maxT=1250;
}