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
            this.generate = new System.Windows.Forms.Button();
            this.expand = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // skglControl1
            // 
            this.skglControl1.BackColor = System.Drawing.Color.Black;
            this.skglControl1.Location = new System.Drawing.Point(12, 10);
            this.skglControl1.Name = "skglControl1";
            this.skglControl1.Size = new System.Drawing.Size(720, 720);
            this.skglControl1.TabIndex = 0;
            this.skglControl1.VSync = false;
            this.skglControl1.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs>(this.display);
            this.skglControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.skglControl1_KeyDown);
            // 
            // generate
            // 
            this.generate.Location = new System.Drawing.Point(738, 10);
            this.generate.Name = "generate";
            this.generate.Size = new System.Drawing.Size(70, 67);
            this.generate.TabIndex = 1;
            this.generate.Text = "generete";
            this.generate.UseVisualStyleBackColor = true;
            this.generate.Click += new System.EventHandler(this.generate_Click);
            // 
            // expand
            // 
            this.expand.Location = new System.Drawing.Point(738, 83);
            this.expand.Name = "expand";
            this.expand.Size = new System.Drawing.Size(70, 23);
            this.expand.TabIndex = 2;
            this.expand.Text = "expand";
            this.expand.UseVisualStyleBackColor = true;
            this.expand.Click += new System.EventHandler(this.expandClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 742);
            this.Controls.Add(this.expand);
            this.Controls.Add(this.generate);
            this.Controls.Add(this.skglControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private SkiaSharp.Views.Desktop.SKGLControl skglControl1;
        private System.Windows.Forms.Button generate;
        private System.Windows.Forms.Button expand;
    }
}

