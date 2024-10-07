namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.EscuelasPorPropuesta = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.PropuestasPorEstadoChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.EscuelasPorEstadoChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.NotaPromedioChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.EscuelasPorPropuesta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropuestasPorEstadoChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EscuelasPorEstadoChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotaPromedioChart)).BeginInit();
            this.SuspendLayout();
            // 
            // EscuelasPorPropuesta
            // 
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.Name = "ChartArea1";
            this.EscuelasPorPropuesta.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.EscuelasPorPropuesta.Legends.Add(legend1);
            this.EscuelasPorPropuesta.Location = new System.Drawing.Point(0, 12);
            this.EscuelasPorPropuesta.Name = "EscuelasPorPropuesta";
            this.EscuelasPorPropuesta.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Grayscale;
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.EscuelasPorPropuesta.Series.Add(series1);
            this.EscuelasPorPropuesta.Size = new System.Drawing.Size(726, 485);
            this.EscuelasPorPropuesta.TabIndex = 0;
            this.EscuelasPorPropuesta.Click += new System.EventHandler(this.EscuelasPorPropuesta_Click);
            // 
            // PropuestasPorEstadoChart
            // 
            chartArea2.Name = "ChartArea1";
            this.PropuestasPorEstadoChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.PropuestasPorEstadoChart.Legends.Add(legend2);
            this.PropuestasPorEstadoChart.Location = new System.Drawing.Point(0, 553);
            this.PropuestasPorEstadoChart.Name = "PropuestasPorEstadoChart";
            this.PropuestasPorEstadoChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SeaGreen;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.PropuestasPorEstadoChart.Series.Add(series2);
            this.PropuestasPorEstadoChart.Size = new System.Drawing.Size(625, 280);
            this.PropuestasPorEstadoChart.TabIndex = 1;
            this.PropuestasPorEstadoChart.Text = "chart1";
            this.PropuestasPorEstadoChart.Click += new System.EventHandler(this.PropuestasPorEstadoChart_Click);
            // 
            // EscuelasPorEstadoChart
            // 
            chartArea3.Name = "ChartArea1";
            this.EscuelasPorEstadoChart.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.EscuelasPorEstadoChart.Legends.Add(legend3);
            this.EscuelasPorEstadoChart.Location = new System.Drawing.Point(732, 12);
            this.EscuelasPorEstadoChart.Name = "EscuelasPorEstadoChart";
            this.EscuelasPorEstadoChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Excel;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedBar;
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.EscuelasPorEstadoChart.Series.Add(series3);
            this.EscuelasPorEstadoChart.Size = new System.Drawing.Size(961, 485);
            this.EscuelasPorEstadoChart.TabIndex = 2;
            this.EscuelasPorEstadoChart.Text = "EscuelasPorEstadoChart";
            // 
            // NotaPromedioChart
            // 
            chartArea4.Name = "ChartArea1";
            this.NotaPromedioChart.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.NotaPromedioChart.Legends.Add(legend4);
            this.NotaPromedioChart.Location = new System.Drawing.Point(646, 503);
            this.NotaPromedioChart.Name = "NotaPromedioChart";
            this.NotaPromedioChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Grayscale;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.NotaPromedioChart.Series.Add(series4);
            this.NotaPromedioChart.Size = new System.Drawing.Size(1013, 413);
            this.NotaPromedioChart.TabIndex = 3;
            this.NotaPromedioChart.Click += new System.EventHandler(this.chart1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1671, 969);
            this.Controls.Add(this.NotaPromedioChart);
            this.Controls.Add(this.EscuelasPorEstadoChart);
            this.Controls.Add(this.PropuestasPorEstadoChart);
            this.Controls.Add(this.EscuelasPorPropuesta);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.EscuelasPorPropuesta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PropuestasPorEstadoChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EscuelasPorEstadoChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotaPromedioChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart EscuelasPorPropuesta;
        private System.Windows.Forms.DataVisualization.Charting.Chart PropuestasPorEstadoChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart EscuelasPorEstadoChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart NotaPromedioChart;
    }
}

