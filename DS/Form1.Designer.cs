namespace DS
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.skglControl1 = new SkiaSharp.Views.Desktop.SKGLControl();
            this.Do1Iteration = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // skglControl1
            // 
            this.skglControl1.BackColor = System.Drawing.Color.Black;
            this.skglControl1.Location = new System.Drawing.Point(12, 12);
            this.skglControl1.Name = "skglControl1";
            this.skglControl1.Size = new System.Drawing.Size(1025, 1025);
            this.skglControl1.TabIndex = 0;
            this.skglControl1.VSync = false;
            this.skglControl1.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs>(this.display);
            this.skglControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.skglControl1_KeyDown);
            // 
            // Do1Iteration
            // 
            this.Do1Iteration.Location = new System.Drawing.Point(1042, 12);
            this.Do1Iteration.Name = "Do1Iteration";
            this.Do1Iteration.Size = new System.Drawing.Size(70, 67);
            this.Do1Iteration.TabIndex = 1;
            this.Do1Iteration.Text = "Do1Iteration";
            this.Do1Iteration.UseVisualStyleBackColor = true;
            this.Do1Iteration.Click += new System.EventHandler(this.Do1Iteration_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1124, 1011);
            this.Controls.Add(this.Do1Iteration);
            this.Controls.Add(this.skglControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private SkiaSharp.Views.Desktop.SKGLControl skglControl1;
        private System.Windows.Forms.Button Do1Iteration;
    }
}

