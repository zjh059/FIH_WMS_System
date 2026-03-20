namespace FIH_WMS_System.UI
{
    partial class MoveStockForm
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
            this.txtFromLoc = new System.Windows.Forms.TextBox();
            this.txtToLoc = new System.Windows.Forms.TextBox();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 40);
            this.label1.Text = "商品编码：";

            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 100);
            this.label2.Text = "源库位：";

            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 160);
            this.label3.Text = "目标库位：";

            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 220);
            this.label4.Text = "移动数量：";

            this.txtGoodsCode.Location = new System.Drawing.Point(180, 37);
            this.txtGoodsCode.Size = new System.Drawing.Size(175, 27);

            this.txtFromLoc.Location = new System.Drawing.Point(180, 97);
            this.txtFromLoc.Size = new System.Drawing.Size(175, 27);

            this.txtToLoc.Location = new System.Drawing.Point(180, 157);
            this.txtToLoc.Size = new System.Drawing.Size(175, 27);

            this.txtQty.Location = new System.Drawing.Point(180, 217);
            this.txtQty.Size = new System.Drawing.Size(175, 27);

            this.btnConfirm.Location = new System.Drawing.Point(120, 280);
            this.btnConfirm.Size = new System.Drawing.Size(152, 40);
            this.btnConfirm.Text = "🔄 确认移库";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);

            this.ClientSize = new System.Drawing.Size(420, 360);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtQty);
            this.Controls.Add(this.txtToLoc);
            this.Controls.Add(this.txtFromLoc);
            this.Controls.Add(this.txtGoodsCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MoveStockForm";
            this.Text = "库位移库";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.TextBox txtFromLoc;
        private System.Windows.Forms.TextBox txtToLoc;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Button btnConfirm;
    }
}