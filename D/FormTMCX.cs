using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace business
{
    public partial class FormTMCX : Form
    {
        private System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection();
        private System.Data.SqlClient.SqlCommand sqlComm = new System.Data.SqlClient.SqlCommand();
        private System.Data.SqlClient.SqlDataReader sqldr;
        private System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter();
        private System.Data.DataSet dSet = new DataSet();

        public string strConn = "";

        public int intUserID = 0;
        public int intUserLimit = 0;
        public string strUserLimit = "";
        public string strUserName = "";
        public int intUserBM = 0;

        private string strDJBH = "";
        private int intDJID = 0;
        private string sDJClass = "";

        private bool isSaved = false;
        private ClassGetInformation cGetInformation;
        
        public FormTMCX()
        {
            InitializeComponent();
        }

        private void FormTMCX_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = strConn;
            sqlComm.Connection = sqlConn;
            sqlDA.SelectCommand = sqlComm;
            cGetInformation = new ClassGetInformation(strConn);
        }

        private void btnTM_Click(object sender, EventArgs e)
        {
            this.textBoxTM.Focus();
            this.textBoxTM.SelectAll();
        }

        private void textBoxTM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                textBoxTM.Text = textBoxTM.Text.ToUpper().Trim();

                if (textBoxTM.Text == "")
                {
                    labelWARN.ForeColor = Color.Red;
                    labelWARN.Text = "请录入商品条码";
                    return;
                }

                initTmVIEW();

                textBoxTM.SelectAll();
            }

        }

        private void initTmVIEW()
        {
            //是否有入库记录
            sqlConn.Open();
            sqlComm.CommandText = "SELECT 商品条码表.ID, 商品条码表.单据编号, 商品条码表.摘要, 商品条码表.日期, 商品条码表.出入库标记 AS 出库, 职员表.职员姓名 AS 操作员 FROM 商品条码表 INNER JOIN 职员表 ON 商品条码表.操作员ID = 职员表.ID WHERE (商品条码表.条码 = N'" + textBoxTM.Text.ToUpper() + "') ORDER BY 商品条码表.日期";
            if (dSet.Tables.Contains("条码表")) dSet.Tables.Remove("条码表");
            sqlDA.Fill(dSet, "条码表");
            dataGridViewTM.DataSource = dSet.Tables["条码表"];
            dataGridViewTM.Columns[0].Visible = false;

            if (dSet.Tables["条码表"].Rows.Count < 1)
            {
                labelWARN.ForeColor = Color.Red;
                labelWARN.Text = "没有相关记录";
            }
            else
            {
                labelWARN.ForeColor = Color.Green;
                labelWARN.Text = "读取条码成功";
            }
            sqlConn.Close();
        }


        private void textBoxTM_Enter(object sender, EventArgs e)
        {
            textBoxTM.SelectAll();
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品条码记录("+textBoxTM.Text.ToUpper()+");　";
            PrintDGV.Print_DataGridView(dataGridViewTM, strT, true, intUserLimit);
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            string strT = "商品条码记录(" + textBoxTM.Text.ToUpper() + ");　";
            PrintDGV.Print_DataGridView(dataGridViewTM, strT, false, intUserLimit);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewTM.SelectedRows.Count < 1)
            {
                MessageBox.Show("请选择要删除的条码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("是否要删除选定的条码？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return;

            System.Data.SqlClient.SqlTransaction sqlta;
            sqlConn.Open();
            sqlta = sqlConn.BeginTransaction();
            sqlComm.Transaction = sqlta;
            try
            {

                //明细
                for (int i = 0; i < dataGridViewTM.SelectedRows.Count; i++)
                {

                    sqlComm.CommandText = "DELETE FROM 商品条码表 WHERE (ID = " + dataGridViewTM.SelectedRows[i].Cells[0].Value.ToString() + ")";
                    sqlComm.ExecuteNonQuery();

                }

                sqlta.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库错误：" + ex.Message.ToString(), "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlta.Rollback();
                return;
            }
            finally
            {
                sqlConn.Close();
            }
            MessageBox.Show("删除条码完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            initTmVIEW();
        }
    }
}