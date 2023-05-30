namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.plot = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttondx1 = new System.Windows.Forms.Button();
            this.buttonSplain = new System.Windows.Forms.Button();
            this.buttondx2 = new System.Windows.Forms.Button();
            this.buttonLagranje = new System.Windows.Forms.Button();
            this.Input = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.plot)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // plot
            // 
            chartArea1.Name = "ChartArea1";
            this.plot.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.plot.Legends.Add(legend1);
            this.plot.Location = new System.Drawing.Point(311, 12);
            this.plot.Name = "plot";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Color = System.Drawing.Color.Blue;
            series1.Legend = "Legend1";
            series1.Name = "Точки";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            this.plot.Series.Add(series1);
            this.plot.Series.Add(series2);
            this.plot.Size = new System.Drawing.Size(611, 276);
            this.plot.TabIndex = 0;
            this.plot.Text = "chart1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttondx1);
            this.groupBox2.Controls.Add(this.buttonSplain);
            this.groupBox2.Controls.Add(this.buttondx2);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(65, 58);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(240, 230);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // buttondx1
            // 
            this.buttondx1.Location = new System.Drawing.Point(6, 101);
            this.buttondx1.Name = "buttondx1";
            this.buttondx1.Size = new System.Drawing.Size(225, 45);
            this.buttondx1.TabIndex = 6;
            this.buttondx1.Text = "Производная 1 порядка";
            this.buttondx1.UseVisualStyleBackColor = true;
            this.buttondx1.Click += new System.EventHandler(this.buttondx1_Click);
            // 
            // buttonSplain
            // 
            this.buttonSplain.Location = new System.Drawing.Point(6, 31);
            this.buttonSplain.Name = "buttonSplain";
            this.buttonSplain.Size = new System.Drawing.Size(225, 45);
            this.buttonSplain.TabIndex = 5;
            this.buttonSplain.Text = "Итерполяция кубическим сплайном";
            this.buttonSplain.UseVisualStyleBackColor = true;
            this.buttonSplain.Click += new System.EventHandler(this.splineButton_Click);
            // 
            // buttondx2
            // 
            this.buttondx2.Location = new System.Drawing.Point(6, 172);
            this.buttondx2.Name = "buttondx2";
            this.buttondx2.Size = new System.Drawing.Size(225, 45);
            this.buttondx2.TabIndex = 4;
            this.buttondx2.Text = "Производная 2 порядка";
            this.buttondx2.UseVisualStyleBackColor = true;
            this.buttondx2.Click += new System.EventHandler(this.buttondx2_Click);
            // 
            // buttonLagranje
            // 
            this.buttonLagranje.Location = new System.Drawing.Point(835, 378);
            this.buttonLagranje.Name = "buttonLagranje";
            this.buttonLagranje.Size = new System.Drawing.Size(87, 45);
            this.buttonLagranje.TabIndex = 4;
            this.buttonLagranje.Text = "Интерполяция методом Лагранжа";
            this.buttonLagranje.UseVisualStyleBackColor = true;
            // 
            // Input
            // 
            this.Input.Location = new System.Drawing.Point(12, 12);
            this.Input.Multiline = true;
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(47, 263);
            this.Input.TabIndex = 2;
            this.Input.Text = "-4 -11\r\n-2 -5\r\n0 0\r\n2 3\r\n4 9";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(71, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(225, 40);
            this.button1.TabIndex = 3;
            this.button1.Text = "Проверить ввод";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonCheckData_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(754, 389);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 300);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Input);
            this.Controls.Add(this.buttonLagranje);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.plot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.plot)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart plot;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox Input;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button buttondx2;
        private System.Windows.Forms.Button buttonLagranje;
        private System.Windows.Forms.Button buttondx1;
        private System.Windows.Forms.Button buttonSplain;
    }
}

