namespace FIH_WMS_System.UI
{
    partial class CheckStockForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new System.Windows.Forms.TextBox();
            this.txtLocCode = new System.Windows.Forms.TextBox();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.txtPhysicalQty = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 40);
            this.label1.Text = "商品编码：";

            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 100);
            this.label2.Text = "所在库位：";

            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 160);
            this.label3.Text = "目标批次号：";

            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 220);
            this.label4.Text = "实际盘点数量：";

            this.txtGoodsCode.Location = new System.Drawing.Point(180, 37);
            this.txtGoodsCode.Size = new System.Drawing.Size(190, 27);

            this.txtLocCode.Location = new System.Drawing.Point(180, 97);
            this.txtLocCode.Size = new System.Drawing.Size(190, 27);

            this.txtBatchNo.Location = new System.Drawing.Point(180, 157);
            this.txtBatchNo.Size = new System.Drawing.Size(190, 27);

            this.txtPhysicalQty.Location = new System.Drawing.Point(180, 217);
            this.txtPhysicalQty.Size = new System.Drawing.Size(190, 27);

            this.btnConfirm.Location = new System.Drawing.Point(120, 280);
            this.btnConfirm.Size = new System.Drawing.Size(152, 40);
            this.btnConfirm.Text = "📋 确认盘点调整";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);

            this.ClientSize = new System.Drawing.Size(420, 360);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtPhysicalQty);
            this.Controls.Add(this.txtBatchNo);
            this.Controls.Add(this.txtLocCode);
            this.Controls.Add(this.txtGoodsCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CheckStockForm";
            this.Text = "库存盘点";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.TextBox txtLocCode;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.TextBox txtPhysicalQty;
        private System.Windows.Forms.Button btnConfirm;
    }
}