namespace AzurePlateRecognition {
    partial class AzurePlateRecognition {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent() {
            this.btnSelectPlate = new System.Windows.Forms.Button();
            this.plate = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.resultTextBox = new System.Windows.Forms.RichTextBox();
            this.croppedPlate = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.plate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.croppedPlate)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelectPlate
            // 
            this.btnSelectPlate.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSelectPlate.Location = new System.Drawing.Point(12, 14);
            this.btnSelectPlate.Name = "btnSelectPlate";
            this.btnSelectPlate.Size = new System.Drawing.Size(75, 23);
            this.btnSelectPlate.TabIndex = 0;
            this.btnSelectPlate.Text = "選擇車牌";
            this.btnSelectPlate.UseVisualStyleBackColor = true;
            this.btnSelectPlate.Click += new System.EventHandler(this.button1_Click);
            // 
            // plate
            // 
            this.plate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plate.Location = new System.Drawing.Point(12, 54);
            this.plate.Name = "plate";
            this.plate.Size = new System.Drawing.Size(566, 266);
            this.plate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.plate.TabIndex = 1;
            this.plate.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // resultTextBox
            // 
            this.resultTextBox.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultTextBox.Location = new System.Drawing.Point(593, 54);
            this.resultTextBox.Name = "resultTextBox";
            this.resultTextBox.ReadOnly = true;
            this.resultTextBox.Size = new System.Drawing.Size(561, 538);
            this.resultTextBox.TabIndex = 3;
            this.resultTextBox.Text = "";
            // 
            // croppedPlate
            // 
            this.croppedPlate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.croppedPlate.Location = new System.Drawing.Point(12, 326);
            this.croppedPlate.Name = "croppedPlate";
            this.croppedPlate.Size = new System.Drawing.Size(566, 266);
            this.croppedPlate.TabIndex = 1;
            this.croppedPlate.TabStop = false;
            // 
            // AzurePlateRecognition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 607);
            this.Controls.Add(this.resultTextBox);
            this.Controls.Add(this.croppedPlate);
            this.Controls.Add(this.plate);
            this.Controls.Add(this.btnSelectPlate);
            this.Name = "AzurePlateRecognition";
            this.Text = "Azure Plate Recognition";
            ((System.ComponentModel.ISupportInitialize)(this.plate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.croppedPlate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSelectPlate;
        private System.Windows.Forms.PictureBox plate;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox resultTextBox;
        private System.Windows.Forms.PictureBox croppedPlate;
    }
}

