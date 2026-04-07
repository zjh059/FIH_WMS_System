namespace FIH_WMS_System.UI
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.chkVoice = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAgvSpeed = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSettings
            // 
            this.gbSettings.Controls.Add(this.label2);
            this.gbSettings.Controls.Add(this.cmbAgvSpeed);
            this.gbSettings.Controls.Add(this.label1);
            this.gbSettings.Controls.Add(this.chkVoice);
            this.gbSettings.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbSettings.Location = new System.Drawing.Point(30, 30);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(460, 180);
            this.gbSettings.TabIndex = 0;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "⚙️ 全局参数配置";
            // 
            // chkVoice
            // 
            this.chkVoice.AutoSize = true;
            this.chkVoice.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkVoice.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.chkVoice.Location = new System.Drawing.Point(30, 50);
            this.chkVoice.Name = "chkVoice";
            this.chkVoice.Size = new System.Drawing.Size(234, 31);
            this.chkVoice.TabIndex = 0;
            this.chkVoice.Text = "🔊 开启系统语音播报";
            this.chkVoice.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label1.Location = new System.Drawing.Point(26, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(324, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "开启后，入库、预警及调度操作将会有语音提示";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(187, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "AGV 监控台刷新频率:";
            // 
            // cmbAgvSpeed
            // 
            this.cmbAgvSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAgvSpeed.FormattingEnabled = true;
            this.cmbAgvSpeed.Items.AddRange(new object[] {
            "实时模式 (1秒)",
            "标准模式 (3秒)",
            "省流模式 (5秒)"});
            this.cmbAgvSpeed.Location = new System.Drawing.Point(230, 117);
            this.cmbAgvSpeed.Name = "cmbAgvSpeed";
            this.cmbAgvSpeed.Size = new System.Drawing.Size(180, 32);
            this.cmbAgvSpeed.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(30, 230);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(460, 45);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "💾 保存并立即生效";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(520, 300);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbSettings);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "系统参数设置";
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.CheckBox chkVoice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbAgvSpeed;
        private System.Windows.Forms.Button btnSave;
    }
}